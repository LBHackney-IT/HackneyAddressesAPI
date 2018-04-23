using HackneyAddressesAPI.Models;
using System.Collections.Generic;

namespace HackneyAddressesAPI.Interfaces
{
    public interface IFilterObjectBuilder
    {
        List<FilterObject> ProcessFilterObjects(List<FilterObject> filterObjects, Dictionary<string, string> mappings);

        List<FilterObject> ProcessQueryParamsToFilterObjects(AddressesQueryParams queryParams, Dictionary<string, string> mappings);
    }
}