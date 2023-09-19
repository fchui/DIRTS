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
    public partial class ConnectedPage : ContentPage
    {
        public ConnectedPage()
        {
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Navigation.PopAsync();
        }
    }
}