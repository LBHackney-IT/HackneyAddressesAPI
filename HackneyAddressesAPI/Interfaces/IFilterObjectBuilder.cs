using HackneyAddressesAPI.Models;
using System.Collections.Generic;

namespace HackneyAddressesAPI.Interfaces
{
    public interface IFilterObjectBuilder
    {
        List<FilterObject> ProcessQueryParamsToFilterObjects(AddressesQueryParams queryParams, Dictionary<string, string> mappings);
    }
}