using LBHAddressesAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHAddressesAPI.Models;
using LBHAddressesAPI.DB;
using System.Data;
using LBHAddressesAPI.Helpers;
using System.Data.Common;
using System.Collections.Specialized;

namespace LBHAddressesAPI.Actions
{
    public class AddressesActions : IAddressesActions
    {
        private IDB_Helper _db_Helper;
        private IFormatter _formatter;
        private IFilterObjectBuilder _fob;
        private IDetailsMapper _detailsMapper;
        private IConfigReader _config;
        private IQueryBuilder _queryBuilder;

        private string llpgConnString = GlobalConstants.LLPG_ADDRESSES_JSON;
        private string nlpgBothConnString = GlobalConstants.NLPGCOMBINED_ADDRESSES_JSON;
        private string nlpgOnlyConnString = GlobalConstants.NLPG_ADDRESSES_JSON;

        public AddressesActions(IDB_Helper db_Helper,
            IQueryBuilder queryBuilder,
            IConfigReader config,
            IFormatter formatter,
            IFilterObjectBuilder filterObjectBuilder,
            IDetailsMapper detailsMapper)
        {
            _db_Helper = db_Helper ?? throw new ArgumentNullException("db_Helper");
            _queryBuilder = queryBuilder ?? throw new ArgumentNullException("llpg_QueryBuilder");
            _formatter = formatter ?? throw new ArgumentNullException("formatter");
            _fob = filterObjectBuilder ?? throw new ArgumentNullException("filterObjectBuilder");
            _detailsMapper = detailsMapper ?? throw new ArgumentNullException("addressDetailsMapper");
            _config = config ?? throw new ArgumentNullException("config");
        }

        public async Task<object> GetAddresses(AddressesQueryParams queryParams, Pagination pagination)
        {
            List<FilterObject> filterObjects = formatAndAddToFilter(queryParams);

            string connString = llpgConnString;

            if (queryParams.Gazetteer == "National")
            {
                connString = nlpgOnlyConnString;
            }
            else if (queryParams.Gazetteer == "Both")
            {
                connString = nlpgBothConnString;
            }

            pagination = await callDatabaseAsyncPagination(filterObjects, pagination, connString);
            DataTable dataTable = await callDatabaseAsync(filterObjects, pagination, connString);

            var resultset = new { resultset = pagination };
            IEnumerable<IAddressTypes> result;
            if (queryParams.Format == "Detailed")
            {
                result = _detailsMapper.MapAddressDetailsGIS(dataTable);
            }
            else
            {
                result = _detailsMapper.MapAddressDetailsSimple(dataTable);
            }

            return new { Addresses = result, metadata = resultset };
        }

        private List<FilterObject> formatAndAddToFilter(AddressesQueryParams queryParams)
        {
            queryParams = _formatter.FormatAddressesQueryParams(queryParams);

            List<FilterObject> filterObjects = _fob.ProcessQueryParamsToFilterObjects(queryParams, _queryBuilder.GetColumnMappings());

            return filterObjects;
        }

        private async Task<DataTable> callDatabaseAsync(List<FilterObject> filterObjects, Pagination pagination, string jsonConnString)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting(jsonConnString + ":ConnectionString").ToString();
            string tableName = _config.getConfigurationSetting(jsonConnString + ":TableName").ToString();

            //Set up Queries and params
            string queryNormal = _queryBuilder.GetAddressesQuery(filterObjects, pagination, tableName);
            DbParameter[] dbParamaters = _queryBuilder.GetParameters(filterObjects);

            //Execute Database
            return _db_Helper.ExecuteToDataTable(conn, queryNormal, dbParamaters);
        }

        private async Task<Pagination> callDatabaseAsyncPagination(List<FilterObject> filterObjects, Pagination pagination, string jsonConnString)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting(jsonConnString + ":ConnectionString").ToString();
            string tableName = _config.getConfigurationSetting(jsonConnString + ":TableName").ToString();

            //Set up Queries and params
            string queryCount = _queryBuilder.GetCountQuery(filterObjects, tableName);
            DbParameter[] dbParamaters = _queryBuilder.GetParameters(filterObjects);

            //Execute Database
            int count = _db_Helper.ExecuteScalarToInt(conn, queryCount, dbParamaters);
            pagination.count = count;

            return pagination;
        }

        public async Task<object> GetAddressesLpikey(string lpikey)
        {
            lpikey = _formatter.FormatLPIKey(lpikey);

            Pagination pagination = new Pagination();
            pagination.offset = 0;
            pagination.limit = 1;

            List<FilterObject> filterObjects = new List<FilterObject>();
            filterObjects.Add(new FilterObject { ColumnName = "LPI_KEY", isWildCard = false, Value = lpikey});

            string connString = llpgConnString;

            pagination = await callDatabaseAsyncPagination(filterObjects, pagination, connString);

            if (pagination.count < 1)
            {
                connString = nlpgOnlyConnString;
                pagination = await callDatabaseAsyncPagination(filterObjects, pagination, connString);
            }

            var resultset = new { resultset = pagination };
            var dataTable = await callDatabaseAsync(filterObjects, pagination, connString);

            var result = _detailsMapper.MapAddressDetailsGIS(dataTable);
            return new { Addresses = result, metadata = resultset };
        }
    }
}
