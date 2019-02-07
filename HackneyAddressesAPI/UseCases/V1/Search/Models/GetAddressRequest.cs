using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Infrastructure.V1.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.UseCases.V1.Search.Models
{
    public class GetAddressRequest : IRequest
    {
        /// <summary>
        /// Exact match
        /// </summary>
        public string addressID { get; set; }
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
            var validator = new GetAddressRequestValidator();
            var castedRequest = request as GetAddressRequest;
            if (castedRequest == null)
                return new RequestValidationResponse(false);
            var validationResult = validator.Validate(castedRequest);

            return new RequestValidationResponse(validationResult);
        }
    }
}
