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
    public class LLPGQueryBuilderOracle : ILLPGQueryBuilder
    {
        Dictionary<string, string> paramColumnNameMappings = new Dictionary<string, string>();

        public LLPGQueryBuilderOracle()
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
            queryWhereClause.Append(" ROWNUM >= " + 0);
            return queryWhereClause.ToString();
        }

        public string GetQuery(List<FilterObject> filterObjects, int offset, int limit)
        {
            //?#? Maybe Extract table names from config file.

            //wholeQuery{ subQuery[ innerQuery( WhereClause ) ] }

            //Where Clause
            string whereClause = CreateQueryWhereClause(filterObjects);

            //Actual Query for returning non paged results
            string innerQuery = "SELECT * FROM LLPG.LLPG_REST_API " + whereClause;

            //Sub Query which has innerquery nested to set limit
            string subQuery = "SELECT rownum rnum, a.* FROM ( " + innerQuery + " ) a WHERE rownum <= " + (offset + limit);

            //Whole Query which has sub query, and therefore inner query nested, to set the offset
            string wholeQuery = "SELECT * FROM ( " + subQuery + " ) WHERE rnum >= " + offset;

            return wholeQuery;
        }

        public string GetCountQuery(List<FilterObject> filterObjects)
        {
            string query =
                    "SELECT COUNT(*) " +
                    "FROM LLPG.LLPG_REST_API ";
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

    }
}
