using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Models
{
    public class StreetsQueryParams
    {
        [DatabaseColumn("STREETNAME", IsWildCardAttr = true)]
        public string StreetName { get; set; }

        [DatabaseColumn("TOWNNAME", IsWildCardAttr = true)]
        public string TownName { get; set; }

        public string Gazetteer { get; set; }


    } // Class Bracket
} // Namespace Bracket
