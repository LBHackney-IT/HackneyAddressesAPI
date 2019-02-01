﻿using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using LBHAddressesAPITest.Helpers.Entities;

namespace LBHAddressesAPITest.Helpers
{
    public static class Fake
    {

        public static AddressDetails GenerateAddress()
        {
          var random = new Faker();

          return new AddressDetails
          {

              //TenancyRef = random.Random.Hash(11),
              //PropertyRef = random.Random.Hash(12),
              //Tenure = random.Random.Hash(3),
              //CurrentBalance = random.Finance.Amount(),
              //LastActionDate = new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
              //LastActionCode = random.Random.Hash(3),
              //ArrearsAgreementStatus = random.Random.Hash(10),
              //ArrearsAgreementStartDate =
              //    new DateTime(random.Random.Int(1900, 1999), random.Random.Int(1, 12), random.Random.Int(1, 28), 9, 30, 0),
              //PrimaryContactName = random.Name.FullName(),
              //PrimaryContactShortAddress = $"{random.Address.BuildingNumber()}\n{random.Address.StreetName()}\n{random.Address.Country()}",
              //PrimaryContactPostcode = random.Random.Hash(10)
          };
        }
    }
}
