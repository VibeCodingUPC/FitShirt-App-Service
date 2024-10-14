using FitShirt.Domain.Security.Models.Aggregates;
using FitShirt.Domain.Security.Models.ValueObjects;
using FitShirt.Domain.Shared.Models.Entities;

namespace FitShirt.Domain.Security.Models.Entities;

public class Role : BaseModel
{
    public UserRoles Name { get; set; }
    public ICollection<User> Users { get; set; }

    public Role()
    {

    }

    public Role(UserRoles userRole)
    {
        Name = userRole;
    }

    public string GetStringName()
    {
        return Name.ToString();
    }

    public static Role ToRoleFromName(string name) 
    { 
        if (Enum.TryParse(typeof(UserRoles), name, true, out var result))
        {
            return new Role((UserRoles)result);
        }

        throw new ArgumentException($"Invalid role name: {name}");
    }
}