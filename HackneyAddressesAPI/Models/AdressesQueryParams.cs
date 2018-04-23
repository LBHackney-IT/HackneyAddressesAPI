using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Models
{
    public class AddressesQueryParams
    {
        //To set standardised names 
        [DatabaseColumn("POSTCODE", IsWildCardAttr = true)]
        public string Postcode { get; set; }

        [DatabaseColumn("UPRN")]
        public string UPRN { get; set; }

        [DatabaseColumn("USRN")]
        public string USRN { get; set; }

        [DatabaseColumn("PROPERTYCLASSCODE", IsWildCardAttr = true)]
        public string PropertyClassCode { get; set; }

        [DatabaseColumn("PROPERTYCLASSPRIMARY")]
        public string PropertyClass { get; set; }

        [DatabaseColumn("ADDRESSSTATUS")]
        public string AddressStatus { get; set; }

        public string Format { get; set; }

        public string Gazetteer { get; set; }
    }
}
