using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace Walruslogics.Advertisement.Database.Models
{
    [Table("RoleClaims")]
    public partial class AppRoleClaim : IdentityRoleClaim<long>
    {
    }
    
    public partial class AppUserClaim : IdentityUserClaim<long>
    {
    }
    
    [Table("UserLogins")]
    public partial class AppUserLogin : IdentityUserLogin<long>
    {
       
    }
    
    [Table("UserRoles")]
    public partial class AppUserRole : IdentityUserRole<long>
    {
      
    }
    [Table("UserTokens")]
    public partial class AppUserToken : IdentityUserToken<long>
    {
       
    }
}
