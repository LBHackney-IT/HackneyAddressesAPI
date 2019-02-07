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
            var commandText = "INSERT INTO [dbo].[dbo.combined_address] ([LPI_KEY],[LPI_LOGICAL_STATUS],[LPI_START_DATE],[LPI_END_DATE],[LPI_LAST_UPDATE_DATE],[USRN],[UPRN],[PARENT_UPRN],[BLPU_START_DATE],[BLPU_END_DATE],[BLPU_STATE],[BLPU_STATE_DATE],[BLPU_CLASS],[USAGE_DESCRIPTION],[USAGE_PRIMARY],[PROPERTY_SHELL],[EASTING],[NORTHING],[RPA],[ORGANISATION],[SAO_TEXT],[UNIT_NUMBER],[PAO_TEXT],[BUILDING_NUMBER],[STREET_DESCRIPTION],[LOCALITY],[WARD],[NEVEREXPORT],[POSTTOWN],[POSTCODE],[LONGITUDE],[LATITUDE],[GAZETTEER])";
            commandText += " VALUES(@LPI_KEY,@LPI_LOGICAL_STATUS,@LPI_START_DATE,@LPI_END_DATE,@LPI_LAST_UPDATE_DATE,@USRN,@UPRN,@PARENT_UPRN,@BLPU_START_DATE,@BLPU_END_DATE,@BLPU_STATE,@BLPU_STATE_DATE,@BLPU_CLASS,@USAGE_DESCRIPTION,@USAGE_PRIMARY,@PROPERTY_SHELL,@EASTING,@NORTHING,@RPA,@ORGANISATION,@SAO_TEXT,@UNIT_NUMBER,@PAO_TEXT,@BUILDING_NUMBER,@STREET_DESCRIPTION,@LOCALITY,@WARD,@NEVEREXPORT,@POSTTOWN,@POSTCODE,@LONGITUDE,@LATITUDE,@GAZETTEER);";
            var command = new SqlCommand(commandText, db);

            command.Parameters.Add("@LPI_KEY", SqlDbType.VarChar);
            command.Parameters["@LPI_KEY"].Value = address.LPI_KEY;
            command.Parameters.Add("@LPI_LOGICAL_STATUS", SqlDbType.VarChar);
            command.Parameters["@LPI_LOGICAL_STATUS"].Value = address.LPI_LOGICAL_STATUS;
            command.Parameters.Add("@LPI_START_DATE", SqlDbType.Int);
            command.Parameters["@LPI_START_DATE"].Value = address.LPI_START_DATE;
            command.Parameters.Add("@LPI_END_DATE", SqlDbType.Int);
            command.Parameters["@LPI_END_DATE"].Value = address.LPI_END_DATE;
            command.Parameters.Add("@LPI_LAST_UPDATE_DATE", SqlDbType.Int);
            command.Parameters["@LPI_LAST_UPDATE_DATE"].Value = address.LPI_LAST_UPDATE_DATE;
            command.Parameters.Add("@USRN", SqlDbType.Int);
            command.Parameters["@USRN"].Value = address.USRN;
            command.Parameters.Add("@UPRN", SqlDbType.Float);
            command.Parameters["@UPRN"].Value = address.UPRN;
            command.Parameters.Add("@PARENT_UPRN", SqlDbType.Float);
            command.Parameters["@PARENT_UPRN"].Value = address.PARENT_UPRN;
            command.Parameters.Add("@BLPU_START_DATE", SqlDbType.Int);
            command.Parameters["@BLPU_START_DATE"].Value = address.BLPU_START_DATE;
            command.Parameters.Add("@BLPU_END_DATE", SqlDbType.Int);
            command.Parameters["@BLPU_END_DATE"].Value = address.BLPU_END_DATE;
            command.Parameters.Add("@BLPU_STATE", SqlDbType.SmallInt);
            command.Parameters["@BLPU_STATE"].Value = address.BLPU_STATE;
            command.Parameters.Add("@BLPU_STATE_DATE", SqlDbType.Int);
            command.Parameters["@BLPU_STATE_DATE"].Value = address.BLPU_STATE_DATE;
            command.Parameters.Add("@BLPU_CLASS", SqlDbType.VarChar);
            command.Parameters["@BLPU_CLASS"].Value = address.BLPU_CLASS;
            command.Parameters.Add("@USAGE_DESCRIPTION", SqlDbType.VarChar);
            command.Parameters["@USAGE_DESCRIPTION"].Value = address.USAGE_DESCRIPTION;
            command.Parameters.Add("@USAGE_PRIMARY", SqlDbType.VarChar);
            command.Parameters["@USAGE_PRIMARY"].Value = address.USAGE_PRIMARY;
            command.Parameters.Add("@PROPERTY_SHELL", SqlDbType.Bit);
            command.Parameters["@PROPERTY_SHELL"].Value = address.PROPERTY_SHELL;
            command.Parameters.Add("@EASTING", SqlDbType.Decimal);
            command.Parameters["@EASTING"].Value = address.EASTING;
            command.Parameters.Add("@NORTHING", SqlDbType.Decimal);
            command.Parameters["@NORTHING"].Value = address.NORTHING;
            command.Parameters.Add("@RPA", SqlDbType.TinyInt);
            command.Parameters["@RPA"].Value = address.RPA;
            command.Parameters.Add("@ORGANISATION", SqlDbType.NVarChar);
            command.Parameters["@ORGANISATION"].Value = address.ORGANISATION;
            command.Parameters.Add("@SAO_TEXT", SqlDbType.NVarChar);
            command.Parameters["@SAO_TEXT"].Value = address.SAO_TEXT;
            command.Parameters.Add("@UNIT_NUMBER", SqlDbType.NVarChar);
            command.Parameters["@UNIT_NUMBER"].Value = address.UNIT_NUMBER;
            command.Parameters.Add("@PAO_TEXT", SqlDbType.NVarChar);
            command.Parameters["@PAO_TEXT"].Value = address.PAO_TEXT;
            command.Parameters.Add("@BUILDING_NUMBER", SqlDbType.NVarChar);
            command.Parameters["@BUILDING_NUMBER"].Value = address.BUILDING_NUMBER;
            command.Parameters.Add("@STREET_DESCRIPTION", SqlDbType.NVarChar);
            command.Parameters["@STREET_DESCRIPTION"].Value = address.STREET_DESCRIPTION;
            command.Parameters.Add("@LOCALITY", SqlDbType.NVarChar);
            command.Parameters["@LOCALITY"].Value = address.LOCALITY;
            command.Parameters.Add("@WARD", SqlDbType.NVarChar);
            command.Parameters["@WARD"].Value = address.WARD;
            command.Parameters.Add("@NEVEREXPORT", SqlDbType.Bit);
            command.Parameters["@NEVEREXPORT"].Value = address.NEVEREXPORT;
            command.Parameters.Add("@POSTTOWN", SqlDbType.NVarChar);
            command.Parameters["@POSTTOWN"].Value = address.POSTTOWN;
            command.Parameters.Add("@POSTCODE", SqlDbType.VarChar);
            command.Parameters["@POSTCODE"].Value = address.POSTCODE;
            command.Parameters.Add("@LONGITUDE", SqlDbType.Float);
            command.Parameters["@LONGITUDE"].Value = address.LONGITUDE;
            command.Parameters.Add("@LATITUDE", SqlDbType.Float);
            command.Parameters["@LATITUDE"].Value = address.LATITUDE;
            command.Parameters.Add("@GAZETTEER", SqlDbType.VarChar);
            command.Parameters["@GAZETTEER"].Value = address.GAZETTEER;


            //TODO: build up the rest of the insert string

            command.ExecuteNonQuery();
        }
    }
}
