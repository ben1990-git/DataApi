namespace DataAPI.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T model);
        Task<T> UpdateAsync(int id,T model);
    }
}
