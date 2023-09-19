using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using EPSApp.Models;
using EPSApp.Views;
using EPSApp.Data;
using System.Collections.Generic;
using System.Linq;

namespace EPSApp.ViewModels.Plants
{
    public class ItemsViewModel : BaseViewModel
    {
        private PlantDBItem _selectedItem;

        public List<Filters> selectedFilters;
        public ObservableCollection<Item> Items { get; }

        public ObservableCollection<PlantDBItem> PlantDBItems { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<PlantDBItem> ItemTapped { get; }
        public Command FilterTapped { get; }
        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            PlantDBItems = new ObservableCollection<PlantDBItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<PlantDBItem>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            FilterTapped = new Command(FilterSelected);
        }

        public ItemsViewModel(List<Filters> filters)
        {
            Title = "Browse";
            PlantDBItems = new ObservableCollection<PlantDBItem>();

            selectedFilters = filters;
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<PlantDBItem>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            FilterTapped = new Command(FilterSelected);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                PlantDBItems.Clear();
                //var items = await DataStore.GetItemsAsync(true);

                ItemDatabase database = new ItemDatabase();
                var filters = await FilterDataStore.GetFiltersAsync(true);
                if (filters.ToList().Count == 0)
                {
                    foreach (var item in database.ListAll())
                    {
                        PlantDBItems.Add(item);
                    }
                }
                else
                {
                    foreach (var item in database.ListFilters(filters.ToList()))
                    {
                        PlantDBItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public PlantDBItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(PlantDBItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Flowers__Trees___Shrubs_}");
        }
        async void FilterSelected(object obj)
        {
            await Shell.Current.GoToAsync(nameof(FilterPage));
        }

        public async void searchList(string entry)
        {
            ItemDatabase database = new ItemDatabase();
            PlantDBItems.Clear();
            var filters = await FilterDataStore.GetFiltersAsync(true);
            if (filters.ToList().Count == 0)
            {
                foreach (var item in database.ListSearch(entry))
                {
                    PlantDBItems.Add(item);
                }
            }
            else
            {
                
                foreach (var item in database.ListFilters(filters.ToList(), entry))
                {
                    PlantDBItems.Add(item);
                }
            }
        }
    }
}
