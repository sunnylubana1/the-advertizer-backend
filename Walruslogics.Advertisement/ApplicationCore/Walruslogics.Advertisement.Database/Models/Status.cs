using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walruslogics.Advertisement.Database.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string? StatusKey { get; set; }
        public string StatusName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
