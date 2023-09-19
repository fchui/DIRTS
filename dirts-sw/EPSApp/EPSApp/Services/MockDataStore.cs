using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EPSApp.Models;

namespace EPSApp.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;
        public MockDataStore()
        {
            var plant_name = new List<String>();
            var plant_description = new List<String>();
            var plant_NThres = new List<String>();
            var plant_PThres = new List<String>();
            var plant_KThres = new List<String>();
            var plant_HumidityThres = new List<String>();
            var plant_PhThres = new List<String>();
            var plant_TemperatureThres = new List<String>();

            var assembly = Assembly.GetExecutingAssembly();
            
            string path = "EPSApp.Capstone_Plantdatabase.csv";
            // For DB implementation we would send a query of this selected database parameters here and get
            // it returned before we start building the item list.
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    plant_name.Add(values[0]);
                    plant_description.Add(values[9]);
                    plant_NThres.Add(values[2]);
                    // P Threshold and K Threshold same value for now
                    plant_PThres.Add(values[3]);
                    plant_KThres.Add(values[3]);
                    plant_HumidityThres.Add(values[5]);
                    plant_PhThres.Add(values[1]);
                    plant_TemperatureThres.Add(values[4]);
                }
                items = new List<Item>();
                for (int i = 1; i < plant_name.Count; i++)
                    items.Add(new Item { Id = Guid.NewGuid().ToString(), 
                        Text = plant_name[i].Trim(), 
                        Description = plant_description[i].Trim(),
                        NThres = plant_NThres[i].Trim(),
                        PThres = plant_PThres[i].Trim(),
                        KThres = plant_KThres[i].Trim(),
                        Humidity = plant_HumidityThres[i].Trim(), 
                        PhThres = plant_PhThres[i].Trim(), 
                        Temperature = plant_TemperatureThres[i].Trim() });
            }
            // Won't be required when db is setup but this code will be here for now to test selecting filter
            foreach (var item in items)
            {
                if (item.Temperature == "Mesic")
                {
                    Console.WriteLine(item.Text + " Plant is Mesic");
                }
            }
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
