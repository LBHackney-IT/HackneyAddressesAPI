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
        public static string GetSingleAddress(GlobalConstants.Format detailed)
        {
            return GetAddressesQuery(detailed) + GetSingleAddressClause();
        }

        public static string GetSearchAddressQuery(GlobalConstants.Format format, SearchAddressRequest request, bool includePaging, bool includeRecompile, bool isCountQuery, ref DynamicParameters dbArgs)
        {
            if (isCountQuery)
            {
                return GetAddressCountQuery() + GetSearchAddressClause(request, includePaging, includeRecompile, ref dbArgs);
            }
            else
            {
                return GetAddressesQuery(format) + GetSearchAddressClause(request, includePaging, includeRecompile, ref dbArgs);
            }

        }

        private static string GetAddressesQuery(GlobalConstants.Format format)
        {
            string selectSimpleColumns = string.Format(" SAO_TEXT as Line1, coalesce(UNIT_NUMBER,'') + ' ' + PAO_TEXT as Line2, BUILDING_NUMBER + ' ' + STREET_DESCRIPTION as Line3, LOCALITY as Line4{0} ", format == GlobalConstants.Format.Simple ? ", POSTTOWN as City, Postcode, UPRN, LPI_KEY as AddressID " : " ");
            string selectDetailedColumns = string.Format(" LPI_KEY as AddressID,UPRN, USRN, PARENT_UPRN as parentUPRN,LPI_Logical_Status as addressStatus,SAO_TEXT as unitName,UNIT_NUMBER as unitNumber,PAO_TEXT as buildingName,BUILDING_NUMBER as buildingNumber,STREET_DESCRIPTION as street,POSTCODE as postcode,LOCALITY as locality,GAZETTEER as gazetteer,ORGANISATION as commercialOccupier, WARD as ward, POSTTOWN as royalMailPostTown,USAGE_DESCRIPTION as usageClassDescription,USAGE_PRIMARY as usageClassPrimary,BLPU_CLASS as usageClassCode, PROPERTY_SHELL as propertyShell,NEVEREXPORT as isNonLocalAddressInLocalGazetteer,EASTING as easting, NORTHING as northing, LONGITUDE as longitude, LATITUDE as latitude, {0} ", selectSimpleColumns);
            string selectParentShells = string.Format(" WITH SEED AS (SELECT * FROM dbo.combined_address L WHERE POSTCODE_NOSPACE LIKE @varPCID UNION ALL SELECT L.* FROM dbo.combined_address L JOIN SEED S ON S.PARENT_UPRN = L.UPRN) SELECT DISTINCT {0} from SEED S ", selectDetailedColumns);

            string query = string.Empty;
            if (format == GlobalConstants.Format.Detailed)
            {
                query = "SELECT " + selectDetailedColumns;
                query += selectSimpleColumns;
            }
            else
            {
                query = string.Format("SELECT {0} ", selectSimpleColumns);
            }
            return query;
        }

       
        private static string GetAddressCountQuery()
        {
            return "SELECT count(1) ";
        }

        private static string GetSingleAddressClause()
        {
            return " FROM dbo.combined_address WHERE LPI_KEY = @key";
        }


        private static string GetSearchAddressClause(SearchAddressRequest request, bool includePaging, bool includeRecompile, ref DynamicParameters dbArgs)
        {

            string clause = string.Format(" FROM dbo.combined_address L WHERE PROPERTY_SHELL <> 1 ");

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
                dbArgs.Add("@primaryClass", request.PropertyClassPrimary.ToString());
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
                clause += " ORDER BY street_description, building_number DESC ";
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
