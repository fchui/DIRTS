using EPSApp.Data;
using EPSApp.Models;
using EPSApp.ViewModels.Analysis;
using EPSApp.ViewModels.Plants;
using EPSApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace EPSApp.ViewModels
{
    public class SuggestionViewModel : BaseViewModel
    {

        public ObservableCollection<PlantDBItem> SuggestionDBItems { get; set; }

        public List<PlantDBItem> AllSuggestionDBItems { get; }
        public List<PlantDBItem> SuggestionDB5Items { get; set; }

        public Command<PlantDBItem> ItemTapped { get; }
        public SuggestionViewModel(IList<Measurement> measurements)
        {
            ItemTapped = new Command<PlantDBItem>(OnItemSelected);

            SuggestionDBItems = new ObservableCollection<PlantDBItem>();

            AllSuggestionDBItems = new List<PlantDBItem>();
            SuggestionDB5Items = new List<PlantDBItem>();

            try
            {
                SuggestionDBItems.Clear();
                AllSuggestionDBItems.Clear();
                ItemDatabase database = new ItemDatabase();

                foreach (var item in database.ListSpecific(measurements[0].Measure, measurements[1].MeasureSimplified, measurements[2].MeasureSimplified))
                {
                    AllSuggestionDBItems.Add(item);
                }
                Random random = new Random();
                SuggestionDB5Items = AllSuggestionDBItems.OrderBy(x => random.Next()).Take(5).ToList();

                foreach (var item in SuggestionDB5Items)
                {
                    SuggestionDBItems.Add(item);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        async void OnItemSelected(PlantDBItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Flowers__Trees___Shrubs_}");
        }
    }
}
