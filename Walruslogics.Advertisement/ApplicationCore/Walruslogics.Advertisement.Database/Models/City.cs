using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.Database.Models
{
  [Table("City")]

  public class City
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public long StateId { get; set; }
    public string StateCode { get; set; }
    public long CountryId { get; set; }
    public string CountryCode { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public DateTime CreatedDt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Flag { get; set; }
    public string WikiDataId { get; set; }
  }
}
