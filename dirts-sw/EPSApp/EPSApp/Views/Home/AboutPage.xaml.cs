using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using EPSApp.Views.Home;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EPSApp.ViewModels.Analysis;
using EPSApp.ViewModels.Home;

namespace EPSApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> UIDeviceList;
        ObservableCollection<IDevice> DeviceList;
        IDevice device;

        public AboutViewModel aboutViewModel;
        public AboutPage()
        {
            InitializeComponent();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            UIDeviceList = new ObservableCollection<IDevice>();
            DeviceList = new ObservableCollection<IDevice>();
                
            lv.ItemsSource = UIDeviceList;
        }
        private async void OnClickChangeText(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (!ble.IsOn)
            {
                DisplayAlert("Notice", "Please turn on Bluetooth", "OK!");
                return;
            }

            var state = ble.State;
            // _ = this.DisplayAlert("Notice", state.ToString(), "OK !");
            if (!adapter.IsScanning)
            {
                UIDeviceList.Clear();
                try
                {

                    adapter.DeviceDiscovered += (s, a) =>
                    {
                        if (a.Device.Name == "DSD TECH")
                        {
                            if (!DeviceList.Contains(a.Device))
                            {
                                DeviceList.Add(a.Device);
                            }
                        }
                    };

                    //We have to test if the device is scanning 
                    if (!ble.Adapter.IsScanning)
                    {
                        button.IsVisible = false;
                        ScanningButton.IsVisible = true;
                        await adapter.StartScanningForDevicesAsync();
                        foreach (var item in DeviceList)
                        {
                            UIDeviceList.Add(item);
                        }
                        await adapter.StopScanningForDevicesAsync();
                        ScanningButton.IsVisible = false;
                        button.IsVisible = true;
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert("Notice", "Please enable permissions in Settings->Apps->EPSApp", "Error !");
                }
            }
            else
            {
                _ = DisplayAlert("Notice", "Please wait for device to finish it's previous scan", "Error !");
            }
        }
        private async void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Console.WriteLine("Clicked Connect");
            if (lv.SelectedItem == null)
            {
                return;
            }
            if (!ble.IsOn)
            {
                DisplayAlert("Notice", "Please turn on Bluetooth", "OK!");
                lv.SelectedItem = null;
                return;
            }
            device = lv.SelectedItem as IDevice;
            Console.WriteLine(device.Name);
            try
            {
                if (device != null)
                {
                    await adapter.ConnectToDeviceAsync(device);
                    await Navigation.PushAsync(new ConnectedPage());
                }
                else
                {
                    DisplayAlert("Notice", "Device was unable to connect properly!", "OK");
                    lv.SelectedItem = null;
                }
            }
            catch (DeviceConnectionException ex)
            {
                //Could not connect to the device
                Console.WriteLine(ex.Message.ToString());
                DisplayAlert("Notice", "Device was unable to connect properly!", "OK");
                lv.SelectedItem = null;
            }
        }

        private async void OnClickDisconnect(object sender, EventArgs e)
        {
            lv.SelectedItem = null;
            UIDeviceList.Clear();
            try
            {
                await adapter.DisconnectDeviceAsync(adapter.ConnectedDevices[0]);
                await adapter.StopScanningForDevicesAsync();
                DisconnectButton.IsVisible = false;
                button.IsVisible = true;
            }
            catch (DeviceConnectionException ex)
            {
                //Could not disconnect safely from device
                DisplayAlert("Notice", ex.Message.ToString(), "OK");
            }

        }

        private void OnClickWaitWarning(object sender, EventArgs e)
        {
            DisplayAlert("Notice", "Please wait for scan to finish", "OK!");
        }

        private void txtErrorBle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            
            if (adapter.ConnectedDevices.Count > 0 && adapter.ConnectedDevices[0].Name == "DSD TECH")
            {
                button.IsVisible = false;
                DisconnectButton.IsVisible = true;
            }
            else
            {
                button.IsVisible = true;
                DisconnectButton.IsVisible = false;
            }

            if (!ble.IsOn)
            {
                lv.SelectedItem = null;
                UIDeviceList.Clear();
                button.IsVisible = true;
                DisconnectButton.IsVisible = false;
            }
        }
    }
}
