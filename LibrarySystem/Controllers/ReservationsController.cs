using LibrarySystem.DTOs.Reservation;
using LibrarySystem.DTOs.Request;
using LibrarySystem.Enums;
using LibrarySystem.Filters;
using LibrarySystem.Services.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController(IReservationService reservationService) : ControllerBase
{
    private readonly IReservationService _reservationService = reservationService;

    [HttpGet("{id}", Name = "GetReservationById")]
    [ValidateIdFilter]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var response = await _reservationService.GetByIdAsync(id);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet("/api/customers/{customerId}/reservations/last")]
    [ValidateIdFilter(nameof(customerId))]
    public async Task<IActionResult> GetLastByCustomerAsync(int customerId)
    {
        var response = await _reservationService.GetLastByCustomerAsync(customerId);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet("/api/books/{bookId}/reservations/last")]
    [ValidateIdFilter(nameof(bookId))]
    public async Task<IActionResult> GetLastByBookAsync(int bookId)
    {
        var response = await _reservationService.GetLastByBookAsync(bookId);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPagedAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _reservationService.GetAllPagedAsync(pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpGet("/api/customers/{customerId}/reservations")]
    [ValidateIdFilter(nameof(customerId))]
    public async Task<IActionResult> GetByCustomerPagedAsync(int customerId, [FromQuery] PaginationRequest pagination)
    {
        var response = await _reservationService.GetByCustomerPagedAsync(customerId, pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpGet("/api/books/{bookId}/reservations")]
    [ValidateIdFilter(nameof(bookId))]
    public async Task<IActionResult> GetByBookPagedAsync(int bookId, [FromQuery] PaginationRequest pagination)
    {
        var response = await _reservationService.GetByBookPagedAsync(bookId, pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ReservationCreateDto dto)
    {
        var response = await _reservationService.CreateAsync(dto);

        return response.Code switch
        {
            ResponseCode.ReservationCreated when response.Result is not null
                => CreatedAtRoute("GetReservationById", new { id = response.Result.Id }, response),

            ResponseCode.CustomerNotFound or ResponseCode.BookNotFound
                => NotFound(response),

            ResponseCode.OpenCustomerReservation or ResponseCode.OpenBookReservation
                => BadRequest(response),

            _ => StatusCode(500, response),
        };
    }

    [HttpPatch("{id}/close")]
    [ValidateIdFilter]
    public async Task<IActionResult> CloseReservationAsync(int id, ReservationCompleteDto dto)
    {
        var response = await _reservationService.CloseReservationAsync(id, dto);

        return response.Code switch
        {
            ResponseCode.ReservationCompleted or
            ResponseCode.ReservationCompletedLate
                => Ok(response),

            ResponseCode.ReservationNotFound
                => NotFound(response),

            ResponseCode.ReservationAlreadyCompleted or
            ResponseCode.ReservationCompleteLaterThanToday
                => BadRequest(response),

            _ => StatusCode(500, response),
        };
    }
}
