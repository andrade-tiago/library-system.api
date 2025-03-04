using AutoMapper;
using LibrarySystem.DTOs.BookReservation;
using LibrarySystem.Models;

namespace LibrarySystem.Mappers;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<BookReservation, BookReservationDto>();
    }
}
