using LibrarySystem.Enums;

namespace LibrarySystem.DTOs.Response;

public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public T? Result { get; set; }
    public Pagination? Pagination { get; set; }
    public Dictionary<string, List<string>>? Errors { get; set; }
    public ResponseCode? Code { get; set; }
}
