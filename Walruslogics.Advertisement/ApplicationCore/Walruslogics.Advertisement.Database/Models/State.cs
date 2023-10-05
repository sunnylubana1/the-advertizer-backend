using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.Database.Models
{
  [Table("State")]
  public class State
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public long CountryId { get; set; }
    public string CountryCode { get; set; }
    public string FipsCode { get; set; }
    public string ISO2 { get; set; }
    public string Type { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime CreatedDt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Flag { get; set; }
    public string WikiDataId { get; set; }
  }
}
