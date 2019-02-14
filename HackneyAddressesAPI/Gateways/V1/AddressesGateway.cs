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
        public async Task<AddressDetails> GetSingleAddressAsync(GetAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new AddressDetails();

            //TODO: Move the query in to a helper so it's in one place!
            string query = QueryBuilder.GetSingleAddress(GlobalConstants.Format.Detailed);
            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                var all = await conn.QueryAsync<AddressDetails>(query,
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
        public async Task<PagedResults<AddressDetails>> SearchAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new PagedResults<AddressDetails>();            
            var dbArgs = new DynamicParameters();//dynamically add parameters to Dapper query
            string query = QueryBuilder.GetSearchAddressQuery(GlobalConstants.Format.Detailed, request, true, true, false, ref dbArgs);
            string countQuery = QueryBuilder.GetSearchAddressQuery(GlobalConstants.Format.Detailed, request, false, false, false, ref dbArgs);
            
            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                string sql = query + " " + countQuery;

                using (var multi = conn.QueryMultipleAsync(sql, dbArgs).Result)
                {
                    var all = multi.Read<AddressDetails>()?.ToList();
                    var totalCount = multi.Read<int>().Single();
                    result.Results = all?.ToList();
                    result.TotalResultsCount = totalCount;
                }
                                
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
        public async Task<PagedResults<AddressDetailsSimple>> SearchSimpleAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken)
        {
            var result = new PagedResults<AddressDetailsSimple>();

            var dbArgs = new DynamicParameters();

            string query = QueryBuilder.GetSearchAddressQuery(GlobalConstants.Format.Simple, request, true, true, false, ref dbArgs);
            string countQuery = QueryBuilder.GetSearchAddressQuery(GlobalConstants.Format.Simple, request, false, false, false, ref dbArgs);


            using (var conn = new SqlConnection(_connectionString))
            {
                //open connection explicity
                conn.Open();
                string sql = query + " " + countQuery;

                using (var multi = conn.QueryMultipleAsync(sql, dbArgs).Result)
                {
                    var all = multi.Read<AddressDetailsSimple>()?.ToList();
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
