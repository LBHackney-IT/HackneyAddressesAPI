using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using FluentAssertions;
using Xunit;
using System.Data.SqlClient;
using LBHAddressesAPI.Gateways.V1;
using LBHAddressesAPI.Models;
using LBHAddressesAPITest.Helpers.Stub;
using System.Threading.Tasks;
using System.Threading;
using LBHAddressesAPITest.Helpers;
using LBHAddressesAPITest.Helpers.Data;
using LBHAddressesAPI.UseCases.V1.Search.Models;

namespace LBHAddressesAPITest.Test.Gateways.V1
{
    public class AddressGatewayTest : IClassFixture<DatabaseFixture>
    {

        readonly DatabaseFixture _databaseFixture;
        private readonly IAddressesGateway _classUnderTest;

        public AddressGatewayTest(DatabaseFixture fixture)
        {
            _databaseFixture = fixture;
            _classUnderTest = new AddressesGateway(_databaseFixture.ConnectionString);
        }

        [Fact]
        public async Task can_retrieve_using_address_id()
        {
            string key = "0123456789abcd";
            var expectedAddress = Fake.GenerateAddressProvidingKey(key);
            TestDataHelper.InsertAddress(expectedAddress, _databaseFixture.Db);

            var response = await _classUnderTest.GetSingleAddressAsync(new GetAddressRequest
            {
                addressID = key
            }, CancellationToken.None);

            response.Should().NotBeNull();
            response.AddressID.Should().BeEquivalentTo(key);

            /*var response = await _classUnderTest.SearchTenanciesAsync(new SearchTenancyRequest
            {
                SearchTerm = tenancyRef,
                PageSize = 10,
                Page = 1
            }, CancellationToken.None);
            //assert
            response.Should().NotBeNull();
            response.Results.Should().NotBeNullOrEmpty();
            response.Results.Count.Should().Be(1);
            response.Results[0].TenancyRef.Should().BeEquivalentTo(tenancyRef);*/


        }


    }
}
