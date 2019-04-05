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
    public class GetAddressCrossReferenceUseCaseTest
    {
        private readonly IGetAddressCrossReferenceUseCase _classUnderTest;
        private readonly Mock<IAddressesGateway> _fakeGateway;


        public GetAddressCrossReferenceUseCaseTest()
        {
            _fakeGateway = new Mock<IAddressesGateway>();

            _classUnderTest = new GetAddressCrossReferenceUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenValidInput__WhenExecuteAsync_GatewayReceivesCorrectInputLength()
        {

            var uprn = 1234578912; 
            _fakeGateway.Setup(s => s.GetAddressCrossReferenceAsync(It.Is<GetAddressCrossReferenceRequest>(i => i.uprn.Equals(1234578912)), CancellationToken.None)).ReturnsAsync(new List<AddressCrossReference>());

            var request = new GetAddressCrossReferenceRequest
            {
                uprn = uprn
            };
                
            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);

            _fakeGateway.Verify(v => v.GetAddressCrossReferenceAsync(It.Is<GetAddressCrossReferenceRequest>(i => i.uprn.Equals(1234578912)), CancellationToken.None));
        }

        [Fact]
        public async Task GivenValidInput_WhenGatewayRespondsWithEmptyList_ThenResponseShouldBeEmptyList()
        {
            //arrange
            var uprn = 1234578912;

            _fakeGateway.Setup(s => s.GetAddressCrossReferenceAsync(It.Is<GetAddressCrossReferenceRequest>(i => i.uprn.Equals(1234578912)), CancellationToken.None))
                .ReturnsAsync(new List<AddressCrossReference>());

            var request = new GetAddressCrossReferenceRequest
            {
                uprn = uprn
            };
            //act
            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            response.AddressCrossReferences.Count.Should().Equals(0);// .Should().BeNull();
        }

        [Fact]
        public async Task GivenUPRN_WhenExecuteAsync_ThenMatchingCrossReferencesShouldBeReturned()
        {
            var crossReferences = new List<AddressCrossReference>
            {
                new AddressCrossReference
                {
                    UPRN = 10024389298, code = "", crossRefKey ="" , name ="" , value ="", endDate = DateTime.Today
                }
            };

            long uprn = 10024389298;
            var request = new GetAddressCrossReferenceRequest
            {
                uprn = uprn
            };
            _fakeGateway.Setup(s => s.GetAddressCrossReferenceAsync(It.Is<GetAddressCrossReferenceRequest>(i => i.uprn.Equals(10024389298)), CancellationToken.None))
                .ReturnsAsync(crossReferences);

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            response.Should().NotBeNull();
            response.AddressCrossReferences.Count().Should().Equals(1);
            response.AddressCrossReferences.FirstOrDefault().UPRN.Should().Equals(uprn);
        }

        [Fact]
        public async Task GivenUPRN_WhenExecuteAsync_ThenOnlyMatchingCrossReferencesShouldBeReturned()
        {
            var crossReferences = new List<AddressCrossReference>
            {
                new AddressCrossReference
                {
                    UPRN = 10024389298, code = "", crossRefKey ="" , name ="" , value ="", endDate = DateTime.Today
                },
                new AddressCrossReference
                {
                    UPRN = 10024389298, code = "", crossRefKey ="" , name ="" , value ="", endDate = DateTime.Today
                },
                new AddressCrossReference
                {
                    UPRN = 10024389291, code = "", crossRefKey ="" , name ="" , value ="", endDate = DateTime.Today
                }
            };

            long uprn = 10024389298;
            var request = new GetAddressCrossReferenceRequest
            {
                uprn = uprn
            };
            _fakeGateway.Setup(s => s.GetAddressCrossReferenceAsync(It.Is<GetAddressCrossReferenceRequest>(i => i.uprn.Equals(10024389298)), CancellationToken.None))
                .ReturnsAsync(crossReferences);

            var response = await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            response.Should().NotBeNull();
            response.AddressCrossReferences.Count().Should().Equals(2);
            response.AddressCrossReferences.FirstOrDefault().UPRN.Should().Equals(uprn);
        }


    }
}
