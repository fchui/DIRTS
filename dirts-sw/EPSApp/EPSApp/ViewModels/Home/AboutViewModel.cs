using System.ComponentModel;
using Plugin.BLE.Abstractions.Contracts;
using System.Runtime.CompilerServices;
using EPSApp.Models;

namespace EPSApp.ViewModels.Home
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        private string _connectionStatus;
        public string Status
        {
            get
            {
                return _connectionStatus;
            }
            set
            {
                _connectionStatus = value;
                RaisePropertyChanged("Status");
            }
        }

        private IDevice _nativeDevice;

        public event PropertyChangedEventHandler PropertyChanged;
        public IDevice NativeDevice
        {
            get
            {
                return _nativeDevice;
            }
            set
            {
                _nativeDevice = value;
                RaisePropertyChanged();
            }
        }
        protected void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
    }
}
