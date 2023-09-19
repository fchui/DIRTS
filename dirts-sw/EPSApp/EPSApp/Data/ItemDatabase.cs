using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using EPSApp.Models;
using System;
using System.IO;

namespace EPSApp.Data
{
    public class ItemDatabase
    {
        private readonly SQLiteConnection _database;
        private static string whereQuery = "WHERE ";
        public static string DbPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Plant.db");

        public ItemDatabase()
        {
            _database = new SQLiteConnection(DbPath);
            _database.CreateTable<PlantDBItem>();
            _database.CreateTable<MeasurementHistoryItem>();
        }
        public void AddMeasurement(string phMeasurement, string humidityMeasurement, string temperatureMeasurement, string humidityMeasurementSimple, string temperatureMeasurementSimple)
        {
            DateTime date = DateTime.Now;
            MeasurementHistoryItem measurementHistoryItem = new MeasurementHistoryItem {
                id = 1,
                Year = date.Year.ToString(),
                Month = date.Month.ToString(),
                Day = date.Day.ToString(),
                Hour = date.Hour.ToString(),
                Minute = date.Minute.ToString(),
                Second = date.Second.ToString(),
                Ph = phMeasurement,
                Humidity = humidityMeasurement,
                Temperature = temperatureMeasurement,
                HumiditySimplified = humidityMeasurementSimple,
                TemperatureSimplified = temperatureMeasurementSimple,
                dateString = date.Year.ToString() + "-" + date.Month.ToString() + "-" + date.Day.ToString() + "\n" + date.Hour.ToString() + ":" + date.Minute.ToString() + ":" + date.Second.ToString()
            };
            _database.Insert(measurementHistoryItem);
        }

        public List<MeasurementHistoryItem> ListAllHistory()
        {
            return _database.Table<MeasurementHistoryItem>().ToList();
        }

        public List<PlantDBItem> ListAll()
        {
            return _database.Table<PlantDBItem>().ToList();
        }
        public List<PlantDBItem> ListFilters(List<Filters> filters)
        {
            string filtersQuery = "";
            filtersQuery = FilterString(filters);

            return _database.Query<PlantDBItem>("SELECT * FROM Sheet1 " + whereQuery + filtersQuery);
        }

        public List<PlantDBItem> ListFilters(List<Filters> filters, string SearchString)
        {
            string filtersQuery = "";
            filtersQuery = FilterString(filters);
            string SearchQuery = "Flowers__Trees___Shrubs_ LIKE '" + SearchString + "%'";
            return _database.Query<PlantDBItem>("SELECT * FROM Sheet1 " + whereQuery + filtersQuery + " AND " + SearchQuery);
        }

        public List<PlantDBItem> ListSearch(string SearchString)
        {
            string SearchQuery = "Flowers__Trees___Shrubs_ LIKE '" + SearchString + "%'";
            return _database.Query<PlantDBItem>("SELECT * FROM Sheet1 " + whereQuery + SearchQuery);
        }

        public List<PlantDBItem> ListSpecific(string PhMeasure, string HumidityMeasure, string TemperatureMeasure)
        {
            string PhQuery = PhMeasure + " BETWEEN PH_LOW AND Ph_high_";
            string HumidityQuery = "Humidity_ LIKE '%" + HumidityMeasure + "%'";
            string TemperatureQuery = "Temperature_ LIKE '%" + TemperatureMeasure + "%'";
            return _database.Query<PlantDBItem>("SELECT * FROM Sheet1 " + whereQuery + PhQuery + " AND " + HumidityQuery + " AND " + TemperatureQuery);
        }

        public List<PlantDBItem> ListSpecific(string PhMeasure, string HumidityMeasure, string TemperatureMeasure, string SearchString)
        {
            string PhQuery = PhMeasure + " BETWEEN PH_LOW AND Ph_high_";
            string HumidityQuery = "Humidity_ LIKE '%" + HumidityMeasure + "%'";
            string TemperatureQuery = "Temperature_ LIKE '%" + TemperatureMeasure + "%'";
            string SearchQuery = "Flowers__Trees___Shrubs_ LIKE '%" + SearchString + "%'";
            return _database.Query<PlantDBItem>("SELECT * FROM Sheet1 " + whereQuery + PhQuery + " AND " + HumidityQuery + " AND " + TemperatureQuery + " AND " + SearchQuery);
        }

        public PlantDBItem SpecificPlantbyName(string Name)
        {
            return _database.Query<PlantDBItem>("SELECT * FROM Sheet1 " + whereQuery + "Flowers__Trees___Shrubs_ LIKE '%" + Name + "%'")[0];
        }

        private string FilterString(List<Filters> filters)
        {
            List<String> humidityFilters = new List<String>();
            List<String> temperatureFilters = new List<String>();
            foreach (Filters filter in filters)
            {
                if (filter.Key.Contains("Humidity"))
                {
                    if (filter.Name == "Low")
                    {
                        humidityFilters.Add("Humidity_ LIKE '%Low%'");
                    }
                    if (filter.Name == "Average")
                    {
                        humidityFilters.Add("Humidity_ LIKE '%Average%'");
                    }
                    if (filter.Name == "High")
                    {
                        humidityFilters.Add("Humidity_ LIKE '%High%'");
                    }
                }
                if (filter.Key.Contains("Temperature"))
                {
                    if (filter.Name == "Frigid")
                    {
                        temperatureFilters.Add("Temperature_ LIKE '%Frigid%'");
                    }
                    if (filter.Name == "Mesic")
                    {
                        temperatureFilters.Add("Temperature_ LIKE '%Mesic%'");
                    }
                    if (filter.Name == "Thermic")
                    {
                        temperatureFilters.Add("Temperature_ LIKE '%Thermic%'");
                    }
                    if (filter.Name == "Hyperthermic")
                    {
                        temperatureFilters.Add("Temperature_ LIKE '%Hyperthermic%'");
                    }
                }
            }

            string humidityQuery = "";
            if (humidityFilters.Count == 2)
            {
                humidityQuery = humidityFilters[0] + " OR " + humidityFilters[1];
            }
            else if (humidityFilters.Count == 1)
            {
                humidityQuery = humidityFilters[0];
            }
            else
            {

            }

            string temperatureQuery = "";
            if (temperatureFilters.Count == 4)
            {
                temperatureQuery = temperatureFilters[0] + " OR " + temperatureFilters[1] + " OR " + temperatureFilters[2] + " OR " + temperatureFilters[3];
            }
            else if (temperatureFilters.Count == 3)
            {
                temperatureQuery = temperatureFilters[0] + " OR " + temperatureFilters[1] + " OR " + temperatureFilters[2];
            }
            else if (temperatureFilters.Count == 2)
            {
                temperatureQuery = temperatureFilters[0] + " OR " + temperatureFilters[1];
            }
            else if (temperatureFilters.Count == 1)
            {
                temperatureQuery = temperatureFilters[0];
            }
            else
            {

            }

            string filtersQuery = "";
            if (temperatureQuery != "" && humidityQuery != "")
            {
                filtersQuery = "(" + humidityQuery + ")" + " AND " + "(" + temperatureQuery + ")";
            }
            else if (temperatureQuery != "")
            {
                filtersQuery = temperatureQuery;
            }
            else if (humidityQuery != "")
            {
                filtersQuery = humidityQuery;
            }
            else
            {
            }
            return filtersQuery;
        }
    }
}