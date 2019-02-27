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
            var addresses = new List<AddressBase>
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
                .ReturnsAsync(new PagedResults<AddressBase>
                {
                    Results = addresses,
                    TotalResultsCount = 1
                });

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            response.Should().NotBeNull();
            response.Addresses.Count().Should().Equals(1);
            response.TotalCount.Should().Equals(1);
            //response.Addresses[0].gazetteer.Should().Equals(gazetteer);
        }



        [Fact]
        public async Task GivenValidInput_WhenGatewayRespondsWithNull_ThenResponseShouldBeNull()
        {
            //arrange
            var postcode = "RM3 0FS";
            
            _fakeGateway.Setup(s => s.SearchAddressesAsync(It.Is<SearchAddressRequest>(i => i.PostCode.Equals("ABCDEFGHIJKLMN")), CancellationToken.None))
                .ReturnsAsync(null as PagedResults<AddressBase>);

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
            var addresses = new List<AddressBase>
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
                .ReturnsAsync(new PagedResults<AddressBase>
                {
                    Results = addresses
                });

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            response.Should().NotBeNull();
            var x = (AddressDetailed)response.Addresses[0];

            x.AddressID.Should().BeEquivalentTo(address.AddressID);
            x.postcode.Should().NotBeNullOrEmpty();

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
