using DataAPI.Models;

namespace DataAPI.Interfaces
{
    public interface IDataService
    {
        Task<Data> GetDataByIdAsync(int id);
        Task<Data> CreateDataAsync(Data model);
        Task<Data> UpdateDataAsync(Data model);
    }
}
