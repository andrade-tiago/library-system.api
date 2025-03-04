using LibrarySystem.DTOs.Reservation;
using LibrarySystem.DTOs.Request;
using LibrarySystem.Enums;
using LibrarySystem.Services.Reservation;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController(IReservationService reservationService) : ControllerBase
{
    private readonly IReservationService _reservationService = reservationService;

    [HttpGet("{id}", Name = "GetReservationById")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var response = await _reservationService.GetByIdAsync(id);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet("/customers/{customerId}/reservations/last")]
    public async Task<IActionResult> GetLastByCustomerAsync(int customerId)
    {
        var response = await _reservationService.GetLastByCustomerAsync(customerId);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet("/books/{bookId}/reservations/last")]
    public async Task<IActionResult> GetLastByBookAsync(int bookId)
    {
        var response = await _reservationService.GetLastByBookAsync(bookId);

        return response.Result is not null ? Ok(response) : NotFound(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetReservationsAsync([FromQuery] PaginationRequest pagination)
    {
        var response = await _reservationService.GetReservationsAsync(pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpGet("/customers/{customerId}/reservations")]
    public async Task<IActionResult> GetReservationsByCustomerAsync(int customerId, [FromQuery] PaginationRequest pagination)
    {
        var response = await _reservationService.GetReservationsByCustomerAsync(customerId, pagination);

        return response.Result?.Count > 0 ? Ok(response) : NotFound(response);
    }

    [HttpGet("/books/{bookId}/reservations")]
    public async Task<IActionResult> GetReservationsByBookAsync(int bookId, [FromQuery] PaginationRequest pagination)
    {
        var response = await _reservationService.GetReservationsByBookAsync(bookId, pagination);

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

    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> CompleteReservationAsync(int id, ReservationCompleteDto dto)
    {
        var response = await _reservationService.CompleteReservationAsync(id, dto);

        return response.Code switch
        {
            ResponseCode.ReservationCompleted or ResponseCode.ReservationCompletedLate
                => Ok(response),

            ResponseCode.ReservationNotFound
                => NotFound(response),

            ResponseCode.ReservationAlreadyCompleted or ResponseCode.ReservationCompleteLaterThanToday
                => BadRequest(response),

            _ => StatusCode(500, response),
        };
    }
}
