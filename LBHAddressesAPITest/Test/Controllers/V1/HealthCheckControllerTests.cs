using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using LBHAddressesAPI.Controllers.V1;

namespace LBHAddressesAPITest.Test.Controllers.V1
{
    public class HealthCheckControllerTests
    {
        private HealthCheckController _classUnderTest;

        public HealthCheckControllerTests()
        {
            _classUnderTest = new HealthCheckController();
        }

        [Fact]
        public void GivenApiIsHealthy_WhenHealthCheckIsPinged_ThenItReturnsSuccessResponse()
        {
            var response = _classUnderTest.HealthCheck();
            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            var objectResult = response as OkObjectResult;
            objectResult.StatusCode.Should().Be(200);
            objectResult.Equals(new Dictionary<string, object> { { "success", true } });
        }
    }
}
