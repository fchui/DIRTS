using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EPSApp.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PairingPage : ContentPage
    {
        public PairingPage()
        {
            InitializeComponent();
        }

        async void OnClickConnect(object sender, EventArgs e)
        {
            String text;
            Button button = sender as Button;
            if (DependencyService.Get<IBluetooth>().checkPairing() == true)
            {
                await DependencyService.Get<IBluetooth>().GetPlatformName();
                text = "";
                button.Text = text;

                if (DependencyService.Get<IBluetooth>().getConnectedStatus() == true)
                {
                    text = "Connected";
                    button.Text = text;
                    await Navigation.PushAsync(new ConnectedPage());
                }
            }
        }
    }
}