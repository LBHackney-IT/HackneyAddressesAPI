using LBHAddressesAPI.Infrastructure.V1.API;
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

        Task<PagedResults<AddressDetails>> SearchAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken);

        Task<PagedResults<AddressDetailsSimple>> SearchSimpleAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken);
    }
}
