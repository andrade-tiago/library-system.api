using LibrarySystem.DTOs.Customer;
using LibrarySystem.DTOs.Request;
using LibrarySystem.Services.Customer;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        [HttpGet("{id}", Name = "GetCustomerById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _customerService.GetByIdAsync(id);

            return response.Result is not null ? Ok(response) : NotFound(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync([FromQuery] PaginationRequest pagination)
        {
            var response = await _customerService.GetCustomersAsync(pagination);

            return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CustomerCreateDto dto)
        {
            var response = await _customerService.CreateAsync(dto);

            return response.Result is not null
                ? CreatedAtRoute("GetCustomerById", new { id = response.Result.Id }, response)
                : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CustomerUpdateDto dto)
        {
            var response = await _customerService.UpdateAsync(id, dto);

            return response.Result is not null ? Ok(response) : NotFound(response);
        }
    }
}
