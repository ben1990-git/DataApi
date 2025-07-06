using DataAPI.DataAccess.Repositories;
using DataAPI.Interfaces;
using DataAPI.Models;
using Microsoft.Extensions.Caching.Memory;



namespace DataAPI.Factories
{
    public class RepositoryFactory : IFactory<IRepository<Data>>
    {
      //  private readonly IMemoryCache _memoryCache;
     //   private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IServiceProvider _provider;

        public RepositoryFactory(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async Task<IRepository<Data>> CreateAsync(string name)
        {
            if (name == "Redis")
                return _provider.GetRequiredService<RedisRepository>();
            if (name == "File")
                return _provider.GetRequiredService<FileRepository>();
            if (name == "DataBase")
                return _provider.GetRequiredService<DataBaseRepository>();
            //if (name == "MemoryCache")
            //    return _provider.GetRequiredService<MemoryCacheRepository>();
            return null;
        }
    }
}
