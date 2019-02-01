using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBHAddressesAPITest.Helpers.Entities
{
    [Table("combined_address")] 
    public class Address
    {

        /// <summary>
        /// LPI_KEY
        /// </summary>
        [Key]
        [Column("LPI_KEY")]
        [MaxLength(14)]
        [Required]
        public string LPI_KEY { get; set; }

        /// <summary>
        /// LPI_LOGICAL_STATUS
        /// </summary>
        [Column("LPI_LOGICAL_STATUS")]
        [MaxLength(18)]
        public string LPI_LOGICAL_STATUS { get; set; }

        /// <summary>
        /// LPI_OFFICIAL_FLAG
        /// </summary>
        [Column("LPI_OFFICIAL_FLAG")]
        [MaxLength(1)]
        public string LPI_OFFICIAL_FLAG { get; set; }

        /// <summary>
        /// LPI_START_DATE
        /// </summary>
        [Column("LPI_START_DATE")]
        public int LPI_START_DATE { get; set; }

        /// <summary>
        /// LPI_END_DATE
        /// </summary>
        [Column("LPI_END_DATE")]
        public int LPI_END_DATE { get; set; }

        /// <summary>
        /// LPI_LAST_UPDATE_DATE
        /// </summary>
        [Column("LPI_LAST_UPDATE_DATE")]
        public int LPI_LAST_UPDATE_DATE { get; set; }

        /// <summary>
        /// USRN
        /// </summary>
        [Column("USRN")]
        public int USRN { get; set; }

        /// <summary>
        /// UPRN
        /// </summary>
        [Column("UPRN")]
        public double UPRN { get; set; }

        /// <summary>
        /// PARENT_UPRN
        /// </summary>
        [Column("PARENT_UPRN")]
        public double PARENT_UPRN { get; set; }

        /// <summary>
        /// BLPU_START_DATE
        /// </summary>
        [Column("BLPU_START_DATE")]
        public int BLPU_START_DATE { get; set; }

        /// <summary>
        /// BLPU_END_DATE
        /// </summary>
        [Column("BLPU_END_DATE")]
        public int BLPU_END_DATE { get; set; }

        /// <summary>
        /// BLPU_STATE
        /// </summary>
        [Column("BLPU_STATE")]
        public int BLPU_STATE { get; set; }

        /// <summary>
        /// BLPU_STATE_DATE
        /// </summary>
        [Column("BLPU_STATE_DATE")]
        public int BLPU_STATE_DATE { get; set; }

        /// <summary>
        /// BLPU_CLASS
        /// </summary>
        [Column("BLPU_CLASS")]
        [MaxLength(4)]
        public string BLPU_CLASS { get; set; }

        /// <summary>
        /// USAGE_DESCRIPTION
        /// </summary>
        [Column("USAGE_DESCRIPTION")]
        [MaxLength(1006)]
        public string USAGE_DESCRIPTION { get; set; }

        /// <summary>
        /// USAGE_PRIMARY
        /// </summary>
        [Column("USAGE_PRIMARY")]
        [MaxLength(160)]
        public string USAGE_PRIMARY { get; set; }

        /// <summary>
        /// PROPERTY_SHELL
        /// </summary>
        [Column("PROPERTY_SHELL")]
        public bool PROPERTY_SHELL { get; set; }

        /// <summary>
        /// EASTING
        /// </summary>
        [Column("EASTING")]
        public decimal EASTING { get; set; }

        /// <summary>
        /// NORTHING
        /// </summary>
        [Column("NORTHING")]
        public decimal NORTHING { get; set; }

        /// <summary>
        /// RPA
        /// </summary>
        [Column("RPA")]
        public byte RPA { get; set; }

        /// <summary>
        /// ORGANISATION
        /// </summary>
        [Column("ORGANISATION")]
        [MaxLength(100)]
        public string ORGANISATION { get; set; }

        /// <summary>
        /// SAO_TEXT
        /// </summary>
        [Column("SAO_TEXT")]
        [MaxLength(90)]
        public string SAO_TEXT { get; set; }

        /// <summary>
        /// UNIT_NUMBER
        /// </summary>
        [Column("UNIT_NUMBER")]
        [MaxLength(17)]
        public string UNIT_NUMBER { get; set; }

        /// <summary>
        /// LPI_LEVEL
        /// </summary>
        [Column("LPI_LEVEL")]
        [MaxLength(30)]
        public string LPI_LEVEL { get; set; }

        /// <summary>
        /// PAO_TEXT
        /// </summary>
        [Column("PAO_TEXT")]
        [MaxLength(90)]
        public string PAO_TEXT { get; set; }

        /// <summary>
        /// BUILDING_NUMBER
        /// </summary>
        [Column("BUILDING_NUMBER")]
        [MaxLength(17)]
        public string BUILDING_NUMBER { get; set; }

        /// <summary>
        /// STREET_DESCRIPTION
        /// </summary>
        [Column("STREET_DESCRIPTION")]
        [MaxLength(100)]
        public string STREET_DESCRIPTION { get; set; }

        /// <summary>
        /// STREET_ADMIN
        /// </summary>
        [Column("STREET_ADMIN")]
        [MaxLength(7)]
        public string STREET_ADMIN { get; set; }

        /// <summary>
        /// LOCALITY
        /// </summary>
        [Column("LOCALITY")]
        [MaxLength(35)]
        public string LOCALITY { get; set; }

        /// <summary>
        /// WARD
        /// </summary>
        [Column("WARD")]
        [MaxLength(100)]
        public string WARD { get; set; }

        /// <summary>
        /// POSTALLY_ADDRESSABLE
        /// </summary>
        [Column("POSTALLY_ADDRESSABLE")]
        [MaxLength(1)]
        public string POSTALLY_ADDRESSABLE { get; set; }

        /// <summary>
        /// NEVEREXPORT
        /// </summary>
        [Column("NEVEREXPORT")]
        public bool NEVEREXPORT { get; set; }

        /// <summary>
        /// POSTTOWN
        /// </summary>
        [Column("POSTTOWN")]
        [MaxLength(30)]
        public string POSTTOWN { get; set; }

        /// <summary>
        /// POSTCODE
        /// </summary>
        [Column("POSTCODE")]
        [MaxLength(8)]
        public string POSTCODE { get; set; }

        /// <summary>
        /// POSTCODE_NOSPACE
        /// </summary>
        [Column("POSTCODE_NOSPACE")]
        [MaxLength(8)]
        public string POSTCODE_NOSPACE { get; set; }

        /// <summary>
        /// LONGITUDE
        /// </summary>
        [Column("LONGITUDE")]
        public double LONGITUDE { get; set; }

        /// <summary>
        /// LATITUDE
        /// </summary>
        [Column("LATITUDE")]
        public double LATITUDE { get; set; }

        /// <summary>
        /// GAZETTEER
        /// </summary>
        [Column("GAZETTEER")]
        [MaxLength(5)]
        public string GAZETTEER { get; set; }

    }
}
