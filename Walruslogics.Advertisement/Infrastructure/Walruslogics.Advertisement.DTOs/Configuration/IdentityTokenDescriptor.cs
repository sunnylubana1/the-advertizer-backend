using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Identity.DTO
{
    public class IdentityTokenDescriptor
    {
        #region Properties

        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public Double Expires { get; set; }

        #endregion
    }
}
