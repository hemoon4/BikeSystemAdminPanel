using Avalonia.Controls;
using Avalonia.Interactivity;
using BikeSystemAdminPanel.ViewModels;
using System;

namespace BikeSystemAdminPanel.Views
{
    public partial class HomePageView : UserControl
    {
        public HomePageView()
        {
            InitializeComponent();
            
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is HomePageViewModel vm)
            {
                await vm.LoadChart();
            }
        }
        private async void OnChartTypeChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (DataContext is HomePageViewModel vm && e.AddedItems.Count > 0)
            {
                await vm.LoadChart();
            }
        }
    }
}