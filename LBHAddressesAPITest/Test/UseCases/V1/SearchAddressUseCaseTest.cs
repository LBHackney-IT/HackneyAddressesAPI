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
using LBHAddressesAPI.Infrastructure.V1.API;

namespace LBHAddressesAPITest
{
    public class SearchAddressUseCaseTest
    {
        private readonly ISearchAddressUseCase _classUnderTest;
        private readonly Mock<IAddressesGateway> _fakeGateway;


        public SearchAddressUseCaseTest()
        {
            _fakeGateway = new Mock<IAddressesGateway>();

            _classUnderTest = new SearchAddressUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenInvalidInput_WhenExecuteAsync_ThenReturnError()
        {
            var request = new SearchAddressRequest
            {                
                Gazetteer = LBHAddressesAPI.Helpers.GlobalConstants.Gazetteer.Local
            };
            var exception = await Assert.ThrowsAsync<BadRequestException>(async () => await _classUnderTest.ExecuteAsync(request, CancellationToken.None));
            Assert.Equal("No filter parameters have been provided", exception.ValidationResponse.ValidationErrors.FirstOrDefault().Message);
        }


        [Fact]
        public async Task GivenLocalGazetteer_WhenExecuteAsync_ThenOnlyLocalAddressesShouldBeReturned()
        {
            var addresses = new List<AddressBase>
            {
                new AddressDetailed
                {
                    AddressKey = "ABCDEFGHIJKLMN", UPRN = 10024389298,USRN = 21320239,ParentUPRN = 10024389282,AddressStatus = "Approved Preferred",UnitName = "FLAT 16",UnitNumber = "",BuildingName = "HAZELNUT COURT",BuildingNumber = "1",Street = "FIRWOOD LANE",Postcode = "RM3 0FS",Locality = "",Gazetteer = "NATIONAL",CommercialOccupier = "",UsageDescription = "Unclassified, Awaiting Classification",UsagePrimary = "Unclassified", UsageCode = "UC",PropertyShell = false,HackneyGazetteerOutOfBoroughAddress = false,Easting = 554189.4500,Northing = 190281.1000,Longitude = 0.2244347,Latitude = 51.590289
                },
                new AddressDetailed
                {
                    AddressKey = "ABCDEFGHIJKLM2", UPRN = 10024389298,USRN = 21320239,ParentUPRN = 10024389282,AddressStatus = "Approved Preferred",UnitName = "FLAT 16",UnitNumber = "",BuildingName = "HAZELNUT COURT",BuildingNumber = "1",Street = "FIRWOOD LANE",Postcode = "RM3 0FS",Locality = "",Gazetteer = "LOCAL",CommercialOccupier  = "",UsageDescription = "Unclassified, Awaiting Classification",UsagePrimary = "Unclassified", UsageCode = "UC",PropertyShell = false,HackneyGazetteerOutOfBoroughAddress = false,Easting = 554189.4500,Northing = 190281.1000,Longitude = 0.2244347,Latitude = 51.590289
                }
            };

            var Postcode = "RM3 0FS";
            var Gazetteer = "LOCAL";
            var request = new SearchAddressRequest
            {
                PostCode = Postcode,
                Gazetteer = LBHAddressesAPI.Helpers.GlobalConstants.Gazetteer.Local
            };
            _fakeGateway.Setup(s => s.SearchAddressesAsync(It.Is<SearchAddressRequest>(i => i.PostCode.Equals("RM3 0FS") && i.Gazetteer == LBHAddressesAPI.Helpers.GlobalConstants.Gazetteer.Local), CancellationToken.None))
                .ReturnsAsync(new PagedResults<AddressBase>
                {
                    Results = addresses,
                    TotalResultsCount = 1
                });

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            response.Should().NotBeNull();
            response.Addresses.Count().Should().Equals(1);
            response.TotalCount.Should().Equals(1);
            //response.Addresses[0].Gazetteer.Should().Equals(Gazetteer);
        }



        [Fact]
        public async Task GivenValidInput_WhenGatewayRespondsWithNull_ThenResponseShouldBeNull()
        {
            //arrange
            var Postcode = "RM3 0FS";
            
            _fakeGateway.Setup(s => s.SearchAddressesAsync(It.Is<SearchAddressRequest>(i => i.PostCode.Equals("ABCDEFGHIJKLMN")), CancellationToken.None))
                .ReturnsAsync(null as PagedResults<AddressBase>);

            var request = new SearchAddressRequest
            {
                PostCode = Postcode
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Addresses.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidPostCode_WhenExecuteAsync_ThenMultipleAddressesShouldBeReturned()
        {
            var addresses = new List<AddressBase>
            {
                new AddressDetailed
                {
                    AddressKey = "ABCDEFGHIJKLMN", UPRN = 10024389298,USRN = 21320239,ParentUPRN = 10024389282,AddressStatus = "Approved Preferred",UnitName = "FLAT 16",UnitNumber = "",BuildingName = "HAZELNUT COURT",BuildingNumber = "1",Street = "FIRWOOD LANE",Postcode = "RM3 0FS",Locality = "",Gazetteer = "NATIONAL",CommercialOccupier = "",UsageDescription = "Unclassified, Awaiting Classification",UsagePrimary = "Unclassified",                UsageCode = "UC",PropertyShell = false,HackneyGazetteerOutOfBoroughAddress = false,Easting = 554189.4500,Northing = 190281.1000,Longitude = 0.2244347,Latitude = 51.590289
                },
                new AddressDetailed
                {
                    AddressKey = "ABCDEFGHIJKLM2", UPRN = 10024389298,USRN = 21320239,ParentUPRN = 10024389282,AddressStatus = "Approved Preferred",UnitName = "FLAT 16",UnitNumber = "",BuildingName = "HAZELNUT COURT",BuildingNumber = "1",Street = "FIRWOOD LANE",Postcode = "RM3 0FS",Locality = "",Gazetteer = "NATIONAL",CommercialOccupier = "",UsageDescription = "Unclassified, Awaiting Classification",UsagePrimary = "Unclassified",                UsageCode = "UC",PropertyShell = false,HackneyGazetteerOutOfBoroughAddress = false,Easting = 554189.4500,Northing = 190281.1000,Longitude = 0.2244347,Latitude = 51.590289
                }
            };

            var Postcode = "RM3 0FS";
            var request = new SearchAddressRequest
            {
                PostCode = Postcode
            };
            _fakeGateway.Setup(s => s.SearchAddressesAsync(It.Is<SearchAddressRequest>(i => i.PostCode.Equals("RM3 0FS")), CancellationToken.None))
                .ReturnsAsync(new PagedResults<AddressBase>
                {
                    Results = addresses,
                    TotalResultsCount = 2
                });

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            response.Should().NotBeNull();
            response.Addresses.Count().Should().Equals(2);
            response.TotalCount.Should().Equals(2);
        }


        [Fact]
        public async Task GivenValidPostCode_WhenExecuteAsync_ThenAddressShouldBeReturned()
        {
            var addresses = new List<AddressBase>();
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
            addresses.Add(address);
            var Postcode = "RM3 0FS";
            var request = new SearchAddressRequest
            {
                PostCode = Postcode
            };
            _fakeGateway.Setup(s => s.SearchAddressesAsync(It.Is<SearchAddressRequest>(i => i.PostCode.Equals("RM3 0FS")), CancellationToken.None))
                .ReturnsAsync(new PagedResults<AddressBase>
                {
                    Results = addresses
                });

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Should().NotBeNull();
            var x = (AddressDetailed)response.Addresses[0];

            x.AddressKey.Should().BeEquivalentTo(address.AddressKey);
            x.Postcode.Should().NotBeNullOrEmpty();

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
