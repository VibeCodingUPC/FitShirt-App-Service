using FitShirt.Domain.Designing.Models.Aggregates;
using FitShirt.Domain.Designing.Models.Commands;
using FitShirt.Domain.Designing.Models.Responses;

namespace FitShirt.Domain.Designing.Services;

public interface IDesignCommandService
{
    Task<DesignResponse> Handle(CreateDesignCommand command);
    Task<DesignResponse> Handle(int id, UpdateDesignCommand command);
    Task<bool> Handle(DeleteDesignCommand command);
}