﻿using LBHAddressesAPI.Models;
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
            string query =  GetAddressesQuery() + GetSearchAddressClause();

            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                var all = await conn.QueryAsync<AddressDetails>(query,
                    new { postcode = request.postCode.Replace(" ", "") + "%" }
                ).ConfigureAwait(false);

                result.Results = all?.ToList();

                var totalCount = await conn.QueryAsync<int>(GetAddressCountQuery() +  GetSearchAddressClause(), new { postcode = request.postCode.Replace(" ", "") + "%" }).ConfigureAwait(false);
                //add to pages results
                result.TotalResultsCount = totalCount.Sum();

                conn.Close();
            }

            return result;
        }


        private static string GetAddressesQuery()
        {
            return "select LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazetteer,ORGANISATION as commercialOccupier,POSTTOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode, PROPERTY_SHELL as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazetteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude";
        }

        private static string GetAddressCountQuery()
        {
            return "select count(1) ";
        }

        private static string GetSingleAddressClause()
        {
            return " from dbo.combined_address WHERE LPI_KEY = @key";
        }

        private static string GetSearchAddressClause()
        {
            return " FROM dbo.combined_address L WHERE POSTCODE_NOSPACE LIKE @postcode AND BLPU_CLASS NOT LIKE 'P%' OPTION(RECOMPILE)";
        }
    }
}
