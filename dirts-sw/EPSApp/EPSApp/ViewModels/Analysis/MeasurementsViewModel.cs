using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

using Xamarin.Forms;

using EPSApp.Models;
using EPSApp.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EPSApp.ViewModels.Analysis
{
    public class Measurement : INotifyPropertyChanged
    {
        public string Label { get; set; }

        private string _measure;
        public string Measure {
            get
            {
                return _measure;
            }
            set
            {
                _measure = value;
                NotifyPropertyChanged("Measure");
            }
        }

        private string _measureSimplified;
        public string MeasureSimplified
        {
            get
            {
                return _measureSimplified;
            }
            set
            {
                _measureSimplified = value;
                NotifyPropertyChanged("MeasureSimplified");
            }

        }
        public string Image { get; set; }

        public string Threshold { get; set; }
        public override string ToString()
        {
            return Label;
        }
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class MeasurementsViewModel : INotifyPropertyChanged
    {
        private Measurement _measurement;
        public Command LoadItemsCommand { get; }
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}