namespace DataAPI.DTOs
{
    public class UpdateDataDtoRequest
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
