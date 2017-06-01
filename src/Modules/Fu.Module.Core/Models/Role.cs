using Fu.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Fu.Module.Core.Models
{
    public class Role : IdentityRole<long, UserRole, IdentityRoleClaim<long>>, IEntityWithTypedId<long>
    {
    }
}