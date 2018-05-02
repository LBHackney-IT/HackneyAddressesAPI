using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Models
{
    public class AddressDetails
    {
        public Int64 uniquePropertyReferenceNumber { get; set; }
        public int uniqueStreetReferenceNumber { get; set; }
        public Int64 parentUniquePropertyReferenceNumber { get; set; }
        public string addressStatus { get; set; } //1 = "Approved Preferred", 3 = "Alternative", 5 = "Candidate", 6 = "Provisional", 7 = "Rejected External",  8 = "Historical", 9 = "Rejected Internal"
        public string unitName { get; set; }
        public string unitNumber { get; set; } //string because can be e.g. "1a"
        public string buildingName { get; set; }
        public string buildingNumber { get; set; }
        public string street { get; set; }
        public string postcode { get; set; }
        public string locality { get; set; } //for NLPG results; should be null in results for LLPG
        public string gazetteer { get; set; } //“hackney” or “national”
        public string commercialOccupier { get; set; }
        public string royalMailPostTown { get; set; }
        public string landPropertyUsage { get; set; }
        public bool? isNonLocalAddressInLocalGazetteer { get; set; } //for LLPG results; should be null in results for NLPG
        public double easting { get; set; }
        public double northing { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
    }
}
