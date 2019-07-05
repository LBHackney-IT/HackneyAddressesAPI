using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LBHAddressesAPI.UseCases.V1.Addresses;
using LBHAddressesAPI.Models;
using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Extensions.Controller;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using System.Web.Http.Cors;

namespace LBHAddressesAPI.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/addresses")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    [EnableCors(origins: "*", headers: "*", methods: "GET")]
    public class GetAddressController : BaseController
    {
        private readonly IGetSingleAddressUseCase _getAddressUseCase;

        public GetAddressController(IGetSingleAddressUseCase addressUseCase)
        {
            _getAddressUseCase = addressUseCase;
        }

        /// <summary>
        /// Returns an address from the given addressID or LPI_Key
        /// </summary>
        /// <param name="addressID"></param>
        /// <returns></returns>
        [HttpGet, MapToApiVersion("1")]
        [Route("{addressID}")]
        [ProducesResponseType(typeof(APIResponse<SearchAddressResponse>), 200)]
        public async Task<IActionResult> GetAddress(string addressID)
        {
            GetAddressRequest request = new GetAddressRequest { addressID = addressID };
            var response = await _getAddressUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);

            return HandleResponse(response);
        }

      

    }
} 