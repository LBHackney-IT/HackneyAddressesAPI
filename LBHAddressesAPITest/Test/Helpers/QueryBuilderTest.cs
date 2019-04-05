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
            response.Replace("  ", " ").Should().Contain("Line1, Line2, Line3, Line4 , Postcode, UPRN , TOWN as Town  ".Replace("  ", " "));
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
            response.Replace("  ", " ").Should().Contain("SELECT lpi_key as addressKey, uprn as uprn, usrn as usrn, parent_uprn as parentUPRN, lpi_logical_status as addressStatus, sao_text as unitName, unit_number as unitNumber, pao_text as buildingName, building_number as buildingNumber, street_description as street, postcode as postcode, locality as locality, town as town, gazetteer as gazetteer, organisation as commercialOccupier, ward as ward, usage_description as usageDescription, usage_primary as usagePrimary, blpu_class as usageCode, planning_use_class as planningUseClass, property_shell as propertyShell, neverexport as hackneyGazetteerOutOfBoroughAddress, easting as easting, northing as northing, longitude as longitude, latitude as latitude,".Replace("  ", " "));
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
