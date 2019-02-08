using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LBHAddressesAPI.UseCases.V1.Addresses;
using LBHAddressesAPI.Models;
using LBHAddressesAPI.Infrastructure.V1.API;
using LBHAddressesAPI.Extensions.Controller;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Helpers;

namespace LBHAddressesAPI.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/addresses")]
    [ProducesResponseType(typeof(APIResponse<object>), 400)]
    [ProducesResponseType(typeof(APIResponse<object>), 500)]
    public class SearchAddressController : BaseController
    {
        private readonly ISearchAddressUseCase _searchAddressUseCase;


        public SearchAddressController(ISearchAddressUseCase searchAddressUseCase)
        {
            _searchAddressUseCase = searchAddressUseCase;
        }
		

        /*[HttpGet, MapToApiVersion("2")]
        [ProducesResponseType(typeof(APIResponse<SearchTenancyResponse>), 200)]
        public async Task<IActionResult> Get([FromQuery]SearchTenancyRequest request)*/
        [ProducesResponseType(typeof(APIResponse<SearchAddressResponse>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetAddresses([FromQuery]string Postcode = null,/*
            [FromQuery]string USRN = null,
            [FromQuery]string UPRN = null,
            [FromQuery]GlobalConstants.PropertyClassPrimary? PropertyClass = null,
            [FromQuery]string PropertyClassCode = null/*,
            [FromQuery]GlobalConstants.AddressStatus AddressStatus = GlobalConstants.AddressStatus.ApprovedPreferred,
            [FromQuery]GlobalConstants.Format Format = GlobalConstants.Format.Simple,
            [FromQuery]GlobalConstants.Gazetteer Gazetteer = GlobalConstants.Gazetteer.Local,*/
            [FromQuery]int? Limit = GlobalConstants.LIMIT,
            [FromQuery]int? Offset = GlobalConstants.OFFSET)
        {
            SearchAddressRequest request = new SearchAddressRequest { postCode = Postcode };
            var response = await _searchAddressUseCase.ExecuteAsync(request, HttpContext.GetCancellationToken()).ConfigureAwait(false);
            return HandleResponse(response);

        }

    }
} 