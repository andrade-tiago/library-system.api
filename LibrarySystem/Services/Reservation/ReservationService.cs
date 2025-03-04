using AutoMapper;
using LibrarySystem.Constants;
using LibrarySystem.DTOs.Reservation;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Repositories.Book;
using LibrarySystem.Repositories.Reservation;
using LibrarySystem.Repositories.Customer;
using Microsoft.Extensions.Options;

namespace LibrarySystem.Services.Reservation;

public class ReservationService(
    IReservationRepository bookReservationRepository,
    IBookRepository bookRepository,
    ICustomerRepository customerRepository,
    IMapper mapper,
    IOptions<ReservationSettings> options
) : IReservationService
{
    private readonly IReservationRepository _bookReservationRepository = bookReservationRepository;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IMapper _mapper = mapper;
    private readonly int _expirationDays = options.Value.ExpirationDays;

    public async Task<ApiResponse<ReservationDto?>> GetByIdAsync(int id)
    {
        ApiResponse<ReservationDto?> response = new();

        var bookReservation = await _bookReservationRepository.GetByIdAsync(id);

        if (bookReservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotFound, response);
            return response;
        }
        _mapper.Map(ResponseStatus.ReservationFetched, response);
        response.Result = _mapper.Map<ReservationDto>(bookReservation);
        return response;
    }

    public async Task<ApiResponse<ReservationDto?>> GetLastByCustomerAsync(int customerId)
    {
        ApiResponse<ReservationDto?> response = new();

        var customerExists = await _customerRepository.CustomerExistsAsync(customerId);
        if (!customerExists)
        {
            _mapper.Map(ResponseStatus.CustomerNotFound, response);
            return response;
        }

        var reservation = await _bookReservationRepository.GetLastByCustomerAsync(customerId);
        if (reservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotFound, response);
            return response;
        }

        _mapper.Map(ResponseStatus.ReservationFetched, response);
        response.Result = _mapper.Map<ReservationDto>(reservation);
        return response;
    }

    public async Task<ApiResponse<ReservationDto?>> GetLastByBookAsync(int bookId)
    {
        ApiResponse<ReservationDto?> response = new();

        var bookExists = await _bookRepository.BookExistsAsync(bookId);
        if (!bookExists)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }

        var reservation = await _bookReservationRepository.GetLastByBookAsync(bookId);
        if (reservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotFound, response);
            return response;
        }

        _mapper.Map(ResponseStatus.ReservationFetched, response);
        response.Result = _mapper.Map<ReservationDto>(reservation);
        return response;
    }

    public async Task<ApiResponse<List<ReservationDto>>> GetReservationsAsync(PaginationRequest pagination)
    {
        ApiResponse<List<ReservationDto>> response = new();

        var reservationTotalCount = await _bookReservationRepository.CountAsync();

        response.Pagination = new Pagination
        {
            CurrentPage  = pagination.Page,
            ItemsPerPage = pagination.PageSize,
            TotalItems   = reservationTotalCount,
        };

        if (pagination.Page > response.Pagination.TotalPages)
        {
            _mapper.Map(ResponseStatus.ReservationEmptyPage, response);
            response.Result = [];
            return response;
        }
        var reservations = await _bookReservationRepository.GetReservationsAsync(pagination.Page, pagination.PageSize);

        _mapper.Map(ResponseStatus.ReservationFetchedMany, response);
        response.Result = _mapper.Map<List<ReservationDto>>(reservations);
        return response;
    }

    public async Task<ApiResponse<List<ReservationDto>>> GetReservationsByCustomerAsync(int customerId, PaginationRequest pagination)
    {
        ApiResponse<List<ReservationDto>> response = new();

        var customerExists = await _customerRepository.CustomerExistsAsync(customerId);
        if (!customerExists)
        {
            _mapper.Map(ResponseStatus.CustomerNotFound, response);
            return response;
        }
        int customerReservationTotalCount = await _bookReservationRepository.CountByCustomerAsync(customerId);

        response.Pagination = new Pagination
        {
            CurrentPage  = pagination.Page,
            ItemsPerPage = pagination.PageSize,
            TotalItems   = customerReservationTotalCount,
        };

        if (pagination.Page > response.Pagination.TotalPages)
        {
            _mapper.Map(ResponseStatus.ReservationEmptyPage, response);
            response.Result = [];
            return response;
        }
        var reservations = await _bookReservationRepository
            .GetReservationsByCustomerAsync(customerId, pagination.Page, pagination.PageSize);

        _mapper.Map(ResponseStatus.ReservationFetchedMany, response);
        response.Result = _mapper.Map<List<ReservationDto>>(reservations);
        return response;
    }

    public async Task<ApiResponse<List<ReservationDto>>> GetReservationsByBookAsync(int bookId, PaginationRequest pagination)
    {
        ApiResponse<List<ReservationDto>> response = new();

        var bookExists = await _bookRepository.BookExistsAsync(bookId);
        if (!bookExists)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }
        int bookReservationTotalCount = await _bookReservationRepository.CountByBookAsync(bookId);

        response.Pagination = new Pagination
        {
            CurrentPage  = pagination.Page,
            ItemsPerPage = pagination.PageSize,
            TotalItems   = bookReservationTotalCount,
        };

        if (pagination.Page > response.Pagination.TotalPages)
        {
            _mapper.Map(ResponseStatus.ReservationEmptyPage, response);
            response.Result = [];
            return response;
        }
        var reservations = _bookReservationRepository
            .GetReservationsByBookAsync(bookId, pagination.Page, pagination.PageSize);

        _mapper.Map(ResponseStatus.ReservationFetchedMany, response);
        response.Result = _mapper.Map<List<ReservationDto>>(reservations);
        return response;
    }

    public async Task<ApiResponse<ReservationDto?>> CreateAsync(ReservationCreateDto dto)
    {
        ApiResponse<ReservationDto?> response = new();

        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customer is null)
        {
            _mapper.Map(ResponseStatus.CustomerNotFound, response);
            return response;
        }

        var book = await _bookRepository.GetByIdAsync(dto.BookId);
        if (book is null)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }

        var lastCustomerReservation = await _bookReservationRepository.GetLastByCustomerAsync(dto.CustomerId);
        if (lastCustomerReservation?.ReturnedDate is not null)
        {
            _mapper.Map(ResponseStatus.OpenCustomerReservation, response);
            return response;
        }
        
        var lastBookReservation = await _bookReservationRepository.GetLastByBookAsync(dto.BookId);
        if (lastBookReservation is not null && lastBookReservation.ReturnedDate is null)
        {
            _mapper.Map(ResponseStatus.OpenBookReservation, response);
            return response;
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var reservation = new Models.Reservation
        {
            Customer    = customer,
            Book        = book,
            CreatedDate = today,
            EndDate     = today.AddDays(_expirationDays),
        };

        var createdReservation = await _bookReservationRepository.CreateAsync(reservation);
        if (createdReservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotCreated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.ReservationCreated, response);
        response.Result = _mapper.Map<ReservationDto>(createdReservation);
        return response;
    }

    public async Task<ApiResponse<ReservationDto?>> CompleteReservationAsync(int id, ReservationCompleteDto dto)
    {
        ApiResponse<ReservationDto?> response = new();

        var reservation = await _bookReservationRepository.GetByIdAsync(id);
        if (reservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotFound, response);
            return response;
        }

        if (reservation.ReturnedDate is not null)
        {
            _mapper.Map(ResponseStatus.ReservationAlreadyCompleted, response);
            return response;
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        if (dto.ReturnedDate is not null)
        {
            if (dto.ReturnedDate > today)
            {
                _mapper.Map(ResponseStatus.ReservationCompleteLaterThanToday, response);
                return response;
            }
            reservation.ReturnedDate = dto.ReturnedDate;
        }
        else
        {
            reservation.ReturnedDate = today;
        }

        var updatedReservation = await _bookReservationRepository.UpdateAsync(reservation);
        if (updatedReservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotCompleted, response);
            return response;
        }
        var successResponseStatus = updatedReservation.ReturnedDate <= updatedReservation.EndDate
            ? ResponseStatus.ReservationCompleted
            : ResponseStatus.ReservationCompletedLate;

        _mapper.Map(successResponseStatus, response);
        response.Result = _mapper.Map<ReservationDto>(updatedReservation);
        return response;
    }
}
