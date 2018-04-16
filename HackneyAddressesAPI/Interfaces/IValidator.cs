using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Interfaces
{
    public interface IValidator
    {
        bool ValidatePostcode(string postcode);

        bool ValidateUPRN(string UPRN);

        bool ValidateUSRN(string USRN);

        ValidationResult ValidateClassCodePrimaryAddressStatus(Dictionary<string, string> filtersToValidate);
    }
}
