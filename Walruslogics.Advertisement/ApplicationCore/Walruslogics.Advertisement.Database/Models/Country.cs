using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.Database.Models
{
  [Table("Country")]
  public class Country
  {
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(3)]
    public string? ISO3 { get; set; }
    [MaxLength(3)]
    public string? NumericCode { get; set; }
    [MaxLength(2)]
    public string? ISO2 { get; set; }
    [MaxLength(255)]
    public string? PhoneCode { get; set; }
    [MaxLength(255)]
    public string? Capital { get; set; }
    [MaxLength(100)]
    public string? Currency { get; set; }
    public string? CurrencyName { get; set; }
    public string? CurrencySymbol { get; set; }
    public string? Tld { get; set; }
    public string? Native { get; set; }
    public string? Region { get; set; }
    public string? Subregion { get; set; }
    public string? TimeZones { get; set; }
    public string? Translations { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? Eemoji { get; set; }
    public string? EmojiU { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Flag { get; set; }
    public string? WikiDataId { get; set; }
    public bool IsActive { get; set; }
  }
}
