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
using HackneyAddressesAPI.DB;
using HackneyAddressesAPI.Logging;
using Microsoft.Extensions.Logging;

namespace HackneyAddressesAPI.Tests.DB
{
    public class DB_LLPGTests
    {
        //[Fact]
        //public async Task llpg_DB_get_llpg_address_by_postcode_returns_list_of_addresses()
        //{
        //    var response = new List<AddressDetails>();
        //    var address1 = new AddressDetails { uniquePropertyReferenceNumber = 0123456789, street = "Back Office, Robert House, 6 - 15 Florfield Road", postcode = "E8 1DT" };
        //    var address2 = new AddressDetails { uniquePropertyReferenceNumber = 0987654321, street = "Maurice Bishop House, 17 Reading Lane", postcode = "E8 1DT" };
        //    response.Add(address1);
        //    response.Add(address2);

        //    //Make sure it is .ReturnsAsync so it returns Task<> properly
        //    //I realise this will always pass, (ive implemented like this for now, read below comment)
        //    //When constructing the method for the moq, insert moq data or fake data for the database or..
        //    //Have we reached bedrock for testing?
        //    Mock<IDB_LLPG> fake_db_llpg = new Mock<IDB_LLPG>();
        //    fake_db_llpg.Setup(DB => DB.GetLlpgAddressesByPostCode("E8 1DT"))
        //        .ReturnsAsync(response);

        //    IDB_LLPG db_llpg = fake_db_llpg.Object;
        //    var results = await db_llpg.GetLlpgAddressesByPostCode("E8 1DT");

        //    var expectedAddr1 = new AddressDetails { uniquePropertyReferenceNumber = 0123456789, street = "Back Office, Robert House, 6 - 15 Florfield Road", postcode = "E8 1DT" };
        //    var expectedAddr2 = new AddressDetails { uniquePropertyReferenceNumber = 0987654321, street = "Maurice Bishop House, 17 Reading Lane", postcode = "E8 1DT" };

        //    var expectedAddresses = new List<AddressDetails>();
        //    expectedAddresses.Add(expectedAddr1);
        //    expectedAddresses.Add(expectedAddr2);

        //    Assert.Equal(JsonConvert.SerializeObject(expectedAddresses), JsonConvert.SerializeObject(results));
        //}

        //[Fact]
        //public async Task llpg_DB_get_llpg_address_by_postcode_returns_list_of_addresses_integration()
        //{
        //    //Technically this one is an integration test?
        //    IDB_LLPG db_llpg = new DB_LLPG();
        //    var results = await db_llpg.GetLlpgAddressesByPostCode("E8 1DT");

        //    //Update the expected addresses below
        //    var expectedAddr1 = new AddressDetails { uniquePropertyReferenceNumber = 0123456789, street = "Back Office, Robert House, 6 - 15 Florfield Road", postcode = "E8 1DT" };
        //    var expectedAddr2 = new AddressDetails { uniquePropertyReferenceNumber = 0987654321, street = "Maurice Bishop House, 17 Reading Lane", postcode = "E8 1DT" };

        //    var expectedAddresses = new List<AddressDetails>();
        //    expectedAddresses.Add(expectedAddr1);
        //    expectedAddresses.Add(expectedAddr2);

        //    Assert.Equal(JsonConvert.SerializeObject(expectedAddresses), JsonConvert.SerializeObject(results));
        //}

        [Fact]
        public async Task return_is_type_address_details_for_db_llpg()
        {
            var loggerAdapter = new Mock<ILoggerAdapter<OracleHelper>>();
            //var configReader = new Mock<IConfigReader>();
            //configReader.Setup(x => x.getConfigurationSetting("ConnectionSettings:LLPG:LLPG_DEV:ConnectionString"))
            //    .Returns("My conn string");

           // IDB_Helper db_llpg = new OracleHelper(loggerAdapter.Object);

            //Create list of FO's
            List<FilterObject> filterObjects = new List<FilterObject>();

            //db_llpg.ExecuteDB(filterObjects, "", "");
        }
    }
}
