﻿using System;
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
            GlobalConstants.Format format = GlobalConstants.Format.Detailed;
            bool parentShell = false;

            string selectSimpleColumns = string.Format(" SAO_TEXT as Line1, coalesce(UNIT_NUMBER,'') + ' ' + PAO_TEXT as Line2, BUILDING_NUMBER + ' ' + STREET_DESCRIPTION as Line3, LOCALITY as Line4{0} ", format == GlobalConstants.Format.Simple ? ", POSTTOWN as City, Postcode, UPRN, LPI_KEY as AddressID " : " ");
            string selectDetailedColumns = string.Format(" LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazetteer,ORGANISATION as commercialOccupier, WARD as ward, POSTTOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode, PROPERTY_SHELL as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazetteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude, {0} ", selectSimpleColumns);
            string selectParentShells = " WITH SEED AS (SELECT * FROM dbo.combined_address L WHERE POSTCODE_NOSPACE LIKE @varPCID UNION ALL SELECT L.* FROM dbo.combined_address L JOIN SEED S ON S.PARENT_UPRN = L.UPRN) SELECT DISTINCT {0} from SEED S ";

            string query = string.Empty;

            if (format == GlobalConstants.Format.Detailed)
            {
                query = parentShell ? query = string.Format(selectParentShells, selectDetailedColumns) : "SELECT " + selectDetailedColumns;
            }
            else
            {
                query = parentShell ? query = string.Format(selectParentShells, selectSimpleColumns) : string.Format("SELECT {0} ", selectSimpleColumns);
            }
            
            query = query;
        }
        private static string GetSearchAddressClause(SearchAddressRequest request, bool includePaging, bool includeRecompile, ref DynamicParameters dbArgs)
        {

            string clause = string.Format(" FROM dbo.combined_address L WHERE PROPERTY_SHELL <> 1 ");

            if (!string.IsNullOrEmpty(request.PostCode))
            {
                dbArgs.Add("@postcode", request.PostCode.Replace(" ", "") + "%");
                clause += " AND POSTCODE_NOSPACE LIKE @postcode  ";
            }

            if (!string.IsNullOrEmpty(request.AddressStatus.ToString())) //AddressStatus/LPI_LOGICAL_STATUS
            {
                dbArgs.Add("@addressStatus", GlobalConstants.MapAddressStatus(request.AddressStatus));
                clause += " AND LPI_LOGICAL_STATUS = @addressStatus ";
            }
            if (request.UPRN != null)
            {
                dbArgs.Add("@uprn", request.UPRN);
                clause += " AND UPRN = @uprn ";
            }

            if (request.USRN != null)
            {
                dbArgs.Add("@usrn", request.USRN);
                clause += " AND USRN = @usrn ";
            }

            if (!string.IsNullOrEmpty(request.PropertyClassPrimary.ToString()))
            {
                dbArgs.Add("@primaryClass", request.PropertyClassPrimary.ToString());
                clause += " AND USAGE_PRIMARY = @primaryClass ";
            }

            if (!string.IsNullOrEmpty(request.PropertyClassCode))
            {
                dbArgs.Add("@propertyClassCode", request.PropertyClassCode + "%");
                clause += " AND BLPU_CLASS LIKE @propertyClassCode ";
            }

            if (request.Gazeteer == GlobalConstants.Gazetteer.Both ? false : true)//Gazetteer
            {
                dbArgs.Add("@gazetteer", request.Gazeteer.ToString());
                clause += " AND Gazetteer = @gazetteer ";
            }

            if (includePaging)//paging
            {
                int page = request.Page;
                int pageSize = request.PageSize;
                int lower = 0;
                lower = page == 0 || page == 1 ? 0 : page * pageSize;
                // paging so if current page passed in is 1 then we set lower bound to be 0 (0 based index). Otherwise we multiply by the page size
                clause += " ORDER BY street_description, building_number DESC ";
                clause += string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", lower, pageSize);
            }
            if (includeRecompile)//recompile
            {
                clause += " OPTION(RECOMPILE) ";
            }
            return clause;
        }


    }
}
