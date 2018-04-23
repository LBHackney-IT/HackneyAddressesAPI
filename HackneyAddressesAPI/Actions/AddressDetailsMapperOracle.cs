using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Actions
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
                    aDetails.AddressID = "999999";

                    if (!dt.Rows[i].IsNull("POSTTOWN"))
                    {
                        aDetails.City = (string)dt.Rows[i]["POSTTOWN"];
                    }

                    if (!dt.Rows[i].IsNull("POSTCODE"))
                    {
                        aDetails.Postcode = (string)dt.Rows[i]["POSTCODE"];
                    }


                    if (!dt.Rows[i].IsNull("BUILDING_NUMBER"))
                    {
                        aDetails.Line1 = (string)dt.Rows[i]["BUILDING_NUMBER"];
                    }

                    aDetails.Line2 = (string)dt.Rows[i]["STREET_DESCRIPTION"];

                    //I understand the code in the document, but I am unsure to what columns I need to use in the DB
                    aDetails.Line3 = "Locality 3?";
                    aDetails.Line4 = "Locality 4?";

                    addressDetailsList.Add(aDetails);
                }

                return addressDetailsList;
            }
            catch (Exception ex)
            {

                throw;
            }
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
                    //handle nulls
                    if (!dt.Rows[i].IsNull("PARENT_UPRN"))
                    {
                        aDetails.parentUniquePropertyReferenceNumber = Convert.ToInt64(dt.Rows[i]["PARENT_UPRN"]);
                    }
                    aDetails.addressStatus = (string)dt.Rows[i]["LPI_LOGICAL_STATUS"];
                    if (!dt.Rows[i].IsNull("SAO_TEXT"))
                    {
                        aDetails.unitName = (string)dt.Rows[i]["SAO_TEXT"];
                    }

                    if (!dt.Rows[i].IsNull("PAO_TEXT"))
                    {
                        aDetails.buildingName = (string)dt.Rows[i]["PAO_TEXT"];
                    }

                    aDetails.street = (string)dt.Rows[i]["STREET_DESCRIPTION"];
                    if (!dt.Rows[i].IsNull("POSTCODE"))
                    {
                        aDetails.postcode = (string)dt.Rows[i]["POSTCODE"];
                    }
                    aDetails.locality = ""; //relevant for NLPG results; should be empty or null for LLPG results
                    aDetails.gazetteer = "hackney";
                    if (!dt.Rows[i].IsNull("ORGANISATION"))
                    {
                        aDetails.commercialOccupier = (string)dt.Rows[i]["ORGANISATION"];
                    }
                    if (!dt.Rows[i].IsNull("POSTTOWN"))
                    {
                        aDetails.royalMailPostTown = (string)dt.Rows[i]["POSTTOWN"];
                    }
                    // aDetails.landPropertyUsage = (string)rows[i]["USAGE"]; //not in the source view yet
                    aDetails.isNonLocalAddressInLocalGazetteer = Convert.ToBoolean(dt.Rows[i]["NEVEREXPORT"]); //for LLPG results; should be null in results for NLPG
                    aDetails.easting = Convert.ToDouble(dt.Rows[i]["EASTING"]);
                    aDetails.northing = Convert.ToDouble(dt.Rows[i]["NORTHING"]);
                    aDetails.longitude = Convert.ToDouble(dt.Rows[i]["LONGITUDE"]);
                    aDetails.latitude = Convert.ToDouble(dt.Rows[i]["LATITUDE"]);

                    if (!dt.Rows[i].IsNull("BUILDING_NUMBER"))
                    {
                        aDetails.buildingNumber = (string)dt.Rows[i]["BUILDING_NUMBER"];
                    }

                    if (!dt.Rows[i].IsNull("UNIT_NUMBER"))
                    {
                        aDetails.unitNumber = (string)dt.Rows[i]["UNIT_NUMBER"];
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
    }
}
