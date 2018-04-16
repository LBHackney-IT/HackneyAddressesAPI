using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Models;

namespace HackneyAddressesAPI.Interfaces
{
    public interface ILLPGActions
    {
        Task<object> GetLlpgAddressesByPostCode(
            string postcode,
            string usageClassCode,
            string usageClassPrimary,
            string addressStatus,
            Pagination pagination,
            string Format
            );

        Task<object> GetLlpgAddressesByUPRN(
            string uprn,
            string usageClassCode,
            string usageClassPrimary,
            string addressStatus,
            Pagination pagination,
            string Format
            );

        Task<object> GetLlpgAddressesByUSRN(
            string usrn,
            string usageClassCode,
            string usageClassPrimary,
            string addressStatus,
            Pagination pagination,
            string Format
            );
    }
}
