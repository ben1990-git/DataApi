using DataAPI.DTOs;
using DataAPI.Models;

namespace DataAPI.Extenstions
{
    public static class DataMapper
    {
        public static DataDto DataToDataDto(this Data Model)
        {
            return new DataDto
            {
                Id = Model.Id,
                CreatedAt = Model.CreatedAt,
                Value = Model.Value,
            };
        }

        public static Data CreateDataDtoToData(this CreateDataDtoRequest Model)
        {
            return new Data
            {
                Value = Model.Value,
                CreatedAt = DateTime.UtcNow,
            };
        }
        public static Data UpdateDataDtoToData(this UpdateDataDtoRequest Model)
        {
            return new Data
            {
                Id = Model.Id,
                Value = Model.Value,
            };

        }
    }
}

