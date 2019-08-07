﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Exceptions;
using System.Text.RegularExpressions;
using LBHAddressesAPI.Infrastructure.V1.Validation;

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

            RuleFor(r => r.PostCode).Matches(new Regex("^((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]))))( )?([0-9][A-Za-z]{2})?)$")).WithMessage("Must provide at least the first part of the postcode.");

            RuleFor(r => r).Must(CheckForAtLeastOneMandatoryFilterProperty).WithMessage("You must provide at least one of (UPRN, USRN, Post code, Street)");

            //RuleFor(r => r.RequestFields).Must(CheckForInvalidProperties).WithMessage("Invalid properties have been provided");
        }

        //private bool CheckForInvalidProperties(List<string> requestFields)
        //{
        //    List<string> allProperties = typeof(SearchAddressRequest).GetProperties().Where(prop => prop.Name != "Errors" || prop.Name != "RequestFields").Select(prop => prop.Name).ToList();
        //    IEnumerable<string> exceptions = requestFields.Except(allProperties, StringComparer.OrdinalIgnoreCase);
        //    if (exceptions.Count() > 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }

        //}

        private bool CheckForAtLeastOneMandatoryFilterProperty(SearchAddressRequest request)
        {
            if (request.UPRN == null && request.USRN == null && request.PostCode == null && request.Street == null)
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
