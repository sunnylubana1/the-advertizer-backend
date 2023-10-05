using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Walruslogics.Advertisement.DTOs
{
  public class UserProfileDTO
  {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string Email { get; set; }
    public string? Address { get; set; }
    public string? Address2 { get; set; }
    public string CountryId { get; set; }
    public string CityId { get; set; }
    public string? ImagePath { get; set; }
    public string? PinCode { get; set; }
    public IFormFile Image { get; set; }
  }
}
