using CrossApp.Models;
using CrossApp.Services;
using CrossApp.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CrossApp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        readonly ISerieService _serieService;

        public ICommand ItemClickCommand { get; }
        public ICommand SearchClickCommand { get; }
        public ObservableCollection<Serie> Items { get; }
        public string SearchedString { get; }

        public MainViewModel(ISerieService serieService, string searchedString = null) : base("CrossApp")
        {
            _serieService = serieService;

            Items = new ObservableCollection<Serie>();
            SearchedString = searchedString;

            ItemClickCommand = new Command<Serie>(async (item)
                => await ItemClickCommandExecute(item));
            SearchClickCommand = new Command<string>(async (text)
                => await SearchClickCommandExecute(text));
        }

        private async Task ItemClickCommandExecute(Serie serie)
        {
            if (serie != null)
                await NavigationService.NavigateToAsync<DetailViewModel>(serie);
        }

        private async Task SearchClickCommandExecute(string text)
        {
            if (text == null)
                await NavigationService.NavigateBackAsync();
            else
            {
                if (SearchedString != "")
                    await NavigationService.NavigateBackAsync();
                await NavigationService.NavigateToAsync<MainViewModel>(text);
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);
            await LoadDataAsync(navigationData as string);
        }

        private async Task LoadDataAsync(string filter=null)
        {
            var result = await _serieService.GetSeriesAsync();

            if (filter != null)
                result.Series = result.Series.Where(x => x.Name.ToLower().Contains(filter.ToLower()));

            AddItems(result);
        }

        private void AddItems(SerieResponse result)
        {
            Items.Clear();
            result?.Series.ToList()?.ForEach(i => Items.Add(i));

            if (Items.Count == 0)
                Items.Add(new Serie()
                {
                    Name = "There's no serie to show",
                    OriginalName = "There's no serie to show"
                });
        }
    }
}
