using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Helpers;
using HackneyAddressesAPI.Interfaces;
using Xunit;
using Moq;
using HackneyAddressesAPI.Models;

namespace HackneyAddressesAPI.Tests.HelperTests
{
    public class FilterObjectBuilderTests
    {
        [Fact]
        public void returns_processed_filter_objects_from_query_params()
        {
            //--Arrange--

            //Input Data
            IFilterObjectBuilder fob = new FilterObjectBuilder();
            AddressesQueryParams queryParams = new AddressesQueryParams();
            queryParams.AddressStatus = "Approved Preferred";
            queryParams.Format = "Simple";
            queryParams.Postcode = "E8 1HH";
            queryParams.PropertyClass = "Residential";
            queryParams.PropertyClassCode = "RD07";
            queryParams.UPRN = "123456789";
            queryParams.USRN = "987654321";

            Dictionary<string, string> paramColumnNameMappings = new Dictionary<string, string>();
            paramColumnNameMappings.Add("UPRN", "UPRN");
            paramColumnNameMappings.Add("POSTCODE", "POSTCODE_NOSPACE");
            paramColumnNameMappings.Add("USRN", "USRN");

            paramColumnNameMappings.Add("PROPERTYCLASSCODE", "BLPU_CLASS");
            paramColumnNameMappings.Add("PROPERTYCLASSPRIMARY", "USAGE_PRIMARY");
            paramColumnNameMappings.Add("ADDRESSSTATUS", "LPI_LOGICAL_STATUS");

            //Result Data to compare to
            List<FilterObject> expectedResults = new List<FilterObject>();
            expectedResults.Add(new FilterObject { Name = "POSTCODE", ColumnName = "POSTCODE_NOSPACE", isWildCard = true, Value = "E8 1HH" });
            expectedResults.Add(new FilterObject { Name = "UPRN", ColumnName = "UPRN", isWildCard = false, Value = "123456789" });
            expectedResults.Add(new FilterObject { Name = "USRN", ColumnName = "USRN", isWildCard = false, Value = "987654321" });

            expectedResults.Add(new FilterObject { Name = "PROPERTYCLASSCODE", ColumnName = "BLPU_CLASS", isWildCard = true, Value = "RD07" });
            expectedResults.Add(new FilterObject { Name = "PROPERTYCLASSPRIMARY", ColumnName = "USAGE_PRIMARY", isWildCard = false, Value = "Residential" });
            expectedResults.Add(new FilterObject { Name = "ADDRESSSTATUS", ColumnName = "LPI_LOGICAL_STATUS", isWildCard = false, Value = "Approved Preferred" });

            expectedResults = expectedResults.OrderBy(o => o.Name).ToList();

            //--Act--
            var results = fob.ProcessQueryParamsToFilterObjects(queryParams, paramColumnNameMappings);

            results = results.OrderBy(o => o.Name).ToList();
            
            //--Assert--
            Assert.Equal(expectedResults, results);
        }
    }
}
