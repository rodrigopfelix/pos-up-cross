using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrossApp.ViewModel;
using CrossApp.ViewModel.Base;
using CrossApp.Views;
using Xamarin.Forms;

namespace CrossApp.Services
{
    public class NavigationService : INavigationService
    {
        protected readonly Dictionary<Type, Type> _mappings;
        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public NavigationService()
        {
            _mappings = new Dictionary<Type, Type>();
            CreateViewModelMappings();
        }

        private void CreateViewModelMappings()
        {
            _mappings.Add(typeof(MainViewModel), typeof(MainView));
            _mappings.Add(typeof(DetailViewModel), typeof(DetailView));
        }

        public async Task Initialize()
        {
            await NavigateToAsync<MainViewModel>();
        }

        public async Task NavigateAndClearBackStackAsync<TViewModel>(object parameter = null) where TViewModel : ViewModelBase
        {
            Page page = CreateAndBindPage(typeof(TViewModel), parameter);
            var navigationPage = CurrentApplication.MainPage as NavigationPage;

            await navigationPage.PushAsync(page);

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);

            if (navigationPage != null && navigationPage.Navigation.NavigationStack.Count > 0)
            {
                var existingPages = navigationPage.Navigation.NavigationStack.ToList();

                foreach (var existingPage in existingPages)
                    if (existingPage != page)
                        navigationPage.Navigation.RemovePage(existingPage);
            }
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage != null)
                await CurrentApplication.MainPage.Navigation.PopAsync();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
            => InternalNavigateToAsync(typeof(TViewModel), null);

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
             => InternalNavigateToAsync(typeof(TViewModel), parameter);

        public Task NavigateToAsync(Type viewModelType)
            => InternalNavigateToAsync(viewModelType, null);

        public Task NavigateToAsync(Type viewModelType, object parameter)
            => InternalNavigateToAsync(viewModelType, parameter);

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);

            var navigationPage = CurrentApplication.MainPage as NavigationPage;

            if (navigationPage != null)
                await navigationPage.PushAsync(page);
            else
                CurrentApplication.MainPage = new NavigationPage(page);

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        private Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
                throw new Exception($"O mapeamento para o tipo {viewModelType} não existe");

            Page page = Activator.CreateInstance(pageType) as Page;
            ViewModelBase viewModel = parameter == null ?
                ViewModelLocator.Instance.Resolve(viewModelType) as ViewModelBase :
                ViewModelLocator.Instance.Resolve(viewModelType, parameter) as ViewModelBase;

            page.BindingContext = viewModel;

            return page;
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
                throw new Exception($"O tipo {viewModelType} não correponde a nenhuma View");

            return _mappings[viewModelType];
        }

        #region Not Implemented

        public Task RemoveLastFromBackStack()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
