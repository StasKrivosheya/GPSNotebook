using GPSNotebook.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace GPSNotebook.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}