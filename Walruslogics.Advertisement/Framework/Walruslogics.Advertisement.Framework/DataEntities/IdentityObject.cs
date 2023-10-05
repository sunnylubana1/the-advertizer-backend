using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.Framework
{
    public class IdentityObject
    {
        #region Properties

        public long UserId { get; set; }

        public long RoleId { get; set; }

        public string Email { get; set; }

        public DateTime ExpiryDate { get; set; }

        #endregion
    }
}
