using System;
using EPSApp.Droid;
using Android.Bluetooth;
using Android.Content;
using Android.App;
using System.Collections.Generic;
using Java.Util;
using System.Text;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformDetails))]

namespace EPSApp.Droid
{
    public class PlatformDetails : IBluetooth
    {
        private BluetoothManager _bluetoothManager;
        private BluetoothAdapter _bluetoothAdapter;
        private BluetoothSocket _bluetoothSocket;

        private BluetoothDevice HC05;
        List<String> averageMeasurements;
        bool connected = false;

        //int test;
        public PlatformDetails()
        {
        }
        public bool getConnectedStatus()
        {
            return connected;
        }

        public List<String> getAverageMeasurement()
        {
            return averageMeasurements;
        }

        public bool checkPairing()
        {
            var ctx = Application.Context;

            _bluetoothManager = (BluetoothManager)ctx.GetSystemService(Context.BluetoothService);
            _bluetoothAdapter = _bluetoothManager.Adapter;

            if (_bluetoothAdapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!_bluetoothAdapter.IsEnabled)
                throw new Exception("Bluetooth adapter is not enabled.");

            // Need to make sure permission for bluetooth is granted at this point
            // They currently need to be manually enabled by the applicaation
            foreach (var bd in _bluetoothAdapter.BondedDevices)
            {
                Console.WriteLine(bd.Name);
                if (bd.Name == "DSD TECH HC-05")
                {
                    HC05 = bd;
                    return true;
                }
            }
            return false;
        }
        public async Task GetPlatformName()
        {

            var ctx = Application.Context;

            _bluetoothManager = (BluetoothManager)ctx.GetSystemService(Context.BluetoothService);
            _bluetoothAdapter = _bluetoothManager.Adapter;

            if (_bluetoothAdapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!_bluetoothAdapter.IsEnabled)
                throw new Exception("Bluetooth adapter is not enabled.");

            if (HC05 == null)
                throw new Exception("Named device not found.");

            _bluetoothSocket = HC05.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            var connectCount = 0;
            do
            {
                try
                {
                    await _bluetoothSocket.ConnectAsync();
                }
                catch (DeviceConnectionException e)
                {
                    _bluetoothSocket.Close();
                    throw e;
                }
                if (connectCount > 3)
                {
                    throw new Exception("HC is a paired device, however unable to connect to HC-05 in 3 attempts");
                }
                connectCount++;
            } while (!_bluetoothSocket.IsConnected);
            Console.WriteLine("Connected!");
            // Read data from the device
            //_bluetoothSocket.InputStream.Read(buffer);
            var test = new byte[128];
            for (int i = 0; i < test.Length; i++)
            {
                test[i] = 0x20;
            }
            
            try
            {
                await _bluetoothSocket.InputStream.ReadAsync(test, 0, test.Length);
            }
            catch (DeviceConnectionException e)
            {   
                // Ensure socket is closed if something fails
                _bluetoothSocket.Close();
                throw e;
            }
            _bluetoothSocket.Close();

            if (test == null)
                throw new Exception("Unable to read data");

            Console.WriteLine("Byte Array is: " + String.Join(" ", test));
            string measurementString;
            measurementString = Encoding.Default.GetString(test);
            Console.WriteLine("The String is: " + measurementString);

            averageMeasurements = AverageMeasurements(measurementString);
            connected = true;
        }

        private List<String> AverageMeasurements(string measurements)
        {
            // averageMeasurements list should be small
            // One spot for each measurement
            List<String> avgMeasurements = new List<String>();
            List<String> rawMeasurements = new List<String>();

            List<String> stringMeasurements = new List<String>();
            string[] splitString;
            int numberTypesOfMeasurements = 0;
            int totalNumberMeasurements = 0;
            int itemCount;

            stringMeasurements = measurements.Split(';').ToList();

            // There is a potential for the string measurement data to have corrupted
            // So we cut the first and and last measurements off to defend against this
            if (stringMeasurements.Count > 1)
            {
                stringMeasurements.RemoveAt(0);
                stringMeasurements.RemoveAt(stringMeasurements.Count-1);
            }
            else
            {
                throw new Exception("Corrupt and not enough data");
            }

            if (stringMeasurements.Count > 0)
            {
                // Count how many types of measurements there are
                numberTypesOfMeasurements = stringMeasurements[0].Count(f => (f == ',')) + 1;
            }

            // Initialize avgMeasurements
            for (int i = 0; i < numberTypesOfMeasurements; i++)
            {
                avgMeasurements.Add("0");
            }


            foreach (var item in stringMeasurements)
            {
                totalNumberMeasurements++;
                splitString = item.Split(',');
                itemCount = 0;
                foreach (var item2 in splitString)
                {
                    // First add calculations in the respective place in the list
                    // Convert to doubles and put it back to string
                    avgMeasurements[itemCount] = (Convert.ToDouble(avgMeasurements[itemCount]) + Convert.ToDouble(item2)).ToString();
                    itemCount++;
                }
            }

            for (int i = 0; i < numberTypesOfMeasurements; i++)
            {
                avgMeasurements[i] = (Convert.ToDouble(avgMeasurements[i]) / totalNumberMeasurements).ToString();
            }
             
            return avgMeasurements;
        }
    }

    [Serializable]
    internal class DeviceConnectionException : Exception
    {
        public DeviceConnectionException()
        {
        }

        public DeviceConnectionException(string message) : base(message)
        {
        }

        public DeviceConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DeviceConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}