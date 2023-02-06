using Application.Bookings;
using Application.Bookings.Ports;
using Application.Guests;
using Application.Guests.Ports;
using Application.Payment.Ports;
using Application.Rooms;
using Application.Rooms.Ports;
using Data;
using Data.Bookings;
using Data.Guests;
using Data.Rooms;
using Domain.Bookings.Ports;
using Domain.Guests.Ports;
using Domain.Rooms.Ports;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentApplication;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(BookingManager));

#region IoC
builder.Services.AddScoped<IGuestManager, GuestManager>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IRoomManager, RoomManager>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingManager, BookingManager>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IPaymentProcessorFactory, PaymentProcessorFactory>();

#endregion


#region DB writing up
var connectionString = builder.Configuration.GetConnectionString("Main");
builder.Services.AddDbContext<HotelDbContext>(
    options => options.UseSqlServer(connectionString));
# endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews()
                .AddJsonOptions(opt =>
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
