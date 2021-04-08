using System.ComponentModel;
using System.Windows.Input;
using Acr.UserDialogs;
using GPSNotebook.Resources;
using GPSNotebook.Validators;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    public class AddEditPinPageViewModel : ViewModelBase
    {
        public AddEditPinPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
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

        #endregion

        #region -- Overrides --

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.TryGetValue(nameof(PinViewModel), out PinViewModel pinViewModel))
            {
                Title = Resource.EditPinTitle;

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
            // take photo or pick from gallery
            UserDialogs.Instance.Alert(nameof(ExecuteImageTapCommand));
        }

        private void ExecuteSaveCommand()
        {
            // save pin to db and navigate to PinsListTab
            UserDialogs.Instance.Alert(nameof(ExecuteSaveCommand));
        }

        #endregion
    }
}
