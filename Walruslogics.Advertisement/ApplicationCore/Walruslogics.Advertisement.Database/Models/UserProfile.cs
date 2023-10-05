using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walruslogics.Advertisement.Database.Models
{
  [Table("UserProfile")]
  public class UserProfile : Base.Base
  {
    [MaxLength(20)]
    public string FirstName { get; set; }

    [MaxLength(20)]
    public string LastName { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(50)]
    public string Email { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [MaxLength(500)]
    public string? Address2 { get; set; }
    public Nullable<int> CountryId { get; set; }
    public Nullable<long> StateId { get; set; }
    public Nullable<long> CityId { get; set; }

    [MaxLength(10)]
    public string? PinCode { get; set; }

    [MaxLength(50)]
    public string? CityName { get; set; }

    [MaxLength(500)]
    public string? ImagePath { get; set; }

    public DateTime? LastPasswordChangeDate { get; set; }
    public string? PassowrdResetToken { get; set; }
    public DateTime? TokenExpiryDate { get; set; }
    public DateTime? LastForgotPasswordDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsExternalLogin { get; set; } = false;
  }
}
