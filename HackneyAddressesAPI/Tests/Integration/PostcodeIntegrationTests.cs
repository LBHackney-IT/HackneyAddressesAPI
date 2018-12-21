using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LBHAddressesAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace LBHAddressesAPI.Tests.Integration
{
    public class PostcodeIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        private static readonly String apiVersion = "v1/Addresses";
        private static readonly String controllerName = "Postcode";
        private readonly String controllerPathPrefix = apiVersion + "/" + controllerName;

        public PostcodeIntegrationTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task return_a_200_result_for_valid_requests()
        {
            var result = await _client.GetAsync(controllerPathPrefix + "/E8 1DT");
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("application/json", result.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task return_a_400_result_for_empty_parameter_string()
        {
            var result = await _client.GetAsync(controllerPathPrefix + "/" + "%20");
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task return_a_500_result_when_there_is_an_internal_server_error()
        {
            var result = await _client.GetAsync(controllerPathPrefix + "/E8 2LT");
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task return_a_json_object_for_valid_request()
        {
            var result = await _client.GetAsync(controllerPathPrefix + "{route to valid request}");
            string result_string = await result.Content.ReadAsStringAsync();

            StringBuilder json = new StringBuilder();
            json.Append("{Append Json format string here}");
            json.Append("{Append Json format string here}");
            json.Append("{Append Json format string here}");

            Assert.Equal(json.ToString(), result_string);
        }


    }
}
