using System;
using Xunit;
using HackneyAddressesAPI.Models;
using System.Collections.Generic;
using Moq;
using System.Linq;
using FluentAssertions;
using LBHAddressesAPI.Gateways.V1;
using LBHAddressesAPI.UseCases.V1.Addresses;
using System.Threading.Tasks;

namespace LBHAddressesAPITest
{
    public class GetAddressUseCaseTest
    {
        private readonly IGetAddressUseCase _classUnderTest;
        private readonly Mock<IAddressesGateway> _fakeGateway;


        public GetAddressUseCaseTest()
        {
            _fakeGateway = new Mock<IAddressesGateway>();

            _classUnderTest = new GetAddressUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenValidInput__WhenExecuteAsync_GatewayReceivesCorrectInputLength()
        {
            var lpi_key = "ABCDEFGHIJKLMN"; //14 characters
            _fakeGateway.Setup(s => s.GetAddressAsync(It.Is<string>(i => i.Equals("ABCDEFGHIJKLMN")))).ReturnsAsync(new AddressDetails());

            var request = lpi_key;

            await _classUnderTest.ExecuteAsync(request);

            _fakeGateway.Verify(v => v.GetAddressAsync(It.Is<string>(i => i.Equals("ABCDEFGHIJKLMN"))));
        }

        [Fact]
        public async Task GivenNullInput_WhenExecuteAsync_ThenShouldThrowException()
        {
            //arrange
            string request = null;
            //act
            //assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.ExecuteAsync(request));
            Assert.Equal("lpi_key must be provided", exception.Message);
        }

        [Fact]
        public async Task GivenBlankStringInput_WhenExecuteAsync_ThenShouldThrowException()
        {
            //arrange
            string request = string.Empty;
            //act
            //assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.ExecuteAsync(request));
            Assert.Equal("lpi_key must be provided", exception.Message);
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLM")]//13 characters
        [InlineData("ABCDEFGHIJKLMNOP")] //15 characters
        public async Task GivenStringNot14Characters_WhenExectueAsync_TheShouldThrowException(string lpi_key)
        {

            //act
            //assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.ExecuteAsync(lpi_key));
            Assert.Equal("lpi_key must be 14 characters", exception.Message);
        }

        [Fact]
        public async Task GivenValidInput_WhenGatewayRespondsWithNull_ThenResponseShouldBeNull()
        {
            //arrange
            var lpi_key = "ABCDEFGHIJKLMN";
            _fakeGateway.Setup(s => s.GetAddressAsync(It.Is<string>(i => i.Equals("ABCDEFGHIJKLMN"))))
                .ReturnsAsync(null as AddressDetails);

            var request = lpi_key;
            //act
            var response = await _classUnderTest.ExecuteAsync(request);
            //assert
            response.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidLPIKey_WhenExecuteAsync_ThenAddressShouldBeReturned()
        {
            var address = new AddressDetails
            {
                AddressID = "ABCDEFGHIJKLMN",
                UPRN = 10024389298,
                USRN = 21320239,
                parentUPRN = 10024389282,
                addressStatus = "Approved Preferred",
                unitName = "FLAT 16",
                unitNumber = "",
                buildingName = "HAZELNUT COURT",
                buildingNumber = "1",
                street = "FIRWOOD LANE",
                postcode = "RM3 0FS",
                locality = "",
                gazetteer = "NATIONAL",
                commercialOccupier = "",
                royalMailPostTown = "",
                usageClassDescription = "Unclassified, Awaiting Classification",
                usageClassPrimary = "Unclassified",
                usageClassCode = "UC",
                propertyShell = false,
                isNonLocalAddressInLocalGazetteer = false,
                easting = 554189.4500,
                northing = 190281.1000,
                longitude = 0.2244347,
                latitude = 51.590289

            };

            var lpi_key = "ABCDEFGHIJKLMN";
            _fakeGateway.Setup(s => s.GetAddressAsync(It.Is<string>(i => i.Equals("ABCDEFGHIJKLMN"))))
                .ReturnsAsync(address);

            var response = await _classUnderTest.ExecuteAsync(lpi_key);

            response.Should().NotBeNull();
            response.AddressID.Should().BeEquivalentTo(address.AddressID);
            response.AddressID.Should().NotBeNullOrEmpty();

            response.UPRN.Should().Be(address.UPRN);
            response.USRN.Should().Be(address.USRN);

            response.addressStatus.Should().NotBeNullOrEmpty();
            response.addressStatus.Should().BeEquivalentTo(address.addressStatus);

            response.unitName.Should().BeEquivalentTo(address.unitName);
            response.unitNumber.Should().BeEquivalentTo(address.unitNumber);
            response.buildingName.Should().BeEquivalentTo(address.buildingName);
            response.buildingNumber.Should().BeEquivalentTo(address.buildingNumber);

            response.street.Should().NotBeNullOrEmpty();
            response.street.Should().BeEquivalentTo(address.street);

            response.postcode.Should().BeEquivalentTo(address.postcode);

            response.locality.Should().BeEquivalentTo(address.locality);

            response.gazetteer.Should().NotBeNullOrEmpty();
            response.gazetteer.Should().BeEquivalentTo(address.gazetteer);

            response.commercialOccupier.Should().BeEquivalentTo(address.commercialOccupier);
            response.royalMailPostTown.Should().BeEquivalentTo(address.royalMailPostTown);
            response.usageClassDescription.Should().BeEquivalentTo(address.usageClassDescription);
            response.usageClassPrimary.Should().BeEquivalentTo(address.usageClassPrimary);
            response.usageClassCode.Should().BeEquivalentTo(address.usageClassCode);
            response.propertyShell.Should().Be(address.propertyShell);
            response.isNonLocalAddressInLocalGazetteer.Should().Be(address.isNonLocalAddressInLocalGazetteer);
            response.easting.Should().Be(address.easting);
            response.northing.Should().Be(address.northing);

            response.longitude.Should().Be(address.longitude);
            response.latitude.Should().Be(address.latitude);
        }
    }
}
