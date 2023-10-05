using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walruslogics.Advertisement.Database.Models;
using Walruslogics.Advertisement.DTOs;

namespace Walruslogics.Advertisement.BusinessLogic
{
    public interface IIdentityBusinessLogic
    {
        WalruslogicResponseObject Login(AppUser user, string roleName, AppUser appUser);
    }
}
