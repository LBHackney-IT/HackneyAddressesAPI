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
                AddressKey = "ABCDEFGHIJKLMN",
                UPRN = 10024389298,
                USRN = 21320239,
                ParentUPRN = 10024389282,
                AddressStatus = "Approved Preferred",
                UnitName = "FLAT 16",
                UnitNumber = "",
                BuildingName = "HAZELNUT COURT",
                BuildingNumber = "1",
                Street = "FIRWOOD LANE",
                Postcode = "RM3 0FS",
                Locality = "",
                Gazetteer = "NATIONAL",
                CommercialOccupier = "",
                UsageDescription = "Unclassified, Awaiting Classification",
                UsagePrimary = "Unclassified",
                UsageCode = "UC",
                PropertyShell = false,
                HackneyGazetteerOutOfBoroughAddress = false,
                Easting = 554189.4500,
                Northing = 190281.1000,
                Longitude = 0.2244347,
                Latitude = 51.590289

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
            x.AddressKey.Should().BeEquivalentTo(address.AddressKey);
            x.AddressKey.Should().NotBeNullOrEmpty();

            x.UPRN.Should().Be(address.UPRN);
            x.USRN.Should().Be(address.USRN);

            x.AddressStatus.Should().NotBeNullOrEmpty();
            x.AddressStatus.Should().BeEquivalentTo(address.AddressStatus);

            x.UnitName.Should().BeEquivalentTo(address.UnitName);
            x.UnitNumber.Should().BeEquivalentTo(address.UnitNumber);
            x.BuildingName.Should().BeEquivalentTo(address.BuildingName);
            x.BuildingNumber.Should().BeEquivalentTo(address.BuildingNumber);

            x.Street.Should().NotBeNullOrEmpty();
            x.Street.Should().BeEquivalentTo(address.Street);

            x.Postcode.Should().BeEquivalentTo(address.Postcode);

            x.Locality.Should().BeEquivalentTo(address.Locality);

            x.Gazetteer.Should().NotBeNullOrEmpty();
            x.Gazetteer.Should().BeEquivalentTo(address.Gazetteer);

            x.CommercialOccupier.Should().BeEquivalentTo(address.CommercialOccupier);
            x.UsageDescription.Should().BeEquivalentTo(address.UsageDescription);
            x.UsagePrimary.Should().BeEquivalentTo(address.UsagePrimary);
            x.UsageCode.Should().BeEquivalentTo(address.UsageCode);
            x.PropertyShell.Should().Be(address.PropertyShell);
            x.HackneyGazetteerOutOfBoroughAddress.Should().Be(address.HackneyGazetteerOutOfBoroughAddress);
            x.Easting.Should().Be(address.Easting);
            x.Northing.Should().Be(address.Northing);

            x.Longitude.Should().Be(address.Longitude);
            x.Latitude.Should().Be(address.Latitude);
        }
    }
}
