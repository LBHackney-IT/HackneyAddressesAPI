﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using LBHAddressesAPI.Helpers;

namespace LBHAddressesAPITest.Test.Helpers
{
    public class GlobalConstantsTest
    {
        [Theory]
        [InlineData("Commercial", GlobalConstants.PropertyClassPrimary.Commercial)]
        [InlineData("Dual Use", GlobalConstants.PropertyClassPrimary.DualUse)]
        [InlineData("Features", GlobalConstants.PropertyClassPrimary.Features)]
        [InlineData("Land", GlobalConstants.PropertyClassPrimary.Land)]
        [InlineData("Object Of Interest", GlobalConstants.PropertyClassPrimary.ObjectOfInterest)]
        [InlineData("Parent Shell", GlobalConstants.PropertyClassPrimary.ParentShell)]
        [InlineData("Residential", GlobalConstants.PropertyClassPrimary.Residential)]
        [InlineData("Unclassified", GlobalConstants.PropertyClassPrimary.Unclassified)]
        public async Task GivenPropertyClass_ThenReturnMappedPropertyClass(string expectedValue, GlobalConstants.PropertyClassPrimary propertyClassPrimary)
        {            
            string testString = GlobalConstants.MapPrimaryPropertyClass(propertyClassPrimary);
            testString.Should().Equals(expectedValue);
        }
    }
}
