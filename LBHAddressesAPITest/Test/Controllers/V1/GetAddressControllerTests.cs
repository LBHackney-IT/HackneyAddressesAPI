using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentAssertions;
using LBHAddressesAPI.Controllers.V1;
using LBHAddressesAPI.Models;
using LBHAddressesAPI.UseCases.V1.Addresses;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LBHAddressesAPITest.Test.Controllers.V1
{
    public class GetAddressControllerTests
    {
        private GetAddressController _classUnderTest;
        private Mock<IGetSingleAddressUseCase> _mock;

        public GetAddressControllerTests()
        {
            _mock = new Mock<IGetSingleAddressUseCase>();
            _classUnderTest = new GetAddressController(_mock.Object);
        }

        [Fact]
        public async Task GivenValidSearchAddressRequest_WhenCallingGet_ThenShouldReturn200()
        {
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<GetAddressRequest>(), CancellationToken.None))
                .ReturnsAsync(new SearchAddressResponse
                {
                    Addresses = new List<AddressDetails>
                    {

                    }
                });
            var lpi_key = "ABCDEFGHIJKLMN";

            var response = await _classUnderTest.GetAddress(lpi_key).ConfigureAwait(false);

            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            objectResult.StatusCode.Should().Be(200);

        }



        



    }
}
