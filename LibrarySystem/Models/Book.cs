﻿namespace LibrarySystem.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<Author> Authors { get; set; }
}
