using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Infrastructure.V1.Validation;
using LBHAddressesAPI.Helpers;
using System;
using System.Collections.Generic;
using FluentValidation.Results;
using System.Linq;

namespace LBHAddressesAPI.UseCases.V1.Search.Models
{
    /// <summary>
    /// SearchAddressRequest V1 
    /// Validated by Validate Method
    /// </summary>
    public class SearchAddressRequest : IRequest, IPagedRequest
    {

        public SearchAddressRequest()
        {
            this.AddressStatus = "approved preferred";
            this.Gazetteer = GlobalConstants.Gazetteer.Both;
        }

        //    [FromQuery]string PropertyClassCode = null/*,
        // Parent shells??


        /// <summary>
        /// Postcode partial match i.e. "E8 4" will return addresses that have a postcode starting with E84** 
        /// (Whitespace is removed automatically)  
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// Building number search 
        /// </summary>
        public string BuildingNumber { get; set; }

        /// <summary>
        /// Wildcard street name search
        /// </summary>
        public string Street { get; set; }
               
        /// <summary>
        /// LOCAL/NATIONAL/BOTH (Defaults to LOCAL)
        /// </summary>
        public GlobalConstants.Gazetteer Gazetteer { get; set; }

        /// <summary>
        /// Filter by UPRN (unique property reference number - unique identifier of the BLPU (Basic Land and Property Unit); a UPRN can have more than one LPI/address. )
        /// </summary>
        public Int64 ? UPRN { get; set; }
        
        /// <summary>
        /// Filter by USRN (unique street reference number - uniquely identifies streets)
        /// </summary>
        public int ? USRN { get; set; }

        /// <summary>
        /// Description of the primary usage, can be:
        /// Commercial
        /// Dual Use
        /// Features
        /// Land
        /// Military
        /// Object of Interest
        /// Parent Shell
        /// Residential
        /// Unclassified
        /// ALL (default) 
        /// </summary>
        public string usagePrimary  { get; set; }

        /// <summary>
        /// Identifies land and property usage according to this system of classification: https://www.geoplace.co.uk/documents/10181/38204/Appendix+C+-+Classifications/ ; this is a textual description
        /// </summary>
        public string usageCode { get; set; }

        /// <summary>
        /// Allows a switch between simple and detailed address
        /// </summary>
        public GlobalConstants.Format Format { get; set; }

        /// <summary>
        /// Allows switch between address statuses:
        /// Alternative
        /// Approved Preferred (Default)
        /// Historical
        /// Provisional
        /// Rejected Internal
        /// </summary>
        public string AddressStatus { get; set; }


        /// <summary>
        /// Page defaults to 1 as paging is 1 index based not 0 index based
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// PageSize defaults to 50 if not provided
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// List of fields passed in as part of the request
        /// </summary>
        public List<string> RequestFields { get; set; }

        /// <summary>
        /// List of errors that have been generated as part of ModeState
        /// </summary>
        internal List<ValidationError> Errors { get; set; }

        /// <summary>
        /// Responsible for validating itself.
        /// Uses SearchAddressRequestValidator to do complex validation
        /// Sets defaults for Page and PageSize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns>RequestValidationResponse</returns>
        public RequestValidationResponse Validate<T>(T request)
        {
            if (request == null)
                return new RequestValidationResponse(false, "request is null");
            var validator = new SearchAddressRequestValidator();
            var castedRequest = request as SearchAddressRequest;
            if (castedRequest == null)
                return new RequestValidationResponse(false);
            ValidationResult valRes = new ValidationResult();
            if (castedRequest.Errors !=null)
            {
                if (castedRequest.Errors.Count > 0)
                {
                    //ValidationResult valRes = new ValidationResult();
                    foreach(var error in castedRequest.Errors)
                    {
                        valRes.Errors.Add(new ValidationFailure(error.FieldName,error.Message));
                    }

                    return new RequestValidationResponse(valRes);
                }
            }            

            //List<string> invalidFields = InvalidFields(castedRequest.RequestFields);

            //if (invalidFields.Count > 0)
            //{
            //    foreach(string field in invalidFields)
            //    {
            //        valRes.Errors.Add(new ValidationFailure(field, string.Format("{0} is not a valid filter field", field)));
            //    }
            //    return new RequestValidationResponse(valRes);
            //}

            var validationResult = validator.Validate(castedRequest);
            //Using 1 based paging (to make it easier for Front Ends to page) so defaults to 1 instead of 0
            //Later down the stack we revert to 0 based paging
            if (castedRequest.Page == 0)
                castedRequest.Page = 1;
            //Sets default page size to 10
            if (castedRequest.PageSize == 0)
                castedRequest.PageSize = 50;
            return new RequestValidationResponse(validationResult);
        }

        private List<string> InvalidFields(List<string> requestFields)
        {
            //possibly make this smarter and compare to the SearchAddressRequest fields??
            List<string> ClassFields = new List<string>() { "postcode","buildingnumber","street","gazetteer", "uprn", "usrn", "propertyclassprimary", "propertyclasscode", "format", "addressstatus" };

            return requestFields.Except(ClassFields).ToList();
        }
    }
}
