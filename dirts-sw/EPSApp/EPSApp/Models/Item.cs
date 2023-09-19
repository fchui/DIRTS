using System;
using SQLite;

namespace EPSApp.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string NThres { get; set; }
        public string PThres { get; set; }
        public string KThres { get; set; }
        public string PhThres { get; set; }

        public string Humidity { get; set; }

        public string Temperature { get; set; }
    }
}
