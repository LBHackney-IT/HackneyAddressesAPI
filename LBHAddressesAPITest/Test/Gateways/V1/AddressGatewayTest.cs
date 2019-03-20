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
            string key = "0123456789abcd";
            var expectedAddress = Fake.GenerateAddressProvidingKey(key);
            TestDataHelper.InsertAddress(expectedAddress, _databaseFixture.Db);

            var response = await _classUnderTest.GetSingleAddressAsync(new GetAddressRequest
            {
                addressID = key
            }, CancellationToken.None);

            response.Should().NotBeNull();
            response.AddressID.Should().BeEquivalentTo(key);
        }

        [Fact]
        public async Task can_retrieve_crossref_using_uprn()
        {
            int uprn = 1234578912;
            TestDataHelper.InsertCrossRef(uprn,_databaseFixture.Db);

            var response = await _classUnderTest.GetAddressCrossReferenceAsync(new GetAddressCrossReferenceRequest
            {
            }, CancellationToken.None);
            response.Should().NotBeNull();
            response[0].UPRN.Should().Equals(uprn);
            //check the list for 

        }


        //[Fact]
        //public async Task GetCorrectQuery()
        //{
            
        //    SearchAddressRequest request = new SearchAddressRequest { Format = GlobalConstants.Format.Detailed, PostCode = "RM3 0FS", Page=0, PageSize=50 };

        //    var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query

        //    //detailed no parent shells
        //    string query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
        //    Debug.WriteLine("--detailed no parent shells");
        //    Debug.WriteLine(query);

        //    //detailed no parent shells count
        //    query = QueryBuilder.GetSearchAddressQuery(request, true, true, true, ref dbArgs);
        //    Debug.WriteLine("--detailed no parent shells count");
        //    Debug.WriteLine(query);

        //    //simple no parent shells 
        //    request.Format = GlobalConstants.Format.Simple;
        //    query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
        //    Debug.WriteLine("--simple no parent shells");
        //    Debug.WriteLine(query);

        //    //simple no parent shells count
        //    query = QueryBuilder.GetSearchAddressQuery(request, true, true, true, ref dbArgs);
        //    Debug.WriteLine("--simple no parent shells count");
        //    Debug.WriteLine(query);
            
        //    //simple parent shells            
        //    query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
        //    Debug.WriteLine("--simple parent shells");
        //    Debug.WriteLine(query);

        //    //simple parent shells count
        //    query = QueryBuilder.GetSearchAddressQuery(request, true, true, true, ref dbArgs);
        //    Debug.WriteLine("--simple parent shells count");
        //    Debug.WriteLine(query);

        //    //detailed parent shells
        //    request.Format = GlobalConstants.Format.Detailed;
        //    request.PropertyClassPrimary = GlobalConstants.PropertyClassPrimary.ParentShell;
        //    query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
        //    Debug.WriteLine("--detailed parent shells");
        //    Debug.WriteLine(query);

        //    //detailed parent shells count
        //    query = QueryBuilder.GetSearchAddressQuery(request, true, true, true, ref dbArgs);
        //    Debug.WriteLine("--detailed parent shells count");
        //    Debug.WriteLine(query);
            
        //}
        


    }
}
