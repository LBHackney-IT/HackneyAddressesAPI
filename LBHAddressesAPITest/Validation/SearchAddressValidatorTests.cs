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
using LBHAddressesAPI.Exceptions;

namespace LBHAddressesAPITest.Validation
{
    [TestFixture]
    public class SearchAddressValidatorTests
    {
        private SearchAddressValidator _classUnderTest;

        [SetUp]
        public void SetUp()
        {
            Environment.SetEnvironmentVariable("ALLOWED_ADDRESSSTATUS_VALUES", "historical;alternative;approved preferred;provisional");
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
        [TestCase("approved preferred,historical")]
        public void GivenAnAllowedAddressStatusValue_WhenCallingValidation_ItReturnsNoErrors(string addressStatusVal)
        {
            var request = new SearchAddressRequest() { AddressStatus = addressStatusVal };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.AddressStatus, request);
        }

        [TestCase(" ")]
        [TestCase("")]
        [TestCase("alternative,  ,something")]
        [TestCase(null)]
        public void GivenAWhitespaceOrEmptyAddressStatusValue_WhenCallingValidation_ItReturnsAnError(string addressStatusVal)
        {
            var request = new SearchAddressRequest() { AddressStatus = addressStatusVal };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.AddressStatus, request);
        }

        [Test]
        public void GivenThereIsNoEnvironmentVariableForAddressStatus_WhenValidationIsInvoked_TheErrorIsReturned()
        {
            Environment.SetEnvironmentVariable("ALLOWED_ADDRESSSTATUS_VALUES", null);

            Action createValidator = () => new SearchAddressValidator();

            createValidator.Should().Throw<MissingEnvironmentVariableException>();
        }
    }
}
