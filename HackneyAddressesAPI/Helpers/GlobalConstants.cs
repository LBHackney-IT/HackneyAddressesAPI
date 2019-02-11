using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Helpers
{
    public static class GlobalConstants
    {

        public const int LIMIT = 50;

        public const int OFFSET = 0;


        //?#? ToDo: Organise these better, and get the streets connection
        public const string LLPG_ADDRESSES_JSON = "ConnectionSettings:LLPG:DEV";

        public const string NLPG_ADDRESSES_JSON = "ConnectionSettings:NLPG:DEV";

        public const string NLPGCOMBINED_ADDRESSES_JSON = "ConnectionSettings:NLPG_COMBINED:DEV";

        public const string LLPG_STREETS_JSON = "ConnectionSettings:LLPG_STREETS:DEV"; 

        public enum Format
        {
            Simple,
            Detailed
        };

        public enum PropertyClassPrimary
        {
            Residential,
            Commercial,
            DualUse,
            ObjectOfInterest,
            Land,
            Features,
            Unclassified,
            ParentShell
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

        public static string MapAddressStatus(AddressStatus status)
        {
            switch (status)
            {
                case AddressStatus.ApprovedPreferred:
                    return "Approved Preferred";
                case AddressStatus.Alternative:
                    return "Alternative";                    
                case AddressStatus.Historical:
                    return "Historical";
                case AddressStatus.Provisional:
                    return "Provisional";
                case AddressStatus.RejectedInternal:
                    return "Rejected Internal";
                default:
                    throw new Exception(string.Format("Invalid Address Status = {0}", status));
            }
        }

        public static string MapPrimaryPropertyClass(PropertyClassPrimary propertyClass)
        {
            /*Commercial
                Dual Use
                Features
                Land
                Military
                Mixed
                Object of Interest
                Parent Shell
                Residential
                Unclassified*/


            switch (propertyClass)
            {
                case PropertyClassPrimary.Residential:
                    return "Residential";
                case PropertyClassPrimary.Commercial:
                    return "Commercial";
                case PropertyClassPrimary.DualUse:
                    return "Dual Use";
                case PropertyClassPrimary.ObjectOfInterest:
                    return "Object of Interest";
                case PropertyClassPrimary.Land:
                    return "Land";
                case PropertyClassPrimary.Features:
                    return "Features";
                case PropertyClassPrimary.Unclassified:
                    return "Unclassified";
                case PropertyClassPrimary.ParentShell:
                    return "Parent Shell";
                default:
                    throw new Exception(string.Format("Invalid property class = {0}",propertyClass.ToString()));
            }
        }

    }
}
