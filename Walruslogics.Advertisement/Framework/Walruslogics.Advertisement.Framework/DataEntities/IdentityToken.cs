using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walruslogics.Advertisement.Database.Models;

namespace Walruslogics.Advertisement.Framework
{
    public class IdentityToken
    {
        #region Properties

        public string Token { get; set; }
        public AppUser User { get; set; }

        #endregion
    }
}
