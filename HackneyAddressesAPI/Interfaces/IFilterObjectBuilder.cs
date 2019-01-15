using LBHAddressesAPI.Models;
using System;
using System.Collections.Generic;

namespace LBHAddressesAPI.Interfaces
{
    public interface IFilterObjectBuilder
    {
        List<FilterObject> ProcessQueryParamsToFilterObjects<T>(T queryParams, Dictionary<string, string> mappings);
    }
}