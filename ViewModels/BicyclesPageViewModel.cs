using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BikeSystemAdminPanel.Models;
using BikeSystemAdminPanel.Database;

namespace BikeSystemAdminPanel.ViewModels
{
    public partial class BicyclesPageViewModel : ViewModelBase
    {
        private readonly IBicycleRepository _repository;

        public BicyclesPageViewModel()
        {
            var dbPath = "data.db";
            _repository = new SqliteBicycleRepository(dbPath);
        }

        [ObservableProperty]
        private ObservableCollection<Bicycle> _bicycles = new();

        [ObservableProperty]
        private Bicycle _newBicycle = new();

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private bool _isStatusSuccess;

        public async Task LoadBicycles()
        {
            var bicycles = await _repository.GetAllBicyclesAsync().ConfigureAwait(false);

            Bicycles.Clear();
            foreach (var bicycle in bicycles)
                Bicycles.Add(bicycle);
        }

        [RelayCommand]
        private async Task AddBicycle()
        {
            StatusMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewBicycle.Name) ||
                string.IsNullOrWhiteSpace(NewBicycle.Type) ||
                NewBicycle.StationId <= 0)
            {
                StatusMessage = "All fields are required, and Station ID must be a positive number.";
                IsStatusSuccess = false;
                return;
            }

            var bicycleToAdd = new Bicycle
            {
                Name = NewBicycle.Name,
                Type = NewBicycle.Type,
                StationId = NewBicycle.StationId
            };

            await _repository.AddBicycleAsync(bicycleToAdd);

            Bicycles.Add(bicycleToAdd);

            StatusMessage = "Bicycle added successfully.";
            IsStatusSuccess = true;
            NewBicycle = new Bicycle();
        }

        [RelayCommand]
        private async Task DeleteBicycle(Bicycle bicycle)
        {
            if (bicycle == null) return;

            await _repository.DeleteBicycleAsync(bicycle.Id).ConfigureAwait(false);

            Bicycles.Remove(bicycle);
        }
    }
}