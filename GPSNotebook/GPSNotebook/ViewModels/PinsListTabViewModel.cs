using Prism.Navigation;

namespace GPSNotebook.ViewModels
{
    public class PinsListTabViewModel : ViewModelBase
    {
        public PinsListTabViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            var a = 1;
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            string value = null;
            var a = parameters.TryGetValue("key", out value);
            var b = value;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            string value = null;
            var a = parameters.TryGetValue("key", out value);
            var b = value;
        }
    }
}
