using Application.Features.IngestWeapon;
using Domain.Abstractions;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddScoped<IWeaponRepository, WeaponRepository>();
builder.Services.AddScoped<IngestWeaponCommandHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseAuthorization();
app.MapControllers();
app.Run();