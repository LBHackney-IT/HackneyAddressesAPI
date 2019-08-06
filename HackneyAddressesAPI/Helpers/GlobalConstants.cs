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

        public enum Gazetteer
        {
            Local,
            Both
        };


        

    }
}
