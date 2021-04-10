﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GPSNotebook.Extensions;
using GPSNotebook.Services.Authorization;
using GPSNotebook.Services.Permissions;
using GPSNotebook.Services.PinService;
using GPSNotebook.Views;
using Plugin.Permissions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    public class MapTabViewModel : TabViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPermissionsService _permissionsService;

        public MapTabViewModel(INavigationService navigationService,
            IPinService pinService,
            IAuthorizationService authorizationService,
            IPermissionsService permissionsService)
            : base(navigationService)
        {
            _pinService = pinService;
            _authorizationService = authorizationService;
            _permissionsService = permissionsService;

            IsActiveChanged += OnTabActivated;
        }

        #region -- Public properties --

        private ObservableCollection<PinViewModel> _pinsCollection;
        public new ObservableCollection<PinViewModel> PinsCollection
        {
            get => _pinsCollection;
            set => SetProperty(ref _pinsCollection, value);
        }

        private CameraPosition _myCameraPosition;
        public CameraPosition MyCameraPosition
        {
            get => _myCameraPosition;
            set => SetProperty(ref _myCameraPosition, value);
        }
        #endregion

        private bool _wasLocationGranted;
        public bool WasLocationGranted
        {
            get => _wasLocationGranted;
            set => SetProperty(ref _wasLocationGranted, value);
        }

        private Pin _selectedPin;
        public Pin SelectedPin
        {
            get => _selectedPin;
            set => SetProperty(ref _selectedPin, value);
        }

        public ICommand SuggestionChosenCommand => new DelegateCommand<PinViewModel>(ExecuteSuggestionChosenCommand);

        #region -- Overrides --

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            WasLocationGranted = await _permissionsService.TryGetPermissionAsync<LocationPermission>();

            if (parameters.TryGetValue(nameof(CameraPosition), out CameraPosition cameraPosition))
            {
                MyCameraPosition = cameraPosition;
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedPin) && SelectedPin != null)
            {
                var pinModelForPopUp = (await _pinService.GetPinsListAsync(pin => pin.Name == SelectedPin.Label)).First();

                var parameters = new NavigationParameters
                {
                    {nameof(PinViewModel), pinModelForPopUp}
                };

                await NavigationService.NavigateAsync(nameof(PinInfoPopupPage), animated:true, useModalNavigation: true, parameters: parameters);
            }

            if (args.PropertyName == nameof(SearchText))
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    PinsToShow = new ObservableCollection<PinViewModel>(PinsCollection.Where(pin => pin.IsFavorite));
                }
                else
                {
                    var preparedSearchText = SearchText.ToLower().Trim();

                    var result = PinsCollection
                        .Where(pin => pin.Name.ToLower().Contains(preparedSearchText) && pin.IsFavorite);

                    PinsToShow = new ObservableCollection<PinViewModel>(result);
                }
            }
        }

        #endregion

        #region -- Private Helpers --

        private async void OnTabActivated(object sender, EventArgs e)
        {
            if (IsActive)
            {
                await UpdatePinsCollectionAsync();
            }
        }

        private async Task UpdatePinsCollectionAsync()
        {
            var pinModels = await _pinService.GetPinsListAsync(
                pin => pin.UserId == _authorizationService.GetCurrentUserId);

            List<PinViewModel> pins = new List<PinViewModel>();

            foreach (var pinModel in pinModels)
            {
                pins.Add(pinModel.ToPinViewModel());
            }

            PinsCollection = new ObservableCollection<PinViewModel>(pins);
            PinsToShow = new ObservableCollection<PinViewModel>(PinsCollection.Where(pin => pin.IsFavorite));
        }

        private void ExecuteSuggestionChosenCommand(PinViewModel pin)
        {
            MyCameraPosition = new CameraPosition(pin.Position, 1.0);
        }

        #endregion
    }
}
