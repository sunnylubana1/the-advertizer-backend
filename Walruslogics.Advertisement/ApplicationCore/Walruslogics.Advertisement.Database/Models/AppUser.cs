using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.Database.Models
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<long>
    {
        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [Column(TypeName = "char(1)")]
        public string LoginProvider { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        // flag to disable user and prevent more tokens from being issued
        public bool IsActive { get; set; } = true;
        // datetimes for when user was created, last time they logged in,
        // last time they changed their email, and last time password was changed
        public DateTime Created { get; set; }
        public DateTime EmailChanged { get; set; }
        public DateTime? PasswordChanged { get; set; }
        public DateTime? LastLogin { get; set; }

        // place to store the new email given that new email confiirmation 
        // needs to round-trip the value. this avoids sending it to the end user.
        [MaxLength(256)]
        public string? NewEmail { get; set; }
    }
}
