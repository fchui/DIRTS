using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using EPSApp.Models;
using EPSApp.Views;
using EPSApp.ViewModels.Plants;

namespace EPSApp.Views
{
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel _viewModel;
        static ViewModels.Plants.FilterViewModel groupedFilters;
        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel();
        }

        public ItemsPage(List<Filters> filters)
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel(filters);
        }
        async void EntryCompleted(object sender, EventArgs e)
        {
            var text = ((Entry)sender).Text; //cast sender to access the properties of the Entry
            _viewModel.searchList(text.ToString());

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
            //groupedFilters.FilterDataStore.GetFiltersAsync();
        }

    }
}
