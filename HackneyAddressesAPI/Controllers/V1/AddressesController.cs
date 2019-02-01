using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LBHAddressesAPI.UseCases.V1.Addresses;
using LBHAddressesAPI.Models;
using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Extensions.Controller;
using LBHAddressesAPI.UseCases.V1.Search.Models;

namespace LBHAddressesAPI.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/addresses")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    public class AddressesController : BaseController
    {
        private readonly IGetAddressUseCase _addressByID;

        public AddressesController(IGetAddressUseCase addressByID)
        {
            _addressByID = addressByID;
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
            SearchAddressRequest request = new SearchAddressRequest { addressID = addressID };
            var response = await _addressByID.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);

            return HandleResponse(response);
        }

        /*[HttpGet, MapToApiVersion("2")]
        [ProducesResponseType(typeof(APIResponse<SearchTenancyResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery]SearchTenancyRequest request)*/
        [ProducesResponseType(typeof(APIResponse<SearchAddressResponse>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAddresses([FromQuery]string Postcode = null/*,
            [FromQuery]string USRN = null,
            [FromQuery]string UPRN = null,
            [FromQuery]GlobalConstants.PropertyClassPrimary? PropertyClass = null,
            [FromQuery]string PropertyClassCode = null/*,
            [FromQuery]GlobalConstants.AddressStatus AddressStatus = GlobalConstants.AddressStatus.ApprovedPreferred,
            [FromQuery]GlobalConstants.Format Format = GlobalConstants.Format.Simple,
            [FromQuery]GlobalConstants.Gazetteer Gazetteer = GlobalConstants.Gazetteer.Local,
            [FromQuery]int? Limit = GlobalConstants.LIMIT,
            [FromQuery]int? Offset = GlobalConstants.OFFSET*/)
        {

            return HandleResponse(new SearchAddressResponse());

        }

    }
} 