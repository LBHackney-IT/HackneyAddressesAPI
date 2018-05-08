using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Interfaces
{
    public interface IValidator
    {
        ValidationResult ValidateAddressesQueryParams(AddressesQueryParams filtersToValidate);
        ValidationResult ValidateAddressesLPIKey(string lpikey);
        ValidationResult ValidateStreetsQueryParams(StreetsQueryParams filtersToValidate);
    }
}
