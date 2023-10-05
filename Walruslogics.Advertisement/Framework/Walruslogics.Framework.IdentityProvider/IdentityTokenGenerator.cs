using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Walruslogics.Advertisement.Framework;

namespace Walruslogics.Framework.IdentityProvider
{
    public class IdentityTokenGenerator
    {
        #region Private-Fields
        private AppConfiguration _appConfiguration;
        public IConfiguration _configuration;
        #endregion

        #region Constructors
        public IdentityTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region Public-Methods

        public IdentityToken GenerateToken(IdentityObject identityObject)
        {
            IdentityToken identityToken = new IdentityToken();

            // 1. Get Symmetric Security key in bytes 
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["IdentityTokenDescriptor:Secret"]));

            // 2. Get Claims Role Claims, UserId and User email.
            List<Claim> claims = GetClaims(identityObject);

            // 3. Generate Security token
            var securityToken = new JwtSecurityToken(
                issuer: _configuration["IdentityTokenDescriptor:Issuer"], //_appConfiguration.IdentityToken.Issuer,
                audience: _configuration["IdentityTokenDescriptor:Audience"],// _appConfiguration.IdentityToken.Audience,
                claims: claims,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(30)), //_appConfiguration.IdentityToken.Expires
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );

            // 4. Write Security token
            identityToken.Token = string.Concat("Bearer ", new JwtSecurityTokenHandler().WriteToken(securityToken));

            return identityToken;
        }

        #endregion

        private List<Claim> GetClaims(IdentityObject identityObject)
        {
            var claims = new List<Claim>();

            claims.Add(GetRoleClaim(identityObject));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, Convert.ToString(identityObject.UserId), ClaimValueTypes.Integer64));
            claims.Add(new Claim(ClaimTypes.Email, Convert.ToString(identityObject.Email), ClaimValueTypes.String));

            return claims;
        }

        private Claim GetRoleClaim(IdentityObject identityObject)
        {
            Claim roleClaim;

            if (identityObject.RoleId == (long)UserRoles.SuperAdmin)
            {
                roleClaim = new Claim(ClaimTypes.Role, UserRoles.SuperAdmin.ToString());
            }
            else if (identityObject.RoleId == (long)UserRoles.Admin)
            {
                roleClaim = new Claim(ClaimTypes.Role, UserRoles.Admin.ToString());
            }
            else // Default User Role
            {
                roleClaim = new Claim(ClaimTypes.Role, UserRoles.User.ToString());
            }

            return roleClaim;
        }
    }
}
