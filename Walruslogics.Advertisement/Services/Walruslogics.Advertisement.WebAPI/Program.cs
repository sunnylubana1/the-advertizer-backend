using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Walruslogics.Advertisement.BusinessLogic;
using Walruslogics.Advertisement.Database.Models;
using Walruslogics.Advertisement.WebAPI;
using Walruslogics.Identity.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AdvertisementDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly("Walruslogics.Advertisement.Database")));

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IIdentityBusinessLogic, IdentityBusinessLogic>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserProfileBusinessLogic, UserProfileBusinessLogic>();
builder.Services.AddScoped<IDropdownBusinessLogic, DropdownBusinessLogic>();


#region Identity

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    options.User.RequireUniqueEmail = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;

}).AddRoles<AppRole>().AddEntityFrameworkStores<AdvertisementDBContext>().AddDefaultTokenProviders();

#endregion

#region JWT-Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var identityToken = AppSettings.GetIdentityToken();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidAudience = identityToken.Audience,
        ValidIssuer = identityToken.Issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityToken.Secret))
    };
});

#endregion

#region Allow Any Origin 

//builder.Services.AddCors(options => options.AddPolicy("AllowEverything", builder => builder.AllowAnyOrigin()
//                                                                                    .AllowAnyHeader()
//                                                                                    .AllowAnyMethod()));

#endregion

#region Allow Specific Origin 

//services.AddCors(options => options.AddPolicy("AllowSpecificDomain", builder => builder.WithOrigins("http://localhost:4200/")));

builder.Services.AddCors(opt =>
{
  opt.AddPolicy(name: "CorsPolicy", builder =>
  {
    builder.WithOrigins("http://localhost:4200", "https://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
  });
});

#endregion

// Configure for file uploads
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartHeadersLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.MultipartBoundaryLengthLimit = int.MaxValue; // Add this line
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();


app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
    RequestPath = new PathString("/wwwroot")
});

app.UseAuthorization();

app.MapControllers();

app.Run();
