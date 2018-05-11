using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;

namespace HackneyAddressesAPI.Interfaces
{
    public interface IFilterObjectBuilder
    {
        List<FilterObject> ProcessQueryParamsToFilterObjects<T>(T queryParams, Dictionary<string, string> mappings);
    }
}