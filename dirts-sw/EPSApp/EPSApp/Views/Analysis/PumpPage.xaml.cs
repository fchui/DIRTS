using EPSApp.Data;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EPSApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PumpPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        IDevice device;

        IList<IService> services;
        IService service;

        IList<ICharacteristic> characteristics;
        ICharacteristic characteristic;
        public PumpPage()
        {
            InitializeComponent();

            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
        }

        async void OnClickSubmit(object sender, EventArgs e)
        {

            string plantSubmitted = entryBox.Text;
            string itemFound = "";
            string itemHumidity = "";
            ItemDatabase database = new ItemDatabase();

            if (entryBox.Text == null || entryBox.Text == "")
            {
                return;
            }
            foreach (var item in database.ListAll())
            {
                if ((item.Flowers__Trees___Shrubs_).Trim() == plantSubmitted.Trim())
                {
                    itemFound = item.Flowers__Trees___Shrubs_;
                    break;
                }
            }
            if (itemFound != "")
            {
                itemHumidity = database.SpecificPlantbyName(itemFound).Humidity_;
            }
            else
            {
                this.DisplayAlert("Notice", "Cannot find plant in database", "OK !");
                return;
            }
            // If connected, send low, medium or high humidity to the hardware.
            if (adapter.ConnectedDevices.Count > 0 && adapter.ConnectedDevices[0].Name == "DSD TECH")
            {
                device = adapter.ConnectedDevices[0];
                //service = device.GetServiceAsync(Guid.Parse("0xFFE0"));
                services = (IList<IService>)await device.GetServicesAsync();
                //characteristic = await service.GetCharacteristicAsync(device.Id);
                for (int i = 0; i < services.Count; i++)
                {
                    if (services[i].Name == "TI SensorTag Smart Keys")
                    {
                        service = services[i];
                    }
                }
                if (service == null)
                {
                    return;
                }
                characteristics = (IList<ICharacteristic>)await service.GetCharacteristicsAsync();

                for (int i = 0; i < characteristics.Count; i++)
                {
                    if (characteristics[i].Name == "TI SensorTag Keys Data")
                    {
                        characteristic = characteristics[i];
                    }
                }
                // Send byte to configure the pump 
                if (itemHumidity.Contains("Low"))
                {
                    await characteristic.WriteAsync(Encoding.UTF8.GetBytes("5"));
                    ConfigurationMessage.Text = plantSubmitted + " is a Low Humidity Plant\nPump Configuration has been\nset to Low Humidity\n Pump will water above 35% threshold";
                }
                else if (itemHumidity.Contains("Average"))
                {
                    await characteristic.WriteAsync(Encoding.UTF8.GetBytes("6"));
                    ConfigurationMessage.Text = plantSubmitted + " is a Medium Humidity Plant\nPump Configuration has been\nset to Medium Humidity\n Pump will water above 55% threshold";
                }
                else
                {
                    await characteristic.WriteAsync(Encoding.UTF8.GetBytes("7"));
                    ConfigurationMessage.Text = plantSubmitted + " is a High Humidity Plant\nPump Configuration has been\nset to High Humidity\n Pump will water above 75% threshold";
                }
            }
            else
            {
                DisplayAlert("Notice", "DIRTS is not connected!", "OK!");
            }
        }

        async void OnClickStop(object sender, EventArgs e)
        {
            // If connected, send low, medium or high humidity to the hardware.
            if (adapter.ConnectedDevices.Count > 0 && adapter.ConnectedDevices[0].Name == "DSD TECH")
            {
                device = adapter.ConnectedDevices[0];
                //service = device.GetServiceAsync(Guid.Parse("0xFFE0"));
                services = (IList<IService>)await device.GetServicesAsync();
                //characteristic = await service.GetCharacteristicAsync(device.Id);
                for (int i = 0; i < services.Count; i++)
                {
                    if (services[i].Name == "TI SensorTag Smart Keys")
                    {
                        service = services[i];
                    }
                }
                if (service == null)
                {
                    return;
                }
                characteristics = (IList<ICharacteristic>)await service.GetCharacteristicsAsync();

                for (int i = 0; i < characteristics.Count; i++)
                {
                    if (characteristics[i].Name == "TI SensorTag Keys Data")
                    {
                        characteristic = characteristics[i];
                    }
                }
                await characteristic.WriteAsync(Encoding.UTF8.GetBytes("8"));
                ConfigurationMessage.Text = "Pump Configuration has been Reset to unconfigured\n Pump will now stop.";
            }
            else
            {
                DisplayAlert("Notice", "DIRTS is not connected!", "OK!");
            }
        }

    }
}