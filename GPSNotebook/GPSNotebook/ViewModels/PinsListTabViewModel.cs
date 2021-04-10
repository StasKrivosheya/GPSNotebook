using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using GPSNotebook.Extensions;
using GPSNotebook.Resources;
using GPSNotebook.Services.Authorization;
using GPSNotebook.Services.PinService;
using GPSNotebook.Views;
using Prism.Commands;
using Prism.Navigation;
using Prism.Navigation.TabbedPages;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    public class PinsListTabViewModel : TabViewModelBase
    {
        private readonly IPinService _pinService;
        private readonly IAuthorizationService _authorizationService;

        public PinsListTabViewModel(INavigationService navigationService,
            IPinService pinService,
            IAuthorizationService authorizationService)
            : base(navigationService)
        {
            _pinService = pinService;
            _authorizationService = authorizationService;

            IsActiveChanged += OnTabActivated;
        }

        #region -- Public Properies --

        private bool _isListVisible = true;
        public bool IsListVisible
        {
            get => _isListVisible;
            set => SetProperty(ref _isListVisible, value);
        }

        private bool _isLabelVisible;
        public bool IsLabelVisible
        {
            get => _isLabelVisible;
            set => SetProperty(ref _isLabelVisible, value);
        }

        private PinViewModel _selectedPin;
        public PinViewModel SelectedPin
        {
            get => _selectedPin;
            set => SetProperty(ref _selectedPin, value);
        }

        private CameraPosition _cameraPosition;
        public CameraPosition CameraPosition
        {
            get => _cameraPosition;
            set => SetProperty(ref _cameraPosition, value);
        }

        private DelegateCommand _addPinCommand;
        public DelegateCommand AddPinCommand =>
            _addPinCommand ?? (_addPinCommand = new DelegateCommand(ExecuteAddPinCommand));

        public ICommand EditPinCommand => new Command<PinViewModel>(ExecuteEditPinCommand);

        public ICommand DeletePinCommand => new Command<PinViewModel>(ExecuteDeletePinCommand);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedPin) && SelectedPin != null)
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    { nameof(CameraPosition), new CameraPosition(SelectedPin.Position, Constants.DEFAULT_CAMERA_ZOOM) }
                };
                SelectedPin = null;
                NavigationService.SelectTabAsync(nameof(MapTab), parameters);
            }

            if (args.PropertyName == nameof(SearchText))
            {
                if (string.IsNullOrEmpty(SearchText))
                {
                    PinsToShow = new ObservableCollection<PinViewModel>(PinsCollection);
                }
                else
                {
                    var preparedSearchText = SearchText.ToLower().Trim();

                    var result = PinsCollection
                        .Where(pin => pin.Name.ToLower().Contains(preparedSearchText) ||
                                      (!string.IsNullOrEmpty(pin.Description) && pin.Description.ToLower().Contains(preparedSearchText)) ||
                                      pin.Latitude.ToLower().Contains(preparedSearchText) ||
                                      pin.Longitude.ToLower().Contains(preparedSearchText));

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

                var lastInput = SearchText;
                SearchText = string.Empty;
                SearchText = lastInput;
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

            PinsToShow = new ObservableCollection<PinViewModel>(PinsCollection);

            IsListVisible = PinsToShow.Any();
            IsLabelVisible = !PinsToShow.Any();
        }

        private async void ExecuteAddPinCommand()
        {
            await NavigationService.NavigateAsync(nameof(AddEditPinPage));
        }

        private async void ExecuteDeletePinCommand(PinViewModel pin)
        {
            ConfirmConfig config = new ConfirmConfig
            {
                Message = $"{Resource.PinDeletionConfirm} {pin.Name}?",
                CancelText = Resource.Cancel,
                OkText = Resource.OkText
            };

            var shouldDelete = await UserDialogs.Instance.ConfirmAsync(config);

            if (shouldDelete)
            {
                await _pinService.DeletePinAsync(pin.ToPinModel());

                await UpdatePinsCollectionAsync();
            }
        }

        private async void ExecuteEditPinCommand(PinViewModel pin)
        {
            var parameters = new NavigationParameters { { nameof(PinViewModel), pin } };

            await NavigationService.NavigateAsync(nameof(AddEditPinPage), parameters);
        }

        #endregion
    }
}
