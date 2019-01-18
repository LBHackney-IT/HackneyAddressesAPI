using System;
using Xunit;
using LBHAddressesAPI.Models;
using System.Collections.Generic;
using Moq;
using System.Linq;
using FluentAssertions;
using LBHAddressesAPI.Gateways.V1;
using LBHAddressesAPI.UseCases.V1.Addresses;
using System.Threading.Tasks;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using System.Threading;

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
            _fakeGateway.Setup(s => s.GetAddressAsync(It.Is<SearchAddressRequest>(i => i.addressID.Equals("ABCDEFGHIJKLMN")), CancellationToken.None)).ReturnsAsync(new AddressDetails());

            var request = new SearchAddressRequest
            {
                addressID = lpi_key
            };
                
            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            _fakeGateway.Verify(v => v.GetAddressAsync(It.Is<SearchAddressRequest>(i => i.addressID.Equals("ABCDEFGHIJKLMN")), CancellationToken.None));
        }

        [Fact]
        public async Task GivenBlankStringInput_WhenExecuteAsync_ThenShouldThrowException()
        {
            //arrange
            SearchAddressRequest request = new SearchAddressRequest { addressID = string.Empty } ;
            //act
            //assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
            Assert.Equal("lpi_key must be provided", exception.Message);
        }

        [Fact]
        public async Task GivenString13Characters_WhenExectueAsync_TheShouldThrowException()
        {
            SearchAddressRequest request = new SearchAddressRequest { addressID = "ABCDEFGHIJKLM" };
            //act
            //assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
            Assert.Equal("lpi_key must be 14 characters", exception.Message);
        }

        [Fact]
        public async Task GivenString15Characters_WhenExectueAsync_TheShouldThrowException()
        {
            SearchAddressRequest request = new SearchAddressRequest { addressID = "ABCDEFGHIJKLMNO" };
            //act
            //assert
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
            Assert.Equal("lpi_key must be 14 characters", exception.Message);
        }

        [Fact]
        public async Task GivenValidInput_WhenGatewayRespondsWithNull_ThenResponseShouldBeNull()
        {
            //arrange
            var lpi_key = "ABCDEFGHIJKLMN";
            
            _fakeGateway.Setup(s => s.GetAddressAsync(It.Is<SearchAddressRequest>(i => i.addressID.Equals("ABCDEFGHIJKLMN")), CancellationToken.None))
                .ReturnsAsync(null as AddressDetails);

            var request = new SearchAddressRequest
            {
                addressID = lpi_key
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Address.Should().BeNull();
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
            var request = new SearchAddressRequest
            {
                addressID = lpi_key
            };
            _fakeGateway.Setup(s => s.GetAddressAsync(It.Is<SearchAddressRequest>(i => i.addressID.Equals("ABCDEFGHIJKLMN")), CancellationToken.None))
                .ReturnsAsync(address);

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Should().NotBeNull();
            response.Address.AddressID.Should().BeEquivalentTo(address.AddressID);
            response.Address.AddressID.Should().NotBeNullOrEmpty();

            response.Address.UPRN.Should().Be(address.UPRN);
            response.Address.USRN.Should().Be(address.USRN);

            response.Address.addressStatus.Should().NotBeNullOrEmpty();
            response.Address.addressStatus.Should().BeEquivalentTo(address.addressStatus);

            response.Address.unitName.Should().BeEquivalentTo(address.unitName);
            response.Address.unitNumber.Should().BeEquivalentTo(address.unitNumber);
            response.Address.buildingName.Should().BeEquivalentTo(address.buildingName);
            response.Address.buildingNumber.Should().BeEquivalentTo(address.buildingNumber);

            response.Address.street.Should().NotBeNullOrEmpty();
            response.Address.street.Should().BeEquivalentTo(address.street);

            response.Address.postcode.Should().BeEquivalentTo(address.postcode);

            response.Address.locality.Should().BeEquivalentTo(address.locality);

            response.Address.gazetteer.Should().NotBeNullOrEmpty();
            response.Address.gazetteer.Should().BeEquivalentTo(address.gazetteer);

            response.Address.commercialOccupier.Should().BeEquivalentTo(address.commercialOccupier);
            response.Address.royalMailPostTown.Should().BeEquivalentTo(address.royalMailPostTown);
            response.Address.usageClassDescription.Should().BeEquivalentTo(address.usageClassDescription);
            response.Address.usageClassPrimary.Should().BeEquivalentTo(address.usageClassPrimary);
            response.Address.usageClassCode.Should().BeEquivalentTo(address.usageClassCode);
            response.Address.propertyShell.Should().Be(address.propertyShell);
            response.Address.isNonLocalAddressInLocalGazetteer.Should().Be(address.isNonLocalAddressInLocalGazetteer);
            response.Address.easting.Should().Be(address.easting);
            response.Address.northing.Should().Be(address.northing);

            response.Address.longitude.Should().Be(address.longitude);
            response.Address.latitude.Should().Be(address.latitude);
        }
    }
}
