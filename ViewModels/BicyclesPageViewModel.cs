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
        private readonly IStationRepository _stationRepository;

        public BicyclesPageViewModel()
        {
            var dbPath = "data.db";
            _repository = new SqliteBicycleRepository(dbPath);
            _stationRepository = new SqliteStationRepository(dbPath);
        }

        [ObservableProperty]
        private ObservableCollection<Bicycle> _bicycles = [];

        [ObservableProperty]
        private ObservableCollection<Station> _stations = [];

        [ObservableProperty]
        private Bicycle _newBicycle = new();

        [ObservableProperty]
        private Station _selectedStation;

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private bool _isStatusSuccess;

        [ObservableProperty]
        private ObservableCollection<string> _bicycleTypes = new()
        {
            "gravel",
            "mountain",
            "BMX",
            "electric",
            "hybrid",
            "city",
            "road",
            "track",
            "touring"
        };

        [ObservableProperty]
        private string _selectedBicycleType;

        [ObservableProperty]
        private bool _isBicyclesEmpty;

        public async Task LoadBicycles()
        {
            var bicycles = await _repository.GetAllBicyclesAsync().ConfigureAwait(false);

            Bicycles.Clear();
            foreach (var bicycle in bicycles) 
            {
                Bicycles.Add(bicycle);
            }
            IsBicyclesEmpty = Bicycles == null || Bicycles.Count == 0;
        }

        public async Task LoadStations()
        {
            var stations = await _stationRepository.GetAllStationsAsync().ConfigureAwait(false);

            Stations.Clear();
            foreach (var station in stations)
            {
                Stations.Add(station);
            }
        }

        [RelayCommand]
        private async Task AddBicycle()
        {
            StatusMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewBicycle.Name) ||
                string.IsNullOrWhiteSpace(SelectedBicycleType) ||
                SelectedStation == null)
            {
                StatusMessage = "All fields are required, and Station must be selected.";
                IsStatusSuccess = false;
                return;
            }

            var bicycleToAdd = new Bicycle
            {
                Name = NewBicycle.Name,
                Type = SelectedBicycleType,
                StationId = SelectedStation.Id
            };

            await _repository.AddBicycleAsync(bicycleToAdd);

            StatusMessage = "Bicycle added successfully.";
            IsStatusSuccess = true;
            NewBicycle = new Bicycle();
            SelectedStation = null;

            await LoadBicycles();
        }

        [RelayCommand]
        private async Task DeleteBicycle(Bicycle bicycle)
        {
            if (bicycle == null) return;

            await _repository.DeleteBicycleAsync(bicycle.Id).ConfigureAwait(false);

            Bicycles.Remove(bicycle);
        
            IsBicyclesEmpty = Bicycles == null || Bicycles.Count == 0;
        }
    }
}