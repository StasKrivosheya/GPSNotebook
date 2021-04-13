using System.Windows.Input;
using GPSNotebook.Models;
using Prism.Commands;
using Prism.Navigation;

namespace GPSNotebook.ViewModels
{
    public class PinInfoPopupPageViewModel : ViewModelBase
    {
        public PinInfoPopupPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }

        #region -- Public Properties --

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

        private string _pinImagePath;
        public string PinImagePath
        {
            get => _pinImagePath;
            set => SetProperty(ref _pinImagePath, value);
        }

        public ICommand GoBackCommand => new DelegateCommand(ExecuteGoBackCommand);

        #endregion

        #region -- Overrides --

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue(nameof(PinViewModel), out PinModel pin))
            {
                Name = pin.Name;
                Description = pin.Description;
                PinImagePath = pin.PinImagePath;
            }
        }

        private async void ExecuteGoBackCommand()
        {
            await NavigationService.GoBackAsync();
        }

        #endregion
    }
}
