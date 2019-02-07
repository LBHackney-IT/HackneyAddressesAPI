using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHAddressesAPI.Models;
using LBHAddressesAPI.Gateways.V1;
using LBHAddressesAPI.Infrastructure.V1.API;
using System.Threading;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Infrastructure.V1.Exceptions;

namespace LBHAddressesAPI.UseCases.V1.Addresses
{
    public class SearchAddressUseCase : ISearchAddressUseCase
    {
        private readonly IAddressesGateway _addressGateway;

        public SearchAddressUseCase(IAddressesGateway addressesGateway)
        {
            _addressGateway = addressesGateway;
        }

        public async Task<SearchAddressResponse> ExecuteAsync(SearchAddressRequest request, CancellationToken cancellationToken)
        {
            //validate
            if (request == null)
                throw new BadRequestException();


            var validationResponse = request.Validate(request);
            if (!validationResponse.IsValid)
                throw new BadRequestException(validationResponse);

            var response = await _addressGateway.SearchAddressesAsync(request, cancellationToken).ConfigureAwait(false);

            if (response == null)
                return new SearchAddressResponse();
            var useCaseResponse = new SearchAddressResponse
            {
                Addresses = response 
            };


            return useCaseResponse;
        }
    }
}
