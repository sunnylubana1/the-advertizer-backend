using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Walruslogics.Advertisement.DTOs;

namespace Walruslogics.Advertisement.BusinessLogic
{
  public interface IDropdownBusinessLogic
  {
    WalruslogicResponseObject CountryDropdown();
    WalruslogicResponseObject StateDropdown(int countryId);
    WalruslogicResponseObject CityDropdown(int countryId);

  }
}
