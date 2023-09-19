using System;
using System.Collections.Generic;
using EPSApp.Models;
using EPSApp.Views;
using EPSApp.Data;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Reflection;
using System.IO;
using System.Collections.ObjectModel;

namespace EPSApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public ObservableCollection<PlantDBItem> Items { get; set; } = new ObservableCollection<PlantDBItem>();
        public AppShell()
        {
            InitializeComponent();
            VersionTracking.Track();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(FilterPage), typeof(FilterPage));
            Routing.RegisterRoute(nameof(MeasurementsPage), typeof(MeasurementsPage));
            Routing.RegisterRoute(nameof(PumpPage), typeof(PumpPage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));

            if (VersionTracking.IsFirstLaunchEver)
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                using (Stream stream = assembly.GetManifestResourceStream("EPSApp.Plant.db"))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);

                        File.WriteAllBytes(ItemDatabase.DbPath, memoryStream.ToArray());
                    }
                }
            }
        }

    }
}

