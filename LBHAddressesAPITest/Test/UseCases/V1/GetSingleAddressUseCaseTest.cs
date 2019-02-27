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
            _fakeGateway.Setup(s => s.GetSingleAddressAsync(It.Is<GetAddressRequest>(i => i.addressID.Equals("ABCDEFGHIJKLMN")), CancellationToken.None)).ReturnsAsync(new AddressDetailed());

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
                .ReturnsAsync(null as AddressDetailed);

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
            var address = new AddressDetailed
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

            var x = (AddressDetailed) response.Addresses[0];
            x.AddressID.Should().BeEquivalentTo(address.AddressID);
            x.AddressID.Should().NotBeNullOrEmpty();

            x.UPRN.Should().Be(address.UPRN);
            x.USRN.Should().Be(address.USRN);

            x.addressStatus.Should().NotBeNullOrEmpty();
            x.addressStatus.Should().BeEquivalentTo(address.addressStatus);

            x.unitName.Should().BeEquivalentTo(address.unitName);
            x.unitNumber.Should().BeEquivalentTo(address.unitNumber);
            x.buildingName.Should().BeEquivalentTo(address.buildingName);
            x.buildingNumber.Should().BeEquivalentTo(address.buildingNumber);

            x.street.Should().NotBeNullOrEmpty();
            x.street.Should().BeEquivalentTo(address.street);

            x.postcode.Should().BeEquivalentTo(address.postcode);

            x.locality.Should().BeEquivalentTo(address.locality);

            x.gazetteer.Should().NotBeNullOrEmpty();
            x.gazetteer.Should().BeEquivalentTo(address.gazetteer);

            x.commercialOccupier.Should().BeEquivalentTo(address.commercialOccupier);
            x.royalMailPostTown.Should().BeEquivalentTo(address.royalMailPostTown);
            x.usageClassDescription.Should().BeEquivalentTo(address.usageClassDescription);
            x.usageClassPrimary.Should().BeEquivalentTo(address.usageClassPrimary);
            x.usageClassCode.Should().BeEquivalentTo(address.usageClassCode);
            x.propertyShell.Should().Be(address.propertyShell);
            x.isNonLocalAddressInLocalGazetteer.Should().Be(address.isNonLocalAddressInLocalGazetteer);
            x.easting.Should().Be(address.easting);
            x.northing.Should().Be(address.northing);

            x.longitude.Should().Be(address.longitude);
            x.latitude.Should().Be(address.latitude);
        }
    }
}
