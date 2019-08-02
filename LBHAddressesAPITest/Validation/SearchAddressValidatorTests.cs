using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Moq;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Validation;
using FluentValidation;
using FluentValidation.TestHelper;

namespace LBHAddressesAPITest.Validation
{
    [TestFixture]
    public class SearchAddressValidatorTests
    {
        private SearchAddressValidator _classUnderTest;

        [SetUp]
        public void SetUp()
        {
            _classUnderTest = new SearchAddressValidator();
        }

        [TestCase("cat")]
        [TestCase("provizional")]
        [TestCase("alternative,hystorical")]
        public void GivenAnAddressStatusValueThatDoesntMachAllowedOnes_WhenCallingValidation_ItReturnsAnError(string addressStatusVal)
        {
            var request = new SearchAddressRequest() { AddressStatus = addressStatusVal };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.AddressStatus, request).WithErrorMessage("Value for the parameter is not valid.");
        }

        [TestCase("alternative")]
        [TestCase("historical")]
        [TestCase("approved preffered,historical")]
        public void GivenAnAllowedAddressStatusValue_WhenCallingValidation_ItReturnsNoErrors(string addressStatusVal)
        {
            var request = new SearchAddressRequest() { AddressStatus = addressStatusVal };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.AddressStatus, request);
        }

        [TestCase(" ")]
        [TestCase("")]
        [TestCase(null)]
        public void GivenAWhitespaceOrEmptyAddressStatusValue_WhenCallingValidation_ItReturnsAnError(string addressStatusVal)
        {
            var request = new SearchAddressRequest() { AddressStatus = addressStatusVal };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.AddressStatus, request);
        }
    }
}
