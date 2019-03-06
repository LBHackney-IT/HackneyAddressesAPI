using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using LBHAddressesAPI.Helpers;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using System.Threading.Tasks;
using Dapper;

namespace LBHAddressesAPITest.Test.Helpers
{
    public class QueryBuilderTest
    {
        [Fact]
        public async Task GivenValidSearchAddressRequest_WhenCallingQueryBuilderWithSimpleFormat_ThenShouldReturnCorrectQuery()
        {
            var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query
            SearchAddressRequest request = new SearchAddressRequest
            {
                Format = GlobalConstants.Format.Simple,
                PostCode = "",
                Gazetteer = GlobalConstants.Gazetteer.Local
            };

            string response = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            response.Replace("  ", " ").Should().Contain("SAO_TEXT as Line1, coalesce(UNIT_NUMBER,'') + ' ' + PAO_TEXT as Line2, BUILDING_NUMBER + ' ' + STREET_DESCRIPTION as Line3, LOCALITY as Line4 ".Replace("  ", " "));
        }

        [Fact]
        public async Task GivenValidSearchAddressRequest_WhenCallingQueryBuilderWithDetailedFormat_ThenShouldReturnCorrectQuery()
        {
            var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query
            SearchAddressRequest request = new SearchAddressRequest
            {
                Format = GlobalConstants.Format.Detailed,
                PostCode = ""
            };

            string response = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            response.Replace("  ", " ").Should().Contain("SELECT LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazetteer,ORGANISATION as commercialOccupier, WARD as ward, TOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode, PROPERTY_SHELL as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazetteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude,".Replace("  ", " "));
        }

        [Fact]
        public async Task GivenValidSearchAddressRequest_WhenCallingQueryBuilderWithParentShell_ThenShouldReturnQueryForIncludingParentShells()
        {
            var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query
            SearchAddressRequest request = new SearchAddressRequest
            {
                Format = GlobalConstants.Format.Simple,
                PostCode = "RM12PR",
                PropertyClassPrimary = "ParentShell"
            };

            string response = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            response.Replace("  ", " ").Should().Contain(";WITH SEED AS (SELECT * FROM dbo.combined_address L".Replace("  ", " "));            
        }

        [Fact]
        public async Task GivenValidSearchAddressRequest_WhenCallingQueryBuilderWithNoGazetteer_ThenShouldReturnQueryForLLPG()
        {
            var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query
            SearchAddressRequest request = new SearchAddressRequest
            {
                Format = GlobalConstants.Format.Simple,
                PostCode = "RM12PR"
            };

            string response = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            response.Replace("  ", " ").Should().Contain("AND Gazetteer = @gazetteer".Replace("  ", " "));
            string gazetteer = dbArgs.Get<dynamic>("gazetteer");
            gazetteer.Should().Equals("Local");
        }

        [Fact]
        public async Task GivenValidSearchAddressRequest_WhenCallingQueryBuilderWithisCountQuery_ThenShouldReturnQueryForCount()
        {
            var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query
            SearchAddressRequest request = new SearchAddressRequest
            {
                Format = GlobalConstants.Format.Simple,
                PostCode = "RM12PR"
            };

            string response = QueryBuilder.GetSearchAddressQuery(request, true, true, true, ref dbArgs);
            response.Replace("  ", " ").Should().Contain("SELECT COUNT(1)".Replace("  ", " "));            
        }

    }
}
