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
        public async Task GivenLocalGazetteer_WhenExecuteAsync_ThenOnlyLocalAddressesShouldBeReturned()
        {
            var addresses = new List<AddressDetailed>
            {
                new AddressDetailed
                {
                    AddressID = "ABCDEFGHIJKLMN", UPRN = 10024389298,USRN = 21320239,parentUPRN = 10024389282,addressStatus = "Approved Preferred",unitName = "FLAT 16",unitNumber = "",buildingName = "HAZELNUT COURT",buildingNumber = "1",street = "FIRWOOD LANE",postcode = "RM3 0FS",locality = "",gazetteer = "NATIONAL",commercialOccupier = "",royalMailPostTown = "",usageClassDescription = "Unclassified, Awaiting Classification",usageClassPrimary = "Unclassified", usageClassCode = "UC",propertyShell = false,isNonLocalAddressInLocalGazetteer = false,easting = 554189.4500,northing = 190281.1000,longitude = 0.2244347,latitude = 51.590289
                },
                new AddressDetailed
                {
                    AddressID = "ABCDEFGHIJKLM2", UPRN = 10024389298,USRN = 21320239,parentUPRN = 10024389282,addressStatus = "Approved Preferred",unitName = "FLAT 16",unitNumber = "",buildingName = "HAZELNUT COURT",buildingNumber = "1",street = "FIRWOOD LANE",postcode = "RM3 0FS",locality = "",gazetteer = "LOCAL",commercialOccupier = "",royalMailPostTown = "",usageClassDescription = "Unclassified, Awaiting Classification",usageClassPrimary = "Unclassified", usageClassCode = "UC",propertyShell = false,isNonLocalAddressInLocalGazetteer = false,easting = 554189.4500,northing = 190281.1000,longitude = 0.2244347,latitude = 51.590289
                }
            };

            var postcode = "RM3 0FS";
            var gazetteer = "LOCAL";
            var request = new SearchAddressRequest
            {
                PostCode = postcode,
                Gazetteer = LBHAddressesAPI.Helpers.GlobalConstants.Gazetteer.Local
            };
            _fakeGateway.Setup(s => s.SearchAddressesAsync(It.Is<SearchAddressRequest>(i => i.PostCode.Equals("RM3 0FS") && i.Gazetteer == LBHAddressesAPI.Helpers.GlobalConstants.Gazetteer.Local), CancellationToken.None))
                .ReturnsAsync(new PagedResults<AddressDetailed>
                {
                    Results = addresses,
                    TotalResultsCount = 1
                });

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            response.Should().NotBeNull();
            response.Addresses.Count().Should().Equals(1);
            response.TotalCount.Should().Equals(1);
            response.Addresses[0].gazetteer.Should().Equals(gazetteer);
        }



        [Fact]
        public async Task GivenValidInput_WhenGatewayRespondsWithNull_ThenResponseShouldBeNull()
        {
            //arrange
            var postcode = "RM3 0FS";
            
            _fakeGateway.Setup(s => s.SearchAddressesAsync(It.Is<SearchAddressRequest>(i => i.PostCode.Equals("ABCDEFGHIJKLMN")), CancellationToken.None))
                .ReturnsAsync(null as PagedResults<AddressDetailed>);

            var request = new SearchAddressRequest
            {
                PostCode = postcode
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.Addresses.Should().BeNull();
        }

        [Fact]
        public async Task GivenValidPostCode_WhenExecuteAsync_ThenMultipleAddressesShouldBeReturned()
        {
            var addresses = new List<AddressDetailed>
            {
                new AddressDetailed
                {
                    AddressID = "ABCDEFGHIJKLMN", UPRN = 10024389298,USRN = 21320239,parentUPRN = 10024389282,addressStatus = "Approved Preferred",unitName = "FLAT 16",unitNumber = "",buildingName = "HAZELNUT COURT",buildingNumber = "1",street = "FIRWOOD LANE",postcode = "RM3 0FS",locality = "",gazetteer = "NATIONAL",commercialOccupier = "",royalMailPostTown = "",usageClassDescription = "Unclassified, Awaiting Classification",usageClassPrimary = "Unclassified",                usageClassCode = "UC",propertyShell = false,isNonLocalAddressInLocalGazetteer = false,easting = 554189.4500,northing = 190281.1000,longitude = 0.2244347,latitude = 51.590289
                },
                new AddressDetailed
                {
                    AddressID = "ABCDEFGHIJKLM2", UPRN = 10024389298,USRN = 21320239,parentUPRN = 10024389282,addressStatus = "Approved Preferred",unitName = "FLAT 16",unitNumber = "",buildingName = "HAZELNUT COURT",buildingNumber = "1",street = "FIRWOOD LANE",postcode = "RM3 0FS",locality = "",gazetteer = "NATIONAL",commercialOccupier = "",royalMailPostTown = "",usageClassDescription = "Unclassified, Awaiting Classification",usageClassPrimary = "Unclassified",                usageClassCode = "UC",propertyShell = false,isNonLocalAddressInLocalGazetteer = false,easting = 554189.4500,northing = 190281.1000,longitude = 0.2244347,latitude = 51.590289
                }
            };

            var postcode = "RM3 0FS";
            var request = new SearchAddressRequest
            {
                PostCode = postcode
            };
            _fakeGateway.Setup(s => s.SearchAddressesAsync(It.Is<SearchAddressRequest>(i => i.PostCode.Equals("RM3 0FS")), CancellationToken.None))
                .ReturnsAsync(new PagedResults<AddressDetailed>
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
            var addresses = new List<AddressDetailed>();
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
            addresses.Add(address);
            var postcode = "RM3 0FS";
            var request = new SearchAddressRequest
            {
                PostCode = postcode
            };
            _fakeGateway.Setup(s => s.SearchAddressesAsync(It.Is<SearchAddressRequest>(i => i.PostCode.Equals("RM3 0FS")), CancellationToken.None))
                .ReturnsAsync(new PagedResults<AddressDetailed>
                {
                    Results = addresses
                });

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Should().NotBeNull();
            response.Addresses[0].AddressID.Should().BeEquivalentTo(address.AddressID);
            response.Addresses[0].postcode.Should().NotBeNullOrEmpty();

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
