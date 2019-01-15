using LBHAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Interfaces
{
    public interface IValidator
    {
        ValidationResult ValidateAddressesQueryParams(AddressesQueryParams filtersToValidate);
        ValidationResult ValidateAddressesLPIKey(string lpikey);
        ValidationResult ValidateStreetsQueryParams(StreetsQueryParams filtersToValidate);

        ValidationResult ValidateStreetsUSRN(string USRN);
    }
}
