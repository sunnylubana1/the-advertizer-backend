using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walruslogics.Advertisement.Database.Models
{
    [Table("UserStatus", Schema = "User")]
    public class UserStatus
    {
        public UserStatus()
        {
           
           
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string? Description { get; set; }
        
        [MaxLength(20)]
        public string StatusKey { get; set; } = null!;
    }
}
