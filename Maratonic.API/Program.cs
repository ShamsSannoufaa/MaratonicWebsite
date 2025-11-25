// Gerekli using'ler: Tüm derleme hatalarýný (CS0234, CS1061) çözer
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Maratonic.Infrastructure; // AppDbContext'inizin olduðu namespace
using Maratonic.Core.Interfaces; // Sizin Servis Arayüzlerinizin olduðu namespace
using Maratonic.Infrastructure.Services; // Servislerinizin olduðu namespace (örnek)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Core Servisleri ve Dependency Injection
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connection String'i okuma
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext ve MySQL Baðlantýsý (Migrations Sorununu Çözen Ayarla)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        // SABAHTAN BERÝ SORUN ÇIKARAN AYAR:
        // EF Core'a Migrations dosyalarýný Infrastructure projesinde aramasýný söyler.
        mySqlOptions => mySqlOptions.MigrationsAssembly("Maratonic.Infrastructure")
    )
);

// 3. Identity (Kullanýcý Yönetimi) Servisini Ekleme (Arkadaþýnýzýn Ýþi Ýçin)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();

// 4. Servis/Repository Kayýtlarý (Sizin CRUD Operasyonlarýnýz Ýçin)
// Bu, Controller'larýn Repository'leri kullanmasýný saðlar.
builder.Services.AddScoped<IRacesService, RacesService>();
builder.Services.AddScoped<IRegistrationsService, RegistrationsService>();
builder.Services.AddScoped<IUsersService, UsersService>();  
builder.Services.AddScoped<IPaymentsService, PaymentsService>();
builder.Services.AddScoped<INotificationsService, NotificationsService>();


// 5. CORS (Angular'ýn API'a Eriþimi Ýçin)
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AngularPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS, Authentication ve Authorization Middleware'lerini ekle
app.UseCors("AngularPolicy");
app.UseAuthentication(); // Önce Auth çalýþmalý
app.UseAuthorization(); // Sonra Yetkilendirme çalýþmalý

app.MapControllers();

app.Run();