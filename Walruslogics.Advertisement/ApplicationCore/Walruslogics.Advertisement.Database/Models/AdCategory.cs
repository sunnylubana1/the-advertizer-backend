using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.Database.Models
{
  [Table("AdCategory")]
  public class AdCategory
  {
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Ad> Ads { get; set; }
  }
}
