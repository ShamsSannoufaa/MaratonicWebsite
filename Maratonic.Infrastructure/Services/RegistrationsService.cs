using Maratonic.Core.Interfaces;
using Maratonic.Core.Entities;
using Maratonic.Infrastructure;

namespace Maratonic.Infrastructure.Services
{
    public class RegistrationsService : IRegistrationsService
    {
        private readonly AppDbContext _context;

        public RegistrationsService(AppDbContext context)
        {
            _context = context;
        }

        // Hata CS0535'i çözer: RegisterAsync'i buraya ekliyoruz
        public Task<Registration> RegisterAsync(string userId, int raceId) => throw new NotImplementedException();
        public Task<List<Registration>> GetUserRegistrationsAsync(string userId) => throw new NotImplementedException();
        // ... diğer metotlar
    }
}