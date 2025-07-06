namespace DataAPI.Interfaces
{
    public interface IFactory<T> where T : class
    {
        Task<T> CreateAsync(string name);
    }
}
