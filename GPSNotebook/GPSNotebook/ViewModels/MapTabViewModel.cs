using System.Collections.Generic;
using System.Collections.ObjectModel;
using GPSNotebook.Services.Authorization;
using GPSNotebook.Services.PinService;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    // --- contains lots of tmp stuff just for debug or trying  ---
    // --- will be replaced with useful code in further commits ---

    public class MapTabViewModel : ViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;

        public MapTabViewModel(INavigationService navigationService,
            IPinService pinService,
            IAuthorizationService authorizationService)
            : base(navigationService)
        {
            _pinService = pinService;
            _authorizationService = authorizationService;

            UpdatePinsCollection();
        }

        #region -- Public properties --

        private ObservableCollection<Pin> _pinsList;

        public ObservableCollection<Pin> PinsList
        {
            get => _pinsList;
            set => SetProperty(ref _pinsList, value);
        }

        #endregion

        #region -- Private Helpers --

        private async void UpdatePinsCollection()
        {
            var pinModels = await _pinService.GetPinsListAsync(
                pin => pin.UserId == _authorizationService.GetCurrentUserId);

            List<Pin> pins = new List<Pin>();

            foreach (var pinModel in pinModels)
            {
                pins.Add(new Pin
                {
                    Position = new Position(double.Parse(pinModel.Latitude), double.Parse(pinModel.Longitude)),
                    IsVisible = pinModel.IsFavorite,
                    Label = pinModel.Name
                });
            }

            PinsList = new ObservableCollection<Pin>(pins);
        }

        #endregion
    }
}
