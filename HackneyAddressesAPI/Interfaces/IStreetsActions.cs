using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Interfaces
{
    public interface IStreetsActions
    {
        Task<object> GetStreets(
            StreetsQueryParams filters,
            Pagination pagination
            );
    }
}
