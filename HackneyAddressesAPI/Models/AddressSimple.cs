﻿using System;

namespace LBHAddressesAPI.Models
{
    public class AddressSimple : AddressBase
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }

        public string Town { get; set; }

        public string Postcode { get; set; }

        public Int64 UPRN { get; set; }

    }
}
