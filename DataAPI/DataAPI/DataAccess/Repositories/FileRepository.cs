using DataAPI.Interfaces;
using DataAPI.Models;
using System.Text.Json;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace DataAPI.DataAccess.Repositories
{
    public class FileRepository : IRepository<Data>
    {
        private readonly IWebHostEnvironment _env;
        public FileRepository(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task<Data> AddAsync(Data model)
        {
            string folderPath = Path.Combine(_env.ContentRootPath, "DataFiles");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);


            DateTime expiry = DateTime.UtcNow.AddMinutes(30);
            string timestamp = expiry.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);

            string fileName = $"{model.Id}_{timestamp}.json";
            string fullPath = Path.Combine(folderPath, fileName);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(model, options);

            await File.WriteAllTextAsync(fullPath, json);

            return model;
        }

        public async Task<Data> GetByIdAsync(int id)
        {
            string folderPath = Path.Combine(_env.ContentRootPath, "DataFiles");

            if (!Directory.Exists(folderPath))
                return null;

            var matchingFiles = Directory
                .EnumerateFiles(folderPath, $"{id}_*.json")
                .ToList();

            if (!matchingFiles.Any())
                return null;


            var validFile = matchingFiles
                .Select(file =>
                {
                    var name = Path.GetFileNameWithoutExtension(file);
                    var parts = name.Split('_');

                    if (parts.Length < 2)
                        return (file: (string?)null, timestamp: DateTime.MinValue);

                    if (DateTime.TryParseExact(parts[1], "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timestamp))
                    {
                        return (file, timestamp);
                    }

                    return (file: (string?)null, timestamp: DateTime.MinValue);
                })
                .Where(x => x.file != null && x.timestamp > DateTime.UtcNow)
                .OrderByDescending(x => x.timestamp)
                .FirstOrDefault();

            if (validFile.file == null)
                return null;

            // Read and return the non-expired file
            string json = await File.ReadAllTextAsync(validFile.file);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Data>(json, options);
        }


        public async Task<Data> UpdateAsync(int id, Data model)
        {
            var result = await GetByIdAsync(id);

            if (result is not null)
            {
                string folderPath = Path.Combine(_env.ContentRootPath, "DataFiles");
                string pattern = $"{model.Id}_*.json";

                string? existingFilePath = Directory
                .GetFiles(folderPath, pattern)
                .FirstOrDefault();
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(model, options);

                await File.WriteAllTextAsync(existingFilePath, json);

            }

            return result;
        }
    }
}
