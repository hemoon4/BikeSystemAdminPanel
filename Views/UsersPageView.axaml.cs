using Avalonia.Controls;
using Avalonia.Interactivity;
using BikeSystemAdminPanel.ViewModels;

namespace BikeSystemAdminPanel.Views
{
    public partial class UsersPageView : UserControl
    {
        public UsersPageView()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object? sender, RoutedEventArgs e)
        {
            if (DataContext is UsersPageViewModel vm)
            {
                await vm.LoadUsers();
            }
        }
    }
}
