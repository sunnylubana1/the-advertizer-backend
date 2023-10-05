using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.Framework
{
  public class EmailTemplate
  {
    public string TemplateFolder
    {
      get
      {
        return "F:\\2023\\walruslogics\\Walruslogics.Advertisement\\Services\\Walruslogics.Advertisement.WebAPI\\Templates\\";
      }
    }

    public string ForgotPassword(string name, string link)
    {
      var path = this.TemplateFolder + "forgot-password.html";
      var html = System.IO.File.ReadAllText(path);

      var keyVals = new List<KeyValuePair<string, string>>();

      keyVals.Add(new KeyValuePair<string, string>("{{Contact}}", name));
      keyVals.Add(new KeyValuePair<string, string>("{{link}}", link));
      return EmailSenderUtility.PrepareTemplate(html, keyVals);
    }

    public string ResetPassword(string name, string link)
    {
      var path = this.TemplateFolder + "reset-password.html";
      var html = System.IO.File.ReadAllText(path);

      var keyVals = new List<KeyValuePair<string, string>>();
      keyVals.Add(new KeyValuePair<string, string>("{{Contact}}", name));
      keyVals.Add(new KeyValuePair<string, string>("{{link}}", link));
      return EmailSenderUtility.PrepareTemplate(html, keyVals);
    }
  }
}
