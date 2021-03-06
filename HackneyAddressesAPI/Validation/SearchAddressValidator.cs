﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Exceptions;
using System.Text.RegularExpressions;
using LBHAddressesAPI.Infrastructure.V1.Validation;
using LBHAddressesAPI.Helpers;

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

            RuleFor(r => r.PostCode).Matches(new Regex("^((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]))))( )?(([0-9][A-Za-z]?[A-Za-z]?)?))$")).WithMessage("Must provide at least the first part of the postcode.");

            RuleFor(r => r).Must(CheckForAtLeastOneMandatoryFilterPropertyWithGazetteerLocal).WithMessage("You must provide at least one of (uprn, usrn, postcode, street, usagePrimary, usageCode), when gazeteer is 'local'.");
            RuleFor(r => r).Must(CheckForAtLeastOneMandatoryFilterPropertyWithGazetteerBoth).WithMessage("You must provide at least one of (uprn, usrn, postcode), when gazetteer is 'both'.");

            RuleFor(r => r.RequestFields).Must(CheckForInvalidProperties).WithMessage("Invalid properties have been provided.");
        }

        private bool CheckForInvalidProperties(List<string> requestFields)
        {
            if (requestFields == null) //When the api runs - this will never be null. However this will become null in the context of other validation tests, making them crash. Because fluent validations testshelper doesn't isolate different rules one from another.
            {
                return true; // returning true, because there can't be any invalid parameter names, when there no parameters provided.
            }

            List<string> allProperties = typeof(SearchAddressRequest).GetProperties().Where(prop => prop.Name != "Errors" && prop.Name != "RequestFields").Select(prop => prop.Name).ToList();

            IEnumerable<string> invalidParameters = requestFields.Except(allProperties, StringComparer.OrdinalIgnoreCase);

            if (invalidParameters.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckForAtLeastOneMandatoryFilterPropertyWithGazetteerLocal(SearchAddressRequest request)
        {
            if(request.Gazetteer == GlobalConstants.Gazetteer.Local && request.UPRN == null && request.USRN == null && request.PostCode == null && request.Street == null && request.usagePrimary == null && request.usageCode == null)
            {
                return false;
            }

            return true;
        }

        private bool CheckForAtLeastOneMandatoryFilterPropertyWithGazetteerBoth(SearchAddressRequest request)
        {
            if(request.Gazetteer == GlobalConstants.Gazetteer.Both && request.UPRN == null && request.USRN == null && request.PostCode == null)
            {
                return false;
            }
            return true;
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
