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
using LBHAddressesAPI.Helpers;

namespace LBHAddressesAPITest.Models
{
    [TestFixture]
    public class SearchAddressRequestTests
    {
        #region Gazetteer
        [Test]
        public void GivenNoInputForGazetteer_WhenSearchAddressRequestObjectIsCreated_ItDefaultsToBoth()
        {
            var _classUnderTest = new SearchAddressRequest();
            _classUnderTest.Gazetteer.Should().Equals(GlobalConstants.Gazetteer.Both);
        }

        [Test]
        public void GivenInputValueLocalForGazetteer_WhenSearchAddressRequestObjectIsCreated_ItIsSetToLocal()
        {
            var _classUnderTest = new SearchAddressRequest() { Gazetteer = GlobalConstants.Gazetteer.Local };
            _classUnderTest.Gazetteer.Should().Equals(GlobalConstants.Gazetteer.Local);
        }

        [Test]
        public void GivenInputValueBothForGazetteer_WhenSearchAddressRequestObjectIsCreated_ItIsSetToBoth()
        {
            var _classUnderTest = new SearchAddressRequest() { Gazetteer = GlobalConstants.Gazetteer.Both };
            _classUnderTest.Gazetteer.Should().Equals(GlobalConstants.Gazetteer.Local);
        }
        #endregion

        #region HackneyGazetteerOutOfBoroughAddress
        [Test] //Gazetteer -> no input, HGOBAddress -> no input
        public void GivenNoInputValueForGazetteerAndHackneyGazetteerOutOfBoroughAddressParameters_WhenSearchAddressRequestObjectIsCreated_GazetteerIsSetToItsDefaultAndHackneyGazetteerOutOfBoroughAddressIsSetToNullBasedOnThat()
        {
            var _classUnderTest = new SearchAddressRequest() { };
            Assert.Null(_classUnderTest.HackneyGazetteerOutOfBoroughAddress);
        }

        [Test] //Gazetteer = Both, HGOBAddress -> no input
        public void GivenGazetteerValueBothAndNoInputForHackneyGazetteerOutOfBoroughAddress_WhenSearchAddressRequestObjectIsCreated_HackneyGazetteerOutOfBoroughAddressIsSetToNull()
        {
            var _classUnderTest = new SearchAddressRequest() { Gazetteer = GlobalConstants.Gazetteer.Both };
            Assert.Null(_classUnderTest.HackneyGazetteerOutOfBoroughAddress);
        }

        [Test] //Gazetteer = Local, HGOBAddress -> no input
        public void GivenGazetteerValueLocalAndNoInputForHackneyGazetteerOutOfBoroughAddress_WhenSearchAddressRequestObjectIsCreated_HackneyGazetteerOutOfBoroughAddressIsSetToFalse()
        {
            var _classUnderTest = new SearchAddressRequest() { Gazetteer = GlobalConstants.Gazetteer.Local };
            Assert.AreEqual(_classUnderTest.HackneyGazetteerOutOfBoroughAddress, false);
        }

        [TestCase(GlobalConstants.Gazetteer.Both, false)]
        [TestCase(GlobalConstants.Gazetteer.Both, true)]
        [TestCase(GlobalConstants.Gazetteer.Local, false)]
        [TestCase(GlobalConstants.Gazetteer.Local, true)]
        public void GivenAHackneyGazetteerOutOfBoroughAddressInputValueAndAnyGazetteerValue_WhenSearchAddressRequestObjectIsCreated_HackneyGazetteerOutOfBoroughAddressIsSetToWhateverItsInputWas(GlobalConstants.Gazetteer gazetteer, bool? hackneyGazetteerOutOfBoroughAddress)
        {
            var _classUnderTest = new SearchAddressRequest() { Gazetteer = gazetteer, HackneyGazetteerOutOfBoroughAddress = hackneyGazetteerOutOfBoroughAddress };
            Assert.AreEqual(_classUnderTest.HackneyGazetteerOutOfBoroughAddress, hackneyGazetteerOutOfBoroughAddress);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GivenNoInputForGazetteerAndInputValueForHackneyGazetteerOutOfBoroughAddress_WhenSearchAddressRequestObjectIsCreated_HackneyGazetteerOutOfBoroughAddressIsSetToWhateverItsInputWas(bool? hackneyGazetteerOutOfBoroughAddress)
        {
            var _classUnderTest = new SearchAddressRequest() { HackneyGazetteerOutOfBoroughAddress = hackneyGazetteerOutOfBoroughAddress };
            Assert.AreEqual(_classUnderTest.HackneyGazetteerOutOfBoroughAddress, hackneyGazetteerOutOfBoroughAddress);
        }
        #endregion

    }
}
