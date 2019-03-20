using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.UseCases.V1.Search.Models
{
    public class GetAddressCrossReferenceResponse 
    {
        [JsonProperty("addresscrossreferences")]
        public List<AddressCrossReference> AddressCrossReferences {get;set;}
        
    }
}
