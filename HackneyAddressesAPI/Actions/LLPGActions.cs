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

        public async Task<object> GetLlpgAddressesByPostCode(
            string postcode,
            string PropertyClassCode,
            string PropertyClass,
            string addressStatus,
            Pagination pagination,
            string Format
            )
        {

            postcode = _formatter.FormatPostcode(postcode);

            List<FilterObject> filterObjects = formatAndAddToFilter(PropertyClassCode, PropertyClass, addressStatus);
            filterObjects.Add(new FilterObject() { Name = "postcode", Value = postcode, isWildCard = true });

            pagination = await callDatabaseAsyncPagination(filterObjects, pagination);
            var resultset = new { resultset = pagination };

            if (Format == "Detailed" || Format == "GIS")
            {
                var result = await callDatabaseAsync(filterObjects, pagination, Format);
                return new { Addresses = result, metadata = resultset };
            }
            else
            {
                var result = await callDatabaseAsyncSimple(filterObjects, pagination);
                return new { Addresses = result, metadata = resultset };
            }
            

            // ?#? Todo Implement pagination for other controllers
            // ?#? Tidy up filter objects class, maybe move it into the Query builder? - Get guidance from code review
            // ?#? Maybe tidy up Actions class, 
            // ?#? WRITE TESTS!!!!!!!
            // ?#? Swagger Documentation
            // ?#? 
        }

        public async Task<object> GetLlpgAddressesByUPRN(
            string uprn,
            string usageClassCode,
            string usageClassPrimary,
            string addressStatus,
            Pagination pagination,
            string Format
            )
        {
            uprn = _formatter.FormatUPRN(uprn);

            List<FilterObject> filterObjects = formatAndAddToFilter(usageClassCode, usageClassPrimary, addressStatus);
            filterObjects.Add(new FilterObject() { Name = "uprn", Value = uprn });

            pagination = await callDatabaseAsyncPagination(filterObjects, pagination);
            var result = await callDatabaseAsync(filterObjects, pagination, Format);

            var resultset = new { resultset = pagination };

            object myObj = new { Addresses = result, metadata = resultset };

            return myObj;
        }

        public async Task<object> GetLlpgAddressesByUSRN(
            string usrn,
            string usageClassCode,
            string usageClassPrimary,
            string addressStatus,
            Pagination pagination,
            string Format
            )
        {
            usrn = _formatter.FormatUSRN(usrn);

            List<FilterObject> filterObjects = formatAndAddToFilter(usageClassCode, usageClassPrimary, addressStatus);
            filterObjects.Add(new FilterObject() { Name = "usrn", Value = usrn });

            pagination = await callDatabaseAsyncPagination(filterObjects, pagination);
            var result = await callDatabaseAsync(filterObjects, pagination, Format);

            var resultset = new { resultset = pagination };

            object myObj = new { Addresses = result, metadata = resultset };

            return myObj;
        }

        private async Task<List<AddressDetails>> callDatabaseAsync(List<FilterObject> filterObjects, Pagination pagination, string Format)
        {
            //Process Filter Objects
            filterObjects = _fob.ProcessFilterObjects(filterObjects, _llpg_QueryBuilder.GetColumnMappings());

            //Get Connection/config settings
            string conn = _config.getConfigurationSetting("ConnectionSettings:LLPG:LLPG_DEV:ConnectionString").ToString();
            string maxConnections = _config.getConfigurationSetting("ConnectionSettings:LLPG:LLPG_DEV:MaxConnections").ToString();

            //Set up Queries and params
            string queryNormal = _llpg_QueryBuilder.GetQuery(filterObjects, pagination.offset, pagination.limit);
            //string queryCount = _llpg_QueryBuilder.GetCountQuery(filterObjects);
            DbParameter[] dbParamaters = _llpg_QueryBuilder.GetParameters(filterObjects);

            //Execute Database
            DataTable dt = _db_Helper.ExecuteToDataTable(conn, queryNormal, dbParamaters);
            //int count = _db_Helper.ExecuteScalarToInt(conn, queryCount, dbParamaters);

            //Map Database to Object
            List<AddressDetails> addressDetailsList = _addressDetailsMapper.MapAddressDetailsGIS(dt);

            return addressDetailsList;
        }

        private async Task<List<AddressDetailsSimple>> callDatabaseAsyncSimple(List<FilterObject> filterObjects, Pagination pagination)
        {
            //Process Filter Objects
            filterObjects = _fob.ProcessFilterObjects(filterObjects, _llpg_QueryBuilder.GetColumnMappings());

            //Get Connection/config settings
            string conn = _config.getConfigurationSetting("ConnectionSettings:LLPG:LLPG_DEV:ConnectionString").ToString();
            string maxConnections = _config.getConfigurationSetting("ConnectionSettings:LLPG:LLPG_DEV:MaxConnections").ToString();

            //Set up Queries and params
            string queryNormal = _llpg_QueryBuilder.GetQuery(filterObjects, pagination.offset, pagination.limit);
            //string queryCount = _llpg_QueryBuilder.GetCountQuery(filterObjects);
            DbParameter[] dbParamaters = _llpg_QueryBuilder.GetParameters(filterObjects);

            //Execute Database
            DataTable dt = _db_Helper.ExecuteToDataTable(conn, queryNormal, dbParamaters);
            //int count = _db_Helper.ExecuteScalarToInt(conn, queryCount, dbParamaters);

            //Map Database to Object
            List<AddressDetailsSimple> addressDetailsList = _addressDetailsMapper.MapAddressDetailsSimple(dt);

            return addressDetailsList;
        }

        private async Task<Pagination> callDatabaseAsyncPagination(List<FilterObject> filterObjects, Pagination pagination)
        {
            //Process Filter Objects
            filterObjects = _fob.ProcessFilterObjects(filterObjects, _llpg_QueryBuilder.GetColumnMappings());

            //Get Connection/config settings
            string conn = _config.getConfigurationSetting("ConnectionSettings:LLPG:LLPG_DEV:ConnectionString").ToString();
            string maxConnections = _config.getConfigurationSetting("ConnectionSettings:LLPG:LLPG_DEV:MaxConnections").ToString();

            //Set up Queries and params
            //string queryNormal = _llpg_QueryBuilder.GetQuery(filterObjects);
            string queryCount = _llpg_QueryBuilder.GetCountQuery(filterObjects);
            DbParameter[] dbParamaters = _llpg_QueryBuilder.GetParameters(filterObjects);


            //Execute Database
            //DataTable dt = _db_Helper.ExecuteToDataTable(conn, queryNormal, dbParamaters);
            int count = _db_Helper.ExecuteScalarToInt(conn, queryCount, dbParamaters);

            pagination.count = count;

            return pagination;
        }

        private List<FilterObject> formatAndAddToFilter(string usageClassCode, string usageClassPrimary, string addressStatus)
        {
            usageClassCode = String.IsNullOrEmpty(usageClassCode) ? null : _formatter.FormatUsageClassCode(usageClassCode);
            usageClassPrimary = String.IsNullOrEmpty(usageClassPrimary) ? null : _formatter.FormatUsageClassPrimary(usageClassPrimary);
            addressStatus = String.IsNullOrEmpty(addressStatus) ? null : _formatter.FormatAddressStatus(addressStatus);

            List<FilterObject> filterObjects = new List<FilterObject>();
            filterObjects.Add(new FilterObject() { Name = "usageClassCode", Value = usageClassCode, isWildCard = true });
            filterObjects.Add(new FilterObject() { Name = "usageClassPrimary", Value = usageClassPrimary });
            filterObjects.Add(new FilterObject() { Name = "addressStatus", Value = addressStatus });

            return filterObjects;
        }
    }
}
