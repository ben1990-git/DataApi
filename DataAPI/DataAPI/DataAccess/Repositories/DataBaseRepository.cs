using DataAPI.DataAccess;
using DataAPI.Interfaces;
using DataAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAPI.DataAccess.Repositories
{
    public class DataBaseRepository : IRepository<Data>
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public DataBaseRepository(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        public async Task<Data> AddAsync(Data model)
        {
            _applicationDBContext.Entrys.Add(model);
            await _applicationDBContext.SaveChangesAsync();
            return model;
        }

        public async Task<Data> GetByIdAsync(int id)
        {
            return await _applicationDBContext.Entrys.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Data> UpdateAsync(int id, Data model)
        {
            var result = await _applicationDBContext.Entrys.FirstOrDefaultAsync(e => e.Id == id);

            if (result != null)
            {
                result.Value = model.Value;
                await _applicationDBContext.SaveChangesAsync();
            }

            return result;
        }
    }
}
