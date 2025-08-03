using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BikeSystemAdminPanel.Models;
using BikeSystemAdminPanel.Database;
using Avalonia;

namespace BikeSystemAdminPanel.ViewModels
{
    public partial class RentalsPageViewModel : ViewModelBase
    {
        private readonly IRentalRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IStationRepository _stationRepository;
        private readonly IBicycleRepository _bicycleRepository;

        public RentalsPageViewModel()
        {
            var dbPath = "data.db";
            _repository = new SqliteRentalRepository(dbPath);
            _userRepository = new SQLiteUserRepository(dbPath);
            _stationRepository = new SqliteStationRepository(dbPath);
            _bicycleRepository = new SqliteBicycleRepository(dbPath);
        }

        [ObservableProperty]
        private ObservableCollection<Rental> _rentals = new();

        [ObservableProperty]
        private ObservableCollection<User> _users = new();

        [ObservableProperty]
        private ObservableCollection<Station> _stations = new();

        [ObservableProperty]
        private ObservableCollection<Bicycle> _bicycles = new();

        [ObservableProperty]
        private Rental _newRental = new();

        [ObservableProperty]
        private User _selectedUser;

        [ObservableProperty]
        private Station _selectedStation;

        [ObservableProperty]
        private Bicycle _selectedBicycle;

        [ObservableProperty]
        private DateTimeOffset? _newRentalStartDate = DateTimeOffset.Now;

        [ObservableProperty]
        private TimeSpan? _newRentalStartTime = DateTimeOffset.Now.TimeOfDay;

        [ObservableProperty]
        private DateTimeOffset? _newRentalEndDate;

        [ObservableProperty]
        private TimeSpan? _newRentalEndTime;

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private bool _isStatusSuccess;

        [ObservableProperty]
        private bool _isRentalsEmpty;

        public async Task LoadDataAsync()
        {
            var usersTask = _userRepository.GetAllUsersAsync();
            var stationsTask = _stationRepository.GetAllStationsAsync();
            var bicyclesTask = _bicycleRepository.GetAllBicyclesAsync();
            var rentalsTask = _repository.GetAllRentalsAsync();

            await Task.WhenAll(usersTask, stationsTask, bicyclesTask, rentalsTask);

            Users.Clear();
            foreach (var user in await usersTask)
            {
                Users.Add(user);
            }

            Stations.Clear();
            foreach (var station in await stationsTask)
            {
                Stations.Add(station);
            }

            Bicycles.Clear();
            foreach (var bicycle in await bicyclesTask)
            {
                Bicycles.Add(bicycle);
            }

            Rentals.Clear();
            foreach (var rental in await rentalsTask)
            {
                Rentals.Add(rental);
            }

            IsRentalsEmpty = Rentals.Count == 0;
        }

        [RelayCommand]
        private async Task SimulateRentalAsync()
        {
            if (Users.Count == 0)
            {
                StatusMessage = "Cannot simulate, users are empty.";
                IsStatusSuccess = false;
                return;
            }

            if (Stations.Count == 0)
            {
                StatusMessage = "Cannot simulate, stations are empty.";
                IsStatusSuccess = false;
                return;
            }

            if (Bicycles.Count == 0)
            {
                StatusMessage = "Cannot simulate, bicycles are empty.";
                IsStatusSuccess = false;
                return;
            }

            var random = new Random();
            var user = Users[random.Next(Users.Count)];
            var station = Stations[random.Next(Stations.Count)];
            var bicycle = Bicycles[random.Next(Bicycles.Count)];

            var now = DateTime.Now;
            var startOfYear = new DateTime(now.Year, 1, 1);
            var daysInYear = DateTime.IsLeapYear(now.Year) ? 366 : 365;
            var randomDay = random.Next(daysInYear);
            var startTime = startOfYear.AddDays(randomDay).AddHours(random.Next(24)).AddMinutes(random.Next(60)).AddSeconds(random.Next(60));

            DateTime? endTime = null;
            if (random.Next(2) == 0)
            {
                var endOfYear = new DateTime(now.Year, 12, 31, 23, 59, 59);
                var timeSpan = endOfYear - startTime;
                if (timeSpan.TotalSeconds > 0)
                {
                    var randomSeconds = random.Next((int)timeSpan.TotalSeconds + 1);
                    endTime = startTime.AddSeconds(randomSeconds);
                }
            }

            var tempRental = new Rental
            {
                StartTime = startTime,
                EndTime = endTime,
                UserPhoneNumber = user.PhoneNumber,
                StationId = station.Id,
                BicycleId = bicycle.Id
            };

            var id = await _repository.AddRentalAsync(tempRental);

            var newRental = new Rental{Id = id, StartTime = startTime, EndTime = endTime, UserPhoneNumber = user.PhoneNumber, StationId = station.Id, BicycleId = bicycle.Id};
            Rentals.Add(newRental);

            IsRentalsEmpty = false;
            StatusMessage = "Rental simulated successfully.";
            IsStatusSuccess = true;
        }
    }
}