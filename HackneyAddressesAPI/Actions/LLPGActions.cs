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

        public async Task<object> GetLlpgAddresses(AddressesQueryParams queryParams, Pagination pagination, string Format)
        {
            List<FilterObject> filterObjects = formatAndAddToFilter(queryParams);

            pagination = await callDatabaseAsyncPagination(filterObjects, pagination);
            var resultset = new { resultset = pagination };
            var dataTable = await callDatabaseAsync(filterObjects, pagination);

            if (Format == "Detailed" || Format == "GIS")
            {
                var result = _addressDetailsMapper.MapAddressDetailsGIS(dataTable);
                return new { Addresses = result, metadata = resultset };
            }
            else
            {
                var result = _addressDetailsMapper.MapAddressDetailsSimple(dataTable);
                return new { Addresses = result, metadata = resultset };
            }
        }

        private List<FilterObject> formatAndAddToFilter(AddressesQueryParams queryParams)
        {
            queryParams = _formatter.FormatQueryParams(queryParams);

            List<FilterObject> filterObjects = _fob.ProcessQueryParamsToFilterObjects(queryParams, _llpg_QueryBuilder.GetColumnMappings());

            return filterObjects;
        }


        private async Task<DataTable> callDatabaseAsync(List<FilterObject> filterObjects, Pagination pagination)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting("ConnectionSettings:LLPG:LLPG_DEV:ConnectionString").ToString();

            //Set up Queries and params
            string queryNormal = _llpg_QueryBuilder.GetQuery(filterObjects, pagination.offset, pagination.limit);
            //string queryCount = _llpg_QueryBuilder.GetCountQuery(filterObjects);
            DbParameter[] dbParamaters = _llpg_QueryBuilder.GetParameters(filterObjects);

            //Execute Database
            return _db_Helper.ExecuteToDataTable(conn, queryNormal, dbParamaters);
        }

        private async Task<Pagination> callDatabaseAsyncPagination(List<FilterObject> filterObjects, Pagination pagination)
        {
            //Get Connection/config settings
            string conn = _config.getConfigurationSetting("ConnectionSettings:LLPG:LLPG_DEV:ConnectionString").ToString();

            //Set up Queries and params
            string queryCount = _llpg_QueryBuilder.GetCountQuery(filterObjects);
            DbParameter[] dbParamaters = _llpg_QueryBuilder.GetParameters(filterObjects);

            //Execute Database
            int count = _db_Helper.ExecuteScalarToInt(conn, queryCount, dbParamaters);
            pagination.count = count;

            return pagination;
        }

        

    }
}
