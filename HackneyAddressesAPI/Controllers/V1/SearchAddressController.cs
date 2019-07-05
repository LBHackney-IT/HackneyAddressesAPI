using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LBHAddressesAPI.UseCases.V1.Addresses;
using LBHAddressesAPI.Models;
using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Extensions.Controller;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Helpers;
using System.Web.Http.Cors;

namespace LBHAddressesAPI.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/addresses")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    [EnableCors(origins: "*", headers: "*", methods: "GET")]
    public class SearchAddressController : BaseController
    {
        private readonly ISearchAddressUseCase _searchAddressUseCase;


        public SearchAddressController(ISearchAddressUseCase searchAddressUseCase)
        {
            _searchAddressUseCase = searchAddressUseCase;
        }


        /// <summary>
        /// Search Controller V1 to search for addresses
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(APIResponse<SearchAddressResponse>), 200)]
        [HttpGet, MapToApiVersion("1")]       
        public async Task<IActionResult> GetAddresses([FromQuery] SearchAddressRequest request)
        {
            var response = await _searchAddressUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);
            //We convert the result to an APIResponse via extensions on BaseController
            return HandleResponse(response);
        }

    }
} 