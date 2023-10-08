using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Walruslogics.Advertisement.BusinessLogic;
using Walruslogics.Advertisement.Database.Models;
using Walruslogics.Advertisement.DTOs;
using Walruslogics.Advertisement.DTOs.Account;
using Walruslogics.Advertisement.Framework;


namespace Walruslogics.Advertisement.WebAPI
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : BaseController
  {
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IIdentityBusinessLogic _identityBusinessLogic;
    private IUserProfileBusinessLogic _userProfileBusinessLogic;
    EmailTemplate _emailTemplates;

    public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        IIdentityBusinessLogic identityBusinessLogic,
        IUserProfileBusinessLogic userProfileBusinessLogic)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _roleManager = roleManager;
      _identityBusinessLogic = identityBusinessLogic;
      _userProfileBusinessLogic = userProfileBusinessLogic;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginIn(LoginModel loginModel, string returnUrl = null)
    {
      returnUrl = returnUrl ?? Url.Content("~/");

      WalruslogicResponseObject responseObject = null;

      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByNameAsync(loginModel.Email);

        if(user != null && user.LoginProvider == "1")
        {
          var roleName = _userManager.GetRolesAsync(user).Result.First();

          var result = await _signInManager.PasswordSignInAsync(user.UserName, loginModel.Password, loginModel.RememberMe, false);

          if (result.Succeeded)
          {
            responseObject = _identityBusinessLogic.Login(user, roleName, user);
          }
          else
          {
            responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Password is not correct");

          }
        }
        else
        {
          if(user != null && user.LoginProvider == "2")
          {
            responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "You have logged in through google login, So, please login through google login", user.LoginProvider);

          }
          else
          {
            responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Email Not Found! Double-check your email or sign up to create a new account.");

          }

        }
      }
      else
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "User name or password field is empty");
      }

      // If we got this far, something failed, redisplay form
      return Ok(responseObject);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegistrationModel user)
    {
      WalruslogicResponseObject responseObject = null;

      if (ModelState.IsValid)
      {
        // Step 1.  Check if User Already exists 
        var appuser = await _userManager.FindByNameAsync(user.Email);

        if (appuser == null)
        {
          var isUserCreated = CreateUser(user);

          if (isUserCreated.Result)
          {
            responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Success.ToString(), "User Created");

          }
          else
          {
            responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Something went wrong, please try again later");
          }
        }
        else
        {
          responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "User already registered, Please use another email to register.");
        }
      }

      return Ok(responseObject);
    }

    [HttpGet("isemailalreadyexists")]
    public async Task<IActionResult> EmailExists(string email)
    {
      bool responseObject = false;

      if (ModelState.IsValid)
      {
        // Step 1.  Check if User Already exists 
        var appuser = await _userManager.FindByNameAsync(email);

        if (appuser == null)
        {
          responseObject = false;

        }
        else
        {
          responseObject = true;
        }
      }

      return Ok(responseObject);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(string returnUrl = null)
    {
      await _signInManager.SignOutAsync();

      if (returnUrl != null)
      {

      }
      else
      {

      }

      WalruslogicResponseObject responseObject = null;
      return Ok(responseObject);
    }

    [HttpPost("loginwithgoogle")]
    public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
    {
      WalruslogicResponseObject responseObject = null;

      try
      {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
          Audience = new List<string> { "1020278669405-76neahgtminq7c1oka5qu8eshc4qto41.apps.googleusercontent.com" }
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);

        var appuser = await _userManager.FindByNameAsync(payload.Email);

        if (appuser != null && appuser.LoginProvider == "2")
        {
          var roleName = _userManager.GetRolesAsync(appuser).Result.First();

          responseObject = _identityBusinessLogic.Login(appuser, roleName, appuser);

          return Ok(responseObject);
        }
        else
        {
          RegistrationModel model = new RegistrationModel()
          {
            FirstName = payload.GivenName,
            LastName = payload.FamilyName,
            Email = payload.Email,
            Password = "Admin@123#"
          };

          var isUserCreated = CreateUser(model, "2");

          if (isUserCreated.Result)
          {
            var systemUser = await _userManager.FindByNameAsync(payload.Email);

            _userProfileBusinessLogic.CreateUserProfile(model, payload.Picture);
            var roleName = _userManager.GetRolesAsync(systemUser).Result.First();

            responseObject = _identityBusinessLogic.Login(systemUser, roleName, systemUser);
          }
          else
          {
            responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "User Created");
          }
        }
      }
      catch (Exception ex)
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "User Created");

      }

      return Ok(responseObject);
    }

    [HttpGet("validatetoken")]
    [Authorize]
    public async Task<IActionResult> ValidateToken()
    {
      WalruslogicResponseObject responseObject = null;

      var systemUser = await _userManager.GetUserAsync(HttpContext.User);
      var roleName = _userManager.GetRolesAsync(systemUser).Result.First();

      responseObject = _identityBusinessLogic.Login(systemUser, roleName, systemUser);

      return Ok(responseObject);
    }

    [HttpPost("forgotpassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPassword data)
    {
      WalruslogicResponseObject responseObject = null;

      var user = await _userManager.FindByNameAsync(data.email);

      if (user == null)
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "User not exists, please enter valid registered email to reset password.");
      }
      else
      {
        if(user.LoginProvider == "1")
        {
          var res = await this.SendForgotPasswordEmail(user);
          responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Success.ToString(), "Forgot password has been sent successfully.");
        }
        else
        {
          responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "You have logged in through google login, So, please change your password through google.");

        }
      }

      return Ok(responseObject);

    }

    [HttpPost("resetpassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
    {
      WalruslogicResponseObject responseObject = null;

      var token = UrlEncryptionUtility.Decrypt(model.Token);
      var user = await _userManager.FindByIdAsync(model.UserId);
      var res = await _userManager.ResetPasswordAsync(user, token, model.ConfirmPassword);

      if (res.Succeeded)
      {
        await this.SendResetPasswordEmail(user);
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Success.ToString(), "Token is not valid or has been expired, please get new reset password link. ");
      }
      else
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Token is not valid or has been expired, please get new reset password link. ");

      }

      return Ok(responseObject);
    }

    [NonAction]
    private async Task<bool> SendForgotPasswordEmail(AppUser user)
    {

      _emailTemplates = new EmailTemplate();

      var token = await _userManager.GeneratePasswordResetTokenAsync(user);

      token = UrlEncryptionUtility.Encrypt(token);


      var confirmationLink = "http://localhost:4200" + $"/auth/resetpassword/{token}/{user.Id}";

      var body = _emailTemplates.ForgotPassword(user.FirstName, confirmationLink);

      EmailSenderUtility.SendEmail(user.FullName,
          user.Email,
          "The Advertiser - Forgot Your Password?",
          body);

      return true;
    }

    [NonAction]
    private async Task<bool> SendResetPasswordEmail(AppUser user)
    {
    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      token = UrlEncryptionUtility.Encrypt(token);

      _emailTemplates = new EmailTemplate();

      var body = _emailTemplates.ResetPassword(
          user.FirstName,
          "http://localhost:4200/" + $"auth/login");

      EmailSenderUtility.SendEmail(user.FullName,
          user.Email,
          "The Advertiser - Password Changed!",
          body);

      return true;
    }

    [NonAction]
    private async Task<bool> CreateUser(RegistrationModel user, string logginProvider = "1")
    {
      bool isUserCreated = false;

      var dbContext = HttpContext.RequestServices.GetService<AdvertisementDBContext>();

      // Begin the transaction
      using (var transaction = await dbContext.Database.BeginTransactionAsync())
      {
        try
        {
          var appUser = new AppUser()
          {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PasswordHash = user.Password,
            UserName = user.Email,
            NormalizedEmail = user.Email,
            Created = DateTime.UtcNow,
            LoginProvider = logginProvider
          };

          // Step 2. Create user
          var result = await _userManager.CreateAsync(appUser, user.Password);

          if (result.Succeeded)
          {
            #region AddUserToRole

            // Step 2: Add user to role
            // Find the role by its name
            var role = await _roleManager.FindByNameAsync("User");

            if (role == null)
            {
              var roleResult = await _roleManager.CreateAsync(new AppRole("User"));

              if (roleResult.Succeeded)
              {
                var addtoRoleResult = await _userManager.AddToRoleAsync(appUser, "User");
                isUserCreated = true;

              }
              else
              {
                isUserCreated = false;
              }
            }
            else
            {
              var addtoRoleResult = await _userManager.AddToRoleAsync(appUser, role.Name);
              isUserCreated = true;
            }

            #endregion

            #region
            //bool isAdded = _userProfileBusinessLogic.CreateUserProfile(user);
            #endregion

            // Commit the transaction if everything is successful
            await transaction.CommitAsync();
          }
          else
          {
            isUserCreated = false;
          }


        }
        catch (Exception ex)
        {
          // Rollback the transaction if an exception occurred
          await transaction.RollbackAsync();

          isUserCreated = false;
        }


        return isUserCreated;

      }
    }
  }
}
