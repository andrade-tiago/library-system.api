using LibrarySystem.DTOs.Customer;
using LibrarySystem.DTOs.Request;
using LibrarySystem.Enums;
using LibrarySystem.Filters;
using LibrarySystem.Services.Customer;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(ICustomerService customerService) : ControllerBase
{
    private readonly ICustomerService _customerService = customerService;

    [HttpGet("{id}", Name = "GetCustomerById")]
    [ValidateIdFilter]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var response = await _customerService.GetByIdAsync(id);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet("by-cpf/{cpf}")]
    public async Task<IActionResult> GetByCpfAsync(string cpf)
    {
        var response = await _customerService.GetByCpfAsync(cpf);

        return response.Code switch
        {
            ResponseCode.CustomerFetched
                => Ok(response),

            ResponseCode.InvalidCpf
                => BadRequest(response),

            _ => NotFound(response),
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPagedAsync([FromQuery] PaginationOptions pagination)
    {
        var response = await _customerService.GetAllPagedAsync(pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CustomerCreateDto dto)
    {
        var response = await _customerService.CreateAsync(dto);

        return response.Code switch
        {
            ResponseCode.CustomerCreated when response.Result is not null
                => CreatedAtRoute("GetCustomerById", new { id = response.Result.Id }, response),

            ResponseCode.InvalidCpf
                => BadRequest(response),

            _ => StatusCode(500, response),
        };
    }

    [HttpPut("{id}")]
    [ValidateIdFilter]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] CustomerUpdateDto dto)
    {
        var response = await _customerService.UpdateAsync(id, dto);

        return response.Code switch
        {
            ResponseCode.CustomerUpdated
                => Ok(response),

            ResponseCode.CustomerNotFound
                => NotFound(response),

            _ => StatusCode(500, response),
        };
    }

    [HttpDelete("{id}")]
    [ValidateIdFilter]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var response = await _customerService.DeleteAsync(id);

        return response.Code switch
        {
            ResponseCode.CustomerDeleted
                => Ok(response),

            ResponseCode.CustomerNotFound or
            ResponseCode.OpenCustomerReservation
                => BadRequest(response),

            _ => StatusCode(500, response),
        };
    }
}
