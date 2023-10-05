using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walruslogics.Advertisement.Framework;

namespace Walruslogics.Advertisement.DTOs
{
    public class ResponseInfo
    {
        public ResponseInfo()
        {
            Messages = new List<string>();
            Code = ResultCode.Failure.ToString();
        }

        /// <summary>
        /// Gets or sets the Status Code.
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the list of message based on result code.
        /// </summary>
        /// <value>The messages.</value>
        public List<string> Messages { get; set; }
    }
}
