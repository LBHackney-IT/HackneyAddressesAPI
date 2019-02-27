using LBHAddressesAPI.Controllers.V1;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using LBHAddressesAPI.UseCases.V1.Addresses;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using System.Threading;
using FluentAssertions;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LBHAddressesAPI.Models;
using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Helpers;

namespace LBHAddressesAPITest.Test.Controllers.V1
{
    public class SearchAddressControllerTests
    {
        private SearchAddressController _classUnderTest;
        private Mock<ISearchAddressUseCase> _mock;

        public SearchAddressControllerTests()
        {
            _mock = new Mock<ISearchAddressUseCase>();
            _classUnderTest = new SearchAddressController(_mock.Object);
        }

        [Fact]
        public async Task GivenValidSearchAddressRequest_WhenCallingGet_ThenShouldReturnAPIResponseListOfAddresses()
        {
            //arrange
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<SearchAddressRequest>(), CancellationToken.None))
                .ReturnsAsync(new SearchAddressResponse
                {
                    Addresses = new List<AddressDetailed>
                    {

                    }
                });

            var request = new SearchAddressRequest
            {
                PostCode = "",
                Gazetteer = GlobalConstants.Gazetteer.Local
            };
            //act
            var response = await _classUnderTest.GetAddresses(request).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            var getAddresses = objectResult?.Value as APIResponse<SearchAddressResponse>;
            getAddresses.Should().NotBeNull();
        }
    }
}
