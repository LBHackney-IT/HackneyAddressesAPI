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
using LBHAddressesAPI.Infrastructure.V1.Exceptions;

namespace LBHAddressesAPITest
{
    public class GetSingleAddressUseCaseTest
    {
        private readonly IGetSingleAddressUseCase _classUnderTest;
        private readonly Mock<IAddressesGateway> _fakeGateway;


        public GetSingleAddressUseCaseTest()
        {
            _fakeGateway = new Mock<IAddressesGateway>();

            _classUnderTest = new GetSingleAddressUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenValidInput__WhenExecuteAsync_GatewayReceivesCorrectInputLength()
        {

            var lpi_key = "ABCDEFGHIJKLMN"; //14 characters
            _fakeGateway.Setup(s => s.GetSingleAddressAsync(It.Is<GetAddressRequest>(i => i.addressID.Equals("ABCDEFGHIJKLMN")), CancellationToken.None)).ReturnsAsync(new AddressDetails());

            var request = new GetAddressRequest
            {
                addressID = lpi_key
            };
                
            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            _fakeGateway.Verify(v => v.GetSingleAddressAsync(It.Is<GetAddressRequest>(i => i.addressID.Equals("ABCDEFGHIJKLMN")), CancellationToken.None));
        }

        [Fact]
        public async Task GivenBlankStringInput_WhenExecuteAsync_ThenShouldThrowException()
        {
            //arrange
            GetAddressRequest request = new GetAddressRequest { addressID = string.Empty } ;
            //act
            var exception = await Assert.ThrowsAsync<BadRequestException>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
            Assert.Equal("addressID must be provided", exception.ValidationResponse.ValidationErrors.FirstOrDefault().Message);
        }

        [Fact]
        public async Task GivenString13Characters_WhenExectueAsync_TheShouldThrowException()
        {
            GetAddressRequest request = new GetAddressRequest { addressID = "ABCDEFGHIJKLM" };
            //act
            //assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
            Assert.Equal("addressID must be 14 characters", exception.ValidationResponse.ValidationErrors.FirstOrDefault().Message);
        }

        [Fact]
        public async Task GivenString15Characters_WhenExectueAsync_TheShouldThrowException()
        {
            GetAddressRequest request = new GetAddressRequest { addressID = "ABCDEFGHIJKLMNO" };
            //act
            //assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
            Assert.Equal("addressID must be 14 characters", exception.ValidationResponse.ValidationErrors.FirstOrDefault().Message);
        }

        [Fact]
        public async Task GivenValidInput_WhenGatewayRespondsWithNull_ThenResponseShouldBeNull()
        {
            //arrange
            var lpi_key = "ABCDEFGHIJKLMN";
            
            _fakeGateway.Setup(s => s.GetSingleAddressAsync(It.Is<GetAddressRequest>(i => i.addressID.Equals("ABCDEFGHIJKLMN")), CancellationToken.None))
                .ReturnsAsync(null as AddressDetails);

            var request = new GetAddressRequest
            {
                addressID = lpi_key
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Addresses.Should().BeNull();
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
            var request = new GetAddressRequest
            {
                addressID = lpi_key
            };
            _fakeGateway.Setup(s => s.GetSingleAddressAsync(It.Is<GetAddressRequest>(i => i.addressID.Equals("ABCDEFGHIJKLMN")), CancellationToken.None))
                .ReturnsAsync(address);

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Should().NotBeNull();
            response.Addresses[0].AddressID.Should().BeEquivalentTo(address.AddressID);
            response.Addresses[0].AddressID.Should().NotBeNullOrEmpty();

            response.Addresses[0].UPRN.Should().Be(address.UPRN);
            response.Addresses[0].USRN.Should().Be(address.USRN);

            response.Addresses[0].addressStatus.Should().NotBeNullOrEmpty();
            response.Addresses[0].addressStatus.Should().BeEquivalentTo(address.addressStatus);

            response.Addresses[0].unitName.Should().BeEquivalentTo(address.unitName);
            response.Addresses[0].unitNumber.Should().BeEquivalentTo(address.unitNumber);
            response.Addresses[0].buildingName.Should().BeEquivalentTo(address.buildingName);
            response.Addresses[0].buildingNumber.Should().BeEquivalentTo(address.buildingNumber);

            response.Addresses[0].street.Should().NotBeNullOrEmpty();
            response.Addresses[0].street.Should().BeEquivalentTo(address.street);

            response.Addresses[0].postcode.Should().BeEquivalentTo(address.postcode);

            response.Addresses[0].locality.Should().BeEquivalentTo(address.locality);

            response.Addresses[0].gazetteer.Should().NotBeNullOrEmpty();
            response.Addresses[0].gazetteer.Should().BeEquivalentTo(address.gazetteer);

            response.Addresses[0].commercialOccupier.Should().BeEquivalentTo(address.commercialOccupier);
            response.Addresses[0].royalMailPostTown.Should().BeEquivalentTo(address.royalMailPostTown);
            response.Addresses[0].usageClassDescription.Should().BeEquivalentTo(address.usageClassDescription);
            response.Addresses[0].usageClassPrimary.Should().BeEquivalentTo(address.usageClassPrimary);
            response.Addresses[0].usageClassCode.Should().BeEquivalentTo(address.usageClassCode);
            response.Addresses[0].propertyShell.Should().Be(address.propertyShell);
            response.Addresses[0].isNonLocalAddressInLocalGazetteer.Should().Be(address.isNonLocalAddressInLocalGazetteer);
            response.Addresses[0].easting.Should().Be(address.easting);
            response.Addresses[0].northing.Should().Be(address.northing);

            response.Addresses[0].longitude.Should().Be(address.longitude);
            response.Addresses[0].latitude.Should().Be(address.latitude);
        }
    }
}
