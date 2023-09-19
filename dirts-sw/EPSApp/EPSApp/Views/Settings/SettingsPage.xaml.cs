using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EPSApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        async void OnButtonClicked(object sender, EventArgs args)
        {
            var address = "EPS.Support@gmail.com";
            var uri = $"mailto:{address}";
            await Launcher.OpenAsync(uri);
        }

        private async void OpenBrowser(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var url = button.ClassId;

            await Browser.OpenAsync(url);
        }
    }
}