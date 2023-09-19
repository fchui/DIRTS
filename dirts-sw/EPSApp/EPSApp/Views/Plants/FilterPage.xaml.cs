using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using EPSApp.Models;
using EPSApp.ViewModels;

namespace EPSApp.Views
{
    public partial class FilterPage : ContentPage
    {
        static ViewModels.Plants.FilterViewModel groupedFilters;
        List<Filters> selectedFilters = new List<Filters>();
        private NavigationPage filteredItemsPage;

        public FilterPage()
        {
            InitializeComponent();
            groupedFilters = new ViewModels.Plants.FilterViewModel();
            BindingContext = groupedFilters;

        }
        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Filters selection in e.CurrentSelection)
            {
                //selectedFilters.Add(selection);
                groupedFilters.SetFiltersToDataStore(selection);
            }
        }
    }
}