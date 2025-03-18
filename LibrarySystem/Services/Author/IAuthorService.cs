using LibrarySystem.DTOs.Author;
using LibrarySystem.DTOs.Request;
using LibrarySystem.DTOs.Response;

namespace LibrarySystem.Services.Author;

public interface IAuthorService
{
    Task<ApiResponse<AuthorDto?>> GetByIdAsync(int id);
    Task<ApiResponse<List<AuthorDto>?>> GetByBookIdAsync(int bookId);
    Task<ApiResponse<List<AuthorDto>>> GetAllPagedAsync(PaginationRequest pagination);
    Task<ApiResponse<AuthorDto>> CreateAsync(AuthorCreateDto createDto);
    Task<ApiResponse<AuthorDto?>> UpdateAsync(int id, AuthorUpdateDto updateDto);
}
