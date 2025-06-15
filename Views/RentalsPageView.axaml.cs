using Avalonia.Controls;
using Avalonia.Interactivity;
using BikeSystemAdminPanel.ViewModels;

namespace BikeSystemAdminPanel.Views
{
    public partial class RentalsPageView : UserControl
    {
        public RentalsPageView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is RentalsPageViewModel vm)
            {
                
                await vm.LoadRentals();
            }
        }
    }
}