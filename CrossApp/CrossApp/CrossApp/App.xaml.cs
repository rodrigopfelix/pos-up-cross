using CrossApp.Services;
using CrossApp.ViewModel.Base;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace CrossApp
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            BuildDependencies();

            InitNavigation();
		}

        private async void InitNavigation()
        {
            var navigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
            await navigationService.Initialize();
        }

        private void BuildDependencies()
        {
            ViewModelLocator.Instance.Build();
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
