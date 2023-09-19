using System;
using SQLite;

namespace EPSApp.Models
{
    [Table("Sheet1")]
    public class PlantDBItem
    {
        [PrimaryKey, AutoIncrement]
        public string Flowers__Trees___Shrubs_ { get; set; }
        public string PH_low { get; set; }
        public string Ph_high_ { get; set; }
        public string Sunlight_ { get; set; }
        public string Humidity_ { get; set; }

        public string Soil_drainage_ { get; set; }

        //public string NPK_ { get; set; }

        public string Plant_Type_ { get; set; }

        public string Temperature_ { get; set; }

        public string Soil_Type_ { get; set; }
    }
}
