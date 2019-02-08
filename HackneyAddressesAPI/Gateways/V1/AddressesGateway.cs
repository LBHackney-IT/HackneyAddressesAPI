using LBHAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Threading;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Infrastructure.V1.API;

namespace LBHAddressesAPI.Gateways.V1
{
    public class AddressesGateway : IAddressesGateway
    {
        private readonly string _connectionString;
        public AddressesGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Return an address for a given LPI_Key
        /// </summary>
        /// <param name="request"></param> 
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AddressDetails> GetSingleAddressAsync(GetAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new AddressDetails();

            //TODO: Move the query in to a helper so it's in one place!
            string query = GetAddressesQuery() + GetSingleAddressClause();
            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                var all = await conn.QueryAsync<AddressDetails>(query,
                    new { key = request.addressID }
                ).ConfigureAwait(false);

                result = all.FirstOrDefault();

                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Return addresses for matching search
        /// </summary>
        /// <param name="request"></param> 
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedResults<AddressDetails>> SearchAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new PagedResults<AddressDetails>();
            
            //TODO: Move the query in to a helper so it's in one place!
            string query =  GetAddressesQuery() + GetSearchAddressClauseWithPaging(request.Page, request.PageSize);

            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                var all = await conn.QueryAsync<AddressDetails>(query,
                    new { postcode = request.postCode.Replace(" ", "") + "%", gazetteer = request.gazeteer.ToString() }
                ).ConfigureAwait(false);

                result.Results = all?.ToList();

                var totalCount = await conn.QueryAsync<int>(GetAddressCountQuery() +  GetSearchAddressClause(true), new { postcode = request.postCode.Replace(" ", "") + "%", gazetteer = request.gazeteer.ToString() }).ConfigureAwait(false);
                //add to pages results
                result.TotalResultsCount = totalCount.Sum();

                conn.Close();
            }

            return result;
        }


        private static string GetAddressesQuery()
        {
            return "SELECT LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazetteer,ORGANISATION as commercialOccupier,POSTTOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode, PROPERTY_SHELL as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazetteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude ";
        }

        private static string GetAddressCountQuery()
        {
            return "SELECT count(1) ";
        }

        private static string GetSingleAddressClause()
        {
            return " FROM dbo.combined_address WHERE LPI_KEY = @key";
        }

        private static string GetSearchAddressClause(bool includeRecompile)
        {
            return string.Format(" FROM dbo.combined_address L WHERE POSTCODE_NOSPACE LIKE @postcode AND Gazetteer = @gazetteer AND BLPU_CLASS NOT LIKE 'P%' {0} ", includeRecompile == true ? "OPTION(RECOMPILE)":"");
        }

        private static string GetSearchAddressClauseWithPaging(int page, int pageSize)
        {
            int lower = 0;
            lower = page == 0 ? 1 : page * pageSize;
            // paging so if current page passed in is 1 then we set lower bound to be 0 (0 based index). Otherwise we multiply by the page size
            string clause = GetSearchAddressClause(false);
            clause += "ORDER BY street_description, building_number DESC ";
            clause += string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY OPTION(RECOMPILE)", lower, pageSize);
            return clause;
        }
    }
}
