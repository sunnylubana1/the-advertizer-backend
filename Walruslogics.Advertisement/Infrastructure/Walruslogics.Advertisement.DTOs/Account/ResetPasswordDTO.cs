using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.DTOs
{
  public class ResetPasswordDTO
  {
    public string Token { get; set; }
    public string UserId { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

  }
}
