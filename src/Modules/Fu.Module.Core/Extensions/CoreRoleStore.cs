using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Fu.Module.Core.Data;
using Fu.Module.Core.Models;


namespace Fu.Module.Core.Extensions
{
    public class CoreRoleStore : RoleStore<Role, MainDbContext, long, UserRole, IdentityRoleClaim<long>>
    {
        public CoreRoleStore(MainDbContext context) : base(context)
        {

        }

        protected override IdentityRoleClaim<long> CreateRoleClaim(Role role, Claim claim)
        {
            return new IdentityRoleClaim<long> { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
        }
        
    }
}
