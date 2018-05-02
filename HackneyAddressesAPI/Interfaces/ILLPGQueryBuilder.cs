using System.Collections.Generic;
using System.Data.Common;
using HackneyAddressesAPI.Models;

namespace HackneyAddressesAPI.Interfaces
{
    public interface ILLPGQueryBuilder
    {
        Dictionary<string, string> GetColumnMappings();

        string GetQuery(List<FilterObject> filterObjects, Pagination pagination, string tableName);

        string GetQueryBoth(List<FilterObject> filterObjects, Pagination pagination, string tableName1, string tableName2);

        string GetCountQuery(List<FilterObject> filterObjects, string tableName);

        string GetCountQueryBoth(List<FilterObject> filterObjects, string tableName1, string tableName2);

        DbParameter[] GetParameters(List<FilterObject> filterObjects);

        
    }
}