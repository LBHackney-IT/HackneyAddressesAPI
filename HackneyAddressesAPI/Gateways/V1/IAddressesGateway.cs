using LBHAddressesAPI.Models;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Gateways.V1
{
    public interface IAddressesGateway
    {
        Task<AddressDetails> GetSingleAddressAsync(GetAddressRequest request, CancellationToken cancellationToken);

        Task<List<AddressDetails>> SearchAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken);
    }
}
