using System.ComponentModel;
using Xamarin.Forms;
using EPSApp.ViewModels;

namespace EPSApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.Plants.ItemDetailViewModel();
        }
    }
}
