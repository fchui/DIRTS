using EPSApp.Data;
using EPSApp.Models;
using EPSApp.Views;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace EPSApp.ViewModels.Analysis
{
    public class AnalysisMenuViewModel : BaseViewModel
    {
        public Command MeasurementTapped { get; }

        public Command PumpTapped { get; }

        public Command HistoryTapped { get; }
        public AnalysisMenuViewModel()
        {
            MeasurementTapped = new Command(MeasurementSelected);
            PumpTapped = new Command(PumpSelected);
            HistoryTapped = new Command(HistorySelected);
            
        }
        async void MeasurementSelected(object obj)
        {
            await Shell.Current.GoToAsync(nameof(MeasurementsPage));
        }

        async void PumpSelected(object obj)
        {
            await Shell.Current.GoToAsync(nameof(PumpPage));
        }

        async void HistorySelected(object obj)
        {
            await Shell.Current.GoToAsync(nameof(HistoryPage));
        }
    }
}
