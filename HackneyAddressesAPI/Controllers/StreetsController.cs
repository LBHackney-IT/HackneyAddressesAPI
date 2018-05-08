using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HackneyAddressesAPI.Helpers;
using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackneyAddressesAPI.Controllers
{
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class StreetsController : Controller
    {
        private IValidator _validator;
        private IStreetsActions _streetsActions;
        private ILoggerAdapter<StreetsController> _logger;

        public StreetsController(IValidator validator,
            IStreetsActions streetsActions,
            ILoggerAdapter<StreetsController> logger)
        {
            _validator = validator ?? throw new ArgumentNullException("validator");
            _streetsActions = streetsActions ?? throw new ArgumentNullException("streetsActions");
            _logger = logger;
        }

        [HttpGet]
        public async Task<JsonResult> GetStreets([FromQuery]string StreetName = null,
            [FromQuery]GlobalConstants.Gazetteer Gazetteer = GlobalConstants.Gazetteer.Local,
            [FromQuery]int? Limit = GlobalConstants.LIMIT,
            [FromQuery]int? Offset = GlobalConstants.OFFSET)
        {
            try
            {
                StreetsQueryParams queryParams = new StreetsQueryParams();

                queryParams.StreetName = WebUtility.UrlDecode(StreetName);
                queryParams.Gazetteer = WebUtility.UrlDecode(Gazetteer.ToString());

                ValidationResult validatorFilterErrors = _validator.ValidateStreetsQueryParams(queryParams);

                if (!validatorFilterErrors.ErrorOccurred)
                {
                    Pagination pagination = new Pagination();
                    pagination.limit = Limit ?? default(int);
                    pagination.offset = Offset ?? default(int);

                    //var result = await _addressesActions.GetLlpgAddresses(
                    //    queryParams,
                    //    pagination);

                    var result = "";

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
    }
}