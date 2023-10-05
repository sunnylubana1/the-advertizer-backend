using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.DTOs
{

    public static class WalruslogicsResponseBuilder
    {
        /// <summary>
        /// Generates the walruslogic response.
        /// </summary>
        /// <returns>The response.</returns>
        /// <param name="code">Result Code</param>
        /// <param name="messages">List of messages based on result-code.</param>
        public static WalruslogicResponseObject GenerateResponse(string code, string messages, object data = null)
        {
            WalruslogicResponseObject responseObject = new WalruslogicResponseObject();

            responseObject.Data = data;
            responseObject.Result.Code = code;
            responseObject.Result.Messages.Add(messages);

            return responseObject;
        }

        /// <summary>
        /// Generates the walruslogic response.
        /// </summary>
        /// <returns>The response.</returns>
        /// <param name="code">Result Code</param>
        /// <param name="messages">List of messages based on result-code.</param>
        public static WalruslogicResponseObject GenerateResponse(string code, List<string> messages, object data = null)
        {
            WalruslogicResponseObject responseObject = new WalruslogicResponseObject();

            responseObject.Data = data;
            responseObject.Result.Code = code;
            responseObject.Result.Messages.AddRange(messages);

            return responseObject;
        }

        /// <summary>
        /// Generates the  response.
        /// </summary>
        /// <returns>The response.</returns>
        /// <param name="code">Result Code.</param>
        /// <param name="message">String of message.</param>
        public static WalruslogicResponseObject GenerateResponse(string code, string message)
        {
            WalruslogicResponseObject responseObject = new WalruslogicResponseObject();

            responseObject.Result.Code = code;
            responseObject.Result.Messages = new List<string>();
            responseObject.Result.Messages.Add(message);

            return responseObject;
        }
    }
}
