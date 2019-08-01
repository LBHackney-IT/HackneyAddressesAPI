using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Moq;
using LBHAddressesAPI.UseCases.V1.Search.Models;
using LBHAddressesAPI.Validation;

namespace LBHAddressesAPITest.Validation
{
    [TestFixture]
    public class SearchAddressValidatorTests
    {
        private ISearchAddressValidator _classUnderTest;

        [SetUp]
        public void SetUp()
        {
            _classUnderTest = new SearchAddressValidator();
        }
    }
}
