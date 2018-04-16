using HackneyAddressesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using HackneyAddressesAPI.Interfaces;
using HackneyAddressesAPI.Actions;
using Newtonsoft.Json;

namespace HackneyAddressesAPI.Tests.Actions
{
    public class PostcodeActionsTests
    {
        //[Fact]
        //public async Task postcode_actions_get_llpg_address_by_postcode_returns_list_of_addresses()
        //{
        //    var response = new List<AddressDetails>();
        //    var address1 = new AddressDetails { uniquePropertyReferenceNumber = 0123456789, street = "Back Office, Robert House, 6 - 15 Florfield Road", postcode = "E8 1DT" };
        //    var address2 = new AddressDetails { uniquePropertyReferenceNumber = 0987654321, street = "Maurice Bishop House, 17 Reading Lane", postcode = "E8 1DT" };
        //    response.Add(address1);
        //    response.Add(address2);

        //    //Make sure it is .ReturnsAsync so it returns Task<> properly
        //    var fakeService = new Mock<IDB_LLPG>();
        //    fakeService.Setup(DB => DB.GetLlpgAddressesByPostCode("E8 1DT"))
        //        .ReturnsAsync(response);

        //    //fIGURE THIS OUT! Do you make Mock of Postcode Actions, or Mock of DB LLPG
        //    //Also passing Null into param for postcode Actions is probably bad, learn it
        //    //
        //    LLPGActions postcodeActions = new LLPGActions(fakeService.Object);
        //    var results = await postcodeActions.GetLlpgAddressesByPostCode("E8 1DT");

        //    var expectedAddr1 = new AddressDetails { uniquePropertyReferenceNumber = 0123456789, street = "Back Office, Robert House, 6 - 15 Florfield Road", postcode = "E8 1DT" };
        //    var expectedAddr2 = new AddressDetails { uniquePropertyReferenceNumber = 0987654321, street = "Maurice Bishop House, 17 Reading Lane", postcode = "E8 1DT" };

        //    var expectedAddresses = new List<AddressDetails>();
        //    expectedAddresses.Add(expectedAddr1);
        //    expectedAddresses.Add(expectedAddr2);

        //    Assert.Equal(JsonConvert.SerializeObject(expectedAddresses), JsonConvert.SerializeObject(results));
        //}
    }
}
