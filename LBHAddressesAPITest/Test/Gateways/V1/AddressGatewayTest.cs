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



    }
}
