using System;
using System.Collections.ObjectModel;
using Prism;
using Prism.Navigation;

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

        private ObservableCollection<PinViewModel> _pinsCollection;
        public ObservableCollection<PinViewModel> PinsCollection
        {
            get => _pinsCollection;
            set => SetProperty(ref _pinsCollection, value);
        }

        private ObservableCollection<PinViewModel> _pinsToShow;
        public ObservableCollection<PinViewModel> PinsToShow
        {
            get => _pinsToShow;
            set => SetProperty(ref _pinsToShow, value);
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
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
