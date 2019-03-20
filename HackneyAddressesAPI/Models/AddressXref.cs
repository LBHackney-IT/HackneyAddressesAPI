using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Models
{
    public class AddressCrossReference
    {
        public string crossRefKey { get; set; }
        public Int64 UPRN { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string value { get; set; }
        public DateTime endDate { get; set; }
    }
}
