using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using EPSApp.Views;
using EPSApp.ViewModels.Analysis;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE;

namespace EPSApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnalysisMenuPage : ContentPage
    {
        AnalysisMenuViewModel _viewModel;
    
        IBluetoothLE ble;
        IAdapter adapter;
        IDevice device;

        string Status;
        public AnalysisMenuPage()
        {
            InitializeComponent();

            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;

            BindingContext = _viewModel = new AnalysisMenuViewModel();
        }
        protected override void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(() => {
                if (adapter.ConnectedDevices.Count > 0 && adapter.ConnectedDevices[0].Name == "DSD TECH")
                {
                    Status = "DIRTS Status: Connected";
                }
                else
                {
                    Status = "Dirts Status: Not Connected";
                }
                
                ConnectionStatusText.Text = Status;
            });
        }
    }
}