using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using LBHAddressesAPITest.Helpers.Entities;

namespace LBHAddressesAPITest.Helpers
{
    public static class Fake
    {

        public static Address GenerateAddress()
        {
          var random = new Faker();

            return new Address
            {
                LPI_KEY = random.Random.AlphaNumeric(14),
                UPRN = random.Random.Double(1, 906700601612),
                USRN = random.Random.Int(200001, 85655511),
                PARENT_UPRN = random.Random.Double(34, 906700526492),
                LPI_LOGICAL_STATUS = random.PickRandom(new List<string> { "Alternative", "Approved preferred", "Historical", "Provisional", "Rejected Internal" }),
                SAO_TEXT = random.Random.String(0,90),
                UNIT_NUMBER = random.Random.String(1,10),
                PAO_TEXT = random.Random.String(0,90),
                BUILDING_NUMBER = random.Random.String(1,11),
                STREET_DESCRIPTION = random.Random.String(2,100),
                POSTCODE = random.Random.String(0,8),
                LOCALITY = random.Random.String(0,35),
                GAZETTEER = random.PickRandom(new List<string> { "LOCAL", "NATIONAL" }),
                ORGANISATION = random.Random.String(0,65),
                POSTTOWN = random.Random.String(0,18),
                USAGE_DESCRIPTION = random.Random.String(4,216),
                USAGE_PRIMARY = random.Random.String(4,28),
                BLPU_CLASS = random.Random.String(0,6),
                PROPERTY_SHELL = random.Random.Bool(),
                NEVEREXPORT = random.Random.Bool(),
                EASTING = random.Random.Decimal(8560.2000m, 655593.0000m),
                NORTHING = random.Random.Decimal(7677.8000m, 1219680.0000m),
                LONGITUDE = random.Address.Longitude(-8.5965771, 1.7627731),
                LATITUDE = random.Address.Latitude(49.8879315, 60.8553035)
            };
        }
    }
}
