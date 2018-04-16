using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackneyAddressesAPI.Helpers;
using HackneyAddressesAPI.Interfaces;
using Xunit;
using Moq;

namespace HackneyAddressesAPI.Tests.HelperTests
{
    public class FormatterTests
    {
        [Theory]
        [InlineData("NR7 2RE", "NR72RE")]
        [InlineData("N168QR", "N168QR")]
        [InlineData("N16 8QR", "N168QR")]
        [InlineData("n168qr", "N168QR")]
        [InlineData("n16 8qr", "N168QR")]
        [InlineData("N16   8QR", "N168QR")]
        [InlineData(" N168qR ", "N168QR")]
        [InlineData(" N16 8QR ", "N168QR")]
        [InlineData("EC1V 8QR ", "EC1V8QR")]
        [InlineData(" EC1v 8QR ", "EC1V8QR")]
        [InlineData(" eC1 V 8QR ", "EC1V8QR")]
        [InlineData("E 1 8QR ", "E18QR")]
        [InlineData("E1 8Qr ", "E18QR")]
        public void return_postcode_formatted(string postcode, string expectedPostcode)
        {
            var postcodeFormatter = new Formatter();
            var result = postcodeFormatter.FormatPostcode(postcode);
            Assert.Equal(expectedPostcode, result);
        }

        [Theory]
        [InlineData("RD02", "RD02")]
        [InlineData("RD", "RD")]
        [InlineData("R d", "RD")]
        [InlineData("R", "R")]
        [InlineData(" R ", "R")]
        [InlineData(" r D1", "RD01")]
        [InlineData(" r D1 2 ", "RD12")]
        [InlineData("D 2", "D2")]
        public void return_usage_class_code_formatted(string classCode, string expectedClassCode)
        {
            var formatter = new Formatter();
            var result = formatter.FormatUsageClassCode(classCode);
            Assert.Equal(expectedClassCode, classCode);
        }

        //Not writing tests for these as they're fairly simple methods.
        //FormatRemoveSpacesAndCapitalise
        //FormatUseageClassCodeTertiary  <- tested via return_usage_class_code_formatted
        //FormatUPRN
        //FormatUSRN
        //FormatUsageClassPrimary
        //FormatAddressStatus
    }
}
