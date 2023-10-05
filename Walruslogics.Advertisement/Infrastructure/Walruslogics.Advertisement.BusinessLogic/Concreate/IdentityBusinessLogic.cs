using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walruslogics.Advertisement.Database.Models;
using Walruslogics.Advertisement.DTOs;
using Walruslogics.Advertisement.Framework;
using Walruslogics.Framework.IdentityProvider;
using Walruslogics.Identity.DTO;

namespace Walruslogics.Advertisement.BusinessLogic
{
    public class IdentityBusinessLogic : IIdentityBusinessLogic
    {
        public IConfiguration _configuration { get; }
        private WalruslogicResponseObject _responseObject;

        public IdentityBusinessLogic(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public WalruslogicResponseObject ConfirmationEmail(string email, string userName, string code)
        {
            throw new NotImplementedException();
        }

        public WalruslogicResponseObject Login(AppUser user, string roleName, AppUser appUser)
        {
            Enum.TryParse(roleName, out UserRoles userRole);
            
            // Generate JWT-Token and Process Login Steps
            IdentityTokenGenerator identityTokenGenerator = new IdentityTokenGenerator(_configuration);

            var token = identityTokenGenerator.GenerateToken(new IdentityObject()
            {
                RoleId = (long)userRole,
                Email = user.Email,
                UserId = user.Id
            });

            token.User = user;

            _responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Success.ToString(), "You have been successfully registered", token);

            return _responseObject;
        }

        public WalruslogicResponseObject Registration(RegistrationModel registrationModel)
        {
            throw new NotImplementedException();
        }
    }
}
