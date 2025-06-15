using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using BikeSystemAdminPanel.Views;

namespace BikeSystemAdminPanel.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private object _currentSplitViewContent;

        [ObservableProperty]
        private ObservableCollection<string> _menuItems = new() { "Users", "Bicycles", "Stations", "Rentals" };

        [ObservableProperty]
        private string _selectedMenuItem;

        partial void OnSelectedMenuItemChanged(string value)
        {
            CurrentSplitViewContent = value switch
            {
                "Users" => new UsersPageView { DataContext = new UsersPageViewModel() },
                "Bicycles" => new BicyclesPageView { DataContext = new BicyclesPageViewModel() },
                "Stations" => new StationsPageView { DataContext = new StationsPageViewModel() },
                "Rentals" => new RentalsPageView { DataContext = new RentalsPageViewModel() },
                _ => null
            };
        }
    }
}