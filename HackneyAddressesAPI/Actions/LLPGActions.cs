using HackneyAddressesAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Models;
using HackneyAddressesAPI.DB;
using HackneyAddressesAPI.Factories;
using System.Data;
using HackneyAddressesAPI.Helpers;
using System.Data.Common;
using System.Collections.Specialized;

namespace HackneyAddressesAPI.Actions
{
    public class LLPGActions : ILLPGActions
    {
        private IDB_Helper _db_Helper;
        private IFormatter _formatter;
        private IFilterObjectBuilder _fob;
        private IAddressDetailsMapper _addressDetailsMapper;
        private IConfigReader _config;
        private ILLPGQueryBuilder _llpg_QueryBuilder;

        public LLPGActions(IDB_Helper db_Helper,
            ILLPGQueryBuilder llpg_QueryBuilder,
            IConfigReader config,
            IFormatter formatter,
            IFilterObjectBuilder filterObjectBuilder,
            IAddressDetailsMapper addressDetailsMapper)
        {
            _db_Helper = db_Helper ?? throw new ArgumentNullException("db_Helper");
            _llpg_QueryBuilder = llpg_QueryBuilder ?? throw new ArgumentNullException("llpg_QueryBuilder");
            _formatter = formatter ?? throw new ArgumentNullException("formatter");
            _fob = filterObjectBuilder ?? throw new ArgumentNullException("filterObjectBuilder");
            _addressDetailsMapper = addressDetailsMapper ?? throw new ArgumentNullException("addressDetailsMapper");
            _config = config ?? throw new ArgumentNullException("config");
        }

        public async Task<object> GetLlpgAddresses(AddressesQueryParams queryParams, Pagination pagination)
        {
            List<FilterObject> filterObjects = formatAndAddToFilter(queryParams);

            string jsonConnString1 = GlobalConstants.LLPGJSONSTRING;
            string jsonConnString2 = GlobalConstants.NLPGJSONSTRING;

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

            Object result = null;
            if (queryParams.Format == "GIS")
            {
                result = _addressDetailsMapper.MapAddressDetailsGIS((DataTable)dataTable);
            }
            else
            {
                result = _addressDetailsMapper.MapAddressDetailsSimple((DataTable)dataTable);
            }

            return new { Addresses = result, metadata = resultset };
        }

        private List<FilterObject> formatAndAddToFilter(AddressesQueryParams queryParams)
        {
            queryParams = _formatter.FormatQueryParams(queryParams);

            List<FilterObject> filterObjects = _fob.ProcessQueryParamsToFilterObjects(queryParams, _llpg_QueryBuilder.GetColumnMappings());

            return filterObjects;
        }

        private async Task<DataTable> callDatabaseAsync(List<FilterObject> filterObjects, Pagination pagination, string jsonConnString)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting(jsonConnString + ":ConnectionString").ToString();
            string tableName = _config.getConfigurationSetting(jsonConnString + ":TableName").ToString();

            //Set up Queries and params
            string queryNormal = _llpg_QueryBuilder.GetQuery(filterObjects, pagination, tableName);
            DbParameter[] dbParamaters = _llpg_QueryBuilder.GetParameters(filterObjects);

            //Execute Database
            return _db_Helper.ExecuteToDataTable(conn, queryNormal, dbParamaters);
        }

        private async Task<Pagination> callDatabaseAsyncPagination(List<FilterObject> filterObjects, Pagination pagination, string jsonConnString)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting(jsonConnString + ":ConnectionString").ToString();
            string tableName = _config.getConfigurationSetting(jsonConnString + ":TableName").ToString();

            //Set up Queries and params
            string queryCount = _llpg_QueryBuilder.GetCountQuery(filterObjects, tableName);
            DbParameter[] dbParamaters = _llpg_QueryBuilder.GetParameters(filterObjects);

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
            string queryCount = _llpg_QueryBuilder.GetCountQueryBoth(filterObjects, tableName1, tableName2);
            DbParameter[] dbParamaters = _llpg_QueryBuilder.GetParameters(filterObjects);

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
            string queryNormal = _llpg_QueryBuilder.GetQueryBoth(filterObjects, pagination, tableName1, tableName2);
            DbParameter[] dbParamaters = _llpg_QueryBuilder.GetParameters(filterObjects);

            //Execute Database
            return _db_Helper.ExecuteToDataTable(conn, queryNormal, dbParamaters);
        }

        public async Task<object> GetLlpgAddressesLpikey(string lpikey)
        {
            lpikey = _formatter.FormatLPIKey(lpikey);

            Pagination pagination = new Pagination();
            pagination.offset = 0;
            pagination.limit = 1;

            List<FilterObject> filterObjects = new List<FilterObject>();
            filterObjects.Add(new FilterObject { ColumnName = "LPI_KEY", isWildCard = false, Value = lpikey});

            string jsonConnString = GlobalConstants.LLPGJSONSTRING;

            pagination = await callDatabaseAsyncPagination(filterObjects, pagination, jsonConnString);

            if (pagination.count < 1)
            {
                jsonConnString = GlobalConstants.NLPGJSONSTRING;
                pagination = await callDatabaseAsyncPagination(filterObjects, pagination, jsonConnString);
            }

            var resultset = new { resultset = pagination };
            var dataTable = await callDatabaseAsync(filterObjects, pagination, jsonConnString);

            var result = _addressDetailsMapper.MapAddressDetailsGIS(dataTable);
            return new { Addresses = result, metadata = resultset };
        }
    }
}
