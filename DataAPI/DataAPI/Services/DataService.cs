using DataAPI.DTOs;
using DataAPI.Extenstions;
using DataAPI.Interfaces;
using DataAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAPI.Services
{
    public class DataService : IDataService
    {
        private readonly IFactory<IRepository<Data>> _factory;
        public DataService(IFactory<IRepository<Data>> factory)
        {
            _factory = factory;
        }

        public async Task<Data> CreateDataAsync(Data model)
        {
            var repos = await Task.WhenAll(
                _factory.CreateAsync("DataBase"),
                _factory.CreateAsync("File"),
                _factory.CreateAsync("Redis")                                          
            );
            // db for easy id set i save in the db first then pass
            // can use guid and cancel automated id in ef but and ther run tho all in parrall
            // but this is for convinance it can be improved 

           var modelWithId=  await  repos.FirstOrDefault().AddAsync(model);

            await Task.WhenAll(repos[1].AddAsync(modelWithId), repos[2].AddAsync(modelWithId)); 

            return model;
        }

        public async Task<Data> GetDataByIdAsync(int id)
        {
            var cache = await _factory.CreateAsync("Redis");
            var data = await cache.GetByIdAsync(id);

            if (data != null)
                return data;

            var file = await _factory.CreateAsync("File");
            data = await file.GetByIdAsync(id);

            if (data != null)
            {
                await cache.AddAsync(data);
                return data;
            }

            var db = await _factory.CreateAsync("DataBase");
            data = await db.GetByIdAsync(id);

            if (data != null)
            {
                await Task.WhenAll(
                    file.AddAsync(data),
                    cache.AddAsync(data)
                );
            }

            return data;
        }

        public async Task<Data> UpdateDataAsync(Data model)
        {
            var db = await _factory.CreateAsync("DataBase");    
            var updatedData=  await db.UpdateAsync(model.Id, model);

            if (updatedData != null)
            {
                var repos = await Task.WhenAll(
                 _factory.CreateAsync("File"),
                 _factory.CreateAsync("Redis"));

                foreach (var repo in repos)
                {
                  var updated = await repo.UpdateAsync(updatedData.Id, updatedData);

                    if (updated is  null)
                        await repo.AddAsync(updatedData);
                };
                return updatedData;
            }
            else
            {
                return null;
            }           
        }
    }





}
