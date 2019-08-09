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
using Microsoft.AspNetCore.Http;

namespace LBHAddressesAPITest.Test.Controllers.V1
{
    public class SearchAddressControllerTests
    {
        private SearchAddressController _classUnderTest;
        private Mock<ISearchAddressUseCase> _mock;

        public SearchAddressControllerTests()
        {
            Environment.SetEnvironmentVariable("ALLOWED_ADDRESSSTATUS_VALUES", "historical;alternative;approved preferred;provisional");
            _mock = new Mock<ISearchAddressUseCase>();

            _classUnderTest = new SearchAddressController(_mock.Object);
            _classUnderTest.ControllerContext = new ControllerContext();
            _classUnderTest.ControllerContext.HttpContext = new DefaultHttpContext();
            _classUnderTest.ControllerContext.HttpContext.Request.QueryString = new QueryString("");
        }


        [Theory]
        [InlineData("RM3 0FS", GlobalConstants.Gazetteer.Local)]
        [InlineData("IG11 7QD", GlobalConstants.Gazetteer.Both)]
        public async Task GivenValidSearchAddressRequest_WhenCallingGet_ThenShouldReturnAPIResponseListOfAddresses(string postcode, GlobalConstants.Gazetteer gazetteer)
        {
            //arrange
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<SearchAddressRequest>(), CancellationToken.None))
                .ReturnsAsync(new SearchAddressResponse
                {
                    Addresses = new List<AddressBase>
                    {

                    }
                });

            var request = new SearchAddressRequest
            {
                PostCode = postcode,
                Gazetteer = gazetteer
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

        [Fact]
        public async Task GivenInvalidSearchAddressRequest_WhenCallingGet_ThenShouldReturnBadRequestObjectResponse()
        {
            //arrange
            var request = new SearchAddressRequest() { AddressStatus = null };

            //act
            var response = await _classUnderTest.GetAddresses(request);

            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
