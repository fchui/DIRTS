using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using EPSApp.Models;
using EPSApp.Services;
using Xamarin.Forms;

namespace EPSApp.ViewModels.Plants
{
    public class FilterViewModel : BaseViewModel
    {
        public List<GroupedFiltersModel> Grouped { get; private set; } = new List<GroupedFiltersModel>();

        public FilterViewModel()
        {
            CreateFiltersCollection();
            FilterDataStore.ClearFilters();
        }

        public async void SetFiltersToDataStore(Filters filter)
        {
            Filters newItem = new Filters();
            await FilterDataStore.AddFilterAsync(filter);
            return;
        }

        void CreateFiltersCollection()
        {
            Grouped.Add(new GroupedFiltersModel("Humidity", new List<Filters>
            {
                new Filters
                {
                    Name= "Low",
                    Key = "Humidity:Low"
                },
                new Filters
                {
                    Name= "Average",
                    Key = "Humidity:Average"
                },
                new Filters
                {
                    Name= "High",
                    Key = "Humidity:High"
                }
            }));

            Grouped.Add(new GroupedFiltersModel("Temperature", new List<Filters>
            {
                new Filters
                {
                    Name= "Frigid",
                    Key = "Temperature:Frigid"
                },
                new Filters
                {
                    Name= "Mesic",
                    Key = "Temperature:Mesic"
                },
                new Filters
                {
                    Name = "Thermic",
                    Key = "Temperature:Thermic"
                },
                new Filters
                {
                    Name = "Hyperthermic",
                    Key = "Temperature:Hyperthermic"
                }
            }));
        }
    }

   
}

