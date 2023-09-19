using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using EPSApp.Views;
using EPSApp.ViewModels.Plants;
using EPSApp.ViewModels.Analysis;

using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using EPSApp.Data;
using EPSApp.Models;

namespace EPSApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurementsPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        IDevice device;

        IList<IService> services;
        IService service;

        IList<ICharacteristic> characteristics;
        ICharacteristic characteristic;

        IList<IDescriptor> descriptors;
        IDescriptor descriptor;

        ItemDatabase database;

        private ItemsViewModel itemsViewModel;
        private MeasurementsViewModel measurementViewModel;

        public IList<Measurement> measurements { get; private set; }

        public IList<String> averageMeasurements { get; private set; }
        public MeasurementsPage()
        {
            InitializeComponent();
            averageMeasurements = new List<String>();
            measurements = new List<Measurement>();

            measurements.Add(new Measurement
            {
                Label = "Ph",
                Measure = "Value",
                Image = ""
            });
            measurements.Add(new Measurement
            {
                Label = "Humidity",
                Measure = "Value",
                MeasureSimplified = "None",
                Image = ""
            });
            measurements.Add(new Measurement
            {
                Label = "Temperature",
                Measure = "Value",
                MeasureSimplified = "None",
                Image = ""
            });
            
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;

            itemsViewModel = new ItemsViewModel();

            measurementsList.ItemsSource = measurements;
            database = new ItemDatabase();
        }
        private async void getMeasure(object sender, EventArgs e)
        {
            if (adapter.ConnectedDevices.Count > 0 && adapter.ConnectedDevices[0].Name == "DSD TECH")
            {
                MeasureButton.IsVisible = false;
                SuggestionButton.IsVisible = false;
                WaitButton.IsVisible = true;
                try
                {
                    await GetAverageMeasure();
                }
                catch (Exception ex)
                {
                    WaitButton.IsVisible = false;
                    MeasureButton.IsVisible = true;
                    DisplayAlert("Notice", "Measurement has failed", "OK!");
                    return;
                }
                WaitButton.IsVisible = false;
                MeasureButton.IsVisible = true;
                SuggestionButton.IsVisible = true;
            }
            else
            {
                DisplayAlert("Notice", "DIRTS is not connected!", "Error !");
            }
        }
        private async void MeasureWarning(object sender, EventArgs e)
        {
            this.DisplayAlert("Notice", "DIRTS is currently measuring the soil", "OK !");
        }
        async Task GetAverageMeasure()
        {
            device = adapter.ConnectedDevices[0];
            services = (IList<IService>)await device.GetServicesAsync();
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

            string readString;
            await characteristic.WriteAsync(Encoding.UTF8.GetBytes("0"));
            List<string> listString = new List<string>();
            characteristic.ValueUpdated += (o, args) =>
            {
                var readBytes = args.Characteristic.Value;
                readString = System.Text.Encoding.UTF8.GetString(readBytes);
                Console.WriteLine(readString);
                listString.Add(readString);
            };
            await characteristic.StartUpdatesAsync();
            await Task.Delay(5000);
            await characteristic.WriteAsync(Encoding.UTF8.GetBytes("2"));
            await characteristic.StopUpdatesAsync();

            List<String> RawResults = new List<String>();
            for (int i = 0; i < listString.Count; i += 1)
            {
                RawResults.Add(listString[i]);
                Console.WriteLine(listString[i]);
            }

            List<double> RawPh = new List<double>();
            List<double> RawHumidity = new List<double>();
            List<double> RawTemperature = new List<double>();

            string[] splitString;
            string stringTrimmed;

            for (int i = 0; i < RawResults.Count; i++)
            {
                stringTrimmed = RawResults[i].Trim(';'); ;
                splitString = stringTrimmed.Split(',');
                RawPh.Add(Convert.ToDouble(splitString[0]));
                RawHumidity.Add(Convert.ToDouble(splitString[1]));
                RawTemperature.Add(Convert.ToDouble(splitString[2]));
            }

            double AveragePh = Queryable.Average(RawPh.AsQueryable());
            double AverageHumidity = Queryable.Average(RawHumidity.AsQueryable());
            double AverageTemperature = Queryable.Average(RawTemperature.AsQueryable());

            measurements[0].Measure = Math.Round(AveragePh, 2).ToString();
            measurements[1].Measure = Math.Round(AverageHumidity, 2).ToString() + " %";
            measurements[2].Measure = Math.Round(AverageTemperature, 2).ToString() + " °C";

            
            measurements[1].MeasureSimplified = checkMeasureSimplifiedHumidity(AverageHumidity);
            measurements[2].MeasureSimplified = checkMeasureSimplifiedTemperature(AverageTemperature);

            database.AddMeasurement(measurements[0].Measure, measurements[1].Measure, measurements[2].Measure, measurements[1].MeasureSimplified, measurements[2].MeasureSimplified);
        }

        private string checkMeasureSimplifiedHumidity(double AverageHumidity)
        {
            if (AverageHumidity >= 75)
            {
                return "High";
            }
            else if (AverageHumidity >= 55)
            {
                return "Average";
            }
            else
            {
                return "Low";
            }
        }

        private string checkMeasureSimplifiedTemperature(double AverageTemperature)
        {
            if (AverageTemperature < 8)
            {
                return "Frigid";
            }
            else if (AverageTemperature <= 15)
            {
                return "Mesic";
            }
            else if (AverageTemperature <= 22)
            {
                return "Thermic";
            }
            else
            {
                return "Hyperthermic";
            }
        }

        async void OnClickSubmit(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SuggestionPage(measurements));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}