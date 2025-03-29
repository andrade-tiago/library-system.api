using AutoMapper;
using LibrarySystem.Constants;
using LibrarySystem.DTOs.Book;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Reservation;
using LibrarySystem.DTOs.Response;
using LibrarySystem.Repositories.Book;
using LibrarySystem.Repositories.Customer;
using LibrarySystem.Repositories.Reservation;
using Microsoft.Extensions.Options;

namespace LibrarySystem.Services.Reservation;

public class ReservationService(
    IReservationRepository reservationRepository,
    IBookRepository bookRepository,
    ICustomerRepository customerRepository,
    IMapper mapper,
    IOptions<ReservationSettings> options
) : IReservationService
{
    private readonly IReservationRepository _reservationRepository = reservationRepository;
    private readonly IBookRepository _bookRepository = bookRepository;
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IMapper _mapper = mapper;
    private readonly int _expirationDays = options.Value.ExpirationDays;

    public async Task<ApiResponse<ReservationDto?>> GetByIdAsync(int id, ReservationQueryOptions queryOptions)
    {
        ApiResponse<ReservationDto?> response = new();

        var bookReservation = await _reservationRepository.GetByIdAsync(id, queryOptions);

        if (bookReservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotFound, response);
            return response;
        }
        _mapper.Map(ResponseStatus.ReservationFetched, response);
        response.Result = _mapper.Map<ReservationDto>(bookReservation);
        return response;
    }

    public async Task<ApiResponse<ReservationDto?>> GetLastByCustomerAsync(int customerId, ReservationQueryOptions queryOptions)
    {
        ApiResponse<ReservationDto?> response = new();

        var customerExists = await _customerRepository.CustomerExistsAsync(customerId);
        if (!customerExists)
        {
            _mapper.Map(ResponseStatus.CustomerNotFound, response);
            return response;
        }

        var reservation = await _reservationRepository.GetLastByCustomerAsync(customerId, queryOptions);
        if (reservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotFound, response);
            return response;
        }

        _mapper.Map(ResponseStatus.ReservationFetched, response);
        response.Result = _mapper.Map<ReservationDto>(reservation);
        return response;
    }

    public async Task<ApiResponse<ReservationDto?>> GetLastByBookAsync(int bookId, ReservationQueryOptions queryOptions)
    {
        ApiResponse<ReservationDto?> response = new();

        var bookExists = await _bookRepository.BookExistsAsync(bookId);
        if (!bookExists)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }

        var reservation = await _reservationRepository.GetLastByBookAsync(bookId, queryOptions);
        if (reservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotFound, response);
            return response;
        }

        _mapper.Map(ResponseStatus.ReservationFetched, response);
        response.Result = _mapper.Map<ReservationDto>(reservation);
        return response;
    }

    public async Task<ApiResponse<List<ReservationDto>>> GetAllPagedAsync(ReservationQueryOptions queryOptions, PaginationOptions paginationOptions)
    {
        ApiResponse<List<ReservationDto>> response = new();

        var reservationTotalCount = await _reservationRepository.CountAsync();

        response.Pagination = new Pagination
        {
            CurrentPage  = paginationOptions.Page,
            ItemsPerPage = paginationOptions.PageSize,
            TotalItems   = reservationTotalCount,
        };

        if (paginationOptions.Page > response.Pagination.TotalPages)
        {
            _mapper.Map(ResponseStatus.ReservationEmptyPage, response);
            response.Result = [];
            return response;
        }
        var reservations = await _reservationRepository.GetAllPagedAsync(queryOptions, paginationOptions);

        _mapper.Map(ResponseStatus.ReservationFetchedMany, response);
        response.Result = _mapper.Map<List<ReservationDto>>(reservations);
        return response;
    }

    public async Task<ApiResponse<List<ReservationDto>>> GetByCustomerPagedAsync(int customerId, ReservationQueryOptions queryOptions, PaginationOptions paginationOptions)
    {
        ApiResponse<List<ReservationDto>> response = new();

        var customerExists = await _customerRepository.CustomerExistsAsync(customerId);
        if (!customerExists)
        {
            _mapper.Map(ResponseStatus.CustomerNotFound, response);
            return response;
        }
        int customerReservationTotalCount = await _reservationRepository.CountByCustomerIdAsync(customerId);

        response.Pagination = new Pagination
        {
            CurrentPage  = paginationOptions.Page,
            ItemsPerPage = paginationOptions.PageSize,
            TotalItems   = customerReservationTotalCount,
        };

        if (response.Pagination.CurrentPage > response.Pagination.TotalPages)
        {
            _mapper.Map(ResponseStatus.ReservationEmptyPage, response);
            response.Result = [];
            return response;
        }
        var reservations = await _reservationRepository
            .GetByCustomerPagedAsync(customerId, queryOptions, paginationOptions);

        _mapper.Map(ResponseStatus.ReservationFetchedMany, response);
        response.Result = _mapper.Map<List<ReservationDto>>(reservations);
        return response;
    }

    public async Task<ApiResponse<List<ReservationDto>>> GetByBookPagedAsync(int bookId, ReservationQueryOptions queryOptions, PaginationOptions paginationOptions)
    {
        ApiResponse<List<ReservationDto>> response = new();

        var bookExists = await _bookRepository.BookExistsAsync(bookId);
        if (!bookExists)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }
        int bookReservationTotalCount = await _reservationRepository.CountByBookIdAsync(bookId);

        response.Pagination = new Pagination
        {
            CurrentPage  = paginationOptions.Page,
            ItemsPerPage = paginationOptions.PageSize,
            TotalItems   = bookReservationTotalCount,
        };

        if (response.Pagination.CurrentPage > response.Pagination.TotalPages)
        {
            _mapper.Map(ResponseStatus.ReservationEmptyPage, response);
            response.Result = [];
            return response;
        }
        var reservations = await _reservationRepository
            .GetByBookPagedAsync(bookId, queryOptions, paginationOptions);

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
        if (customer.DeletedAt.HasValue)
        {
            _mapper.Map(ResponseStatus.CustomerUnavailable, response);
            return response;
        }

        var book = await _bookRepository.GetByIdAsync(dto.BookId, new BookGetByIdOptions { IncludeAuthors = false });
        if (book is null)
        {
            _mapper.Map(ResponseStatus.BookNotFound, response);
            return response;
        }
        if (!book.IsAvailable)
        {
            _mapper.Map(ResponseStatus.BookNotAvailable, response);
            return response;
        }

        var lastCustomerReservation = await _reservationRepository.GetLastByCustomerAsync(dto.CustomerId, ReservationQueryOptions.Default);
        if (lastCustomerReservation?.ReturnedDate is not null)
        {
            _mapper.Map(ResponseStatus.OpenCustomerReservation, response);
            return response;
        }
        
        var lastBookReservation = await _reservationRepository.GetLastByBookAsync(dto.BookId, ReservationQueryOptions.Default);
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

        var createdReservation = await _reservationRepository.CreateAsync(reservation);
        if (createdReservation is null)
        {
            _mapper.Map(ResponseStatus.ReservationNotCreated, response);
            return response;
        }
        _mapper.Map(ResponseStatus.ReservationCreated, response);
        response.Result = _mapper.Map<ReservationDto>(createdReservation);
        return response;
    }

    public async Task<ApiResponse<ReservationDto?>> CloseReservationAsync(int id, ReservationCompleteDto dto)
    {
        ApiResponse<ReservationDto?> response = new();

        var reservation = await _reservationRepository.GetByIdAsync(id, ReservationQueryOptions.Default);
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

        var updatedReservation = await _reservationRepository.UpdateAsync(reservation);
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
