using AutoMapper;
using LibrarySystem.DTOs.Reservation;
using LibrarySystem.Models;

namespace LibrarySystem.Mappers;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, ReservationDto>();
    }
}
