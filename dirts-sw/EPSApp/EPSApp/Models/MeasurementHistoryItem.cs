using System;
using SQLite;

namespace EPSApp.Models
{
    [Table("MeasurementHistory")]
    public class MeasurementHistoryItem
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }

        public string Ph { get; set; }
        public string Humidity { get; set; }
        public string Temperature { get; set; }
        public string HumiditySimplified { get; set; }
        public string TemperatureSimplified { get; set; }

        public string dateString { get; set; }

    }
}
