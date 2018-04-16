using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Helpers;
using HackneyAddressesAPI.Interfaces;
using Xunit;
using Moq;
using HackneyAddressesAPI.Models;

namespace HackneyAddressesAPI.Tests.Actions
{
    public class ValidationTests
    {
        //Postcode test now simple, but have left in
        [Theory]
        [InlineData("NR7 2RE", true)]
        [InlineData("NR", true)]
        [InlineData("NR5677", true)]
        [InlineData("8QR", true)]
        [InlineData("", false)]
        [InlineData("    ", false)]
        [InlineData(null, false)]
        [InlineData("N168QR", true)]
        [InlineData("N16 8QR", true)]
        [InlineData("n168qr", true)]
        [InlineData("n16 8qr", true)]
        [InlineData("N16   8QR", true)]
        [InlineData(" N168QR ", true)]
        [InlineData(" N16 8QR ", true)]
        [InlineData("EC1V 8QR ", true)]
        [InlineData(" EC1V 8QR ", true)]
        [InlineData(" EC1 V 8QR ", true)]
        [InlineData("E 1 8QR ", true)]
        [InlineData("E1 8QR ", true)]
        [InlineData("E1V 9ZQ", true)]
        public void return_a_boolean_if_postcode_is_valid(string postcode, bool expected)
        {
            var postcodeValidator = new Validator();
            var result = postcodeValidator.ValidatePostcode(postcode);
            Assert.Equal(expected, result);
        }

        //Implement UPRN Tests - but is really needed as the checking is simple?
        [Theory]
        [InlineData("012345678912", true)]
        [InlineData("0213H12312", false)]
        [InlineData("012 231 24 1238 129", false)]
        [InlineData("0123", false)]
        [InlineData("", false)]
        [InlineData("    ", false)]
        [InlineData(null, false)]
        [InlineData("987654321", true)]
        [InlineData("986 623 213", true)]
        [InlineData(" 9282128", true)]
        [InlineData(" 22393 282    22", true)]
        [InlineData(" 9129281 ", true)]
        public void return_a_boolean_if_UPRN_is_valid(string uprn, bool expected)
        {
            var uprnValidator = new Validator();
            var result = uprnValidator.ValidateUPRN(uprn);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("RD02")]
        [InlineData("RD")]
        [InlineData("R d")]
        [InlineData("R")]
        [InlineData(" R ")]
        [InlineData(" r D1")]
        [InlineData("D 2")]
        public void return_null_if_usage_class_code_is_valid(string classCode)
        {
            var validator = new Validator();
            object result = validator.UsageClassCodeChecker(classCode);
            Assert.Null(result);
        }

        [Theory]
        [InlineData("RDD90")]
        [InlineData("02")]
        [InlineData("RrDD21")]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        [InlineData("2D 2")]
        [InlineData("1D")]
        public void return_ApiErrorMessage_if_usage_class_code_is_not_valid(string classCode)
        {
            var validator = new Validator();
            var result = validator.UsageClassCodeChecker(classCode);
            Assert.IsType<ApiErrorMessage>(result);
        }

        //Not writing tests for these as they're fairly simple methods.
        //Usage Class Primary checker
        //Address Status Checker
        //RemoveSpacesAndCapitalize
        //ValidateUSRN

    }
}
