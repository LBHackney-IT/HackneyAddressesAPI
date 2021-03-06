﻿using Dapper;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
            string selectDetailedColumns = " lpi_key as addressKey, uprn as uprn, usrn as usrn, parent_uprn as parentUPRN, lpi_logical_status as addressStatus, sao_text as unitName, unit_number as unitNumber, pao_text as buildingName, building_number as buildingNumber, street_description as street, postcode as postcode, locality as locality, gazetteer as gazetteer, organisation as commercialOccupier, ward as ward, usage_description as usageDescription, usage_primary as usagePrimary, blpu_class as usageCode, planning_use_class as planningUseClass, property_shell as propertyShell, neverexport as hackneyGazetteerOutOfBoroughAddress, easting as easting, northing as northing, longitude as longitude, latitude as latitude, lpi_start_date as addressStartDate, lpi_end_date as addressEndDate, lpi_last_update_date as addressChangeDate, blpu_start_date as propertyStartDate, blpu_end_date as propertyEndDate, blpu_last_update_date as propertyChangeDate, {0} ";
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
                //return string.Format(" ;WITH SEED AS (SELECT * FROM dbo.combined_address L {0} UNION ALL SELECT L.* FROM dbo.combined_address L JOIN SEED S ON S.PARENT_UPRN = L.UPRN) {1} from SEED S {2} ", GetSearchAddressClause(request, false, false, ref dbArgs), isCountQuery ? selectedColumns : "SELECT DISTINCT " + selectedColumns, isCountQuery ? GetSearchAddressClause(request, false, false, ref dbArgs) : GetSearchAddressClause(request, includePaging, includeRecompile, ref dbArgs));
                return string.Format(GetParentShellAddressClause(request,isCountQuery, ref dbArgs, includePaging));
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

        private static string GetParentShellAddressClause(SearchAddressRequest request, bool isCountQuery, ref DynamicParameters dbArgs, bool includePaging)
        {
            string parentShellQuery = string.Empty;
            parentShellQuery += " ;WITH SEED AS ";
            parentShellQuery += " (";

            parentShellQuery += " SELECT * FROM dbo.combined_address L ";
            parentShellQuery += GetSearchAddressClause(request, false, false, ref dbArgs);

            parentShellQuery += " UNION ALL ";
            parentShellQuery += " SELECT L.* FROM dbo.combined_address L ";
            parentShellQuery += " JOIN SEED S ON S.PARENT_UPRN = L.UPRN ";
            parentShellQuery += ") ";

            if (isCountQuery)
            {
                parentShellQuery += " select count(1) from( ";
            }
            else
            {

                if (request.Format == GlobalConstants.Format.Detailed)
                {
                    parentShellQuery += " select t.* from( ";
                }
                else
                {
                    parentShellQuery += " select t.Line1, t.Line2, t.Line3, t.Line4, t.POSTCODE, t.UPRN, t.TOWN as Town  from( ";
                }
            }
            parentShellQuery += " SELECT distinct ";
            parentShellQuery += " lpi_key as addressKey,  uprn as uprn, ";
            parentShellQuery += " usrn as usrn, parent_uprn as parentUPRN, lpi_logical_status as addressStatus, sao_text as unitName, ";
            parentShellQuery += " unit_number as unitNumber, pao_text as buildingName, building_number as buildingNumber, street_description as street, ";
            parentShellQuery += " postcode as postcode, locality as locality, gazetteer as gazetteer, organisation as commercialOccupier, ward as ward, ";
            parentShellQuery += " usage_description as usageDescription, usage_primary as usagePrimary, blpu_class as usageCode, planning_use_class as planningUseClass, ";
            parentShellQuery += " property_shell as propertyShell, neverexport as hackneyGazetteerOutOfBoroughAddress, easting as easting, northing as northing, ";
            parentShellQuery += " longitude as longitude, latitude as latitude, lpi_start_date as addressStartDate, lpi_end_date as addressEndDate, lpi_last_update_date as addressChangeDate, blpu_start_date as propertyStartDate, blpu_end_date as propertyEndDate, blpu_last_update_date as propertyChangeDate,  Line1, Line2, Line3, Line4 , town as town, paon_start_num ";

            parentShellQuery += " from SEED S  ";
            parentShellQuery += GetSearchAddressClause(request, false, false, ref dbArgs);

            parentShellQuery += " ) t";
            if (!isCountQuery && request.Format == GlobalConstants.Format.Detailed)
            {
                parentShellQuery += " ORDER BY town,                                    ";
                parentShellQuery += " (CASE WHEN postcode IS NULL THEN 1 ELSE 0 END), ";
                //parentShellQuery += " postcode, ";
                parentShellQuery += " street,                                     ";
                parentShellQuery += " (CASE WHEN (paon_start_num IS NULL or paon_start_num = 0) THEN 1 ELSE 0 END),  paon_start_num,   ";
                parentShellQuery += " (CASE WHEN buildingNumber IS NULL THEN 1 ELSE 0 END), buildingNumber,   ";
                parentShellQuery += " (CASE WHEN unitNumber IS NULL THEN 1 ELSE 0 END),  unitNumber,                                     ";
                parentShellQuery += " (CASE WHEN unitName IS NULL THEN 1 ELSE 0 END), unitName  ";
            }
            if (!isCountQuery && request.Format == GlobalConstants.Format.Simple)
            {
                parentShellQuery += " order by Line1, Line2, Line3, Line4, Town ";
            }
            if (!isCountQuery && includePaging)
            {
                parentShellQuery += DoPaging(request);// " OFFSET 0 ROWS FETCH NEXT 50 ROWS ONLY  OPTION(RECOMPILE) ;";
            }
            return parentShellQuery;
        }

        private static string DoPaging(SearchAddressRequest request)
        {
            int page = request.Page;
            int pageSize = request.PageSize;
            int lower = 0;
            lower = page == 0 || page == 1 ? 0 : (page - 1) * pageSize;
            return string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", lower, pageSize);
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
            if (!string.IsNullOrEmpty(request.usagePrimary))
            {
                if (request.usagePrimary.ToLower().Replace(" ", "").Contains("parentshell"))
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
                dbArgs.Add("@postcode", request.PostCode.Replace(" ", "") + "%", DbType.AnsiString);
                clause += " AND POSTCODE_NOSPACE LIKE @postcode  ";
            }

            if (!string.IsNullOrEmpty(request.BuildingNumber))
            {
                dbArgs.Add("@buildingnumber", request.BuildingNumber + "%");
                clause += " AND BUILDING_NUMBER LIKE @buildingnumber  ";
            }

            if (!string.IsNullOrEmpty(request.Street))
            {
                dbArgs.Add("@street", request.Street + "%");
                clause += " AND STREET_DESCRIPTION LIKE @street  ";
            }

            if (!string.IsNullOrEmpty(request.AddressStatus)) //AddressStatus/LPI_LOGICAL_STATUS
            {
                string[] addressStatuses = request.AddressStatus.ToString().Split(",").Distinct(StringComparer.CurrentCultureIgnoreCase).ToArray();
                if (addressStatuses.Count() == 1)
                {
                    dbArgs.Add("@addressStatus", request.AddressStatus.ToString(), DbType.AnsiString);
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
                dbArgs.Add("@addressStatus", "Approved Preferred", DbType.AnsiString);
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

            if (!string.IsNullOrEmpty(request.usagePrimary))
            {
                string[] propertyClasses = request.usagePrimary.ToString().Split(",").Distinct(StringComparer.CurrentCultureIgnoreCase).ToArray();
                propertyClasses = propertyClasses.Where(x => x.ToLower() != "parent shell").ToArray();
                if (propertyClasses.Count() != 0)
                {
                    if (propertyClasses.Count() == 1)
                    {
                        dbArgs.Add("@primaryClass", request.usagePrimary, DbType.AnsiString);
                        clause += " AND USAGE_PRIMARY = @primaryClass ";
                    }
                    else
                    {
                        dbArgs.Add("@primaryClass", propertyClasses);
                        clause += " AND USAGE_PRIMARY IN @primaryClass ";
                    }
                }
            }

            if (!string.IsNullOrEmpty(request.usageCode))
            {
                string[] classCodes = request.usageCode.Split(",").Distinct(StringComparer.CurrentCultureIgnoreCase).ToArray();
                if (classCodes.Count() == 1)
                {
                    dbArgs.Add("@propertyClassCode", request.usageCode + "%", DbType.AnsiString);
                    clause += " AND BLPU_CLASS LIKE @propertyClassCode ";
                }
                else
                {
                    dbArgs.Add("@propertyClassCode", classCodes);
                    clause += " AND BLPU_CLASS IN @propertyClassCode ";
                }
            }

            if (request.Gazetteer == GlobalConstants.Gazetteer.Local)//Gazetteer
            {
                dbArgs.Add("@gazetteer", request.Gazetteer.ToString(), DbType.AnsiString);
                dbArgs.Add("@neverexport", request.HackneyGazetteerOutOfBoroughAddress, DbType.Boolean);
                clause += " AND Gazetteer = @gazetteer ";
                clause += " AND neverexport = @neverexport ";
            }
            else if(request.Gazetteer == GlobalConstants.Gazetteer.Both && request.HackneyGazetteerOutOfBoroughAddress != null)
            {
                dbArgs.Add("@neverexport", request.HackneyGazetteerOutOfBoroughAddress, DbType.Boolean);
                clause += " AND neverexport = @neverexport ";
            }

            if (includePaging)//paging
            {
                int page = request.Page;
                int pageSize = request.PageSize;
                int lower = 0;
                lower = page == 0 || page == 1 ? 0 : page * pageSize;
                // paging so if current page passed in is 1 then we set lower bound to be 0 (0 based index). Otherwise we multiply by the page size

                clause += @" ORDER BY town,
                                     (CASE WHEN postcode IS NULL THEN 1 ELSE 0 END),                                     
                                     street_description, 
                                     (CASE WHEN (paon_start_num IS NULL or paon_start_num = 0) THEN 1 ELSE 0 END), 
                                     paon_start_num,
                                     (CASE WHEN building_number IS NULL THEN 1 ELSE 0 END),
                                     building_number,
                                     (CASE WHEN unit_number IS NULL THEN 1 ELSE 0 END),
                                     unit_number,
                                     (CASE WHEN sao_text IS NULL THEN 1 ELSE 0 END),
                                     sao_text ";


                clause += DoPaging(request);// string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", lower, pageSize);
            }
            if (includeRecompile)//recompile
            {
                clause += " OPTION(RECOMPILE) ";
            }
            return clause;
        }
    }
}
