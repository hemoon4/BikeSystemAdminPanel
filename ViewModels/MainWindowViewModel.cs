using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using BikeSystemAdminPanel.Views;
using System.Threading.Tasks;

namespace BikeSystemAdminPanel.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private object _currentSplitViewContent;

        [ObservableProperty]
        private ObservableCollection<string> _menuItems = [ "Home", "Users", "Bicycles", "Stations", "Rentals" ];

        [ObservableProperty]
        private string _selectedMenuItem = "Home";

        partial void OnSelectedMenuItemChanged(string value)
        {
            CurrentSplitViewContent = value switch
            {
                "Home" => new HomePageView { DataContext = new HomePageViewModel() },
                "Users" => new UsersPageView { DataContext = new UsersPageViewModel() },
                "Bicycles" => new BicyclesPageView { DataContext = new BicyclesPageViewModel() },
                "Stations" => new StationsPageView { DataContext = new StationsPageViewModel() },
                "Rentals" => new RentalsPageView { DataContext = new RentalsPageViewModel() },
                _ => null
            };
        }

        public async Task LoadPage() {
            if (CurrentSplitViewContent is null) {
                var model = new HomePageViewModel();
                CurrentSplitViewContent = new HomePageView { DataContext = model }; 
                await model.LoadChart();
            }
        }
    }
}