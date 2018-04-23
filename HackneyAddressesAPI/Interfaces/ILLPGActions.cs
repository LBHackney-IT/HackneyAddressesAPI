using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Models;

namespace HackneyAddressesAPI.Interfaces
{
    public interface ILLPGActions
    {
        Task<object> GetLlpgAddresses(
            AddressesQueryParams filters,
            Pagination pagination,
            string Format
            );
    }
}
