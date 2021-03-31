﻿using Prism.Mvvm;
using Prism.Navigation;

namespace GPSNotebook.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigatedAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region -- Public Properties --

        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        #endregion

        #region -- IInitialize Implementation --

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        #endregion

        #region -- INavigatedAware Implementation --

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        #endregion


        #region -- IDestructible Implementation --

        public virtual void Destroy()
        {
        }

        #endregion
    }
}
