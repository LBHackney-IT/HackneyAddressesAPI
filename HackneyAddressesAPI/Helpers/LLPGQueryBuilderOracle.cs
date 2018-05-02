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
    public class AddressesQueryBuilderOracle : ILLPGQueryBuilder
    {
        Dictionary<string, string> paramColumnNameMappings = new Dictionary<string, string>();

        public AddressesQueryBuilderOracle()
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
        }

        public Dictionary<string, string> GetColumnMappings()
        {
            return paramColumnNameMappings;
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

        public string GetQuery(List<FilterObject> filterObjects, Pagination pagination, string tableName)
        {
            var offset = pagination.offset;
            var limit = pagination.limit;

            //wholeQuery{ subQuery[ innerQuery( WhereClause ) ] }

            //Where Clause
            string whereClause = CreateQueryWhereClause(filterObjects);

            //Actual Query for returning non paged results
            // FROM NLPG.NLPG_REST_API
            string innerQuery = "SELECT * FROM " + tableName + " " + whereClause;

            //Sub Query which has innerquery nested to set limit
            string subQuery = "SELECT rownum rnum, a.* FROM ( " + innerQuery + " ) a WHERE rownum <= " + (offset + limit);
            subQuery += " ORDER BY STREET_DESCRIPTION, BUILDING_NUMBER";

            //Whole Query which has sub query, and therefore inner query nested, to set the offset
            string wholeQuery = "SELECT * FROM ( " + subQuery + " ) WHERE rnum >= " + offset;

            return wholeQuery;
        }

        public string GetCountQuery(List<FilterObject> filterObjects, string tableName)
        {
            string query =
                    "SELECT COUNT(*) " +
                    "FROM " + tableName + " ";
            return query + CreateQueryWhereClause(filterObjects);
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

        public DbParameter[] GetParameters(List<FilterObject> filterObjects)
        {
            return GetOracleParameters(filterObjects);
        }

        public string GetQueryBoth(List<FilterObject> filterObjects, Pagination pagination, string tableName1, string tableName2)
        {
            var offset = pagination.offset;
            var limit = pagination.limit;

            //wholeQuery{ subQuery[ innerQueryCombined< innerQuery1( WhereClause ) UNION ALL innerQuery2( WhereClause ) > ] }

            //Where Clause
            string whereClause = CreateQueryWhereClause(filterObjects);

            //Actual Query for returning non paged results
            string innerQuery1 = "SELECT * FROM " + tableName1 + " " + whereClause;
            string innerQuery2 = "SELECT * FROM " + tableName2 + " " + whereClause;

            string innerQueryCombined = "SELECT * FROM ( " + innerQuery1 + " UNION ALL " + innerQuery2 + " ) a order by a.STREET_DESCRIPTION, a.BUILDING_NUMBER";

            //Sub Query which has innerquery nested to set limit
            string subQuery = "SELECT rownum rnum, a.* FROM ( " + innerQueryCombined + " ) a WHERE rownum <= " + (offset + limit);

            //Whole Query which has sub query, and therefore inner query nested, to set the offset
            string wholeQuery = "SELECT * FROM ( " + subQuery + " ) WHERE rnum >= " + offset;

            return wholeQuery;
        }

        public string GetCountQueryBoth(List<FilterObject> filterObjects, string tableName1, string tableName2)
        {
            string whereClause = CreateQueryWhereClause(filterObjects);

            string innerQuery1 = "SELECT * FROM " + tableName1 + " " + whereClause;
            string innerQuery2 = "SELECT * FROM " + tableName2 + " " + whereClause;

            string countQueryCombined = "SELECT COUNT (*) FROM ( " + innerQuery1 + " UNION ALL " + innerQuery2 + " )";

            return countQueryCombined;
        }
    }
}
