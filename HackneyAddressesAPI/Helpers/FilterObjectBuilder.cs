using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Helpers
{

    //Create Interface
    public class FilterObjectBuilder : IFilterObjectBuilder
    {
        private List<FilterObject> ProcessFilterObjects(List<FilterObject> filterObjects, Dictionary<string, string> mappings)
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

        public List<FilterObject> ProcessQueryParamsToFilterObjects<T>(T queryParams, Dictionary<string, string> mappings)
        {
            List<FilterObject> filterObjects = new List<FilterObject>();
                              
            foreach (var prop in queryParams.GetType().GetProperties())
            {
                //Get Values of Properties in Class
                var foValue = prop.GetValue(queryParams);

                //Any non-null values
                if (foValue != null)
                {
                    //Set the decorated attributes into a class
                    var fobject = SetDatabaseColumnAttributes(prop);
                    
                    if (fobject != null)
                    {
                        //Add value 
                        fobject.Value = foValue.ToString();
                        filterObjects.Add(fobject);
                    }
                }
            }

            //Column name mappings
            return ProcessFilterObjects(filterObjects, mappings);
        }

        public FilterObject SetDatabaseColumnAttributes(PropertyInfo prop)
        {
            FilterObject fo = new FilterObject();

            var CustomAttr = prop.GetCustomAttributes(typeof(DatabaseColumnAttribute), false).FirstOrDefault();
            CustomAttr = (DatabaseColumnAttribute) CustomAttr;

            var attribute = (DatabaseColumnAttribute) prop.GetCustomAttributes(typeof(DatabaseColumnAttribute), false).FirstOrDefault();

            if (attribute == null)
            {
                return null;    
            }

            fo.Name = attribute.ColumnNameAttr;
            fo.ColumnName = attribute.ColumnNameAttr;
            fo.isWildCard = attribute.IsWildCardAttr;

            return fo;  
        }
    }
}
