using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHAddressesAPI.Models;

namespace LBHAddressesAPI.Interfaces
{
    public interface IAddressesActions
    {
        Task<object> GetAddresses(
            AddressesQueryParams filters,
            Pagination pagination
            );

        Task<object> GetAddressesLpikey(
            string lpikey
            );
    }
}
