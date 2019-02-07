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
    }
}
