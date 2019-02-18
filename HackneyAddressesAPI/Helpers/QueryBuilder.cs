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

        private static string GetAddressesQuery(SearchAddressRequest request, bool includePaging, bool includeRecompile, bool isCountQuery, ref DynamicParameters dbArgs)
        {
            string selectedColumns = string.Empty;
            string selectDetailedColumns = " LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazetteer,ORGANISATION as commercialOccupier, WARD as ward, TOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode, PROPERTY_SHELL as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazetteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude, {0} ";
            string selectSimpleColumns = " SAO_TEXT as Line1, coalesce(UNIT_NUMBER,'') + ' ' + PAO_TEXT as Line2, BUILDING_NUMBER + ' ' + STREET_DESCRIPTION as Line3, LOCALITY as Line4 {0}";
            GlobalConstants.Format format = request.Format;
            if (isCountQuery)
            {
                selectedColumns = "SELECT COUNT(1) ";
            }
            else
            {
                if (format == GlobalConstants.Format.Detailed)
                {
                    selectedColumns = string.Format(selectDetailedColumns, string.Format(selectSimpleColumns,""));
                }
                else
                {
                    selectedColumns = string.Format(selectSimpleColumns, format == GlobalConstants.Format.Simple ? ", TOWN as City, Postcode, UPRN, LPI_KEY as AddressID " : " ");
                }
            }
            if (IncludeParentShell(request))
            {
                //TODO: need to work out how to get the query working in this one...
                return string.Format(" ;WITH SEED AS (SELECT * FROM dbo.combined_address L WHERE POSTCODE_NOSPACE LIKE @postcode UNION ALL SELECT L.* FROM dbo.combined_address L JOIN SEED S ON S.PARENT_UPRN = L.UPRN) {0} from SEED S {1} ", isCountQuery ? selectedColumns : "SELECT DISTINCT " + selectedColumns, isCountQuery ? GetSearchAddressClause(request, false, false, ref dbArgs) : GetSearchAddressClause(request, includePaging, includeRecompile, ref dbArgs));
            }
            else
            {
                if (isCountQuery)
                {
                    return selectedColumns + GetSearchAddressClause(request, false, includeRecompile, ref dbArgs);
                }
                else
                {
                    return "SELECT " + selectedColumns + GetSearchAddressClause(request, includePaging, includeRecompile, ref dbArgs);
                }
            }
        }

        /// <summary>
        /// test to decide whether parent shells should be included in query. 
        /// Can come from PropertyClassPrimary being set to ParentShell
        /// Can also come from 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static bool IncludeParentShell(SearchAddressRequest request)
        {
            if (request.PropertyClassPrimary == GlobalConstants.PropertyClassPrimary.ParentShell)
            {
                return true;
            }
            else
                return false;
        }

        private static string GetAddressesQuery(GlobalConstants.Format format)
        {
            string selectSimpleColumns = string.Format(" SAO_TEXT as Line1, coalesce(UNIT_NUMBER,'') + ' ' + PAO_TEXT as Line2, BUILDING_NUMBER + ' ' + STREET_DESCRIPTION as Line3, LOCALITY as Line4{0} ", format == GlobalConstants.Format.Simple ? ", TOWN as City, Postcode, UPRN, LPI_KEY as AddressID " : " ");
            string selectDetailedColumns = string.Format(" LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazetteer,ORGANISATION as commercialOccupier, WARD as ward, TOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode, PROPERTY_SHELL as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazetteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude, {0} ", selectSimpleColumns);

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

            if (!string.IsNullOrEmpty(request.AddressStatus.ToString())) //AddressStatus/LPI_LOGICAL_STATUS
            {
                dbArgs.Add("@addressStatus", GlobalConstants.MapAddressStatus(request.AddressStatus));
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

            if (!string.IsNullOrEmpty(request.PropertyClassPrimary.ToString()))
            {
                dbArgs.Add("@primaryClass", GlobalConstants.MapPrimaryPropertyClass((GlobalConstants.PropertyClassPrimary)request.PropertyClassPrimary));
                clause += " AND USAGE_PRIMARY = @primaryClass ";
            }

            if (!string.IsNullOrEmpty(request.PropertyClassCode))
            {
                dbArgs.Add("@propertyClassCode", request.PropertyClassCode + "%");
                clause += " AND BLPU_CLASS LIKE @propertyClassCode ";
            }

            if (request.Gazeteer == GlobalConstants.Gazetteer.Both ? false : true)//Gazetteer
            {
                dbArgs.Add("@gazetteer", request.Gazeteer.ToString());
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
