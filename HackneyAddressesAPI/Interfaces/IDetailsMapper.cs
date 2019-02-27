using LBHAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LBHAddressesAPI.Interfaces
{

    public interface IDetailsMapper
    {

        List<AddressDetailed> MapAddressDetailsGIS(DataTable dt);
        List<AddressSimple> MapAddressDetailsSimple(DataTable dt);

        List<StreetDetails> MapStreetDetails(DataTable dt);
    }
}
