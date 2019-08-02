using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LBHAddressesAPI.UseCases.V1.Search.Models;

namespace LBHAddressesAPI.Validation
{
    public class SearchAddressValidator : AbstractValidator<SearchAddressRequest>, ISearchAddressValidator
    {
        public SearchAddressValidator()
        {
            throw new NotImplementedException();
        }
    }
}
