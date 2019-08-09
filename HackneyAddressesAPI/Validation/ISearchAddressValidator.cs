using FluentValidation.Results;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Validation
{
    public interface ISearchAddressValidator
    {
        ValidationResult Validate(SearchAddressRequest instance);
    }
}
