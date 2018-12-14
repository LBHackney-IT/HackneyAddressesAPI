using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Models;
using LBHAddressesAPI.Gateways.V1;

namespace LBHAddressesAPI.UseCases.V1.Addresses
{
    public class GetAddressUseCase : IGetAddressUseCase
    {

        private readonly IAddressesGateway _addressGateway;

        public GetAddressUseCase(IAddressesGateway addressesGateway)
        {
            _addressGateway = addressesGateway;
        }

        public async Task<AddressDetails> ExecuteAsync(string lpi_key)
        {
            if(lpi_key == null || string.IsNullOrEmpty(lpi_key))
            {
                throw new Exception("lpi_key must be provided");
            }
            else if(lpi_key.Length != 14)
            {
                throw new Exception("lpi_key must be 14 characters");
            }
            var response = await _addressGateway.GetAddressAsync(lpi_key).ConfigureAwait(false);

            return response;
        }
    }
}
