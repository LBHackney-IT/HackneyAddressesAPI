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

        #region Address status validation
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
        #endregion
        // Below explanations all use the postcodes IG11 7QD and E5 3XW 
        //"Incode" refers to the whole second part of the postcode (i.e. 3XW, 7QD)
        //"Outcode" refers to the whole first part of the postcode (Letter(s) and number(s) - i.e. IG11, E5)
        //"Area" refers to the first letter(s) of the postcode (i.e.  IG, E)
        //"District" refers to first number(s) to appear in the postcode (i.e. 11, 5)
        #region Postcode validation
        [TestCase("CR1 3ED")]
        [TestCase("NE7")]
        public void GivenAPostCodeValueInUpperCase_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("w2 5jq")]
        [TestCase("ne7")]
        public void GivenAPostCodeValueInLowerCase_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("w2 5JQ")]
        [TestCase("E11 5ra")]
        public void GivenAPostCodeValueInLowerCaseAndUpperCase_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("CR13ED")]
        [TestCase("RE15AD")]
        public void GivenPostCodeValueWithoutSpaces_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("NW")]
        [TestCase("E")]
        public void GivenOnlyAnAreaPartOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage("Must provide at least the first part of the postcode.");
        }

        [TestCase("17 9LL")]
        [TestCase("8 1LA")]
        public void GivenOnlyAnIncodeAndADistrictPartsOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage("Must provide at least the first part of the postcode.");
        }

        [TestCase("NW 9LL")]
        [TestCase("NR1LW")]
        public void GivenOnlyAnIncodeAndAnAreaPartsOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage("Must provide at least the first part of the postcode.");
        }

        [TestCase("1LL")]
        [TestCase(" 6BQ")]
        public void GivenOnlyAnIncodePartOfThePostCode_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage("Must provide at least the first part of the postcode.");
        }

        [TestCase("NW9")]
        [TestCase("RH5 ")]
        public void GivenOnlyAnOutcodePartOfPostCode_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("E8 1LL")]
        [TestCase("SW17 1JK")]
        public void GivenBothPartsOfPostCode_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("IG117QDfdsfdsfd")]
        [TestCase("E1llolol")]
        public void GivenAValidPostcodeFolowedByRandomCharacters_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage("Must provide at least the first part of the postcode.");
        }

        [TestCase("EEE")]
        [TestCase("THE")]
        public void GivenThreeCharacters_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage("Must provide at least the first part of the postcode.");
        }

        #endregion
        #region QueryParameterValidation

        [TestCase("asdfghjk", "postcode", "RM3 0FS")]
        [TestCase("ccvbbvv", "postcode", "E5 0DW")]
        public void GivenAnInvalidFilterParameterAndMandatoryParameter_WhenCallingValidation_ItReturnsAnError(string queryParameter1, string queryParameter2, string postcode)
        {
            var queryStringParameters = new List<string>() { queryParameter1, queryParameter2 };
            var request = new SearchAddressRequest() { PostCode=postcode, RequestFields = queryStringParameters };

            _classUnderTest.ShouldHaveValidationErrorFor(x => x.RequestFields, request).WithErrorMessage("Invalid properties have been provided.");
        }

        [TestCase("uprn", "postcode", "RM3 0FS")]
        [TestCase("usrn", "postcode", "E5 0DW")]
        public void GivenOnlyValidFilterParameters_WhenCallingValidation_ItReturnsNoErrors(string queryParameter1, string queryParameter2, string postcode)
        {
            var queryStringParameters = new List<string>() { queryParameter1, queryParameter2 };
            var request = new SearchAddressRequest() { PostCode=postcode, RequestFields = queryStringParameters };

            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.RequestFields, request);
        }

        [TestCase("RequestFields", "postcode", "E5 9TT")]
        [TestCase("Errors", "postcode", "E7 2JT")]
        public void GivenInvalidFilterParameterWhoseNameMatchesOneOfSearchAddressRequestPropertiesThatAreNotUsedToGetOrFilterData_WhenCallingValidation_ItReturnsAnError(string queryParameter1, string queryParameter2, string postcode) //we also provide postcode, because it's mandatory and the other validation will interfere with this test if it's not put in.
        {
            var queryStringParameters = new List<string>() { queryParameter1, queryParameter2 };
            var request = new SearchAddressRequest() { PostCode = postcode, RequestFields = queryStringParameters };

            _classUnderTest.ShouldHaveValidationErrorFor(x => x.RequestFields, request).WithErrorMessage("Invalid properties have been provided.");
        }

        [TestCase("ubrn", "addrezzstatus", "postcode", "RM3 0FS")]
        [TestCase("uzzrn", "gazziityr", "postcode", "E5 0DW")]
        public void GivenMultipleInvalidFilterParametersAndMandatoryParameter_WhenCallingValidation_ItReturnsAnError(string queryParameter1, string queryParameter2, string queryParameter3, string postcode)
        {
            var queryStringParameters = new List<string>() { queryParameter1, queryParameter2, queryParameter3 };
            var request = new SearchAddressRequest() { PostCode = postcode, RequestFields = queryStringParameters };

            _classUnderTest.ShouldHaveValidationErrorFor(x => x.RequestFields, request).WithErrorMessage("Invalid properties have been provided.");
        }

        #endregion
        #region Request object validation

        [TestCase(12345)]
        public void GivenARequestWithOnlyAUPRN_WhenCallingValidation_ItReturnsNoError(int uprn)
        {
            var request = new SearchAddressRequest() { UPRN = uprn };
            _classUnderTest.TestValidate(request).ShouldNotHaveError();
        }

        [TestCase(12345)]
        public void GivenARequestWithOnlyAUSRN_WhenCallingValidation_ItReturnsNoError(int usrn)
        {
            var request = new SearchAddressRequest() { USRN = usrn };
            _classUnderTest.TestValidate(request).ShouldNotHaveError();
        }

        [TestCase("SW1A 1AA")]
        public void GivenARequestWithOnlyAPostCode_WhenCallingValidation_ItReturnsNoError(string postcode)
        {
            var request = new SearchAddressRequest() { PostCode = postcode };
            _classUnderTest.TestValidate(request).ShouldNotHaveError();
        }

        [TestCase("Sesame street")]
        public void GivenARequestWithOnlyAStreet_WhenCallingValidation_ItReturnsNoError(string street)
        {
            var request = new SearchAddressRequest() { Street = street };
            _classUnderTest.TestValidate(request).ShouldNotHaveError();
        }

        [TestCase("12345")]
        public void GivenARequestWithBuildingNumberAndNoMandatoryFields_WhenCallingValidation_ItReturnsAnError(string buildingNumber)
        {
            var request = new SearchAddressRequest() { BuildingNumber = buildingNumber };
            _classUnderTest.TestValidate(request).ShouldHaveError().WithErrorMessage("You must provide at least one of (UPRN, USRN, Post code, Street)");
        }

        #endregion

        //Keep this at the end....
        [Test]
        public void GivenThereIsNoEnvironmentVariableForAddressStatus_WhenValidationIsInvoked_TheErrorIsReturned()
        {
            Environment.SetEnvironmentVariable("ALLOWED_ADDRESSSTATUS_VALUES", null);

            Action createValidator = () => new SearchAddressValidator();

            createValidator.Should().Throw<MissingEnvironmentVariableException>();
        }
    }
}
