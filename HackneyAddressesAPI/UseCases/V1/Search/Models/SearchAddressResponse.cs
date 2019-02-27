using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.UseCases.V1.Search.Models
{
    public class SearchAddressResponse : IPagedResponse
    {
        [JsonProperty("address")]
        public List<AddressDetails> Addresses {get;set;}

        [JsonProperty("page_count")]
        public int PageCount { get; set; }
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }


        public int CalculatePageCount(int pageSize, int totalResultsCount)
        {
            if (totalResultsCount == 0)
                return 1;
            //eg 100 / 10 = 10
            if (totalResultsCount % pageSize == 0)
                return totalResultsCount / pageSize;
            //eg 101 / 10 = 10.1 so we cast to 10 and add 1 (11)
            var pageCount = (int)(totalResultsCount / pageSize) + 1;
            if (pageCount == 0)
                pageCount = 1;
            return pageCount;
        }
    }
}
