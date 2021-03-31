using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;

namespace GPSNotebook.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        public HomePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "TMP: main page";
        }
    }
}
