using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Walruslogics.Advertisement.BusinessLogic;
using Walruslogics.Advertisement.DTOs;
using Walruslogics.Advertisement.Framework;

namespace Walruslogics.Advertisement.WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DropdownController : ControllerBase
  {
    private IDropdownBusinessLogic _dropdownBusinessLogic;

    public DropdownController(IDropdownBusinessLogic dropdownBusinessLogic)
    {
      _dropdownBusinessLogic = dropdownBusinessLogic;
    }

    [HttpGet("getcountrydropdown")]
    public async Task<IActionResult> GetCountryDropdown()
    {
      WalruslogicResponseObject responseObject;
      try
      {
        responseObject = _dropdownBusinessLogic.CountryDropdown();
      }
      catch (Exception ex)
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Something happend wrong");

      }

      return Ok(responseObject);
    }

    [HttpGet("getstatedropdown")]
    public async Task<IActionResult> GetStateDropdown(int countryId)
    {
      WalruslogicResponseObject responseObject;
      try
      {
        responseObject = _dropdownBusinessLogic.StateDropdown(countryId);
      }
      catch (Exception ex)
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Something happend wrong");

      }

      return Ok(responseObject);
    }

    [HttpGet("getcitydropdown")]
    public async Task<IActionResult> GetCityDropdown(int countryid)
    {
      WalruslogicResponseObject responseObject;
      try
      {
        responseObject = _dropdownBusinessLogic.CityDropdown(countryid);
      }
      catch (Exception ex)
      {
        responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "Something happend wrong");

      }

      return Ok(responseObject);
    }

  }
}
