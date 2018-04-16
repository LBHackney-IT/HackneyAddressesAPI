using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Actions
{

    //Create Interface
    public class FilterObjectBuilder : IFilterObjectBuilder
    {

        public List<FilterObject> ProcessFilterObjects(List<FilterObject> filterObjects, Dictionary<string, string> mappings)
        {
            try
            {
                List<FilterObject> filterObjectsProcessed = new List<FilterObject>();

                foreach (var item in filterObjects)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        var columnName = "";
                        mappings.TryGetValue(item.Name, out columnName);

                        item.ColumnName = columnName;

                        filterObjectsProcessed.Add(item);
                    }
                }

                return filterObjectsProcessed;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
