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
        throw new NotImplementedException();
    }

    public async Task<Service?> GetPremiumServiceAsync()
    {
        throw new NotImplementedException();
    }
}