using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.Interfaces
{
    public interface IFormatter
    {
        string FormatPostcode(string postcode);

        string FormatUPRN(string uprn);

        string FormatUSRN(string uprn);

        string FormatUsageClassCode(string code);

        string FormatUsageClassPrimary(string code);

        string FormatAddressStatus(string code);

    }
}
