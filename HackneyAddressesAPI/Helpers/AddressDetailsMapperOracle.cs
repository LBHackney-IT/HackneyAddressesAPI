using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Helpers
{
    public class AddressDetailsMapperOracle : IAddressDetailsMapper
    {

        public List<AddressDetailsSimple> MapAddressDetailsSimple(DataTable dt)
        {
            try
            {
                List<AddressDetailsSimple> addressDetailsList = new List<AddressDetailsSimple>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AddressDetailsSimple aDetails = new AddressDetailsSimple();
                    aDetails.AddressID = CheckNullString(dt, i, "LPI_KEY"); // LPI KEY

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

                    aDetails.uniquePropertyReferenceNumber = Convert.ToInt64(dt.Rows[i]["UPRN"]); //doing an explicit cast e.g. (Int64)rows[i]["UPRN"] doesn't work for some reason
                    aDetails.uniqueStreetReferenceNumber = Convert.ToInt32(dt.Rows[i]["USRN"]);

                    if (!dt.Rows[i].IsNull("PARENT_UPRN"))
                    {
                        aDetails.parentUniquePropertyReferenceNumber = Convert.ToInt64(dt.Rows[i]["PARENT_UPRN"]);
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
                    // aDetails.landPropertyUsage = (string)rows[i]["USAGE"]; //not in the source view yet
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
                    strRetVal = (string)dt.Rows[rowindex][columnName];
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

            return Convert.ToBoolean(strRetVal);
        }
    }

}