using OnDemandCarWash.Context;
using Microsoft.EntityFrameworkCore;
using OnDemandCarWash.Repositories;
using OnDemandCarWash.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<EmailService,EmailService>();
builder.Services.AddScoped<IWasherRepository, WasherRepository>();
builder.Services.AddScoped<WasherService, WasherService>();

builder.Services.AddDbContext<CarWashDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("Default")));

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
