using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Infrastructure.V1.Validation;
using LBHAddressesAPI.Helpers;

namespace LBHAddressesAPI.UseCases.V1.Search.Models
{
    /// <summary>
    /// SearchTenancyRequest V2 uses 5 Fields to search on allowing for greater filtering
    /// One of the fields must be populated
    /// Validated by Validate Method
    /// </summary>
    public class SearchAddressRequest : IRequest, IPagedRequest
    {
        
        
        public string postCode { get; set; }

        public GlobalConstants.Gazetteer gazeteer { get; set; }
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
            var validationResult = validator.Validate(castedRequest);
            //Using 1 based paging (to make it easier for Front Ends to page) so defaults to 1 instead of 0
            //Later down the stack we revert to 0 based paging
            if (castedRequest.Page == 0)
                castedRequest.Page = 1;
            //Sets default page size to 10
            if (castedRequest.PageSize == 0)
                castedRequest.PageSize = 10;
            return new RequestValidationResponse(validationResult);
        }
        /// <summary>
        /// Page defaults to 1 as paging is 1 index based not 0 index based
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// PageSize defaults to 10 
        /// </summary>
        public int PageSize { get; set; }
    }
}
