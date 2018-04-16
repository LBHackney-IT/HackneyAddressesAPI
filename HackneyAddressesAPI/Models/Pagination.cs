using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Models
{
    public class Pagination
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int count { get; set; }
    }
}
