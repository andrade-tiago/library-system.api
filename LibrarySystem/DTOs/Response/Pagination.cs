﻿namespace LibrarySystem.DTOs.Response;

public class Pagination
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
}
