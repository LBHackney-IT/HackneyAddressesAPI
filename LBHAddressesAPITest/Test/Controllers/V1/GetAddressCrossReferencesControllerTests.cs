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
    public class GetAddressCrossReferenceControllerTests
    {
        private GetAddressCrossReferenceController _classUnderTest;
        private Mock<IGetAddressCrossReferenceUseCase> _mock;

        public GetAddressCrossReferenceControllerTests()
        {
            _mock = new Mock<IGetAddressCrossReferenceUseCase>();
            _classUnderTest = new GetAddressCrossReferenceController(_mock.Object);
        }

        [Fact]
        public async Task GivenValidAddressRequest_WhenCallingGet_ThenShouldReturnAPIResponseListOfAddresses()
        {
            //arrange
            _mock.Setup(s => s.ExecuteAsync(It.IsAny<GetAddressCrossReferenceRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetAddressCrossReferenceResponse
                {
                    AddressCrossReferences = new List<AddressCrossReference>
                    {

                    }
                });
            Int64 uprn = 12345;
            
            //act
            var response = await _classUnderTest.GetAddressCrossReference(uprn).ConfigureAwait(false);
            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            var objectResult = response as ObjectResult;
            var getAddresses = objectResult?.Value as APIResponse<GetAddressCrossReferenceResponse>;
            getAddresses.Should().NotBeNull();
        }
    }
}
