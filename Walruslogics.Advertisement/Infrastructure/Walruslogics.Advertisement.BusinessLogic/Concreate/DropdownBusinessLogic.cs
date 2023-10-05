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
  public class DropdownBusinessLogic : IDropdownBusinessLogic
  {
    private IGenericRepository<Country> _countryRepository;
    private IGenericRepository<State> _stateRepository;
    private IGenericRepository<City> _cityRepository;
    private WalruslogicResponseObject _responseObject;


    public DropdownBusinessLogic(IGenericRepository<Country> countryRepository,
      IGenericRepository<State> stateRepository, IGenericRepository<City> cityRepository
      ) {
      _countryRepository = countryRepository;
      _stateRepository = stateRepository;
      _cityRepository = cityRepository;
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
    public WalruslogicResponseObject StateDropdown(int countryId)
    {
      var states = _stateRepository.GetByCriteria(x => x.CountryId == countryId).ToList();

      List<ListItemContent> listItemContents = new List<ListItemContent>();

      foreach (var item in states)
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
    public WalruslogicResponseObject CityDropdown(int countryId)
    {
      var cities = _cityRepository.GetByCriteria(x => x.CountryId == countryId).ToList();

      List<ListItemContent> listItemContents = new List<ListItemContent>();

      foreach (var item in cities)
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
