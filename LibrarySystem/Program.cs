using DotNetEnv;
using LibrarySystem.Data;
using LibrarySystem.Filters;
using LibrarySystem.Middlewares;
using LibrarySystem.Repositories.Author;
using LibrarySystem.Repositories.Book;
using LibrarySystem.Services.Author;
using LibrarySystem.Services.Book;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();

if (builder.Environment.IsDevelopment())
{
    Env.Load();
}

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão não foi encontrada.");
}

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(connectionString)
);

var app = builder.Build();

app.UseMiddleware<ValidationMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
