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

        [Fact]
        public async Task GetCorrectQuery()
        {
            //string selectSimpleColumns = string.Format(" SAO_TEXT as Line1, coalesce(UNIT_NUMBER,'') + ' ' + PAO_TEXT as Line2, BUILDING_NUMBER + ' ' + STREET_DESCRIPTION as Line3, LOCALITY as Line4{0} ", format == GlobalConstants.Format.Simple ? ", POSTTOWN as City, Postcode, UPRN, LPI_KEY as AddressID " : " ");
            //string selectDetailedColumns = string.Format(" LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazetteer,ORGANISATION as commercialOccupier, WARD as ward, POSTTOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode, PROPERTY_SHELL as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazetteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude, {0} ", selectSimpleColumns);
            //string selectParentShells = " WITH SEED AS (SELECT * FROM dbo.combined_address L WHERE POSTCODE_NOSPACE LIKE @varPCID UNION ALL SELECT L.* FROM dbo.combined_address L JOIN SEED S ON S.PARENT_UPRN = L.UPRN) SELECT DISTINCT {0} from SEED S ";

            SearchAddressRequest request = new SearchAddressRequest { Format = GlobalConstants.Format.Detailed, PostCode = "RM3 0FS", Page=0, PageSize=50 };

            var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query

            //detailed no parent shells
            string query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            Debug.WriteLine("--detailed no parent shells");
            Debug.WriteLine(query);

            //detailed no parent shells count
            query = QueryBuilder.GetSearchAddressQuery(request, true, true, true, ref dbArgs);
            Debug.WriteLine("--detailed no parent shells count");
            Debug.WriteLine(query);

            //simple no parent shells 
            request.Format = GlobalConstants.Format.Simple;
            query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            Debug.WriteLine("--simple no parent shells");
            Debug.WriteLine(query);

            //simple no parent shells count
            query = QueryBuilder.GetSearchAddressQuery(request, true, true, true, ref dbArgs);
            Debug.WriteLine("--simple no parent shells count");
            Debug.WriteLine(query);
            
            //simple parent shells            
            query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            Debug.WriteLine("--simple parent shells");
            Debug.WriteLine(query);

            //simple parent shells count
            query = QueryBuilder.GetSearchAddressQuery(request, true, true, true, ref dbArgs);
            Debug.WriteLine("--simple parent shells count");
            Debug.WriteLine(query);

            //detailed parent shells
            request.Format = GlobalConstants.Format.Detailed;
            request.PropertyClassPrimary = GlobalConstants.PropertyClassPrimary.ParentShell;
            query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            Debug.WriteLine("--detailed parent shells");
            Debug.WriteLine(query);

            //detailed parent shells count
            query = QueryBuilder.GetSearchAddressQuery(request, true, true, true, ref dbArgs);
            Debug.WriteLine("--detailed parent shells count");
            Debug.WriteLine(query);


            Console.Read();
        }
        


    }
}
