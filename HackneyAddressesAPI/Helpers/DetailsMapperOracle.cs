using LBHAddressesAPI.Interfaces;
using LBHAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Helpers
{
    public class DetailsMapperOracle : IDetailsMapper
    {
        //for the simple address format
        public List<AddressDetailsSimple> MapAddressDetailsSimple(DataTable dt)
        {
            try
            {
                List<AddressDetailsSimple> addressDetailsList = new List<AddressDetailsSimple>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AddressDetailsSimple aDetails = new AddressDetailsSimple();
                    aDetails.AddressID = CheckNullString(dt, i, "LPI_KEY"); // LPI KEY
                    aDetails.UPRN = Convert.ToInt64(dt.Rows[i]["UPRN"]); //doing an explicit cast e.g. (Int64)rows[i]["UPRN"] doesn't work for some reason

                    if (!dt.Rows[i].IsNull("POSTTOWN"))
                    {
                        aDetails.City = (string)dt.Rows[i]["POSTTOWN"];
                    }

                    if (!dt.Rows[i].IsNull("POSTCODE"))
                    {
                        aDetails.Postcode = (string)dt.Rows[i]["POSTCODE"];
                    }

                    if (!dt.Rows[i].IsNull("SAO_TEXT") && (!dt.Rows[i].IsNull("UNIT_NUMBER") || !dt.Rows[i].IsNull("PAO_TEXT")))
                    {
                        aDetails.Line1 = CheckNullString(dt, i, "SAO_TEXT");
                        aDetails.Line2 = AddIfDataExists(dt, i, "UNIT_NUMBER", "PAO_TEXT");
                        aDetails.Line3 = AddIfDataExists(dt, i, "BUILDING_NUMBER", "STREET_DESCRIPTION");
                        aDetails.Line4 = CheckNullString(dt, i, "LOCALITY");
                    }
                    else if (!dt.Rows[i].IsNull("SAO_TEXT"))
                    {
                        aDetails.Line1 = CheckNullString(dt, i, "SAO_TEXT");
                        aDetails.Line2 = AddIfDataExists(dt, i, "BUILDING_NUMBER", "STREET_DESCRIPTION");
                        aDetails.Line3 = CheckNullString(dt, i, "LOCALITY");
                    }
                    else if (!dt.Rows[i].IsNull("UNIT_NUMBER") || !dt.Rows[i].IsNull("PAO_TEXT"))
                    {
                        aDetails.Line1 = AddIfDataExists(dt, i, "UNIT_NUMBER", "PAO_TEXT");
                        aDetails.Line2 = AddIfDataExists(dt, i, "BUILDING_NUMBER", "STREET_DESCRIPTION");
                        aDetails.Line3 = CheckNullString(dt, i, "LOCALITY");
                    }
                    else
                    {
                        aDetails.Line1 = AddIfDataExists(dt, i, "BUILDING_NUMBER", "STREET_DESCRIPTION");
                        aDetails.Line2 = CheckNullString(dt, i, "LOCALITY");
                    }

                    addressDetailsList.Add(aDetails);
                }

                return addressDetailsList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private string AddIfDataExists(DataTable dt, int rowindex, string fieldName1, string fieldName2)
        {
            string strRetVal = "";
            if (!dt.Rows[rowindex].IsNull(fieldName1))
            {
                strRetVal = (string)dt.Rows[rowindex][fieldName1] + " ";
            }

            if (!dt.Rows[rowindex].IsNull(fieldName2))
            {
                strRetVal += (string)dt.Rows[rowindex][fieldName2];
            }

            return strRetVal.Trim();
        }

        //for the detailed address format
        public List<AddressDetails> MapAddressDetailsGIS(DataTable dt)
        {
            try
            {
                var dataTable = dt;

                //make a list of addressdetails
                List<AddressDetails> addressDetailsList = new List<AddressDetails>();

                //populate the list
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AddressDetails aDetails = new AddressDetails();

                    aDetails.AddressID = CheckNullString(dt, i, "LPI_KEY"); // LPI KEY
                    aDetails.UPRN = Convert.ToInt64(dt.Rows[i]["UPRN"]); //doing an explicit cast e.g. (Int64)rows[i]["UPRN"] doesn't work for some reason
                    aDetails.USRN = Convert.ToInt32(dt.Rows[i]["USRN"]);

                    if (!dt.Rows[i].IsNull("PARENT_UPRN"))
                    {
                        aDetails.parentUPRN = Convert.ToInt64(dt.Rows[i]["PARENT_UPRN"]);
                    }

                    aDetails.addressStatus = CheckNullString(dt, i, "LPI_LOGICAL_STATUS");
                    aDetails.unitName = CheckNullString(dt, i, "SAO_TEXT");
                    aDetails.buildingName = CheckNullString(dt, i, "PAO_TEXT");
                    aDetails.street = CheckNullString(dt, i, "STREET_DESCRIPTION");
                    aDetails.postcode = CheckNullString(dt, i, "POSTCODE");
                    aDetails.locality = CheckNullString(dt, i, "LOCALITY");
                    aDetails.gazetteer = "Hackney";
                    if (aDetails.locality.Trim() == "")
                    {
                        aDetails.gazetteer = "National";
                    }
                    aDetails.commercialOccupier = CheckNullString(dt, i, "ORGANISATION");
                    aDetails.royalMailPostTown = CheckNullString(dt, i, "POSTTOWN");
                    //aDetails.primaryUsage = CheckNullString(dt, i, "USAGE_PRIMARY");
                    //aDetails.landPropertyUsage = CheckNullString(dt, i, "USAGE_DESCRIPTION");
                    aDetails.isNonLocalAddressInLocalGazetteer = CheckNullBool(dt, i, "NEVEREXPORT"); //for LLPG results; should be null in results for NLPG
                    aDetails.easting = Convert.ToDouble(dt.Rows[i]["EASTING"]);
                    aDetails.northing = Convert.ToDouble(dt.Rows[i]["NORTHING"]);
                    aDetails.longitude = Convert.ToDouble(dt.Rows[i]["LONGITUDE"]);
                    aDetails.latitude = Convert.ToDouble(dt.Rows[i]["LATITUDE"]);
                    aDetails.buildingNumber = CheckNullString(dt, i, "BUILDING_NUMBER");
                    aDetails.unitNumber = CheckNullString(dt, i, "UNIT_NUMBER");

                    addressDetailsList.Add(aDetails);
            
                }

                return addressDetailsList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string CheckNullString(DataTable dt, int rowindex, string columnName)
        {
            string strRetVal = "";

            if (dt.Columns.Contains(columnName))
            {
                if (!dt.Rows[rowindex].IsNull(columnName))
                {
                    strRetVal = dt.Rows[rowindex][columnName].ToString();
                }
            }

            return strRetVal;
        }

        private bool? CheckNullBool(DataTable dt, int rowindex, string columnName)
        {
            string strRetVal = CheckNullString(dt, rowindex, columnName);

            if (strRetVal == "")
            {
                return null;
            }

            if (strRetVal == "0")
            {
                return false;
            }
            
            if (strRetVal == "1")
            {
                return true;
            }

            return Convert.ToBoolean(strRetVal);
        }

        public List<StreetDetails> MapStreetDetails(DataTable dt)
        {
            try
            {
                List<StreetDetails> streetDetailsList = new List<StreetDetails>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StreetDetails sDetails = new StreetDetails();
                    sDetails.county = CheckNullString(dt, i, "COUNTY_NAME");
                    sDetails.locality = CheckNullString(dt, i, "LOCALITY_NAME");
                    sDetails.streetDescription = CheckNullString(dt, i, "STREET_DESCRIPTOR");
                    sDetails.streetEndEasting = CheckNullString(dt, i, "STREET_END_X");
                    sDetails.streetEndLatitude = CheckNullString(dt, i, "STREET_END_LAT");
                    sDetails.streetEndLongitude = CheckNullString(dt, i, "STREET_END_LON");

                    sDetails.streetEndNorthing = CheckNullString(dt, i, "STREET_END_Y");
                    sDetails.streetStartEasting = CheckNullString(dt, i, "STREET_START_X");
                    sDetails.streetStartLatitude = CheckNullString(dt, i, "STREET_START_LAT");
                    sDetails.streetStartLongitude = CheckNullString(dt, i, "STREET_START_LON");
                    sDetails.streetStartNorthing = CheckNullString(dt, i, "STREET_START_Y");
                    sDetails.town = CheckNullString(dt, i, "TOWN_NAME");
                    sDetails.USRN = Convert.ToInt32(dt.Rows[i]["USRN"]);

                    streetDetailsList.Add(sDetails);
                }

                return streetDetailsList;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }

}