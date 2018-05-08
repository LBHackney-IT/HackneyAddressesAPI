using HackneyAddressesAPI.Helpers;
using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Actions
{
    public class StreetsActions : IStreetsActions
    {
        private IDB_Helper _db_Helper;
        private IFormatter _formatter;
        private IFilterObjectBuilder _fob;
        private IStreetDetailsMapper _addressDetailsMapper;
        private IConfigReader _config;
        private IQueryBuilder _streetsQueryBuilder;

        public StreetsActions(IDB_Helper db_Helper,
            IQueryBuilder streetsQueryBuilder,
            IConfigReader config,
            IFormatter formatter,
            IFilterObjectBuilder filterObjectBuilder,
            IStreetDetailsMapper addressDetailsMapper)
        {
            _db_Helper = db_Helper ?? throw new ArgumentNullException("db_Helper");
            _streetsQueryBuilder = streetsQueryBuilder ?? throw new ArgumentNullException("llpg_QueryBuilder");
            _formatter = formatter ?? throw new ArgumentNullException("formatter");
            _fob = filterObjectBuilder ?? throw new ArgumentNullException("filterObjectBuilder");
            _addressDetailsMapper = addressDetailsMapper ?? throw new ArgumentNullException("addressDetailsMapper");
            _config = config ?? throw new ArgumentNullException("config");
        }

        public async Task<object> GetStreets(StreetsQueryParams queryParams, Pagination pagination)
        {
            List<FilterObject> filterObjects = formatAndAddToFilter(queryParams);

            string jsonConnString1 = GlobalConstants.LLPG_ADDRESSES_JSON;
            string jsonConnString2 = GlobalConstants.NLPG_ADDRESSES_JSON;

            Object resultset = null;
            Object dataTable = null;

            if (queryParams.Gazetteer == "National")
            {
                pagination = await callDatabaseAsyncPagination(filterObjects, pagination, jsonConnString2);
                dataTable = await callDatabaseAsync(filterObjects, pagination, jsonConnString2);
            }
            else if (queryParams.Gazetteer == "Both")
            {
                pagination = await callDatabaseAsyncPaginationBoth(filterObjects, pagination, jsonConnString1, jsonConnString2);
                dataTable = await callDatabaseAsyncBoth(filterObjects, pagination, jsonConnString1, jsonConnString2);
            }
            else
            {
                pagination = await callDatabaseAsyncPagination(filterObjects, pagination, jsonConnString1);
                dataTable = await callDatabaseAsync(filterObjects, pagination, jsonConnString1);
            }

            resultset = new { resultset = pagination };

            var result = _addressDetailsMapper.MapStreetDetails((DataTable)dataTable);
   

            return new { Addresses = result, metadata = resultset };
        }

        private List<FilterObject> formatAndAddToFilter(StreetsQueryParams queryParams)
        {
            queryParams = _formatter.FormatStreetsQueryParams(queryParams);

            List<FilterObject> filterObjects = _fob.ProcessQueryParamsToFilterObjects(queryParams, _streetsQueryBuilder.GetColumnMappings());

            return filterObjects;
        }

        private async Task<DataTable> callDatabaseAsync(List<FilterObject> filterObjects, Pagination pagination, string jsonConnString)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting(jsonConnString + ":ConnectionString").ToString();
            string tableName = _config.getConfigurationSetting(jsonConnString + ":TableName").ToString();

            //Set up Queries and params
            string queryNormal = _streetsQueryBuilder.GetAddressesQuery(filterObjects, pagination, tableName);
            DbParameter[] dbParamaters = _streetsQueryBuilder.GetParameters(filterObjects);

            //Execute Database
            return _db_Helper.ExecuteToDataTable(conn, queryNormal, dbParamaters);
        }

        private async Task<Pagination> callDatabaseAsyncPagination(List<FilterObject> filterObjects, Pagination pagination, string jsonConnString)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting(jsonConnString + ":ConnectionString").ToString();
            string tableName = _config.getConfigurationSetting(jsonConnString + ":TableName").ToString();

            //Set up Queries and params
            string queryCount = _streetsQueryBuilder.GetCountQuery(filterObjects, tableName);
            DbParameter[] dbParamaters = _streetsQueryBuilder.GetParameters(filterObjects);

            //Execute Database
            int count = _db_Helper.ExecuteScalarToInt(conn, queryCount, dbParamaters);
            pagination.count = count;

            return pagination;
        }

        private async Task<Pagination> callDatabaseAsyncPaginationBoth(List<FilterObject> filterObjects, Pagination pagination, string jsonConnString1, string jsonConnString2)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting(jsonConnString1 + ":ConnectionString").ToString();

            string tableName1 = _config.getConfigurationSetting(jsonConnString1 + ":TableName").ToString();
            string tableName2 = _config.getConfigurationSetting(jsonConnString2 + ":TableName").ToString();

            //Set up Queries and params
            string queryCount = ""; // _streetsQueryBuilder.GetCountQueryBoth(filterObjects, tableName1, tableName2);
            DbParameter[] dbParamaters = _streetsQueryBuilder.GetParameters(filterObjects);

            //Execute Database
            int count = _db_Helper.ExecuteScalarToInt(conn, queryCount, dbParamaters);
            pagination.count = count;

            return pagination;
        }

        private async Task<DataTable> callDatabaseAsyncBoth(List<FilterObject> filterObjects, Pagination pagination, string jsonConnString1, string jsonConnString2)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting(jsonConnString1 + ":ConnectionString").ToString();

            string tableName1 = _config.getConfigurationSetting(jsonConnString1 + ":TableName").ToString();
            string tableName2 = _config.getConfigurationSetting(jsonConnString2 + ":TableName").ToString();

            //Set up Queries and params
            string queryNormal = ""; // _streetsQueryBuilder.GetQueryBoth(filterObjects, pagination, tableName1, tableName2);
            DbParameter[] dbParamaters = _streetsQueryBuilder.GetParameters(filterObjects);

            //Execute Database
            return _db_Helper.ExecuteToDataTable(conn, queryNormal, dbParamaters);
        }



    }
}
