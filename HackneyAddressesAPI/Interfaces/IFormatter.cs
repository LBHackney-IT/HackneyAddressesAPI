using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Interfaces
{
    public interface IFormatter
    {
        AddressesQueryParams FormatAddressesQueryParams(AddressesQueryParams queryParams);

        StreetsQueryParams FormatStreetsQueryParams(StreetsQueryParams queryParams);

        string FormatLPIKey(string lpikey);

        string FormatUSRN(string usrn);
    }
}
