using LBHAddressesAPI.Helpers;
using LBHAddressesAPI.Interfaces;
using LBHAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Actions
{
    public class StreetsActions : IStreetsActions
    {
        private IDB_Helper _db_Helper;
        private IFormatter _formatter;
        private IFilterObjectBuilder _fob;
        private IDetailsMapper _detailsMapper;
        private IConfigReader _config;
        private IQueryBuilder _queryBuilder;

        private string llpgConnString = GlobalConstants.LLPG_STREETS_JSON;


        public StreetsActions(IDB_Helper db_Helper,
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

        public async Task<object> GetStreets(StreetsQueryParams queryParams, Pagination pagination)
        {
            List<FilterObject> filterObjects = formatAndAddToFilter(queryParams);

            string connString = llpgConnString;

            pagination = await callDatabaseAsyncPagination(filterObjects, pagination, connString);
            DataTable dataTable = await callDatabaseAsync(filterObjects, pagination, connString);

            var resultset = new { resultset = pagination };

            var result = _detailsMapper.MapStreetDetails(dataTable);

            return new { Addresses = result, metadata = resultset };
        }

        private List<FilterObject> formatAndAddToFilter(StreetsQueryParams queryParams)
        {
            queryParams = _formatter.FormatStreetsQueryParams(queryParams);

            List<FilterObject> filterObjects = _fob.ProcessQueryParamsToFilterObjects(queryParams, _queryBuilder.GetColumnMappings());

            return filterObjects;
        }

        private async Task<DataTable> callDatabaseAsync(List<FilterObject> filterObjects, Pagination pagination, string jsonConnString)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting(jsonConnString + ":ConnectionString").ToString();
            string tableName = _config.getConfigurationSetting(jsonConnString + ":TableName").ToString();

            //Set up Queries and params
            string queryNormal = _queryBuilder.GetStreetsQuery(filterObjects, pagination, tableName);
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

        public async Task<object> GetStreetsByUSRN(string usrn)
        {
            usrn = _formatter.FormatUSRN(usrn);

            Pagination pagination = new Pagination();
            pagination.offset = 0;
            pagination.limit = 1;

            List<FilterObject> filterObjects = new List<FilterObject>();

            filterObjects.Add(new FilterObject { ColumnName = "USRN", isWildCard = false, Value = usrn });

            string connString = llpgConnString;

            pagination = await callDatabaseAsyncPagination(filterObjects, pagination, connString);

            var resultset = new { resultset = pagination };
            var dataTable = await callDatabaseAsync(filterObjects, pagination, connString);

            var result = _detailsMapper.MapStreetDetails(dataTable);
            return new { Addresses = result, metadata = resultset };

        }

    }
}
