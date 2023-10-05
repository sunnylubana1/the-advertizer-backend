using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.DTOs
{
    public class WalruslogicResponseObject
    {
        public WalruslogicResponseObject()
        {
            Result = new ResponseInfo();
        }
        public ResponseInfo  Result {get; set;}
        public object Data { get; set;}
    }
}
