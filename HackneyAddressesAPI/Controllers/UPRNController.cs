using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using System.Net;
using HackneyAddressesAPI.Actions;
using HackneyAddressesAPI.Helpers;

namespace HackneyAddressesAPI.Controllers
{
    [Produces("application/json")]
    [Route("v1/Addresses/[controller]")]
    public class UPRNController : Controller
    {
        private ILLPGActions _LLPGActions;
        private IValidator _validator;
        private IFormatter _formatter;
        private ILoggerAdapter<UPRNController> _logger;

        public UPRNController(IValidator validator,
            IFormatter formatter,
            ILLPGActions LLPGActions,
            ILoggerAdapter<UPRNController> logger)
        {
            _validator = validator ?? throw new ArgumentNullException("validator");
            _formatter = formatter ?? throw new ArgumentNullException("formatter");
            _LLPGActions = LLPGActions ?? throw new ArgumentNullException("LLPGActions");
            _logger = logger;
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

        [HttpGet]
        public async Task<JsonResult> Get([FromQuery]string UPRN,
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