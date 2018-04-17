using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Helpers;
using HackneyAddressesAPI.Actions;
using HackneyAddressesAPI.Models;
using Microsoft.Extensions.Configuration;
using HackneyAddressesAPI.DB;
using System.IO;
using Microsoft.Extensions.Logging;

namespace HackneyAddressesAPI.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class AddressesController : Controller
    {
        private IValidator _validator;
        private ILLPGActions _LLPGActions;
        private ILoggerAdapter<AddressesController> _logger;

        public AddressesController(IValidator validator,
            ILLPGActions LLPGActions,
            ILoggerAdapter<AddressesController> logger)
        {
            _validator = validator ?? throw new ArgumentNullException("validator");
            _LLPGActions = LLPGActions ?? throw new ArgumentNullException("LLPGActions");
            _logger = logger;
        }

        // ?#? Todo logging to database, check logging with Selwyn to see if it meets Hackney standards for logging.


        //Maybe change param to [FromBody] and pass in an object?
        //Then could pass the object into validation and validate each thing seperately
        //What would the query look like?
        //Can have nested Objects? i.e LLPG object with Pagination object inside, will this break DI?

        /// <summary>
        /// Search property details via post code.
        /// </summary>
        /// <param name="Postcode">Full or partial post code. 
        /// Acceptable inputs: 'E8 2HH', 'E8', 'E8 2', 'e82hh', 'e82'.</param>
        /// <param name="PropertyClass">Primary usage of the property. 
        /// Accepted Values: 'Commercial', 'Features', 'Land', 'Object of Interest', 'Parent Shell', 'Residential', 'Military', 'Dual Use', 'Unclassified'.</param>
        /// <param name="PropertyClassCode">Code specifying usage of the property at a more granular level.
        /// For details see: <a href='https://www.geoplace.co.uk/documents/10181/38204/Appendix+C+-+Classifications/' target='_blank'>Geoplace classifications</a>. 
        /// Acceptable inputs: 'RD07', 'RD', 'R', 'rd07, 'rd 2'.</param>
        /// <param name="AddressStatus"> Status of address in the address lifecycle.
        /// Accepted Values: 'Approved Preferred', 'Alternative', 'Provisional', 'Historical'.
        /// Default Value = 'Approved Preferred'.</param>
        /// <param name="Format">Addresses can be returned in two different formats.
        /// For details see: <a href='#' target='_new'>Insert Link here</a>.
        /// Accepted Values: 'Simple', 'Detailed'.
        /// Default Value = 'Simple'.</param>
        /// <param name="Gazetteer">Search Hackney or National Gazetteer.
        /// Accepted Values: 'Hackney', 'National', 'Both'.
        /// Default Value = 'Hackney'.
        /// 'Hackney' is only local to Hackney, 'National' is everything outside of Hackney, and 'Both' searches Hackney and National.</param>
        /// <param name="Limit">Return only a maximum n items.
        /// Default Value = 50.</param>
        /// <param name="Offset">Skip the first n items - inclusive.
        /// Default Value = 0.</param>
        /// <returns>Returns a list of Addresses depending upon the input format specified above i.e. 'Simple' or 'Detailed'.
        /// For details see: <a href = '#' target='_new'>Insert Link here</a>.
        /// </returns>
        [Route("/Addresses")]
        [HttpGet]
        public async Task<JsonResult> GetPostcode(string Postcode,
            [FromQuery]string PropertyClass = null,
            [FromQuery]string PropertyClassCode = null,
            [FromQuery]string AddressStatus = GlobalConstants.ADDRESS_STATUS,
            [FromQuery]string Format = GlobalConstants.FORMAT,
            [FromQuery]string Gazetteer = GlobalConstants.GAZETTEER,
            [FromQuery]int? Limit = GlobalConstants.LIMIT,
            [FromQuery]int? Offset = GlobalConstants.OFFSET)
        {
            //BLPU UsageClassCode optional, UsageClassPrimary optional, AddressStatus optional, gazzetteer is optional (sort at end)
            try
            {
                Postcode = WebUtility.UrlDecode(Postcode);
                PropertyClassCode = WebUtility.UrlDecode(PropertyClassCode);
                PropertyClass = WebUtility.UrlDecode(PropertyClass);
                AddressStatus = WebUtility.UrlDecode(AddressStatus);
                // Code review with Sachin
                // Make Enum list for constants 
                if (_validator.ValidatePostcode(Postcode))
                {
                    //Change this to object instead of dictionary?
                    Dictionary<string, string> filtersToValidate = new Dictionary<string, string>();
                    filtersToValidate.Add("usageClassCode", PropertyClassCode);
                    filtersToValidate.Add("usageClassPrimary", PropertyClass);
                    filtersToValidate.Add("addressStatus", AddressStatus);

                    ValidationResult validatorFilterErrors = _validator.ValidateClassCodePrimaryAddressStatus(filtersToValidate);

                    if (!validatorFilterErrors.ErrorOccurred)
                    {
                        Pagination pagination = new Pagination();
                        pagination.limit = Limit ?? default(int);
                        pagination.offset = Offset ?? default(int);
                        //Pass in object from the start
                        var result = await _LLPGActions.GetLlpgAddressesByPostCode(
                            Postcode,
                            PropertyClassCode,
                            PropertyClass,
                            AddressStatus,
                            pagination,
                            Format);

                        var json = Json(new { result, ErrorCode = "0", ErrorMessage = "" });
                        json.StatusCode = 200;
                        json.ContentType = "application/json";

                        return json;
                    }
                    else
                    {
                        var errors = validatorFilterErrors.ErrorMessages;

                        var json = Json(errors);
                        json.StatusCode = 400;
                        json.ContentType = "application/json";
                        return json;
                    }
                }
                else
                {
                    var errors = new List<ApiErrorMessage>
                    {
                        new ApiErrorMessage
                        {
                            developerMessage = "Postcode is not in a valid format",
                            userMessage = "Postcode is not in a valid format"
                        }
                    };

                    var json = Json(errors);
                    json.StatusCode = 400;
                    json.ContentType = "application/json";
                    return json;
                }
            }
            catch (Exception ex)
            {
                var errors = new List<ApiErrorMessage>
                {
                    new ApiErrorMessage
                    {
                        developerMessage = ex.Message,
                        userMessage = "We had some problems processing your request"
                    }
                };
                _logger.LogError(ex.Message);
                var json = Json(errors);
                json.StatusCode = 500;
                json.ContentType = "application/json";
                return json;
            }
        }

        /// <summary>
        /// Search property details via UPRN.
        /// </summary>
        /// <param name="UPRN">Full UPRN Number.</param>
        /// <param name="PropertyClass">Primary usage of the property. 
        /// Accepted Values: 'Commercial', 'Features', 'Land', 'Object of Interest', 'Parent Shell', 'Residential', 'Military', 'Dual Use', 'Unclassified'.</param>
        /// <param name="PropertyClassCode">Code specifying usage of the property at a more granular level.
        /// For details see: <a href='https://www.geoplace.co.uk/documents/10181/38204/Appendix+C+-+Classifications/' target='_blank'>Geoplace classifications</a>. 
        /// Acceptable inputs: 'RD07', 'RD', 'R', 'rd07, 'rd 2'.</param>
        /// <param name="AddressStatus"> Status of address in the address lifecycle.
        /// Accepted Values: 'Approved Preferred', 'Alternative', 'Provisional', 'Historical'.
        /// Default Value = 'Approved Preferred'.</param>
        /// <param name="Format">Addresses can be returned in two different formats.
        /// For details see: <a href='#' target='_new'>Insert Link here</a>.
        /// Accepted Values: 'Simple', 'Detailed'.
        /// Default Value = 'Simple'.</param>
        /// <param name="Gazetteer">Search Hackney or National Gazetteer.
        /// Accepted Values: 'Hackney', 'National', 'Both'.
        /// Default Value = 'Hackney'.
        /// 'Hackney' is only local to Hackney, 'National' is everything outside of Hackney, and 'Both' searches Hackney and National.</param>
        /// <param name="Limit">Return only a maximum n items.
        /// Default Value = 50.</param>
        /// <param name="Offset">Skip the first n items - inclusive.
        /// Default Value = 0.</param>
        /// <returns>Returns a list of Addresses depending upon the input format specified above i.e. 'Simple' or 'Detailed'.
        /// For details see: <a href = '#' target='_new'>Insert Link here</a>.
        /// </returns>
        [Route("/{UPRN}")]
        [HttpGet]
        public async Task<JsonResult> GetUPRN([FromQuery]string UPRN,
            [FromQuery]string PropertyClass = null,
            [FromQuery]string PropertyClassCode = null,
            [FromQuery]string AddressStatus = GlobalConstants.ADDRESS_STATUS,
            [FromQuery]string Format = GlobalConstants.FORMAT,
            [FromQuery]string Gazetteer = GlobalConstants.GAZETTEER,
            [FromQuery]int? Limit = GlobalConstants.LIMIT,
            [FromQuery]int? Offset = GlobalConstants.OFFSET)
        {
            //BLPU UsageClassCode optional, UsageClassPrimary optional, AddressStatus optional, gazzetteer is optional (sort at end)
            try
            {
                UPRN = WebUtility.UrlDecode(UPRN);
                PropertyClassCode = WebUtility.UrlDecode(PropertyClassCode);
                PropertyClass = WebUtility.UrlDecode(PropertyClass);
                AddressStatus = WebUtility.UrlDecode(AddressStatus);

                if (_validator.ValidateUPRN(UPRN))
                {
                    Dictionary<string, string> filtersToValidate = new Dictionary<string, string>();
                    filtersToValidate.Add("usageClassCode", PropertyClassCode);
                    filtersToValidate.Add("usageClassPrimary", PropertyClass);
                    filtersToValidate.Add("addressStatus", AddressStatus);

                    ValidationResult validatorFilterErrors = _validator.ValidateClassCodePrimaryAddressStatus(filtersToValidate);

                    if (!validatorFilterErrors.ErrorOccurred)
                    {
                        Pagination pagination = new Pagination();
                        pagination.limit = Limit ?? default(int);
                        pagination.offset = Offset ?? default(int);
                        var result = await _LLPGActions.GetLlpgAddressesByUPRN(
                            UPRN,
                            PropertyClassCode,
                            PropertyClass,
                            AddressStatus,
                            pagination,
                            Format);

                        var json = Json(new { Result = result, ErrorCode = "0", ErrorMessage = "" });
                        json.StatusCode = 200;
                        json.ContentType = "application/json";
                        return json;
                    }
                    else
                    {
                        var errors = validatorFilterErrors.ErrorMessages;

                        var json = Json(errors);
                        json.StatusCode = 400;
                        json.ContentType = "application/json";
                        return json;
                    }
                }
                else
                {
                    var errors = new List<ApiErrorMessage>
                    {
                        new ApiErrorMessage
                        {
                            developerMessage = "UPRN is not in a valid format",
                            userMessage = "UPRN is not in valid format"
                        }
                    };

                    var json = Json(errors);
                    json.StatusCode = 400;
                    json.ContentType = "application/json";
                    return json;
                }
            }
            catch (Exception ex)
            {
                var errors = new List<ApiErrorMessage>
                {
                    new ApiErrorMessage
                    {
                        developerMessage = ex.Message,
                        userMessage = "We had some problems processing your request"
                    }
                };
                _logger.LogError(ex.Message);
                var json = Json(errors);
                json.StatusCode = 500;
                json.ContentType = "application/json";
                return json;
            }
        }
    }
}