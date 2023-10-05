using Walruslogics.Advertisement.Framework;

namespace Walruslogics.Advertisement.WebAPI
{
    public static class AppSettings
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static IdentityTokenDescriptor GetIdentityToken()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

                builder.AddEnvironmentVariables();
                Configuration = builder.Build();

                IdentityTokenDescriptor identityTokenDescriptor = new IdentityTokenDescriptor
                {
                    Audience = Configuration["IdentityTokenDescriptor:Audience"],
                    Issuer = Configuration["IdentityTokenDescriptor:Issuer"],
                    Secret = Configuration["IdentityTokenDescriptor:Secret"],
                    Expires = Convert.ToDouble(Configuration["IdentityTokenDescriptor:Expires"])
                };

                return identityTokenDescriptor;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Token Configuration missing");
            }
        }
    }
}
