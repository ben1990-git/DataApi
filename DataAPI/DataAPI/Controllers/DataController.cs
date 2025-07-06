using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DataAPI.Interfaces;
using DataAPI.Extenstions;
using DataAPI.DTOs;


namespace DataAPI.Controllers
{
    [ApiController]
    [Route("api/Data")]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;
        public DataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// retrive data by id only rgisterd users 
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> Data(int id)
        {
            if (id == 0)
            {
                return BadRequest("Invalid request: ID must be greater than 0.");
            }

            try
            {
                var result = await _dataService.GetDataByIdAsync(id);
                if (result is not null)
                    return Ok(result.DataToDataDto());
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return Problem();
            }

        }

        /// <summary>
        /// create new data entry. allowd only by admin.
        /// </summary>

        [HttpPost("Create")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] CreateDataDtoRequest createDataDto)
        {
            try
            {
                var result = await _dataService.CreateDataAsync(createDataDto.CreateDataDtoToData());

                return CreatedAtAction(nameof(Create), new { id = result.Id }, result.DataToDataDto());
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }

        /// <summary>
        /// update existing  data entry. allowd only by admin.
        /// existing data is retived by id in the request body. 
        /// </summary>

        [HttpPut("Update")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] UpdateDataDtoRequest UpdateDataDto)
        {
            try
            {
                var result = await _dataService.UpdateDataAsync(UpdateDataDto.UpdateDataDtoToData());
                if (result is not null)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound($"data with {UpdateDataDto.Id} not found ");
                }
            }
            catch (Exception ex)
            {
                return Problem();
            }
        }
    }
}

