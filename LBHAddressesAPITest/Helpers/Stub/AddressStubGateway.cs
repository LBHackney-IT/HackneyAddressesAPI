using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace LBHAddressesAPITest.Helpers.Stub
{
    public class AddressStubGateway : IRepository<AddressDetails>
    {
        private readonly SqlConnection _db;

        public AddressStubGateway(SqlConnection db)
        {
            _db = db;
            if (db.State != ConnectionState.Open)
                db.Open();
        }

        public async Task<AddressDetails> GetAddressAsync(string lpi_key)
        {
            SqlCommand command = null;

            var result = new AddressDetails();

            string query = "select LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazeteer,ORGANISATION as commercialOccupier,POSTTOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode,'' as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazeteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude";
            query += " from dbo.combined_address WHERE LPI_KEY = @key";

            command = new SqlCommand(query, _db);

            var all = await _db.QueryAsync<AddressDetails>(query,
                    new { key = lpi_key }
                ).ConfigureAwait(false);

            result = all.FirstOrDefault();

            return result;
        }
    }
}
