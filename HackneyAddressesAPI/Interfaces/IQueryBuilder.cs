using System.Collections.Generic;
using System.Data.Common;
using LBHAddressesAPI.Models;

namespace LBHAddressesAPI.Interfaces
{
    public interface IQueryBuilder
    {
        Dictionary<string, string> GetColumnMappings();

        string GetAddressesQuery(List<FilterObject> filterObjects, Pagination pagination, string tableName);

        string GetStreetsQuery(List<FilterObject> filterObjects, Pagination pagination, string tableName);

        string GetCountQuery(List<FilterObject> filterObjects, string tableName);

        DbParameter[] GetParameters(List<FilterObject> filterObjects);

        
    }
}