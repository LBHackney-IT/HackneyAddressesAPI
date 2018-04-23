﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Helpers
{
    public static class GlobalConstants
    {
        //public const string ADDRESS_STATUS = "Approved Preferred";

        //public const string FORMAT = "DEFAULT";

        //public const string GAZETTEER = "LOCAL";

        public const int LIMIT = 50;

        public const int OFFSET = 0;

        
        public enum Format
        {
            Simple,
            Detailed
        };

        public enum PropertyClassPrimary
        {
            Residential,
            ObjectOfInterest,
            Land,
            Features,
            Unclassified,
            ParentShell,
            Commercial
        };

        public enum AddressStatus
        {
            ApprovedPreferred,
            Alternative,
            Historical,
            Provisional,
            RejectedInternal
        };

        public enum Gazetteer
        {
            Local,
            National,
            Both
        };




    }


}
