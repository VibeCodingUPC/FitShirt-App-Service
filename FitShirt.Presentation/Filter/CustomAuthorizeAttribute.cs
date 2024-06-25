using FitShirt.Domain.Security.Models.Aggregates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FitShirt.Presentation.Filter;

public class CustomAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string[] _roles;

    public CustomAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }
    
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"] as User;

        if (_roles.Any() && !_roles.Contains(user!.Role.Name))
        {
            context.Result = new ForbidResult();
        }
    }
}