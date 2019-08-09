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
        //"Incode" refers to the whole second part of the postcode (i.e. 3XW, 7QD) from (E11 3XW, W3 7QD)
        //"Outcode" refers to the whole first part of the postcode (Letter(s) and number(s) - i.e. IG11, E5) from (IG11 9LL, E5 2LL)
        //"Area" refers to the first letter(s) of the postcode (i.e.  IG, E) from (IG11 9LL, E5 2LL)
        //"District" refers to first number(s) to appear in the postcode (i.e. 11, 5) from (IG11 9LL, E5 2LL)
        //"Sector" refers to the number in the second part of the postcode (i.e. 9, 7) from (SW2 9DN, NE4 7JU)
        //"Unit" refers to the letters in the second part of the postcode (i.e. DN, JU) from (SW2 9DN, NE4 7JU)
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

        [TestCase("w2 ")]
        [TestCase("E11 ")]
        public void GivenAnOutcodeWithSpace_WhenCallingValidation_ItReturnsNoErrors(string postCode)
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

        [TestCase("E9")] //A9
        [TestCase("S5")]
        [TestCase("S11")] //A99
        [TestCase("W12")]
        [TestCase("NW9")] //AA9
        [TestCase("RH5")]
        [TestCase("SW17")] // AA99
        [TestCase("NE17")]
        [TestCase("W4R")] // A9A
        [TestCase("N1C")]
        [TestCase("NW1W")] // AA9A
        [TestCase("CR1H")]
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

        [TestCase("SW11 9")]
        [TestCase("e14 2")]
        public void GivenAnOutcodeAndASector_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("SW7 9A")]
        [TestCase("n12 8F")]
        public void GivenAnOutcodeAndASectorAndTheFirstLetterOfTheUnit_WhenCallingValidation_ItReturnsNoErrors(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldNotHaveValidationErrorFor(x => x.PostCode, request);
        }

        [TestCase("N8 LL")]
        [TestCase("NW11 AE")]
        public void GivenAnOutcodeAndAUnit_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage("Must provide at least the first part of the postcode.");
        }

        [TestCase("S10 H")]
        [TestCase("W1 J")]
        public void GivenAnOutcodeAndOnlyOneLetterOfAUnit_WhenCallingValidation_ItReturnsAnError(string postCode)
        {
            var request = new SearchAddressRequest() { PostCode = postCode };
            _classUnderTest.ShouldHaveValidationErrorFor(x => x.PostCode, request).WithErrorMessage("Must provide at least the first part of the postcode.");
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

        [TestCase("someValue")]
        public void GivenARequestWithOnlyUsagePrimary_WhenCallingValidation_ItReturnsNoError(string UsagePrimary)
        {
            var request = new SearchAddressRequest() { usagePrimary = UsagePrimary };
            _classUnderTest.TestValidate(request).ShouldNotHaveError();
        }

        [TestCase("otherValue")]
        public void GivenARequestWithOnlyUsageCode_WhenCallingValidation_ItReturnsNoError(string UsageCode)
        {
            var request = new SearchAddressRequest() { usageCode = UsageCode };
            _classUnderTest.TestValidate(request).ShouldNotHaveError();
        }

        [TestCase("12345")]
        public void GivenARequestWithBuildingNumberAndNoMandatoryFields_WhenCallingValidation_ItReturnsAnError(string buildingNumber)
        {
            var request = new SearchAddressRequest() { BuildingNumber = buildingNumber };
            _classUnderTest.TestValidate(request).ShouldHaveError().WithErrorMessage("You must provide at least one of (uprn, usrn, postcode, street, usagePrimary, usageCode).");
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
