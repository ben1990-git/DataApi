using DataAPI.Interfaces;
using DataAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;


namespace DataAPI.DataAccess.Repositories
{
    public class MemoryCacheRepository : IRepository<Data>
    {
        private readonly IMemoryCache _cache;
        public MemoryCacheRepository(IMemoryCache cache)
        {
            _cache = cache;

        }
        public async Task<Data> AddAsync(Data model)
        {
            string key = $"user_{model.Id}";

            _cache.Set(key, model, TimeSpan.FromMinutes(10));

            return model;
        }

        public async Task<Data> GetByIdAsync(int id)
        {

            string key = $"user_{id}";

            if (_cache.TryGetValue(key, out Data model))
            {
                return model;
            }
            else
            {
                return null;
            }

        }

        public Task<Data> UpdateAsync(int id, Data model)
        {
            throw new NotImplementedException();
        }
    }
}
