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

        public const string LLPG_ADDRESSES_JSON = "ConnectionSettings:LLPG:DEV";

        public const string NLPG_ADDRESSES_JSON = "ConnectionSettings:NLPG:DEV";

        public const string NLPGCOMBINED_ADDRESSES_JSON = "ConnectionSettings:NLPG_COMBINED:DEV";



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
