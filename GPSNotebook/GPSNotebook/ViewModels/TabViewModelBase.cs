using System;
using System.Collections.ObjectModel;
using Prism;
using Prism.Navigation;
using Xamarin.Forms.GoogleMaps;

namespace GPSNotebook.ViewModels
{
    public class TabViewModelBase : ViewModelBase, IActiveAware
    {
        private bool _isActive;

        public TabViewModelBase(INavigationService navigationService)
            : base(navigationService)
        {
        }

        #region -- Public Properties --

        private ObservableCollection<Pin> _pinsCollection;
        public ObservableCollection<Pin> PinsCollection
        {
            get => _pinsCollection;
            set => SetProperty(ref _pinsCollection, value);
        }

        #endregion

        #region -- IActiveAware Implementation --

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, RaiseIsActiveChanged);
        }
        public event EventHandler IsActiveChanged;

        #endregion

        #region -- Protected Helpers --

        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
