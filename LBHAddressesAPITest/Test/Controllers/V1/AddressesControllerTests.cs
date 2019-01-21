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
    public class AddressesControllerTests
    {
        private AddressesController _classUnderTest;
        private Mock<IGetAddressUseCase> _mock;

        public AddressesControllerTests()
        {
            _mock = new Mock<IGetAddressUseCase>();
            _classUnderTest = new AddressesController(_mock.Object);
        }

        [Fact]
        public async Task GivenValidSearchAddressRequest_WhenCallingGet_ThenShouldReturn200()
        {
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<SearchAddressRequest>(), CancellationToken.None))
                .ReturnsAsync(new SearchAddressResponse
                {
                    Address = new AddressDetails
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
