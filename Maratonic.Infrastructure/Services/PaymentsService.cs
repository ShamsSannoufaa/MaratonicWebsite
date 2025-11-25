using System;

using Maratonic.Core.Interfaces;

namespace Maratonic.Infrastructure.Services;

public class PaymentsService : IPaymentsService
{
    private readonly AppDbContext _context;

    public PaymentsService(AppDbContext context)
    {
        _context = context;
    }

    // Şimdilik boş
}

