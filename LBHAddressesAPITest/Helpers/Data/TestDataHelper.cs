using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using LBHAddressesAPITest.Helpers.Entities;

namespace LBHAddressesAPITest.Helpers.Data
{
    public static class TestDataHelper
    {
        public static void InsertAddress(Address address, SqlConnection db)
        {
            var commandText = "INSERT INTO [dbo].[hackney_address] ([LPI_KEY],[LPI_LOGICAL_STATUS],[LPI_OFFICIAL_FLAG],[LPI_START_DATE],[LPI_END_DATE],[LPI_LAST_UPDATE_DATE],[USRN],[UPRN],[PARENT_UPRN],[BLPU_START_DATE],[BLPU_END_DATE],[BLPU_STATE],[BLPU_STATE_DATE],[BLPU_CLASS],[USAGE_DESCRIPTION],[USAGE_PRIMARY],[PROPERTY_SHELL],[EASTING],[NORTHING],[RPA],[ORGANISATION],[SAO_TEXT],[UNIT_NUMBER],[LPI_LEVEL],[PAO_TEXT],[BUILDING_NUMBER],[STREET_DESCRIPTION],[STREET_ADMIN],[LOCALITY],[WARD],[POSTALLY_ADDRESSABLE],[NEVEREXPORT],[POSTTOWN],[POSTCODE],[POSTCODE_NOSPACE],[LONGITUDE],[LATITUDE],[GAZETTEER])";
            commandText += " VALUES(@LPI_KEY,@LPI_LOGICAL_STATUS,@LPI_OFFICIAL_FLAG,@LPI_START_DATE,@LPI_END_DATE,@LPI_LAST_UPDATE_DATE,@USRN,@UPRN,@PARENT_UPRN,@BLPU_START_DATE,@BLPU_END_DATE,@BLPU_STATE,@BLPU_STATE_DATE,@BLPU_CLASS,@USAGE_DESCRIPTION,@USAGE_PRIMARY,@PROPERTY_SHELL,@EASTING,@NORTHING,@RPA,@ORGANISATION,@SAO_TEXT,@UNIT_NUMBER,@LPI_LEVEL,@PAO_TEXT,@BUILDING_NUMBER,@STREET_DESCRIPTION,@STREET_ADMIN,@LOCALITY,@WARD,@POSTALLY_ADDRESSABLE,@NEVEREXPORT,@POSTTOWN,@POSTCODE,@POSTCODE_NOSPACE,@LONGITUDE,@LATITUDE,@GAZETTEER);";
            var command = new SqlCommand(commandText, db);

            command.Parameters.Add("@LPI_KEY", SqlDbType.NVarChar);
            command.Parameters["@LPI_KEY"].Value = address.LPI_KEY;
            
            //TODO: build up the rest of the insert string

            command.ExecuteNonQuery();
        }
    }
}
