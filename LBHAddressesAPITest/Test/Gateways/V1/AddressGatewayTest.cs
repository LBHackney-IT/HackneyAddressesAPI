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
using LBHAddressesAPI.Helpers;
using System.Diagnostics;

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
            string key = "xxxxxxxxxxxxxx";
            //var expectedAddress = Fake.GenerateAddressProvidingKey(key);
            TestDataHelper.InsertAddress(key, _databaseFixture.Db);

            var response = await _classUnderTest.GetSingleAddressAsync(new GetAddressRequest
            {
                addressID = key
            }, CancellationToken.None);

            response.Should().NotBeNull();
            response.AddressKey.Should().BeEquivalentTo(key);

            TestDataHelper.DeleteAddress(key, _databaseFixture.Db);
        }

        [Fact]
        public async Task can_retrieve_crossref_using_uprn()
        {
            int uprn = 1234578912;
            TestDataHelper.InsertCrossRef(uprn,_databaseFixture.Db);

            var response = await _classUnderTest.GetAddressCrossReferenceAsync(new GetAddressCrossReferenceRequest
            {
                uprn = uprn
            }, CancellationToken.None);
            response.Should().NotBeNull();
            response[0].UPRN.Should().Equals(uprn);

            TestDataHelper.DeleteCrossRef(uprn,_databaseFixture.Db);

        }
    }
}
