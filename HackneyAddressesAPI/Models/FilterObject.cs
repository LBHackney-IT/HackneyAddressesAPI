using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Models
{
    public class FilterObject
    {
        public string Name { get; set; }
        
        public string Value { get; set; }

        public string ColumnName { get; set; }

        public bool isWildCard { get; set; }

    }
}
