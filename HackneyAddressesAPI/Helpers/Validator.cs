using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Interfaces;
using System.Text.RegularExpressions;
using System.Text;
using HackneyAddressesAPI.Models;

namespace HackneyAddressesAPI.Helpers
{
    public class Validator : IValidator
    {
        //List of errors returned make object wrapper with boolean & list of errors
        public ValidationResult ValidateClassCodePrimaryAddressStatus(Dictionary<string, string> filtersToValidate)
        {
            List<ApiErrorMessage> myErrors = new List<ApiErrorMessage>();

            bool hasError = false;

            string usageClassValue;
            if (filtersToValidate.TryGetValue("usageClassCode", out usageClassValue))
            {
                if (!string.IsNullOrWhiteSpace(usageClassValue))
                {
                    var error = UsageClassCodeChecker(usageClassValue);
                    if (error != null)
                    {
                        myErrors.Add(error);
                        hasError = true;
                    }
                }
            }

            string usageClassPrimary;
            if (filtersToValidate.TryGetValue("usageClassPrimary", out usageClassPrimary))
            {
                if (!string.IsNullOrWhiteSpace(usageClassPrimary))
                {
                    var error = UsageClassPrimaryChecker(usageClassPrimary);
                    if (error != null)
                    {
                        myErrors.Add(error);
                        hasError = true;
                    }
                }
            }

            string addressStatus;
            if (filtersToValidate.TryGetValue("addressStatus", out addressStatus))
            {
                if (!string.IsNullOrWhiteSpace(addressStatus))
                {
                    var error = AddressStatusChecker(addressStatus);
                    if (error != null)
                    {
                        myErrors.Add(error);
                        hasError = true;
                    }
                }
            }

            ValidationResult validationObject = new ValidationResult();
            validationObject.ErrorOccurred = hasError;
            validationObject.ErrorMessages = myErrors;

            return validationObject;
        }

        // Again, should I use OUT keyword, instead of returning null? 
        public ApiErrorMessage UsageClassPrimaryChecker(string classprimary)
        {
            List<string> classPrimaryList = new List<string>();
            classPrimaryList.Add("Residential");
            classPrimaryList.Add("ObjectOfInterest");
            classPrimaryList.Add("Land");
            classPrimaryList.Add("Features");
            classPrimaryList.Add("Unclassified");
            classPrimaryList.Add("ParentShell");
            classPrimaryList.Add("Commercial");

            classprimary = this.RemoveSpacesAndCapitalize(classprimary);

            if (classPrimaryList.Contains(classprimary, StringComparer.OrdinalIgnoreCase))
            {
                return null;
            }
            else
            {
                return new ApiErrorMessage
                {
                    developerMessage = "Invalid Usage Class Primary",
                    userMessage = "Invalid Usage Class Primary"
                };
            }
        }

        public ApiErrorMessage AddressStatusChecker(string addressStatus)
        {
            List<string> addressStatusList = new List<string>();
            addressStatusList.Add("Alternative");
            addressStatusList.Add("ApprovedPreferred");
            addressStatusList.Add("Historical");
            addressStatusList.Add("Provisional");
            addressStatusList.Add("RejectedInternal");

            addressStatus = this.RemoveSpacesAndCapitalize(addressStatus);

            if (addressStatusList.Contains(addressStatus, StringComparer.OrdinalIgnoreCase))
            {
                return null;
            }
            else
            {
                return new ApiErrorMessage
                {
                    developerMessage = "Invalid Address Status",
                    userMessage = "Invalid Address Status"
                };
            }
        }

        public ApiErrorMessage UsageClassCodeChecker(string classCode)
        {
            classCode = RemoveSpacesAndCapitalize(classCode);
            int classCodeLength = classCode.Length;

            var pattern1 = "^([A-Z]{1,2})$";
            var pattern2 = "^([A-Z]{2}[0-9]{1,2})$";

            var patternReg1 = new Regex(pattern1, RegexOptions.IgnoreCase);
            var patternReg2 = new Regex(pattern2, RegexOptions.IgnoreCase);

            switch (classCodeLength)
            {
                case 1:
                case 2:
                    if (!patternReg1.IsMatch(classCode))
                    {
                        return new ApiErrorMessage
                        {
                            developerMessage = "Usage Class Code Primary/Secondary codes invalid format: A or AA",
                            userMessage = "Usage Class Code Primary/Secondary codes invalid format: A or AA"
                        };
                    }
                    break;
                case 3:
                case 4:
                    if (!patternReg2.IsMatch(classCode))
                    {
                        return new ApiErrorMessage
                        {
                            developerMessage = "Usage Class Code Primary/Secondary/Tertiary codes invalid format: AA0 or AA00",
                            userMessage = "Usage Class Code Primary/Secondary/Tertiary codes invalid format: AA0 or AA00"
                        }; 
                    }
                    break;
                default:
                    return new ApiErrorMessage
                    {
                        developerMessage = "Usage Class Code Length is incorrect length.",
                        userMessage = "Usage Class Code Length is incorrect length."
                    };

            }
            return null;
        }

        private string RemoveSpacesAndCapitalize(string myStr)
        {
            return myStr.Replace(" ", "").ToUpperInvariant();
        }

        public bool ValidatePostcode(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
                return false;
            postcode = postcode.Replace(" ", "");
            return true;
        }

        public bool ValidateUPRN(string uprn)
        {
            if (string.IsNullOrWhiteSpace(uprn))
                return false;
            uprn = uprn.Replace(" ", "");
            var uprnPattern = "^[0-9]{10,12}$";
            var uprnReg = new Regex(uprnPattern, RegexOptions.IgnoreCase);
            return uprnReg.IsMatch(uprn);
        }

        public bool ValidateUSRN(string usrn)
        {
            if (string.IsNullOrWhiteSpace(usrn))
                return false;
            usrn = usrn.Replace(" ", "");
            var uprnPattern = "^[0-9]{6,12}$";
            var uprnReg = new Regex(uprnPattern, RegexOptions.IgnoreCase);
            return uprnReg.IsMatch(usrn);
        }
    }
}
