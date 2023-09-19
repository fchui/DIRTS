using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using EPSApp.Views;
using EPSApp.ViewModels.Analysis;
using EPSApp.Data;
using EPSApp.Models;

namespace EPSApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        ItemDatabase database;
        List<MeasurementHistoryItem> lv;

        public HistoryPage()
        {
            InitializeComponent();
            database = new ItemDatabase();
            lv = database.ListAllHistory();
            historyList.ItemsSource = lv;
        }
    }
}