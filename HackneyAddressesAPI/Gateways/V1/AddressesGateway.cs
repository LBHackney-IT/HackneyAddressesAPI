using LBHAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using System.Threading;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Helpers;

namespace LBHAddressesAPI.Gateways.V1
{
    public class AddressesGateway : IAddressesGateway
    {
        private readonly string _connectionString;
        public AddressesGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Return an address for a given LPI_Key
        /// </summary>
        /// <param name="request"></param> 
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<AddressDetailed> GetSingleAddressAsync(GetAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new AddressDetailed();

            //TODO: Move the query in to a helper so it's in one place!
            string query = QueryBuilder.GetSingleAddress(GlobalConstants.Format.Detailed);
            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                var all = await conn.QueryAsync<AddressDetailed>(query,
                    new { key = request.addressID }
                ).ConfigureAwait(false);

                result = all.FirstOrDefault();

                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// Return Detailed addresses for matching search
        /// </summary>
        /// <param name="request"></param> 
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedResults<AddressBase>> SearchAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new PagedResults<AddressBase>();            
            var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query
            string query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            string countQuery = QueryBuilder.GetSearchAddressQuery(request, false, false, true, ref dbArgs);
            GlobalConstants.Format format = request.Format;

            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                string sql = query + " " + countQuery;

                using (var multi = conn.QueryMultipleAsync(sql, dbArgs).Result)
                {
                    
                    if (format == GlobalConstants.Format.Detailed)
                    {                        
                        var all = multi.Read<AddressDetailed>()?.ToList();
                        var totalCount = multi.Read<int>().Single();
                        result.Results = all?.ToList().ConvertAll(x => (AddressBase)x);
                        result.TotalResultsCount = totalCount;
                    }
                    else
                    {
                        var all = multi.Read<AddressSimple>()?.ToList();
                        var totalCount = multi.Read<int>().Single();
                        result.Results = all?.ToList().ConvertAll(x => (AddressBase)x);
                        result.TotalResultsCount = totalCount;
                    }             
                    
                }
                                
                conn.Close();
            }
            return result;
        }

        
        public async Task<List<AddressCrossReference>> GetAddressCrossReferenceAsync(GetAddressCrossReferenceRequest request, CancellationToken cancellationToken)
        {
            var result = new List<AddressCrossReference>();

            string query = QueryBuilder.GetCrossReferences(request);
            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                var all = await conn.QueryAsync<AddressCrossReference>(query,
                    new { UPRN = request.uprn }
                ).ConfigureAwait(false);

                result = all.ToList();

                conn.Close();
            }


            return result;
        }

        /// <summary>
        /// Return Simple addresses for matching search
        /// </summary>
        /// <param name="request"></param> 
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedResults<AddressSimple>> SearchSimpleAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new PagedResults<AddressSimple>();
            var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query
            string query = QueryBuilder.GetSearchAddressQuery(request, true, true, false, ref dbArgs);
            string countQuery = QueryBuilder.GetSearchAddressQuery(request, false, false, true, ref dbArgs);

            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                string sql = query + " " + countQuery;

                using (var multi = conn.QueryMultipleAsync(sql, dbArgs).Result)
                {
                    var all = multi.Read<AddressSimple>()?.ToList();
                    var totalCount = multi.Read<int>().Single();
                    result.Results = all?.ToList();
                    result.TotalResultsCount = totalCount;
                }

                conn.Close();
            }
            return result;
        }





    }
}
