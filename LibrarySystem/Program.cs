using DotNetEnv;
using LibrarySystem.Constants;
using LibrarySystem.Data;
using LibrarySystem.Filters;
using LibrarySystem.Mappers;
using LibrarySystem.Repositories.Author;
using LibrarySystem.Repositories.Book;
using LibrarySystem.Repositories.Customer;
using LibrarySystem.Repositories.Reservation;
using LibrarySystem.Services.Author;
using LibrarySystem.Services.Book;
using LibrarySystem.Services.Customer;
using LibrarySystem.Services.Reservation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelStateFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddAutoMapper(typeof(AuthorProfile));
builder.Services.AddAutoMapper(typeof(BookProfile));
builder.Services.AddAutoMapper(typeof(CustomerProfile));
builder.Services.AddAutoMapper(typeof(ReservationProfile));
builder.Services.AddAutoMapper(typeof(ApiResponseProfile));

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

builder.Services.Configure<ReservationSettings>(
    builder.Configuration.GetSection("ReservationSettings")
);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
