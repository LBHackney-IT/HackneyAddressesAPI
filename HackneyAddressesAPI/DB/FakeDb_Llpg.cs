using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackneyAddressesAPI.DB
{
    public class FakeDB_LLPG
    {
        //public async Task<List<AddressDetails>> GetLlpgAddressesByPostCode(string postcode, Dictionary<string, string> dict)
        //{
        //    var response = new List<AddressDetails>();
        //    response.Add(new AddressDetails { uniquePropertyReferenceNumber = 012345678912, street = "Back Office, Robert House, 6 - 15 Florfield Road", postcode = "E8 1DT" });
        //    response.Add(new AddressDetails { uniquePropertyReferenceNumber = 012345678923, street = "Maurice Bishop House, 17 Reading Lane", postcode = "E8 1DT" });
        //    switch (postcode)
        //    {
        //        case "E8 1DT":
        //            return response.ToList();
        //        case "E8 2LT":
        //            throw new Exception();
        //        default:
        //            return null;
        //    }
        //}

        //public async Task<List<AddressDetails>> GetLlpgAddressesByUPRN(string uprn)
        //{
        //    var response = new List<AddressDetails>();
        //    response.Add(new AddressDetails { uniquePropertyReferenceNumber = 012345678912, street = "Back Office, Robert House, 6 - 15 Florfield Road", postcode = "E8 1DT" });
        //    response.Add(new AddressDetails { uniquePropertyReferenceNumber = 012345678923, street = "Maurice Bishop House, 17 Reading Lane", postcode = "E8 1DT" });
        //    switch (uprn)
        //    {
        //        case "012345678912":
        //            return response.ToList();
        //        case "012345678923":
        //            throw new Exception();
        //        default:
        //            return null;
        //    }
        //}

        //public Task<List<AddressDetails>> GetLlpgAddressesByUSRN(string usrn)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
