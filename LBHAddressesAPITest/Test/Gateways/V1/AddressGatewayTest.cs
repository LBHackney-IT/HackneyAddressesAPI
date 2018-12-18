using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using FluentAssertions;
using Xunit;
using System.Data.SqlClient;
using LBHAddressesAPI.Gateways.V1;
using HackneyAddressesAPI.Models;
using LBHAddressesAPITest.Helpers.Stub;

namespace LBHAddressesAPITest.Test.Gateways.V1
{
    public class AddressGatewayTest : IClassFixture<DatabaseFixture>
    {
        private readonly SqlConnection db;
        private IRepository<AddressDetails> _AddressGateway; 

        public AddressGatewayTest(DatabaseFixture fixture)
        {
            db = fixture.Db;
            _AddressGateway = new AddressStubGateway(db);
        }



    }
}
