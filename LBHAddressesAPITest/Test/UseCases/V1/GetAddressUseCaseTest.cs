using System;
using Xunit;
using HackneyAddressesAPI.Models;
using System.Collections.Generic;
using Moq;
using System.Linq;
using FluentAssertions;
using LBHAddressesAPI.Gateways.V1;
using LBHAddressesAPI.UseCases.V1.Addresses;
using System.Threading.Tasks;

namespace LBHAddressesAPITest
{
    public class GetAddressUseCaseTest
    {
        private readonly IGetAddressUseCase _classUnderTest;
        private readonly Mock<IAddressesGateway> _fakeGateway;
        

        public GetAddressUseCaseTest()
        {
            _fakeGateway = new Mock<IAddressesGateway>();

            _classUnderTest = new GetAddressUseCase(_fakeGateway.Object);
        }

        [Fact]
        public async Task GivenValidInput__WhenExecuteAsync_GatewayReceivesCorrectInput()
        {
            var lpi_key = "ABCDEFGHIJKLMN";
            _fakeGateway.Setup(s => s.GetAddressAsync(It.Is<string>(i => i.Equals("ABCDEFGHIJKLMN")))).ReturnsAsync(new AddressDetails());

            var request = lpi_key;

            await _classUnderTest.ExecuteAsync(request);

            _fakeGateway.Verify(v => v.GetAddressAsync(It.Is<string>(i => i.Equals("ABCDEFGHIJKLMN"))));
        }

        [Fact]
        public async Task GivenNullInput_WhenExecuteAsync_ThenShouldThrowException()
        {
            //arrange
            string request = null;
            //act
            //assert
            // ReSharper disable once ExpressionIsAlwaysNull
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.ExecuteAsync(request));
            Assert.Equal("lpi_key must be provided", exception.Message);
        }

        [Fact]
        public async Task GivenBlankStringInput_WhenExecuteAsync_ThenShouldThrowException()
        {
            //arrange
            string request = string.Empty;
            //act
            //assert
            // ReSharper disable once ExpressionIsAlwaysNull
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.ExecuteAsync(request));
            Assert.Equal("lpi_key must be provided", exception.Message);
        }

        [Theory]
        [InlineData("ABCDEFGHIJKLM")]//13 characters
        [InlineData("ABCDEFGHIJKLMNOP")] //15 characters
        public async Task GivenStringNot14Characters_WhenExectueAsync_TheShouldThrowException(string lpi_key)
        {
            
            //act
            //assert
            // ReSharper disable once ExpressionIsAlwaysNull
            var exception = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.ExecuteAsync(lpi_key));
            Assert.Equal("lpi_key must be 14 characters", exception.Message);
        }

        [Fact]
        public async Task GivenValidInput_WhenGatewayRespondsWithNull_ThenResponseShouldBeNull()
        {
            //arrange
            var lpi_key = "ABCDEFGHIJKLMN";
            _fakeGateway.Setup(s => s.GetAddressAsync(It.Is<string>(i => i.Equals("ABCDEFGHIJKLMN"))))
                .ReturnsAsync(null as AddressDetails);

            var request = lpi_key;
            //act
            var response = await _classUnderTest.ExecuteAsync(request);
            //assert
            response.Should().BeNull();
        }




        /*var tenancyAgreementRef = "Test";
            _fakeGateway.Setup(s => s.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.TenancyRef.Equals("Test")), CancellationToken.None))
                .ReturnsAsync(new PagedResults<TenancyListItem>());

            var request = new SearchTenancyRequest
            {
                TenancyRef = tenancyAgreementRef
            };
            //act
            await _classUnderTest.ExecuteAsync(request, CancellationToken.None);
            //assert
            _fakeGateway.Verify(v => v.SearchTenanciesAsync(It.Is<SearchTenancyRequest>(i => i.TenancyRef.Equals("Test")), CancellationToken.None));*/

        //check LPI_Key is in correct format

        //Check single address is returned for given lpi key

        //check no address is returned for incorrect lpi key




    }
}
