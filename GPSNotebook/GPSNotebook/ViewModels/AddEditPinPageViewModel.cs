using System;
using System.ComponentModel;
using System.Windows.Input;
using Acr.UserDialogs;
using GPSNotebook.Models;
using GPSNotebook.Resources;
using GPSNotebook.Services.Authorization;
using GPSNotebook.Services.Permissions;
using GPSNotebook.Services.PinService;
using GPSNotebook.Validators;
using Plugin.Permissions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    public class AddEditPinPageViewModel : ViewModelBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IPinService _pinService;
        private readonly IPermissionsService _permissionsService;

        private int PinId { get; set; }

        public AddEditPinPageViewModel(INavigationService navigationService,
            IAuthorizationService authorizationService,
            IPinService pinService,
            IPermissionsService permissionsService)
            : base(navigationService)
        {
            _authorizationService = authorizationService;
            _pinService = pinService;
            _permissionsService = permissionsService;
        }

        #region -- Public Properties

        private string _pinImagePath;
        public string PinImagePath
        {
            get => _pinImagePath;
            set => SetProperty(ref _pinImagePath, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set => SetProperty(ref _isFavorite, value);
        }

        private DelegateCommand _imageTapCommand;
        public DelegateCommand ImageTapCommand =>
            _imageTapCommand ?? (_imageTapCommand = new DelegateCommand(ExecuteImageTapCommand));

        private DelegateCommand _saveCommand;

        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand));

        public ICommand MapClickedCommand => new Command<Position>(ExecuteMapClickedCommand);

        private PinViewModel _tappedPin;
        public PinViewModel TappedPin
        {
            get => _tappedPin;
            set => SetProperty(ref _tappedPin, value);
        }

        public bool IsAllInputsFilled =>
            !string.IsNullOrEmpty(Name) &&
            !string.IsNullOrEmpty(Latitude) &&
            !string.IsNullOrEmpty(Longitude);

        private bool _wasLocationGranted;
        public bool WasLocationGranted
        {
            get => _wasLocationGranted;
            set => SetProperty(ref _wasLocationGranted, value);
        }

        #endregion

        #region -- Overrides --

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            WasLocationGranted = await _permissionsService.TryGetPermissionAsync<LocationPermission>();

            if (parameters.TryGetValue(nameof(PinViewModel), out PinViewModel pinViewModel))
            {
                Title = Resource.EditPinTitle;

                PinId = pinViewModel.Id;
                PinImagePath = pinViewModel.PinImagePath;
                Name = pinViewModel.Name;
                Description = pinViewModel.Description;
                Latitude = pinViewModel.Latitude;
                Longitude = pinViewModel.Longitude;
                IsFavorite = pinViewModel.IsFavorite;
            }
            else
            {
                Title = Resource.AddPinTitle;

                PinImagePath = Constants.DEFAULT_IMAGE_PLACEHOLDER;
            }
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Latitude) ||
                args.PropertyName == nameof(Longitude))
            {
                var wereCoordinatesSet = Latitude != null &&
                                         Longitude != null;

                var wereCoordinatesCleared = Latitude == string.Empty ||
                                             Longitude == string.Empty;

                if (CoordinatesValidator.Validate(Latitude, CoordinatesValidator.Latitude) &&
                    CoordinatesValidator.Validate(Longitude, CoordinatesValidator.Longitude))
                {
                    TappedPin = new PinViewModel
                    {
                        Name = string.Empty,
                        Position = new Position(double.Parse(Latitude), double.Parse(Longitude))
                    };
                }
                // there's no need to show this very alert while user holds backspace
                else if (wereCoordinatesSet && !wereCoordinatesCleared)
                {
                    await UserDialogs.Instance.AlertAsync(Resource.InvalidCoordinates);
                }

            }
        }

        #endregion

        #region -- Private Helpers --

        private void ExecuteMapClickedCommand(Position point)
        {
            Latitude = point.Latitude.ToString();
            Longitude = point.Longitude.ToString();
        }

        private void ExecuteImageTapCommand()
        {
            UserDialogs.Instance.ActionSheet(new ActionSheetConfig()
                .SetTitle(Resource.ChooseFrom)
                .Add(Resource.Camera, PickFromCamera, "ic_camera_alt_black.png")
                .Add(Resource.Gallery, PickFromGallery, "ic_collections_black.png")
            );
        }

        private async void ExecuteSaveCommand()
        {
            if (IsAllInputsFilled)
            {
                var pinModel = new PinModel
                {
                    Id = PinId,
                    UserId = _authorizationService.GetCurrentUserId,
                    Name = Name,
                    Description = Description,
                    IsFavorite = IsFavorite,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    PinImagePath = PinImagePath
                };

                if (PinId == 0)
                {
                    await _pinService.InsertPinAsync(pinModel);
                }
                else
                {
                    await _pinService.UpdatePinAsync(pinModel);
                }

                await NavigationService.GoBackAsync();
            }
            else
            {
                await UserDialogs.Instance.AlertAsync(Resource.AddEditPinErrorAlert);
            }
        }

        private async void PickFromGallery()
        {
            if (await _permissionsService.TryGetPermissionAsync<StoragePermission>())
            {
                var photo = await MediaPicker.PickPhotoAsync();

                if (photo != null)
                {
                    PinImagePath = photo.FullPath;
                }
            }
        }

        private async void PickFromCamera()
        {
            if (await _permissionsService.TryGetPermissionAsync<CameraPermission>())
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
                {
                    Title = $"ProfileBook{DateTime.Now:dd-MM-yyyy_hh.mm.ss}.jpg"
                });

                if (photo != null)
                {
                    PinImagePath = photo.FullPath;
                }
            }
        }

        #endregion
    }
}
