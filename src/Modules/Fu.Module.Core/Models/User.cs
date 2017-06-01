using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Fu.Infrastructure.Models;

namespace Fu.Module.Core.Models
{
    public class User : IdentityUser<long, IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>>, IEntityWithTypedId<long>
    {
        public User()
        {
            CreatedOn = DateTimeOffset.Now;
            UpdatedOn = DateTimeOffset.Now;
        }

        public Guid UserGuid { get; set; }

        public string FullName { get; set; }
           
        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

    }
}
