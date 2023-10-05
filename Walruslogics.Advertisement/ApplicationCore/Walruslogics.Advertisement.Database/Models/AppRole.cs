using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walruslogics.Advertisement.Database.Models
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole<long>
    {   public AppRole() : base()
        {
        }
            public AppRole(string roleName): base(roleName)
        {
            Name = roleName;
        }
        
        [MaxLength(20)]
        public string? RoleKey { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string? Description { get; set; }
    }
}
