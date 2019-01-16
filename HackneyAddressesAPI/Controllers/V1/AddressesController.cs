using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LBHAddressesAPI.UseCases.V1.Addresses;
using LBHAddressesAPI.Models;
using LBHAddressesAPI.Infrastructure.V1.API;

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
        [ProducesResponseType(typeof(APIResponse<AddressDetails>), 200)]
        public async Task<IActionResult> GetAddress(string addressID)
        {
            var response = await _addressByID.ExecuteAsync(addressID).ConfigureAwait(false);

            return HandleResponse(response);
        }
        
    }
} 