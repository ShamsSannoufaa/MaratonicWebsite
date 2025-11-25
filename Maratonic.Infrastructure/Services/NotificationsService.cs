using System;
using Maratonic.Core.Interfaces;

namespace Maratonic.Infrastructure.Services;

public class NotificationsService : INotificationsService
{
    private readonly AppDbContext _context;

    public NotificationsService(AppDbContext context)
    {
        _context = context;
    }

    // Şimdilik boş
}
