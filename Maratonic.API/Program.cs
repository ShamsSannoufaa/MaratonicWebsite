using Microsoft.EntityFrameworkCore; // AddDbContext için
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; // UseMySql ve ServerVersion için

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// 1. Connection String'i okuma
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext'i servislere ekleme
builder.Services.AddDbContext<Maratonic.Infrastructure.AppDbContext>(options =>
    options.UseMySql(connectionString,
        // MySQL sunucu sürümünüzü belirtin. (Genellikle 8.0)
        ServerVersion.AutoDetect(connectionString)
    )
);

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
