using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace EPSApp.ViewModels.Settings
{
    public class Settings : ContentView
    {
        public Settings()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Settings" }
                }
            };
        }
    }
}