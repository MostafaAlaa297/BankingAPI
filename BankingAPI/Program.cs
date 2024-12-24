using BankingAPI.Data;
using BankingAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
IServiceCollection serviceCollection = builder.Services.AddDbContext<BankingContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddScoped<AccountService>();

builder.Services.AddCors(options => { options.AddDefaultPolicy(builder => { builder.WithOrigins("https://localhost:7280").AllowAnyHeader().AllowAnyMethod(); }); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
