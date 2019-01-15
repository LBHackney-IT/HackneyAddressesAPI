using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBHAddressesAPI.Helpers;
using LBHAddressesAPI.Interfaces;
using Xunit;
using Moq;
using LBHAddressesAPI.Models;
using System.Data.Common;

namespace LBHAddressesAPI.Tests.HelperTests
{
    public class LLPGQueryBuilderOracleTests
    {
        [Fact]
        public void return_correct_query_when_filter_objects_are_passed()
        {
            //--Arrange--
            List<FilterObject> filterObjects = new List<FilterObject>();
            filterObjects.Add(new FilterObject { Name = "POSTCODE", ColumnName = "POSTCODE_NOSPACE", isWildCard = true, Value = "E8 1HH" });
            filterObjects.Add(new FilterObject { Name = "UPRN", ColumnName = "UPRN", isWildCard = false, Value = "123456789" });
            filterObjects.Add(new FilterObject { Name = "USRN", ColumnName = "USRN", isWildCard = false, Value = "987654321" });

            filterObjects.Add(new FilterObject { Name = "PROPERTYCLASSCODE", ColumnName = "BLPU_CLASS", isWildCard = true, Value = "RD07" });
            filterObjects.Add(new FilterObject { Name = "PROPERTYCLASSPRIMARY", ColumnName = "USAGE_PRIMARY", isWildCard = false, Value = "Residential" });
            filterObjects.Add(new FilterObject { Name = "ADDRESSSTATUS", ColumnName = "LPI_LOGICAL_STATUS", isWildCard = false, Value = "Approved Preferred" });

            Pagination pagination = new Pagination();
            pagination.offset = 0;
            pagination.limit = 5;

            IQueryBuilder _queryBuilder = new QueryBuilderOracle();

            //Expected
            var expected = "SELECT * FROM ( SELECT rownum rnum, a.* FROM ( SELECT * FROM LLPG.LLPG_REST_API WHERE POSTCODE_NOSPACE LIKE :POSTCODE_NOSPACE|| '%' AND UPRN = :UPRN AND USRN = :USRN AND BLPU_CLASS LIKE :BLPU_CLASS|| '%' AND USAGE_PRIMARY = :USAGE_PRIMARY AND LPI_LOGICAL_STATUS = :LPI_LOGICAL_STATUS AND ROWNUM >= 0 ) a WHERE rownum <= 5 ) WHERE rnum >= 0";

            //--Act--
            var result = _queryBuilder.GetAddressesQuery(filterObjects, pagination, "LLPG.LLPG_REST_API");

            //--Assert--
            Assert.Equal(expected, result);
        }


        [Fact]
        public void return_correct_count_query_when_filter_objects_passed()
        {
            //--Arrange--
            List<FilterObject> filterObjects = new List<FilterObject>();
            filterObjects.Add(new FilterObject { Name = "POSTCODE", ColumnName = "POSTCODE_NOSPACE", isWildCard = true, Value = "E8 1HH" });
            filterObjects.Add(new FilterObject { Name = "UPRN", ColumnName = "UPRN", isWildCard = false, Value = "123456789" });
            filterObjects.Add(new FilterObject { Name = "USRN", ColumnName = "USRN", isWildCard = false, Value = "987654321" });

            filterObjects.Add(new FilterObject { Name = "PROPERTYCLASSCODE", ColumnName = "BLPU_CLASS", isWildCard = true, Value = "RD07" });
            filterObjects.Add(new FilterObject { Name = "PROPERTYCLASSPRIMARY", ColumnName = "USAGE_PRIMARY", isWildCard = false, Value = "Residential" });
            filterObjects.Add(new FilterObject { Name = "ADDRESSSTATUS", ColumnName = "LPI_LOGICAL_STATUS", isWildCard = false, Value = "Approved Preferred" });

            IQueryBuilder _queryBuilder = new QueryBuilderOracle();

            //Expected
            var expected = "SELECT COUNT(*) FROM LLPG.LLPG_REST_API WHERE POSTCODE_NOSPACE LIKE :POSTCODE_NOSPACE|| '%' AND UPRN = :UPRN AND USRN = :USRN AND BLPU_CLASS LIKE :BLPU_CLASS|| '%' AND USAGE_PRIMARY = :USAGE_PRIMARY AND LPI_LOGICAL_STATUS = :LPI_LOGICAL_STATUS AND ROWNUM >= 0";

            //--Act--
            var result = _queryBuilder.GetCountQuery(filterObjects, "LLPG.LLPG_REST_API");

            //--Assert--
            Assert.Equal(expected, result);
        }

        //Redundant, function is simple so no need to test.
        //[Fact]
        //public void return_correct_DBparameters_from_filter_object()
        //{
        //    //--Arrange--
        //    List<FilterObject> filterObjects = new List<FilterObject>();
        //    filterObjects.Add(new FilterObject { Name = "POSTCODE", ColumnName = "POSTCODE_NOSPACE", isWildCard = true, Value = "E8 1HH" });
        //    filterObjects.Add(new FilterObject { Name = "UPRN", ColumnName = "UPRN", isWildCard = false, Value = "123456789" });
        //    filterObjects.Add(new FilterObject { Name = "USRN", ColumnName = "USRN", isWildCard = false, Value = "987654321" });

        //    filterObjects.Add(new FilterObject { Name = "PROPERTYCLASSCODE", ColumnName = "BLPU_CLASS", isWildCard = true, Value = "RD07" });
        //    filterObjects.Add(new FilterObject { Name = "PROPERTYCLASSPRIMARY", ColumnName = "USAGE_PRIMARY", isWildCard = false, Value = "Residential" });
        //    filterObjects.Add(new FilterObject { Name = "ADDRESSSTATUS", ColumnName = "LPI_LOGICAL_STATUS", isWildCard = false, Value = "Approved Preferred" });

        //    ILLPGQueryBuilder _queryBuilder = new LLPGQueryBuilderOracle();

        //    //Expected
        //    var expected = new DbParameter[6];
        //   

        //    //--Act--
        //    var result = _queryBuilder.GetParameters(filterObjects);

        //    //--Assert--
        //    Assert.Equal(expected, result);
        //}
    }
}
