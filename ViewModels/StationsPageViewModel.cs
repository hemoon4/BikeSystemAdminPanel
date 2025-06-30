using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BikeSystemAdminPanel.Models;
using BikeSystemAdminPanel.Database;

namespace BikeSystemAdminPanel.ViewModels
{
    public partial class StationsPageViewModel : ViewModelBase
    {
        private readonly IStationRepository _repository;

        public StationsPageViewModel()
        {
            var dbPath = "data.db";
            _repository = new SqliteStationRepository(dbPath);
        }

        [ObservableProperty]
        private ObservableCollection<Station> _stations = new();

        [ObservableProperty]
        private Station _newStation = new();

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private bool _isStatusSuccess;

        public async Task LoadStations()
        {
            var stations = await _repository.GetAllStationsAsync().ConfigureAwait(false);

            Stations.Clear();
            foreach (var station in stations)
            {
                Stations.Add(station);
            }
        }

        [RelayCommand]
        private async Task AddStation()
        {
            StatusMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewStation.Name) ||
                string.IsNullOrWhiteSpace(NewStation.Address) ||
                NewStation.NumberOfBicyclesHold < 0)
            {
                StatusMessage = "Name and Address are required, and Number of Bicycles Held must be non-negative.";
                IsStatusSuccess = false;
                return;
            }

            var stationToAdd = new Station
            {
                Name = NewStation.Name,
                Address = NewStation.Address,
                NumberOfBicyclesHold = 0
            };

            await _repository.AddStationAsync(stationToAdd);

            StatusMessage = "Station added successfully.";
            IsStatusSuccess = true;
            NewStation = new Station();

            await LoadStations();
        }

        [RelayCommand]
        private async Task DeleteStation(Station station)
        {
            if (station == null) return;

            await _repository.DeleteStationAsync(station.Id).ConfigureAwait(false);

            Stations.Remove(station);
        }
    }
}