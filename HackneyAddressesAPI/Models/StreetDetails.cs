using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Models
{
    public class StreetDetails
    {
        public int uniqueStreetReferenceNumber { get; set; }

        public string streetDescription { get; set; }

        public string  locality { get; set; }

        public string county { get; set; }

        public string town { get; set; }



        public string streetStartEasting { get; set; }

        public string streetStartNorthing { get; set; }

        public string streetEndEasting { get; set; }

        public string streetEndNorthing { get; set; }

        public string streetStartLongitude { get; set; }

        public string streetStartLatitude { get; set; }

        public string streetEndLongitude { get; set; }

        public string streetEndLatitude { get; set; }

    }
}
