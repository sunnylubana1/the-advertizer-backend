using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.Database.Models
{
  public class Ad : Base
  {
    [MaxLength(100)]
    public string Title { get; set; }

    [ForeignKey("User")]
    public long UserId { get; set; }

    [ForeignKey("AdCategory")]
    public int AdCategoryId { get; set; }

    [MaxLength(1)]
    public int AddPackageId { get; set; }
    public int CountryId { get; set; }
    public long CityId { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
    [MaxLength(20)]
    public string? Coupon { get; set; }

    [MaxLength(50)]
    public string FileName { get; set; }
    [MaxLength(500)]
    public string ImagePath { get; set; }
    [MaxLength(200)]
    public string? Link { get; set; }
    public bool IsActive { get; set; } = true;
    
    public virtual AppUser User { get; set; }
    public virtual AdCategory AdCategory { get; set; }

  }
}
