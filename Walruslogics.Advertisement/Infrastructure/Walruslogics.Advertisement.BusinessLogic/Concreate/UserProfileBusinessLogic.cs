using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walruslogics.Advertisement.Database.Models;
using Walruslogics.Advertisement.DTOs;
using Walruslogics.Advertisement.DTOs.User;
using Walruslogics.Advertisement.Framework;
using Walruslogics.Identity.Repository;

namespace Walruslogics.Advertisement.BusinessLogic
{
  public class UserProfileBusinessLogic : IUserProfileBusinessLogic
  {
    private IGenericRepository<UserProfile> _genericRepository;
    private IGenericRepository<Country> _countryRepository;
    private IGenericRepository<City> _cityRepository;
    private WalruslogicResponseObject _responseObject;
    public UserProfileBusinessLogic(IGenericRepository<UserProfile> genericRepository,
      IGenericRepository<Country> countryRepository,
      IGenericRepository<City> cityRepository)
    {
      _genericRepository = genericRepository;
      _countryRepository = countryRepository;
      _cityRepository = cityRepository;
    }
    public bool CreateUserProfile(RegistrationModel registrationModel, string profilePicture)
    {
      UserProfile userProfile = new UserProfile();

      userProfile.FirstName = registrationModel.FirstName;
      userProfile.LastName = registrationModel.LastName;
      userProfile.CreatedBy = 1;
      userProfile.Email = registrationModel.Email;
      userProfile.CreationDateTime = DateTime.UtcNow;
      userProfile.IsActive = true;


      _genericRepository.Add(userProfile);

      var result = _genericRepository.SaveChanges();

      return result == (int)ResultCode.Success;
    }

    public WalruslogicResponseObject AddUserProfile(UserProfileDTO userProfileDTO)
    {
      var listOfMessages = new List<string>();
      bool isAdd = false;
      var userProfile = _genericRepository.GetById(Convert.ToInt64(userProfileDTO.Id));

      if (userProfile == null)
      {
        userProfile = new UserProfile();
        isAdd = true;
      }

      var state = _cityRepository.GetById(Convert.ToInt64(userProfileDTO.CityId));

      userProfile.StateId = _cityRepository.GetById(Convert.ToInt64(userProfileDTO.CityId)).StateId;
      userProfile.Id = Convert.ToInt64(userProfileDTO.Id);
      userProfile.FirstName = userProfileDTO.FirstName;
      userProfile.LastName = userProfileDTO.LastName;
      userProfile.PhoneNumber = userProfileDTO.PhoneNumber;
      userProfile.Address = userProfileDTO.Address;
      userProfile.Address2 = userProfileDTO.Address2.Trim() == "undefined" ? null : userProfileDTO.Address2;
      userProfile.CreatedBy = Convert.ToInt64(userProfileDTO.Id);
      userProfile.Email = userProfileDTO.Email;
      userProfile.CountryId = Convert.ToInt32(userProfileDTO.CountryId);
      userProfile.CityId = Convert.ToInt64(userProfileDTO.CityId);
      userProfile.CreationDateTime = DateTime.UtcNow;
      userProfile.ModifiedBy = Convert.ToInt64(userProfileDTO.Id);
      userProfile.IsActive = true;
      userProfile.IsExternalLogin = false;
      userProfile.ImagePath = userProfileDTO.ImagePath;
      userProfile.PinCode = userProfileDTO.PinCode;


      if (!isAdd)
      {
        _genericRepository.Update(userProfileDTO.Id, userProfile);
      }
      else
      {
        _genericRepository.Add(userProfile);
      }

      var result = _genericRepository.SaveChanges();

      if (result == (int)ResultCode.Success)
      {
        listOfMessages.Add("User Profile Added Successfully");

        // Create Success Response
        _responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Success.ToString(), listOfMessages);
      }
      else
      {
        listOfMessages.Add("User Profile not added successfully. Ops!! Something went wrong!");
        // Create Error Response
        _responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), listOfMessages);
      }


      return _responseObject;
    }

    public WalruslogicResponseObject GetUserProfile(string email)
    {
      UserProfileDTO userProfileDTO = new UserProfileDTO();


      var userProfile = _genericRepository.GetByCriteria(x => x.Email == email).FirstOrDefault();

      if (userProfile != null)
      {
        userProfileDTO.Id = userProfile.Id.ToString();
        userProfileDTO.Email = userProfile.Email;
        userProfileDTO.FirstName = userProfile.FirstName;
        userProfileDTO.LastName = userProfile.LastName;
        userProfileDTO.PhoneNumber = userProfile.PhoneNumber;
        userProfileDTO.ImagePath = userProfile.ImagePath;
        userProfileDTO.Address = userProfile.Address;
        userProfileDTO.Address2 = userProfile.Address2;
        userProfileDTO.CityId = userProfile.CityId.ToString();
        userProfileDTO.CountryId = userProfile.CountryId.ToString();
        userProfileDTO.PinCode = userProfile.PinCode ;

        _responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Success.ToString(), "", userProfileDTO);

      }
      else
      {
        

        _responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Failure.ToString(), "", null);

      }


      return _responseObject;
    }

    public WalruslogicResponseObject CountryDropdown()
    {
      var countries = _countryRepository.GetAll().ToList();

      List<ListItemContent> listItemContents = new List<ListItemContent>();

      foreach (var item in countries)
      {
        ListItemContent listItemContent = new ListItemContent();

        listItemContent.Text = item.Name;
        listItemContent.Value = item.Id;
        listItemContent.selected = false;

        listItemContents.Add(listItemContent);
      }

      _responseObject = WalruslogicsResponseBuilder.GenerateResponse(ResultCode.Success.ToString(), "", listItemContents);

      return _responseObject;
    }
  }
}
