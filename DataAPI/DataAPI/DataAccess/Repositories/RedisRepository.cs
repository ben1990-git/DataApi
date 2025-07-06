using DataAPI.Interfaces;
using DataAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;


namespace DataAPI.DataAccess.Repositories
{
    public class RedisRepository : IRepository<Data>
    {
        private readonly IDistributedCache _cache;
        public RedisRepository(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<Data> AddAsync(Data model)
        {
            var json = JsonSerializer.Serialize(model);
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            await _cache.SetStringAsync(model.Id.ToString(), json, options);
            return model;

        }

        public async Task<Data> GetByIdAsync(int id)
        {
            var cached = await _cache.GetStringAsync(id.ToString());

            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<Data>(cached);
            }
            else
            {
                return null;
            }
        }

        public async Task<Data> UpdateAsync(int id, Data model)
        {
            var cached = await _cache.GetStringAsync(id.ToString());

            if (!string.IsNullOrEmpty(cached))
            {
                var json = JsonSerializer.Serialize(model);

                await _cache.SetStringAsync(model.Id.ToString(), json);
                
            }
            return model;
            
        }
    }

}

