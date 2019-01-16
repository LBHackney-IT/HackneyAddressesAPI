using System.Net;
using LBHAddressesAPI.Infrastructure.V1.Validation;

namespace LBHAddressesAPI.Infrastructure.V1.Exceptions
{
    public class BadRequestException : APIException
    {
        public RequestValidationResponse ValidationResponse { get; set; }

        public BadRequestException():base(HttpStatusCode.BadRequest,"Request is null")
        {

        }

        public BadRequestException(RequestValidationResponse validationResponse)
        {
            StatusCode = HttpStatusCode.BadRequest;
            ValidationResponse = validationResponse;
        }
    }
}
