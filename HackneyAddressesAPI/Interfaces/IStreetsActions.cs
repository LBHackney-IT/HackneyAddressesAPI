using LBHAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Interfaces
{
    public interface IStreetsActions
    {
        Task<object> GetStreets(
            StreetsQueryParams filters,
            Pagination pagination
            );

        Task<object> GetStreetsByUSRN(
            string USRN
            );
    }
}
