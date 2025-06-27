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
            Console.WriteLine("OnLoaded called");
            if (DataContext is HomePageViewModel vm)
            {
                Console.WriteLine("loading chart");
                await vm.LoadChart();
            }
        }
    }
}