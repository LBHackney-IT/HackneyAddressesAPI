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
                    aDetails.AddressID = CheckNull(dt, i, "LPI_KEY"); // LPI KEY

                    if (!dt.Rows[i].IsNull("POSTTOWN"))
                    {
                        aDetails.City = (string)dt.Rows[i]["POSTTOWN"];
                    }

                    if (!dt.Rows[i].IsNull("POSTCODE"))
                    {
                        aDetails.Postcode = (string)dt.Rows[i]["POSTCODE"];
                    }


                    if (!dt.Rows[i].IsNull("UNIT_NUMBER") && !dt.Rows[i].IsNull("PAO_TEXT"))
                    {
                        aDetails.Line1 = CheckNull(dt, i, "UNIT_NUMBER");
                        aDetails.Line2 = CheckNull(dt, i, "PAO_TEXT");
                        aDetails.Line3 = AddIfDataExists(dt, i, "BUILDING_NUMBER", "STREET_DESCRIPTION");
                        aDetails.Line4 = CheckNull(dt, i, "LOCALITY");
                    }
                    else if (!dt.Rows[i].IsNull("UNIT_NUMBER"))
                    {
                        aDetails.Line1 = CheckNull(dt, i, "UNIT_NUMBER");
                        aDetails.Line2 = AddIfDataExists(dt, i, "BUILDING_NUMBER", "STREET_DESCRIPTION");
                        aDetails.Line3 = CheckNull(dt, i, "LOCALITY");
                    }
                    else if (!dt.Rows[i].IsNull("PAO_TEXT"))
                    {
                        aDetails.Line1 = CheckNull(dt, i, "PAO_TEXT");
                        aDetails.Line2 = AddIfDataExists(dt, i, "BUILDING_NUMBER", "STREET_DESCRIPTION");
                        aDetails.Line3 = CheckNull(dt, i, "LOCALITY");
                    }
                    else
                    {
                        aDetails.Line1 = AddIfDataExists(dt, i, "BUILDING_NUMBER", "STREET_DESCRIPTION");
                        aDetails.Line2 = CheckNull(dt, i, "LOCALITY");
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

                    aDetails.addressStatus = CheckNull(dt, i, "LPI_LOGICAL_STATUS");
                    aDetails.unitName = CheckNull(dt, i, "SAO_TEXT");
                    aDetails.buildingName = CheckNull(dt, i, "PAO_TEXT");
                    aDetails.street = CheckNull(dt, i, "STREET_DESCRIPTION");
                    aDetails.postcode = CheckNull(dt, i, "POSTCODE");
                    aDetails.locality = ""; //relevant for NLPG results; should be empty or null for LLPG results
                    aDetails.gazetteer = "hackney";
                    aDetails.commercialOccupier = CheckNull(dt, i, "ORGANISATION");
                    aDetails.royalMailPostTown = CheckNull(dt, i, "POSTTOWN");
                    // aDetails.landPropertyUsage = (string)rows[i]["USAGE"]; //not in the source view yet
                    aDetails.isNonLocalAddressInLocalGazetteer = Convert.ToBoolean(dt.Rows[i]["NEVEREXPORT"]); //for LLPG results; should be null in results for NLPG
                    aDetails.easting = Convert.ToDouble(dt.Rows[i]["EASTING"]);
                    aDetails.northing = Convert.ToDouble(dt.Rows[i]["NORTHING"]);
                    aDetails.longitude = Convert.ToDouble(dt.Rows[i]["LONGITUDE"]);
                    aDetails.latitude = Convert.ToDouble(dt.Rows[i]["LATITUDE"]);
                    aDetails.buildingNumber = CheckNull(dt, i, "BUILDING_NUMBER");
                    aDetails.unitNumber = CheckNull(dt, i, "UNIT_NUMBER");

                    addressDetailsList.Add(aDetails);
            
                }

                return addressDetailsList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string CheckNull(DataTable dt, int rowindex, string fieldName)
        {
            string strRetVal = "";
            if (!dt.Rows[rowindex].IsNull(fieldName))
            {
                strRetVal = (string)dt.Rows[rowindex][fieldName];
            }
            return strRetVal;
        }
    }

}






