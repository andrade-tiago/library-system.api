using AutoMapper;
using LibrarySystem.Constants;
using LibrarySystem.DTOs.Response;
namespace LibrarySystem.Mappers;

public class ApiResponseProfile : Profile
{
    public ApiResponseProfile()
    {
        CreateMap(typeof(ResponseStatus), typeof(ApiResponse<>))
            .ForMember("Result",     opt => opt.Ignore())
            .ForMember("Errors",     opt => opt.Ignore())
            .ForMember("Pagination", opt => opt.Ignore());
    }
}
