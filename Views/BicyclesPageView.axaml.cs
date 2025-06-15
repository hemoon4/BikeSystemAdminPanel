using Avalonia.Controls;
using Avalonia.Interactivity;
using BikeSystemAdminPanel.ViewModels;

namespace BikeSystemAdminPanel.Views
{
    public partial class BicyclesPageView : UserControl
    {
        public BicyclesPageView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;

        }

        private async void OnLoaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is BicyclesPageViewModel vm)
            {
                await vm.LoadBicycles();
            }
        }
    }
}