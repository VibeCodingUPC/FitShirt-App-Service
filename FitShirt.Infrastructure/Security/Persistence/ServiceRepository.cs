using FitShirt.Domain.Security.Models.Entities;
using FitShirt.Domain.Security.Repositories;
using FitShirt.Infrastructure.Shared.Contexts;
using FitShirt.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FitShirt.Infrastructure.Security.Persistence;

public class ServiceRepository : BaseRepository<Service>, IServiceRepository
{
    public ServiceRepository(FitShirtDbContext context) : base(context)
    {
    }

    public async Task<Service?> GetFreeServiceAsync()
    {
        return await _context.Services.FirstOrDefaultAsync(service => service.Name == "Free");
    }

    public async Task<Service?> GetPremiumServiceAsync()
    {
        return await _context.Services.FirstOrDefaultAsync(service => service.Name == "Premium");
    }
}