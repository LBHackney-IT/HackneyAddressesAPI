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

        /// <summary>
        /// Search property details.
        /// </summary>
        /// <param name="Postcode">Full or partial post code. 
        /// Acceptable inputs: 'E8 2HH', 'E8', 'E8 2', 'e82hh', 'e82'.</param>
        /// /// <param name="USRN">USRN.</param>
        /// <param name="UPRN">UPRN.</param>
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
        public async Task<JsonResult> GetPostcode([FromQuery]string Postcode = null,
            [FromQuery]string USRN = null,
            [FromQuery]string UPRN = null,
            [FromQuery]GlobalConstants.PropertyClassPrimary? PropertyClass = null,
            [FromQuery]string PropertyClassCode = null,
            [FromQuery]GlobalConstants.AddressStatus AddressStatus = GlobalConstants.AddressStatus.ApprovedPreferred, // Enum this
            [FromQuery]GlobalConstants.Format Format = GlobalConstants.Format.Simple, //Enum this
            [FromQuery]GlobalConstants.Gazetteer Gazetteer = GlobalConstants.Gazetteer.Local, //Enum this 
            [FromQuery]int? Limit = GlobalConstants.LIMIT,
            [FromQuery]int? Offset = GlobalConstants.OFFSET)
        {
            //BLPU UsageClassCode optional, UsageClassPrimary optional, AddressStatus optional, gazzetteer is optional (sort at end)
            try
            {
                AddressesQueryParams queryParams = new AddressesQueryParams();
                
                queryParams.Postcode = WebUtility.UrlDecode(Postcode);
                queryParams.UPRN = WebUtility.UrlDecode(UPRN);
                queryParams.USRN = WebUtility.UrlDecode(USRN);
                queryParams.PropertyClassCode = WebUtility.UrlDecode(PropertyClassCode);
                queryParams.PropertyClass = WebUtility.UrlDecode(PropertyClass.ToString());
                queryParams.AddressStatus = WebUtility.UrlDecode(AddressStatus.ToString());

                ValidationResult validatorFilterErrors = _validator.ValidateAddressesQueryParams(queryParams);

                var value = Format.ToString();

                // Code review with Sachin
                // Make Enum list for constants 
                // ValidationResult validatorFilterErrors = _validator.ValidateAddressesQueryParams(queryParams);

                if (!validatorFilterErrors.ErrorOccurred)
                {
                    Pagination pagination = new Pagination();
                    pagination.limit = Limit ?? default(int);
                    pagination.offset = Offset ?? default(int);
                    //Pass in object from the start
                    var result = await _LLPGActions.GetLlpgAddresses(
                        queryParams,
                        pagination,
                        value);

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
        
    } // Class Bracket
} // Namespace Bracket