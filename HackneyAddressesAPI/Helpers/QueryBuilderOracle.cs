using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Helpers
{
    //Create Interface
    public class QueryBuilderOracle : IQueryBuilder
    {
        Dictionary<string, string> paramColumnNameMappings = new Dictionary<string, string>();

        public QueryBuilderOracle()
        {
            setMappings();
        }
        
        private void setMappings()
        {
            //Make this XML serializable
            //Set Mappings from the column attribute
            //(AttributeName, ColumnName in DB);

            paramColumnNameMappings.Add("UPRN", "UPRN");
            paramColumnNameMappings.Add("POSTCODE", "POSTCODE_NOSPACE");
            paramColumnNameMappings.Add("USRN", "USRN");

            paramColumnNameMappings.Add("PROPERTYCLASSCODE", "BLPU_CLASS");
            paramColumnNameMappings.Add("PROPERTYCLASSPRIMARY", "USAGE_PRIMARY");
            paramColumnNameMappings.Add("ADDRESSSTATUS", "LPI_LOGICAL_STATUS");
            paramColumnNameMappings.Add("STREETNAME", "STREET_DESCRIPTOR_NOSPACE");
        }

        public Dictionary<string, string> GetColumnMappings()
        {
            return paramColumnNameMappings;
        }

        public string GetAddressesQuery(List<FilterObject> filterObjects, Pagination pagination, string tableName)
        {
            //wholeQuery{ subQuery[ innerQuery( WhereClause ) ] }

            //Where Clause
            string whereClause = CreateQueryWhereClause(filterObjects);

            //Actual Query for returning non paged results
            string innerQuery = GetInnerQuery(tableName, whereClause);

            //Sub Query which has innerquery nested to set limit
            string subQuery = GetSubQuery(innerQuery, pagination);
            subQuery += " ORDER BY STREET_DESCRIPTION, BUILDING_NUMBER";

            //Whole Query which has sub query, and therefore inner query nested, to set the offset
            string wholeQuery = GetWholeQuery(subQuery, pagination);

            return wholeQuery;
        }

        public string GetStreetsQuery(List<FilterObject> filterObjects, Pagination pagination, string tableName)
        {
            //wholeQuery{ subQuery[ innerQuery( WhereClause ) ] }

            //Where Clause
            string whereClause = CreateQueryWhereClause(filterObjects);

            //Actual Query for returning non paged results
            string innerQuery = GetInnerQuery(tableName, whereClause);

            //Sub Query which has innerquery nested to set limit
            string subQuery = GetSubQuery(innerQuery, pagination);

            //Whole Query which has sub query, and therefore inner query nested, to set the offset
            string wholeQuery = GetWholeQuery(subQuery, pagination);

            return wholeQuery;
        }

        public string GetCountQuery(List<FilterObject> filterObjects, string tableName)
        {
            string query =
                    "SELECT COUNT(*) " +
                    "FROM " + tableName + " ";
            return query + CreateQueryWhereClause(filterObjects);
        }

        private string CreateQueryWhereClause(List<FilterObject> filterObjects)
        {
            StringBuilder queryWhereClause = new StringBuilder();
            queryWhereClause.Append("WHERE");
            if (filterObjects.Count > 0)
            {
                foreach (var item in filterObjects)
                {
                    if (item.isWildCard)
                    {
                        queryWhereClause.Append(" " + item.ColumnName + " LIKE :" + item.ColumnName + "|| '%' AND");
                    }
                    else
                    {
                        queryWhereClause.Append(" " + item.ColumnName + " = :" + item.ColumnName + " AND");
                    }
                }
            }
            queryWhereClause.Append(" ROWNUM >= 0");
            return queryWhereClause.ToString();
        }

        private string GetInnerQuery(string tableName, string whereClause)
        {
            return "SELECT * FROM " + tableName + " " + whereClause;
        }

        private string GetSubQuery(string innerQuery, Pagination pagination)
        {
            return "SELECT rownum rnum, a.* FROM ( " + innerQuery + " ) a WHERE rownum <= " + (pagination.offset + pagination.limit);
        }

        private string GetWholeQuery(string subQuery, Pagination pagination)
        {
            return "SELECT * FROM ( " + subQuery + " ) WHERE rnum >= " + pagination.offset;
        }

        public DbParameter[] GetParameters(List<FilterObject> filterObjects)
        {
            return GetOracleParameters(filterObjects);
        }

        private OracleParameter[] GetOracleParameters(List<FilterObject> filterObjects)
        {
            List<OracleParameter> oparams = new List<OracleParameter>();

            foreach (var item in filterObjects)
            {
                oparams.Add(new OracleParameter(item.ColumnName, item.Value));
            }

            return oparams.ToArray();
        }
    }
}
