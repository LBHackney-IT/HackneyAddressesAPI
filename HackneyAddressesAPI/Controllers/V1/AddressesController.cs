using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LBHAddressesAPI.UseCases.V1.Addresses;
using LBHAddressesAPI.Models;

namespace LBHAddressesAPI.Controllers.V1
{
    //[ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/addresses")]
    public class AddressesController : Controller
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
        [HttpGet]
        [Route("{addressID}")]
        [ProducesResponseType(typeof(AddressDetails), 200)]
        public async Task<IActionResult> GetAddress(string addressID)
        {
            var response = _addressByID.ExecuteAsync(addressID);

            return Ok(response);
        }
    }
} 