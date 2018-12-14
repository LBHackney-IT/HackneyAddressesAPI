using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Gateways.V1
{
    public interface IAddressesGateway
    {
        Task<AddressDetails> GetAddressAsync(string lpi_key);
    }
}
