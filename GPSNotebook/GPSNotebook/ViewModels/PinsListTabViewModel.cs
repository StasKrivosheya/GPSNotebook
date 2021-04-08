using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Acr.UserDialogs;
using GPSNotebook.Extensions;
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

        public ICommand EditPinCommand => new Command<Pin>(ExecuteEditPinCommand);

        public ICommand DeletePinCommand => new Command<Pin>(ExecuteDeletePinCommand);

        #endregion

        #region -- Overrides --

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(SelectedPin) && SelectedPin != null)
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    { nameof(CameraPosition), new CameraPosition(SelectedPin.Position, 1.0) }
                };
                SelectedPin = null;
                NavigationService.SelectTabAsync(nameof(MapTab), parameters);
            }
        }

        #endregion

        #region -- Private Helpers -- 

        private void OnTabActivated(object sender, EventArgs e)
        {
            if (IsActive)
            {
                UpdatePinsCollection();
            }
        }

        private async void UpdatePinsCollection()
        {
            var pinModels = await _pinService.GetPinsListAsync(
                pin => pin.UserId == _authorizationService.GetCurrentUserId);

            List<PinViewModel> pins = new List<PinViewModel>();

            foreach (var pinModel in pinModels)
            {
                pins.Add(pinModel.ToPinViewModel());
            }

            PinsCollection = new ObservableCollection<PinViewModel>(pins);

            IsListVisible = PinsCollection.Any();
            IsLabelVisible = !PinsCollection.Any();
        }

        private async void ExecuteAddPinCommand()
        {
            await NavigationService.NavigateAsync(nameof(AddEditPinPage));
        }

        private void ExecuteDeletePinCommand(Pin pin)
        {
            UserDialogs.Instance.Alert(nameof(ExecuteDeletePinCommand) + " " + pin);
        }

        private void ExecuteEditPinCommand(Pin pin)
        {
            UserDialogs.Instance.Alert(nameof(ExecuteEditPinCommand) + " " + pin);
        }

        #endregion
    }
}
