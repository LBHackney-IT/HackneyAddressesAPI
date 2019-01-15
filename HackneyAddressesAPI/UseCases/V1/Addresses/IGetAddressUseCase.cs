using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.UseCases.V1.Addresses
{
    public interface IGetAddressUseCase
    {
        Task<AddressDetails> ExecuteAsync(string lpi_key);
    }
}
