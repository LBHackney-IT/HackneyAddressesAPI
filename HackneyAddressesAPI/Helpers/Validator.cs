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
        public ValidationResult ValidateAddressesQueryParams(AddressesQueryParams filtersToValidate)
        {
            List<ApiErrorMessage> myErrors = new List<ApiErrorMessage>();

            bool hasError = false;

            //Postcode
            if (!string.IsNullOrWhiteSpace(filtersToValidate.Postcode))
            {
                var error = ValidatePostcode(filtersToValidate.Postcode);
                if (error != null)
                {
                    myErrors.Add(error);
                    hasError = true;
                }
            }

            //UPRN
            if (!string.IsNullOrWhiteSpace(filtersToValidate.UPRN))
            {
                var error = ValidateUPRN(filtersToValidate.UPRN);
                if (error != null)
                {
                    myErrors.Add(error);
                    hasError = true;
                }
            }

            //USRN
            if (!string.IsNullOrWhiteSpace(filtersToValidate.USRN))
            {
                var error = ValidateUSRN(filtersToValidate.USRN);
                if (error != null)
                {
                    myErrors.Add(error);
                    hasError = true;
                }
            }

            //Property Class Code
            if (!string.IsNullOrWhiteSpace(filtersToValidate.PropertyClassCode))
            {
                var error = UsageClassCodeChecker(filtersToValidate.PropertyClassCode);
                if (error != null)
                {
                    myErrors.Add(error);
                    hasError = true;
                }
            }
            
            //Property Class Primary
            if (!string.IsNullOrWhiteSpace(filtersToValidate.PropertyClass))
            {
                var error = UsageClassPrimaryChecker(filtersToValidate.PropertyClass);
                if (error != null)
                {
                    myErrors.Add(error);
                    hasError = true;
                }
            }
            
            //Address Status
            if (!string.IsNullOrWhiteSpace(filtersToValidate.AddressStatus))
            {
                var error = AddressStatusChecker(filtersToValidate.AddressStatus);
                if (error != null)
                {
                    myErrors.Add(error);
                    hasError = true;
                }
            }

            //Format
            if (!string.IsNullOrWhiteSpace(filtersToValidate.Format))
            {
                //?#? To Implement
            }

            //Gazetteer
            if (!string.IsNullOrWhiteSpace(filtersToValidate.Gazetteer))
            {
                //?#? To Implement
            }

            ValidationResult validationObject = new ValidationResult();
            validationObject.ErrorOccurred = hasError;
            validationObject.ErrorMessages = myErrors;

            return validationObject;
        }

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

        //Redundant function
        public ApiErrorMessage ValidatePostcode(string postcode)
        {
            postcode = postcode.Replace(" ", "");
            if (postcode.Length < 2 || postcode.Length > 7)
            {
                return new ApiErrorMessage
                {
                    developerMessage = "Postcode length invalid, allowed: 2-7 chars (spaces are automatically removed)",
                    userMessage = "Postcode length invalid, allowed: 2-7 chars (spaces are automatically removed)"
                };
            }
            return null;
        }

        public ApiErrorMessage ValidateUPRN(string uprn)
        {
            uprn = uprn.Replace(" ", "");
            var uprnPattern = "^[0-9]{10,12}$";
            var uprnReg = new Regex(uprnPattern, RegexOptions.IgnoreCase);
            if  (!uprnReg.IsMatch(uprn))
            {
                return new ApiErrorMessage
                {
                    developerMessage = "UPRN is invalid, please input digits only length 10-12.",
                    userMessage = "UPRN is invalid, please input digits only length 10-12."
                };
            }
            return null;
        }

        public ApiErrorMessage ValidateUSRN(string usrn)
        {
            usrn = usrn.Replace(" ", "");
            var usrnPattern = "^[0-9]{6,12}$";
            var usrnReg = new Regex(usrnPattern, RegexOptions.IgnoreCase);
            if (!usrnReg.IsMatch(usrn))
            {
                return new ApiErrorMessage
                {
                    developerMessage = "UPRN is invalid, please input digits only length 6-12.",
                    userMessage = "UPRN is invalid, please input digits only length 6-12."
                };
            }
            return null;
        }
    }
}
