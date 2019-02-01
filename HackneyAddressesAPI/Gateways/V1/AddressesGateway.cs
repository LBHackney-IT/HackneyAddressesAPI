using LBHAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Threading;
using LBHAddressesAPI.UseCases.V1.Search.Models;

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
        public async Task<AddressDetails> GetAddressAsync(SearchAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new AddressDetails();

            //string query = "select LPI_KEY as AddressID, UPRN, USRN, PARENT_UPRN as parentUPRN, LPI_LOGICAL_STATUS as addressStatus, SAO_TEXT as unitName, '' as unitNumber, PAO_TEXT as buildingName, BUILDING_NUMBER as buildingNumber, STREET_DESCRIPTION as street, POSTCODE as postcode, LOCALITY as locality, GAZETTEER as gazeteer, ORGANISATION as commercialOccupier, POSTTOWN as royalMailPostTown, '' as landPropertyUsage, NEVEREXPORT as isNonLocalAddressInLocalGazeteer, EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude  from dbo.combined_address WHERE LPI_KEY = @key";
            string query = "select LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazetteer,ORGANISATION as commercialOccupier,POSTTOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode, PROPERTY_SHELL as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazetteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude";
            query += " from dbo.combined_address WHERE LPI_KEY = @key";
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

        public async Task<List<AddressDetails>> SearchAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken)
        {

            var result = new List<AddressDetails>();



            return result;
        }

    }
}
