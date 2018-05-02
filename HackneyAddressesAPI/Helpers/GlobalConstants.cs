using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Helpers
{
    public static class GlobalConstants
    {

        public const int LIMIT = 50;

        public const int OFFSET = 0;

        public const string LLPGJSONSTRING = "ConnectionSettings:LLPG:LLPG_DEV";

        public const string NLPGJSONSTRING = "ConnectionSettings:NLPG:NLPG_DEV";



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
