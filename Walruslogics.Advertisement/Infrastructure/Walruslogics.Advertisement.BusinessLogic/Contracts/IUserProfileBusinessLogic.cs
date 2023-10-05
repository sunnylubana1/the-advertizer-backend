using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walruslogics.Advertisement.DTOs;

namespace Walruslogics.Advertisement.BusinessLogic
{
    public interface IUserProfileBusinessLogic
    {
        WalruslogicResponseObject AddUserProfile(UserProfileDTO userProfileDTO);

        bool CreateUserProfile(RegistrationModel registrationModel, string profilePicture);

        WalruslogicResponseObject GetUserProfile(string email);
        WalruslogicResponseObject CountryDropdown();

  }
}
