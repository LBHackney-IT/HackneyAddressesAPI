using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Exceptions;

namespace LBHAddressesAPI.Validation
{
    public class SearchAddressValidator : AbstractValidator<SearchAddressRequest>, ISearchAddressValidator
    {
        private readonly string[] allowedAddressStatusValues;

        public SearchAddressValidator()
        {
            try { allowedAddressStatusValues = Environment.GetEnvironmentVariable("ALLOWED_ADDRESSSTATUS_VALUES").Split(";"); }
            catch (Exception) { throw new MissingEnvironmentVariableException("ALLOWED_ADDRESSSTATUS_VALUES"); }

            RuleFor(r => r.AddressStatus).NotNull().NotEmpty();
            RuleFor(r => r.AddressStatus).Must(CanBeAnyCombinationOfAllowedValues).WithMessage("Value for the parameter is not valid.");
        }

        private bool CanBeAnyCombinationOfAllowedValues(string addressStatus)
        {
            if(string.IsNullOrEmpty(addressStatus))
            {
                return false;
            }
            var separateValuesArray = addressStatus.Split(",");

            foreach(string value in separateValuesArray)
            {
                if (!allowedAddressStatusValues.Contains(value))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
