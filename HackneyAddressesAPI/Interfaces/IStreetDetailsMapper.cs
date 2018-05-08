using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Interfaces
{
    public interface IStreetDetailsMapper
    {
        List<StreetDetails> MapStreetDetails(DataTable dt);
    }
}
