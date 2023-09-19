using EPSApp.Models;
using EPSApp.ViewModels;
using EPSApp.ViewModels.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EPSApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuggestionPage : ContentPage
    {
        SuggestionViewModel _viewModel;
        public IList<Measurement> measurements { get; private set; }

        public IList<string> messages { get; private set; }

        public SuggestionPage(IList<Measurement> measurements)
        {
            InitializeComponent();

            BindingContext = _viewModel = new SuggestionViewModel(measurements);
            if (_viewModel.SuggestionDBItems.Count == 0)
            {
                NoneFoundLabel.IsVisible = true;
            }
            else
            {
                NoneFoundLabel.IsVisible = false;
            }
        }
    }
}