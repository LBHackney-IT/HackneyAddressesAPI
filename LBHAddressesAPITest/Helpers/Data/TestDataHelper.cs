using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using LBHAddressesAPITest.Helpers.Entities;
using System.IO;

namespace LBHAddressesAPITest.Helpers.Data
{
    public static class TestDataHelper
    {
        public static void InsertAddress(Address address, SqlConnection db)
        {
            var commandText = "insert into [dbo].[hackney_address] ([LPI_KEY], [LPI_LOGICAL_STATUS], [LPI_OFFICIAL_FLAG], [LPI_START_DATE], [LPI_END_DATE], [LPI_LAST_UPDATE_DATE], [USRN], [UPRN], [PARENT_UPRN], [BLPU_START_DATE], [BLPU_END_DATE], [BLPU_STATE], [BLPU_STATE_DATE], [BLPU_CLASS], [USAGE_DESCRIPTION], [USAGE_PRIMARY], [PROPERTY_SHELL], [EASTING], [NORTHING], [RPA], [ORGANISATION], [SAO_TEXT], [UNIT_NUMBER], [LPI_LEVEL], [PAO_TEXT], [BUILDING_NUMBER], [STREET_DESCRIPTION], [STREET_ADMIN], [LOCALITY], [WARD], [POSTALLY_ADDRESSABLE], [NEVEREXPORT], [TOWN], [POSTCODE], [POSTCODE_NOSPACE], [LONGITUDE], [LATITUDE], [GAZETTEER]) ";
            commandText += " VALUES(@LPI_KEY,@LPI_LOGICAL_STATUS,@LPI_OFFICIAL_FLAG,@LPI_START_DATE,@LPI_END_DATE,@LPI_LAST_UPDATE_DATE,@USRN,@UPRN,@PARENT_UPRN,@BLPU_START_DATE,@BLPU_END_DATE,@BLPU_STATE,@BLPU_STATE_DATE,@BLPU_CLASS,@USAGE_DESCRIPTION,@USAGE_PRIMARY,@PROPERTY_SHELL,@EASTING,@NORTHING,@RPA,@ORGANISATION,@SAO_TEXT,@UNIT_NUMBER,@LPI_LEVEL,@PAO_TEXT,@BUILDING_NUMBER,@STREET_DESCRIPTION,@STREET_ADMIN,@LOCALITY,@WARD,@POSTALLY_ADDRESSABLE,@NEVEREXPORT,@TOWN,@POSTCODE,@POSTCODE_NOSPACE,@LONGITUDE,@LATITUDE,@GAZETTEER);";
 
            var command = new SqlCommand(commandText, db);

            command.Parameters.Add("@LPI_KEY", SqlDbType.VarChar );
            command.Parameters.Add("@LPI_LOGICAL_STATUS", SqlDbType.VarChar );
            command.Parameters.Add("@LPI_OFFICIAL_FLAG", SqlDbType.VarChar );
            command.Parameters.Add("@LPI_START_DATE", SqlDbType.Int );
            command.Parameters.Add("@LPI_END_DATE", SqlDbType.Int );
            command.Parameters.Add("@LPI_LAST_UPDATE_DATE", SqlDbType.Int );
            command.Parameters.Add("@USRN", SqlDbType.Int );
            command.Parameters.Add("@UPRN", SqlDbType.Float );
            command.Parameters.Add("@PARENT_UPRN", SqlDbType.Float );
            command.Parameters.Add("@BLPU_START_DATE", SqlDbType.Int );
            command.Parameters.Add("@BLPU_END_DATE", SqlDbType.Int );
            command.Parameters.Add("@BLPU_STATE", SqlDbType.SmallInt );
            command.Parameters.Add("@BLPU_STATE_DATE", SqlDbType.Int );
            command.Parameters.Add("@BLPU_CLASS", SqlDbType.VarChar );
            command.Parameters.Add("@USAGE_DESCRIPTION", SqlDbType.VarChar );
            command.Parameters.Add("@USAGE_PRIMARY", SqlDbType.VarChar );
            command.Parameters.Add("@PROPERTY_SHELL", SqlDbType.Bit );
            command.Parameters.Add("@EASTING", SqlDbType.Decimal );
            command.Parameters.Add("@NORTHING", SqlDbType.Decimal );
            command.Parameters.Add("@RPA", SqlDbType.TinyInt );
            command.Parameters.Add("@ORGANISATION", SqlDbType.NVarChar );
            command.Parameters.Add("@SAO_TEXT", SqlDbType. NVarChar);
            command.Parameters.Add("@UNIT_NUMBER", SqlDbType.NVarChar );
            command.Parameters.Add("@LPI_LEVEL", SqlDbType.NVarChar );
            command.Parameters.Add("@PAO_TEXT", SqlDbType.NVarChar );
            command.Parameters.Add("@BUILDING_NUMBER", SqlDbType.NVarChar );
            command.Parameters.Add("@STREET_DESCRIPTION", SqlDbType.NVarChar );
            command.Parameters.Add("@STREET_ADMIN", SqlDbType.VarChar );
            command.Parameters.Add("@LOCALITY", SqlDbType.NVarChar );
            command.Parameters.Add("@WARD", SqlDbType.NVarChar );
            command.Parameters.Add("@POSTALLY_ADDRESSABLE", SqlDbType.VarChar );
            command.Parameters.Add("@NEVEREXPORT", SqlDbType.Bit );
            command.Parameters.Add("@TOWN", SqlDbType.NVarChar );
            command.Parameters.Add("@POSTCODE", SqlDbType.VarChar );
            command.Parameters.Add("@POSTCODE_NOSPACE", SqlDbType.VarChar );
            command.Parameters.Add("@LONGITUDE", SqlDbType.Float );
            command.Parameters.Add("@LATITUDE", SqlDbType.Float );
            command.Parameters.Add("@GAZETTEER", SqlDbType.VarChar );

            command.Parameters["@LPI_KEY"].Value = address.LPI_KEY;
            command.Parameters["@LPI_LOGICAL_STATUS"].Value = address.LPI_LOGICAL_STATUS;
            command.Parameters["@LPI_OFFICIAL_FLAG"].Value = address.LPI_OFFICIAL_FLAG;
            command.Parameters["@LPI_START_DATE"].Value = address.LPI_START_DATE;
            command.Parameters["@LPI_END_DATE"].Value = address.LPI_END_DATE;
            command.Parameters["@LPI_LAST_UPDATE_DATE"].Value = address.LPI_LAST_UPDATE_DATE;
            command.Parameters["@USRN"].Value = address.USRN;
            command.Parameters["@UPRN"].Value = address.UPRN;
            command.Parameters["@PARENT_UPRN"].Value = address.PARENT_UPRN;
            command.Parameters["@BLPU_START_DATE"].Value = address.BLPU_START_DATE;
            command.Parameters["@BLPU_END_DATE"].Value = address.BLPU_END_DATE;
            command.Parameters["@BLPU_STATE"].Value = address.BLPU_STATE;
            command.Parameters["@BLPU_STATE_DATE"].Value = address.BLPU_STATE_DATE;
            command.Parameters["@BLPU_CLASS"].Value = address.BLPU_CLASS;
            command.Parameters["@USAGE_DESCRIPTION"].Value = address.USAGE_DESCRIPTION;
            command.Parameters["@USAGE_PRIMARY"].Value = address.USAGE_PRIMARY;
            command.Parameters["@PROPERTY_SHELL"].Value = address.PROPERTY_SHELL;
            command.Parameters["@EASTING"].Value = address.EASTING;
            command.Parameters["@NORTHING"].Value = address.NORTHING;
            command.Parameters["@RPA"].Value = address.RPA;
            command.Parameters["@ORGANISATION"].Value = address.ORGANISATION;
            command.Parameters["@SAO_TEXT"].Value = address.SAO_TEXT;
            command.Parameters["@UNIT_NUMBER"].Value = address.UNIT_NUMBER;
            command.Parameters["@LPI_LEVEL"].Value = address.LPI_LEVEL;
            command.Parameters["@PAO_TEXT"].Value = address.PAO_TEXT;
            command.Parameters["@BUILDING_NUMBER"].Value = address.BUILDING_NUMBER;
            command.Parameters["@STREET_DESCRIPTION"].Value = address.STREET_DESCRIPTION;
            command.Parameters["@STREET_ADMIN"].Value = address.STREET_ADMIN;
            command.Parameters["@LOCALITY"].Value = address.LOCALITY;
            command.Parameters["@WARD"].Value = address.WARD;
            command.Parameters["@POSTALLY_ADDRESSABLE"].Value = address.POSTALLY_ADDRESSABLE;
            command.Parameters["@NEVEREXPORT"].Value = address.NEVEREXPORT;
            command.Parameters["@TOWN"].Value = address.POSTTOWN;
            command.Parameters["@POSTCODE"].Value = address.POSTCODE;
            command.Parameters["@POSTCODE_NOSPACE"].Value = address.POSTCODE_NOSPACE;
            command.Parameters["@LONGITUDE"].Value = address.LONGITUDE;
            command.Parameters["@LATITUDE"].Value = address.LATITUDE;
            command.Parameters["@GAZETTEER"].Value = address.GAZETTEER;           


            command.ExecuteNonQuery();
        }
    }
}
