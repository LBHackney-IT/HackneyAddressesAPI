using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Helpers
{
    public class Formatter : IFormatter
    {
        public string FormatPostcode(string postcode)
        {
            postcode = this.FormatRemoveSpacesAndCapitalise(postcode);
            //Old way of formatting postcode which added a space.
            //postcode = postcode.Insert(postcode.IndexOf(postcode.Substring(postcode.Length - 3)), " ");
            return postcode;
        }

        public string FormatRemoveSpacesAndCapitalise(string stringToClean)
        {
            if (stringToClean == null)
            {
                return stringToClean;
            }

            stringToClean = stringToClean.Replace(" ", "");
            stringToClean = stringToClean.ToUpperInvariant();
            return stringToClean;
        }

        public string FormatUsageClassCodeTertiary(string code)
        {
            code = this.FormatRemoveSpacesAndCapitalise(code);
            return code.Length < 2 ? "0" + code : code;
        }

        public string FormatUsageClassCode(string code)
        {
            code = this.FormatRemoveSpacesAndCapitalise(code);
            int codeLength = code.Length;

            if (codeLength > 2)
            {
                var tertiaryCode = code.Substring(2);
                code = code.Replace(tertiaryCode, "");
                code += this.FormatUsageClassCodeTertiary(tertiaryCode);
            }

            return code;
        }

        public string FormatUPRN(string uprn)
        {
            return uprn.Replace(" ", "");
        }

        public string FormatUSRN(string usrn)
        {
            return usrn.Replace(" ", "");
        }

        public string FormatUsageClassPrimary(string classPrimary)
        {
            Dictionary<string, string> classPrimaryMapping = new Dictionary<string, string>();
            classPrimaryMapping.Add("RESIDENTIAL","Residential");
            classPrimaryMapping.Add("OBJECT","Object Of Interest");
            classPrimaryMapping.Add("LAND","Land");
            classPrimaryMapping.Add("FEATURES","Features");
            classPrimaryMapping.Add("UNCLASSIFIED","Unclassified");
            classPrimaryMapping.Add("PARENT","ParentShell");
            classPrimaryMapping.Add("COMMERCIAL","Commercial");

            classPrimary = FormatRemoveSpacesAndCapitalise(classPrimary);

            
            foreach (var item in classPrimaryMapping)
            {
                if (classPrimary.IndexOf(item.Key) > -1)
                {
                    string val;
                    if (classPrimaryMapping.TryGetValue(item.Key, out val))
                    {
                        return val;
                    }
                }
            }

            return "Residential";
        }

        public string FormatAddressStatus(string addressStatus)
        {
            Dictionary<string, string> addressStatusMapping = new Dictionary<string, string>();
            addressStatusMapping.Add("ALTERNATIVE","Alternative");
            addressStatusMapping.Add("APPROVED","Approved Preferred");
            addressStatusMapping.Add("HISTORICAL","Historical");
            addressStatusMapping.Add("PROVISIONAL","Provisional");
            addressStatusMapping.Add("REJECTED","Rejected Internal");

            addressStatus = FormatRemoveSpacesAndCapitalise(addressStatus);


            foreach (var item in addressStatusMapping)
            {
                if (addressStatus.IndexOf(item.Key) > -1)
                {
                    string val;
                    if (addressStatusMapping.TryGetValue(item.Key, out val))
                    {
                        return val;
                    }
                }
            }

            return "Alternative";
        }
    }
}
