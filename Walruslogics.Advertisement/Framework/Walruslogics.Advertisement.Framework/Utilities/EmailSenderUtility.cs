using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.Framework
{
  public static class EmailSenderUtility
  {
    //public static bool SendEmail(string name,
    //    string email,
    //    string subject,
    //    string body,
    //    List<KeyValuePair<string, string>> cCs = null,
    //    List<KeyValuePair<string, string>> bCcs = null,
    //    List<KeyValuePair<string, string>> attachments = null)
    //{
    //    var apiKey = "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"; // "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";
    //    var client = new SendGridClient(apiKey);
    //    var from = new EmailAddress("support@edmleadnetwork.com", "Whitelist Scrub Service");
    //    var to = new EmailAddress(email, name);

    //    var plainTextContent = "";
    //    var htmlContent = body;

    //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

    //    cCs = cCs ?? new List<KeyValuePair<string, string>>();
    //    bCcs = bCcs ?? new List<KeyValuePair<string, string>>();
    //    attachments = attachments ?? new List<KeyValuePair<string, string>>();

    //    cCs.AddRange(
    //      new List<KeyValuePair<string, string>>() {
    //            new KeyValuePair<string, string>("paralellmatrix@gmail.com", "Mubsher")
    //          //new KeyValuePair<string, string>("mateen.arif@graymath.com", "Mateen")
    //      });

    //    if (cCs.Count > 0)
    //        msg.AddCcs(cCs.Select(x => new EmailAddress(x.Key, x.Value)).ToList());

    //    if (bCcs.Count > 0)
    //        msg.AddBccs(bCcs.Select(x => new EmailAddress(x.Key, x.Value)).ToList());

    //    foreach (var item in attachments)
    //        msg.AddAttachment(item.Key, item.Value);



    //    var response = client.SendEmailAsync(msg).Result;

    //    return response.StatusCode == System.Net.HttpStatusCode.OK;
    //}


    public static void SendEmail(string name,
        string email,
        string subject,
        string body,
        List<KeyValuePair<string, string>> cCs = null,
        List<KeyValuePair<string, string>> bCcs = null,
        List<KeyValuePair<string, string>> attachments = null)
    {
      using (MailMessage mail = new MailMessage())
      {
        mail.From = new MailAddress("scrub@whitelistdata.com");
        mail.To.Add(email);
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        cCs = cCs ?? new List<KeyValuePair<string, string>>();
        bCcs = bCcs ?? new List<KeyValuePair<string, string>>();
        attachments = attachments ?? new List<KeyValuePair<string, string>>();

        //bCcs.AddRange(
        //  new List<KeyValuePair<string, string>>() {
        //            new KeyValuePair<string, string>("paralellmatrix@gmail.com", "Mubsher"),
        //            //new KeyValuePair<string, string>("mateen.arif@graymath.com", "Mateen")
        //  });

        if (cCs.Count > 0)
          mail.CC.Add(string.Join(",", cCs.Select(x => x.Key)));

        if (bCcs.Count > 0)
          mail.Bcc.Add(string.Join(",", bCcs.Select(x => x.Key)));

        foreach (var item in attachments)
          mail.Attachments.Add(new System.Net.Mail.Attachment(item.Key));

        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
        {
          smtp.UseDefaultCredentials = false;
          smtp.Credentials = new NetworkCredential("scrub@whitelistdata.com", "nrusrafrbaqweyjc");
          smtp.EnableSsl = true;
          smtp.Send(mail);
        }
      }
    }



    public static string PrepareTemplate(string template, List<KeyValuePair<string, string>> keyVals)
    {
      foreach (var item in keyVals)
      {
        template = template.Replace(item.Key, item.Value);
      }

      var templateKeysRegex = Regex.Matches(template, @"{\b\S+?\b}");
      if (templateKeysRegex.Count != 0)
      {
        string tk = "";
        foreach (var t in templateKeysRegex)
          tk += t + "/";
        throw new Exception("Email: not all tokens were provided. Pending list /" + tk);
      }

      return template;
    }
  }
}
