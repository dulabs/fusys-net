using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Fu.Module.Core.ViewModels.Users
{
    public class DetailsUserViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Access Failed Count")]
        public int AccessFailedCount { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        public Guid UserGuid { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name ="Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; }

        [Display(Name = "Lockout End")]
        public DateTimeOffset LockoutEnd { get; set; }

        [Display(Name = "Is Deleted?")]
        public bool IsDeleted { get; set; }

        [Display(Name ="Role")]
        public long RoleId { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Display(Name = "Two Factor Authentication Enabled")]
        public bool TwoFactorEnabled { get; set; }

        public string SecurityStamp { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}
