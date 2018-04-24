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

        [Theory]
        [InlineData("RD02")]
        [InlineData("RD")]
        [InlineData("R d")]
        [InlineData("R")]
        [InlineData(" R ")]
        [InlineData(" r D1")]
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
        [InlineData("2D 2")]
        [InlineData("1D")]
        [InlineData("D 2")]
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
