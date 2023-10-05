using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using Walruslogics.Advertisement.BusinessLogic;
using Walruslogics.Advertisement.Database.Models;
using Walruslogics.Advertisement.DTOs;
using Walruslogics.Advertisement.Framework;
using Walruslogics.Framework;

namespace Walruslogics.Advertisement.WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserProfileController : ControllerBase
  {
    private IUserProfileBusinessLogic _userProfileBusinessLogic;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _webHost;



    public UserProfileController(IUserProfileBusinessLogic userProfileBusinessLogic, UserManager<AppUser> userManager, IWebHostEnvironment webHost)
    {
      _userProfileBusinessLogic = userProfileBusinessLogic;
      _userManager = userManager;
      _webHost = webHost;
    }

    [HttpGet("getuserprofile")]
    public async Task<IActionResult> GetUserProfile(string email)
    {
      WalruslogicResponseObject responseObject;

      try
      {
        responseObject = _userProfileBusinessLogic.GetUserProfile(email);
      }
      catch (Exception ex)
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Something happend wrong");

      }

      return Ok(responseObject);
    }

    [HttpPost("updateuserprofile")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileDTO userProfileDTO)
    {
      WalruslogicResponseObject responseObject;

      try
      {
        var formCollection = await Request.ReadFormAsync();
        var file = formCollection.Files.First();

        if (file.FileName != "default.jpg")
        {
          string wwwrootpath = _webHost.WebRootPath;


          string filename = Guid.NewGuid().ToString();
          var uploadPath = Path.Combine(wwwrootpath, string.Format(@"Images\User\{0}\", userProfileDTO.Id));

          if (!Directory.Exists(uploadPath))
          {
            Directory.CreateDirectory(uploadPath);
          }

          var extension = Path.GetExtension(file.FileName);

          var strFilePath = Path.Combine(uploadPath, userProfileDTO.Id + extension);


          using (FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.ReadWrite))
          {
            file.CopyTo(fs);
          }

          var dbPath = Path.Combine(string.Format(@"{0}://{1}/Images/User/{2}/", Request.Scheme, Request.Host, userProfileDTO.Id));

          userProfileDTO.ImagePath = dbPath;

          ImageUtility.ScaleResizeAndSaveImages(uploadPath, strFilePath);

        }

        var appuser = await _userManager.FindByNameAsync(userProfileDTO.Email);
        appuser.FirstName = userProfileDTO.FirstName;
        appuser.LastName = userProfileDTO.LastName;

        await _userManager.UpdateAsync(appuser);

        responseObject = _userProfileBusinessLogic.AddUserProfile(userProfileDTO);
      }
      catch (Exception ex)
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Something happend wrong");

      }

      return Ok(responseObject);
    }

    [HttpGet("getcountrydropdown")]
    public async Task<IActionResult> GetCountryDropdown()
    {
      WalruslogicResponseObject responseObject;
      try
      {
        responseObject = _userProfileBusinessLogic.CountryDropdown();
      }
      catch (Exception ex)
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Something happend wrong");

      }

      return Ok(responseObject);
    }

    [HttpGet("getstatedropdown")]
    public async Task<IActionResult> GetCountryDropdown(int counryid)
    {
      WalruslogicResponseObject responseObject;
      try
      {
        responseObject = _userProfileBusinessLogic.CountryDropdown();
      }
      catch (Exception ex)
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Something happend wrong");

      }

      return Ok(responseObject);
    }
  }
}
