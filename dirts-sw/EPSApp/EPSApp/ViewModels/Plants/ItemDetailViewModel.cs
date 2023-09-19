using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EPSApp.Data;
using EPSApp.Models;
using Xamarin.Forms;

namespace EPSApp.ViewModels.Plants
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        public string Id { get; set; }

        private string plantName;
        public string PlantName
        {
            get => plantName;
            set => SetProperty(ref plantName, value);
        }

        private string phLow;
        public string PhLow
        {
            get => phLow;
            set => SetProperty(ref phLow, value);
        }

        private string phHigh;
        public string PhHigh
        {
            get => phHigh;
            set => SetProperty(ref phHigh, value);
        }

        private string sunlight;
        public string Sunlight
        {
            get => sunlight;
            set => SetProperty(ref sunlight, value);
        }

        private string humidity;
        public string Humidity
        {
            get => humidity;
            set => SetProperty(ref humidity, value);
        }

        private string soilDrainage;
        public string SoilDrainage
        {
            get => soilDrainage;
            set => SetProperty(ref soilDrainage, value);
        }

        private string plantType;
        public string PlantType
        {
            get => plantType;
            set => SetProperty(ref plantType, value);
        }

        private string temperature;
        public string Temperature
        {
            get => temperature;
            set => SetProperty(ref temperature, value);
        }

        private string soilType;
        public string SoilType
        {
            get => soilType;
            set => SetProperty(ref soilType, value);
        }
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string plant)
        {
            ItemDatabase database = new ItemDatabase();

            try
            {
                var item = database.SpecificPlantbyName(itemId);
                PlantName = item.Flowers__Trees___Shrubs_;
                PhLow = item.PH_low;
                PhHigh = item.Ph_high_;
                Sunlight = item.Sunlight_;
                Humidity = item.Humidity_;
                SoilDrainage = item.Soil_drainage_;
                PlantType = item.Plant_Type_;
                Temperature = item.Temperature_;
                SoilType = item.Soil_Type_;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}

