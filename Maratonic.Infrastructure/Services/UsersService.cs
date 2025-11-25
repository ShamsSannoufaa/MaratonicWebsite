using System;
using Maratonic.Core.Interfaces;

namespace Maratonic.Infrastructure.Services;

public class UsersService : IUsersService
{
    private readonly AppDbContext _context;

    public UsersService(AppDbContext context)
    {
        _context = context;
    }

    // Şimdilik boş
}
