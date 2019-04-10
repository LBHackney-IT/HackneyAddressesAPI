using Dapper;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Helpers
{
    public class QueryBuilder
    {
        public static string GetSingleAddress(GlobalConstants.Format format)
        {
            return GetAddressesQuery(format) + " FROM dbo.combined_address WHERE LPI_KEY = @key";
        }

        public static string GetSearchAddressQuery(SearchAddressRequest request, bool includePaging, bool includeRecompile, bool isCountQuery, ref DynamicParameters dbArgs)
        {
            return GetAddressesQuery(request, includePaging, includeRecompile, isCountQuery, ref dbArgs);
        }

        /// <summary>
        /// Does all the work for returning the right query with the right select and where parameters for the incoming request
        /// </summary>
        /// <param name="request">Request object from the API call</param>
        /// <param name="includePaging">Whether to include paging</param>
        /// <param name="includeRecompile">Whether to include the recompile...(Not sure if this is needed)</param>
        /// <param name="isCountQuery">The DB call needs the count in order to effectively do the paging</param>
        /// <param name="dbArgs">ref object which builds up the database parameter arguments</param>
        /// <returns>the SQL query to be run on the database</returns>
        private static string GetAddressesQuery(SearchAddressRequest request, bool includePaging, bool includeRecompile, bool isCountQuery, ref DynamicParameters dbArgs)
        {
            string selectedColumns = string.Empty;
            string selectDetailedColumns = " lpi_key as addressKey, uprn as uprn, usrn as usrn, parent_uprn as parentUPRN, lpi_logical_status as addressStatus, sao_text as unitName, unit_number as unitNumber, pao_text as buildingName, building_number as buildingNumber, street_description as street, postcode as postcode, locality as locality, town as town, gazetteer as gazetteer, organisation as commercialOccupier, ward as ward, usage_description as usageDescription, usage_primary as usagePrimary, blpu_class as usageCode, planning_use_class as planningUseClass, property_shell as propertyShell, neverexport as hackneyGazetteerOutOfBoroughAddress, easting as easting, northing as northing, longitude as longitude, latitude as latitude, {0} ";
            string selectSimpleColumns = " Line1, Line2, Line3, Line4 {0}, TOWN as Town";
            GlobalConstants.Format format = request.Format;
            if (isCountQuery)
            {
                selectedColumns = "SELECT COUNT(1) ";
            }
            else
            {
                if (format == GlobalConstants.Format.Detailed)
                {
                    //requested format is detailed so we request columns but also include simple format
                    selectedColumns = string.Format(selectDetailedColumns, string.Format(selectSimpleColumns,""));
                }
                else
                {
                    //Requested format is simple so we amend query accordingly
                    selectedColumns = string.Format(selectSimpleColumns, format == GlobalConstants.Format.Simple ? ", Postcode, UPRN " : " ");
                }
            }
            if (IncludeParentShell(request))
            {
                //if parent shells are needed we need to take into account parents with no postcode hence query changes
                return string.Format(" ;WITH SEED AS (SELECT * FROM dbo.combined_address L {0} UNION ALL SELECT L.* FROM dbo.combined_address L JOIN SEED S ON S.PARENT_UPRN = L.UPRN) {1} from SEED S {2} ", GetSearchAddressClause(request, false, false, ref dbArgs), isCountQuery ? selectedColumns : "SELECT DISTINCT " + selectedColumns, isCountQuery ? GetSearchAddressClause(request, false, false, ref dbArgs) : GetSearchAddressClause(request, includePaging, includeRecompile, ref dbArgs));
            }
            else
            {
                if (isCountQuery)
                {
                    //count query so we change format of query
                    return selectedColumns + GetSearchAddressClause(request, false, includeRecompile, ref dbArgs);
                }
                else
                {
                    //not count query so we format the query accordingly
                    return "SELECT " + selectedColumns + GetSearchAddressClause(request, includePaging, includeRecompile, ref dbArgs);
                }
            }
        }

        public static string GetCrossReferences(GetAddressCrossReferenceRequest request)
        {
            return string.Format("SELECT [xref_key] as crossRefKey,[uprn],[xref_code] as code,[xref_name] as name,[xref_value] as value,[xref_end_date] as endDate FROM dbo.hackney_xref where UPRN = @UPRN ");
        }

        /// <summary>
        /// test to decide whether parent shells should be included in query. 
        /// Can come from PropertyClassPrimary being set to ParentShell
        /// Can also come from other fields (to be determined)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>whether to include parent shells or not</returns>
        private static bool IncludeParentShell(SearchAddressRequest request)
        {
            if (!string.IsNullOrEmpty(request.PropertyClassPrimary))
            {
                if (request.PropertyClassPrimary.Replace(" ", "").Contains("ParentShell"))
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        private static string GetAddressesQuery(GlobalConstants.Format format)
        {
            string selectSimpleColumns = " Line1, Line2, Line3, Line4, TOWN as Town ";
            string selectDetailedColumns = string.Format(" lpi_key as addressKey, uprn as uprn, usrn as usrn, parent_uprn as parentUPRN, lpi_logical_status as addressStatus, sao_text as unitName, unit_number as unitNumber, pao_text as buildingName, building_number as buildingNumber, street_description as street, postcode as postcode, locality as locality, town as town, gazetteer as gazetteer, organisation as commercialOccupier, ward as ward, usage_description as usageDescription, usage_primary as usagePrimary, blpu_class as usageCode, planning_use_class as planningUseClass, property_shell as propertyShell, neverexport as hackneyGazetteerOutOfBoroughAddress, easting as easting, northing as northing, longitude as longitude, latitude as latitude, {0} ", selectSimpleColumns);

            string query = string.Empty;
            if (format == GlobalConstants.Format.Detailed)
            {
                query = "SELECT " + selectDetailedColumns;
            }
            else
            {
                query = string.Format("SELECT {0} ", selectSimpleColumns);
            }
            return query;
        }
        

        /// <summary>
        /// Formats the Where clause of the SQL query depending on the provided paramaters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="includePaging"></param>
        /// <param name="includeRecompile"></param>
        /// <param name="dbArgs"></param>
        /// <returns>WHERE clause portion of the SQL query</returns>
        private static string GetSearchAddressClause(SearchAddressRequest request, bool includePaging, bool includeRecompile, ref DynamicParameters dbArgs)
        {
            string clause = string.Empty;
            if (IncludeParentShell(request))
            {
                clause = " WHERE 1=1 ";
            }
            else
            {
                clause = string.Format(" FROM dbo.combined_address L WHERE PROPERTY_SHELL <> 1 ");
            }
            

            if (!string.IsNullOrEmpty(request.PostCode))
            {
                dbArgs.Add("@postcode", request.PostCode.Replace(" ", "") + "%");
                clause += " AND POSTCODE_NOSPACE LIKE @postcode  ";
            }

            if (!string.IsNullOrEmpty(request.BuildingNumber))
            {
                dbArgs.Add("@buildingnumber", request.BuildingNumber + "%");
                clause += " AND BUILDING_NUMBER LIKE @buildingnumber  ";
            }

            if (!string.IsNullOrEmpty(request.Street))
            {
                dbArgs.Add("@street", "%" + request.Street + "%");
                clause += " AND STREET_DESCRIPTION LIKE @street  ";
            }

            if (!string.IsNullOrEmpty(request.AddressStatus)) //AddressStatus/LPI_LOGICAL_STATUS
            {
                string[] addressStatuses = request.AddressStatus.ToString().Split();
                if (addressStatuses.Count() == 1)
                {
                    dbArgs.Add("@addressStatus", request.AddressStatus.ToString());
                    clause += " AND LPI_LOGICAL_STATUS = @addressStatus ";
                }
                else
                {
                    //need to convert address statuses
                    dbArgs.Add("@addressStatus", addressStatuses);
                    clause += " AND LPI_LOGICAL_STATUS IN @addressStatus ";
                }
            }
            else // No address status default it to approved preferred
            {
                dbArgs.Add("@addressStatus", "Approved Preferred");
                clause += " AND LPI_LOGICAL_STATUS = @addressStatus ";
            }


            if (request.UPRN != null)
            {
                dbArgs.Add("@uprn", request.UPRN);
                clause += " AND UPRN = @uprn ";
            }

            if (request.USRN != null)
            {
                dbArgs.Add("@usrn", request.USRN);
                clause += " AND USRN = @usrn ";
            }

            if (!string.IsNullOrEmpty(request.PropertyClassPrimary))
            {
                string[] propertyClasses = request.PropertyClassPrimary.ToString().Split();
                if (propertyClasses.Count() == 1)
                {
                    dbArgs.Add("@primaryClass", request.PropertyClassPrimary);
                    clause += " AND USAGE_PRIMARY = @primaryClass ";
                }
                else
                {
                    dbArgs.Add("@primaryClass", propertyClasses);
                    clause += " AND USAGE_PRIMARY IN @primaryClass ";
                }
            }

            if (!string.IsNullOrEmpty(request.PropertyClassCode))
            {
                string[] classCodes = request.PropertyClassCode.Split(',');
                if (classCodes.Count() == 1)
                {
                    dbArgs.Add("@propertyClassCode", request.PropertyClassCode + "%");
                    clause += " AND BLPU_CLASS LIKE @propertyClassCode ";
                }
                else
                {
                    dbArgs.Add("@propertyClassCode", classCodes);
                    clause += " AND BLPU_CLASS IN @propertyClassCode ";
                }
            }

            if (request.Gazetteer == GlobalConstants.Gazetteer.Both ? false : true)//Gazetteer
            {
                dbArgs.Add("@gazetteer", request.Gazetteer.ToString());
                clause += " AND Gazetteer = @gazetteer ";
            }

            if (includePaging)//paging
            {
                int page = request.Page;
                int pageSize = request.PageSize;
                int lower = 0;
                lower = page == 0 || page == 1 ? 0 : page * pageSize;
                // paging so if current page passed in is 1 then we set lower bound to be 0 (0 based index). Otherwise we multiply by the page size

                if (request.Format == GlobalConstants.Format.Detailed)
                {
                    clause += " ORDER BY street_description, building_number DESC ";
                }
                else
                {
                    clause += " ORDER BY Line2, Line1 DESC ";
                }


                clause += string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", lower, pageSize);
            }
            if (includeRecompile)//recompile
            {
                clause += " OPTION(RECOMPILE) ";
            }
            return clause;
        }
    }
}
