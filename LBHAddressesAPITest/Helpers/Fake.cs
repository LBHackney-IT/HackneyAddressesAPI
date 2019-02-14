using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using LBHAddressesAPITest.Helpers.Entities;

namespace LBHAddressesAPITest.Helpers
{
    public static class Fake
    {

        public static Address GenerateAddressProvidingKey(string LPI_KEY)
        {
            var random = new Faker();

            return GetAddress(LPI_KEY, random.PickRandom(new List<string> { "RM3 0FS", "IG11 7QD", "SS12 0DW", "E8 1DY", "E9 7BT", "E16 2QU", "E8 3PE" }));

        }

        public static Address GenerateAddress()
        {
            var random = new Faker();

            return GetAddress(random.Random.AlphaNumeric(14), random.PickRandom(new List<string> { "RM3 0FS","IG11 7QD","SS12 0DW","E8 1DY","E9 7BT","E16 2QU","E8 3PE"}) );            
        }

        private static Address GetAddress(string LPI_KEY, string POSTCODE)
        {
            var random = new Faker();

            return new Address
            {
                LPI_KEY = LPI_KEY,
                UPRN = random.Random.Int(1, 856555),
                USRN = random.Random.Int(200001, 856555),
                PARENT_UPRN = random.Random.Int(34, 856555),
                LPI_OFFICIAL_FLAG = random.PickRandom(new List<string> { "Y", "N" }),
                LPI_LOGICAL_STATUS = random.PickRandom(new List<string> { "Alternative", "Approved preferred", "Historical", "Provisional", "Rejected Internal" }),
                SAO_TEXT = random.Random.AlphaNumeric(9),
                UNIT_NUMBER = random.Random.AlphaNumeric(9),
                LPI_LEVEL = random.Random.AlphaNumeric(9),
                PAO_TEXT = random.Random.AlphaNumeric(9),
                BUILDING_NUMBER = random.Random.AlphaNumeric(9),
                STREET_DESCRIPTION = random.Random.AlphaNumeric(9),
                STREET_ADMIN = random.Random.AlphaNumeric(6),
                POSTCODE = POSTCODE,
                POSTCODE_NOSPACE = POSTCODE.Replace(" ", ""),
                LOCALITY = random.Random.AlphaNumeric(9),
                WARD = random.Random.AlphaNumeric(9),
                POSTALLY_ADDRESSABLE = random.PickRandom(new List<string> { "Y", "N"}),
                GAZETTEER = random.PickRandom(new List<string> { "LOCAL", "NATIONAL" }),
                ORGANISATION = random.Random.AlphaNumeric(9),
                POSTTOWN = random.Random.AlphaNumeric(9),
                USAGE_DESCRIPTION = random.Random.AlphaNumeric(9),
                USAGE_PRIMARY = random.PickRandom(new List<string> { "Commercial", "Dual Use","Features","Land","Military","Mixed","Object of Interest","Other (Ordnance Survey Only)","Parent Shell","Residential","Unclassified" }),
                BLPU_CLASS = random.Random.AlphaNumeric(5),
                PROPERTY_SHELL = random.Random.Bool(),
                NEVEREXPORT = random.Random.Bool(),
                EASTING = random.Random.Decimal(8560.20m, 655593.00m),
                NORTHING = random.Random.Decimal(7677.80m, 1219680.00m),
                LONGITUDE = random.Address.Longitude(-8.5965771, 1.7627731),
                LATITUDE = random.Address.Latitude(49.8879315, 60.8553035)
            };
        }

    }
}
