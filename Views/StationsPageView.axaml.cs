using Avalonia.Controls;
using Avalonia.Interactivity;
using BikeSystemAdminPanel.ViewModels;

namespace BikeSystemAdminPanel.Views
{
    public partial class StationsPageView : UserControl
    {
        public StationsPageView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;

        }
        
        private async void OnLoaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is StationsPageViewModel vm)
            {
                await vm.LoadStations();
            }
        }
    }
}