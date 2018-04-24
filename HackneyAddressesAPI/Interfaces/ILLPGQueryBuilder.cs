using System.Collections.Generic;
using System.Data.Common;
using HackneyAddressesAPI.Models;

namespace HackneyAddressesAPI.Interfaces
{
    public interface ILLPGQueryBuilder
    {
        Dictionary<string, string> GetColumnMappings();

        string GetCountQuery(List<FilterObject> filterObjects);

        string GetQuery(List<FilterObject> filterObjects, int offset, int limit);

        DbParameter[] GetParameters(List<FilterObject> filterObjects);

        
    }
}