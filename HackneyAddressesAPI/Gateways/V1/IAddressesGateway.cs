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
        Task<AddressDetailed> GetSingleAddressAsync(GetAddressRequest request, CancellationToken cancellationToken);

        Task<PagedResults<AddressBase>> SearchAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken);

        Task<PagedResults<AddressSimple>> SearchSimpleAddressesAsync(SearchAddressRequest request, CancellationToken cancellationToken);
    }
}
