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

        public async Task LoadRentals()
        {
            var rentals = await _repository.GetAllRentalsAsync().ConfigureAwait(false);

            Rentals.Clear();
            foreach (var rental in rentals)
            {
                Rentals.Add(rental);
            }
                
            Rentals.Add(new Rental() { Id = 1, StartTime = DateTime.Now.AddHours(-1), EndTime = DateTime.Now, UserPhoneNumber = "123-456-7890", StationId = 1, BicycleId = 101 });
            Rentals.Add(new Rental() { Id = 2, StartTime = DateTime.Now.AddHours(-2), EndTime = DateTime.Now.AddMinutes(-30), UserPhoneNumber = "234-567-8901", StationId = 2, BicycleId = 102 });
            Rentals.Add(new Rental() { Id = 3, StartTime = DateTime.Now.AddHours(-3), EndTime = DateTime.Now.AddMinutes(-20), UserPhoneNumber = "345-678-9012", StationId = 3, BicycleId = 103 });
            Rentals.Add(new Rental() { Id = 4, StartTime = DateTime.Now.AddHours(-4), EndTime = DateTime.Now.AddMinutes(-10), UserPhoneNumber = "456-789-0123", StationId = 4, BicycleId = 104 });
            Rentals.Add(new Rental() { Id = 5, StartTime = DateTime.Now.AddHours(-5), EndTime = DateTime.Now.AddMinutes(-5), UserPhoneNumber = "567-890-1234", StationId = 5, BicycleId = 105 });
            Rentals.Add(new Rental() { Id = 6, StartTime = DateTime.Now.AddHours(-6), EndTime = DateTime.Now.AddMinutes(-2), UserPhoneNumber = "678-901-2345", StationId = 6, BicycleId = 106 });
            Rentals.Add(new Rental() { Id = 7, StartTime = DateTime.Now.AddHours(-7), EndTime = DateTime.Now.AddMinutes(-1), UserPhoneNumber = "789-012-3456", StationId = 7, BicycleId = 107 });
            Rentals.Add(new Rental() { Id = 8, StartTime = DateTime.Now.AddHours(-8), EndTime = DateTime.Now, UserPhoneNumber = "890-123-4567", StationId = 8, BicycleId = 108 });
            Rentals.Add(new Rental() { Id = 9, StartTime = DateTime.Now.AddHours(-9), EndTime = DateTime.Now.AddMinutes(-30), UserPhoneNumber = "901-234-5678", StationId = 9, BicycleId = 109 });
            Rentals.Add(new Rental() { Id = 10, StartTime = DateTime.Now.AddHours(-10), EndTime = DateTime.Now.AddMinutes(-20), UserPhoneNumber = "012-345-6789", StationId = 10, BicycleId = 110 });
            Rentals.Add(new Rental() { Id = 11, StartTime = DateTime.Now.AddHours(-11), EndTime = DateTime.Now.AddMinutes(-10), UserPhoneNumber = "123-456-7891", StationId = 11, BicycleId = 111 });
            Rentals.Add(new Rental() { Id = 12, StartTime = DateTime.Now.AddHours(-12), EndTime = DateTime.Now.AddMinutes(-5), UserPhoneNumber = "234-567-8902", StationId = 12, BicycleId = 112 });
            Rentals.Add(new Rental() { Id = 13, StartTime = DateTime.Now.AddHours(-13), EndTime = DateTime.Now.AddMinutes(-2), UserPhoneNumber = "345-678-9013", StationId = 13, BicycleId = 113 });
            Rentals.Add(new Rental() { Id = 14, StartTime = DateTime.Now.AddHours(-14), EndTime = DateTime.Now.AddMinutes(-1), UserPhoneNumber = "456-789-0124", StationId = 14, BicycleId = 114 });
            Rentals.Add(new Rental() { Id = 15, StartTime = DateTime.Now.AddHours(-15), EndTime = DateTime.Now, UserPhoneNumber = "567-890-1235", StationId = 15, BicycleId = 115 });
            Rentals.Add(new Rental() { Id = 16, StartTime = DateTime.Now.AddHours(-16), EndTime = DateTime.Now.AddMinutes(-30), UserPhoneNumber = "678-901-2346", StationId = 16, BicycleId = 116 });
            Rentals.Add(new Rental() { Id = 17, StartTime = DateTime.Now.AddHours(-17), EndTime = DateTime.Now.AddMinutes(-20), UserPhoneNumber = "789-012-3457", StationId = 17, BicycleId = 117 });
            Rentals.Add(new Rental() { Id = 18, StartTime = DateTime.Now.AddHours(-18), EndTime = DateTime.Now.AddMinutes(-10), UserPhoneNumber = "890-123-4568", StationId = 18, BicycleId = 118 });
            Rentals.Add(new Rental() { Id = 19, StartTime = DateTime.Now.AddHours(-19), EndTime = DateTime.Now.AddMinutes(-5), UserPhoneNumber = "901-234-5679", StationId = 19, BicycleId = 119 });
            Rentals.Add(new Rental() { Id = 20, StartTime = DateTime.Now.AddHours(-20), EndTime = DateTime.Now.AddMinutes(-2), UserPhoneNumber = "012-345-6780", StationId = 20, BicycleId = 120 });
            Rentals.Add(new Rental() { Id = 21, StartTime = DateTime.Now.AddHours(-21), EndTime = DateTime.Now.AddMinutes(-1), UserPhoneNumber = "123-456-7892", StationId = 21, BicycleId = 121 });
            Rentals.Add(new Rental() { Id = 22, StartTime = DateTime.Now.AddHours(-22), EndTime = DateTime.Now, UserPhoneNumber = "234-567-8903", StationId = 22, BicycleId = 122 });
            Rentals.Add(new Rental() { Id = 23, StartTime = DateTime.Now.AddHours(-23), EndTime = DateTime.Now.AddMinutes(-30), UserPhoneNumber = "345-678-9014", StationId = 23, BicycleId = 123 });
            Rentals.Add(new Rental() { Id = 24, StartTime = DateTime.Now.AddHours(-24), EndTime = DateTime.Now.AddMinutes(-20), UserPhoneNumber = "456-789-0125", StationId = 24, BicycleId = 124 });
            Rentals.Add(new Rental() { Id = 25, StartTime = DateTime.Now.AddHours(-25), EndTime = DateTime.Now.AddMinutes(-10), UserPhoneNumber = "567-890-1236", StationId = 25, BicycleId = 125 });
            Rentals.Add(new Rental() { Id = 26, StartTime = DateTime.Now.AddHours(-26), EndTime = DateTime.Now.AddMinutes(-5), UserPhoneNumber = "678-901-2347", StationId = 26, BicycleId = 126 });
            Rentals.Add(new Rental() { Id = 27, StartTime = DateTime.Now.AddHours(-27), EndTime = DateTime.Now.AddMinutes(-2), UserPhoneNumber = "789-012-3458", StationId = 27, BicycleId = 127 });
            Rentals.Add(new Rental() { Id = 28, StartTime = DateTime.Now.AddHours(-28), EndTime = DateTime.Now.AddMinutes(-1), UserPhoneNumber = "890-123-4569", StationId = 28, BicycleId = 128 });
            Rentals.Add(new Rental() { Id = 29, StartTime = DateTime.Now.AddHours(-29), EndTime = DateTime.Now, UserPhoneNumber = "901-234-5680", StationId = 29, BicycleId = 129 });
            Rentals.Add(new Rental() { Id = 30, StartTime = DateTime.Now.AddHours(-30), EndTime = DateTime.Now.AddMinutes(-30), UserPhoneNumber = "012-345-6781", StationId = 30, BicycleId = 130 });
            Rentals.Add(new Rental() { Id = 31, StartTime = DateTime.Now.AddHours(-31), EndTime = DateTime.Now.AddMinutes(-20), UserPhoneNumber = "123-456-7893", StationId = 31, BicycleId = 131 });
            Rentals.Add(new Rental() { Id = 32, StartTime = DateTime.Now.AddHours(-32), EndTime = DateTime.Now.AddMinutes(-10), UserPhoneNumber = "234-567-8904", StationId = 32, BicycleId = 132 });
            Rentals.Add(new Rental() { Id = 33, StartTime = DateTime.Now.AddHours(-33), EndTime = DateTime.Now.AddMinutes(-5), UserPhoneNumber = "345-678-9015", StationId = 33, BicycleId = 133 });
            Rentals.Add(new Rental() { Id = 34, StartTime = DateTime.Now.AddHours(-34), EndTime = DateTime.Now.AddMinutes(-2), UserPhoneNumber = "456-789-0126", StationId = 34, BicycleId = 134 });
            Rentals.Add(new Rental() { Id = 35, StartTime = DateTime.Now.AddHours(-35), EndTime = DateTime.Now.AddMinutes(-1), UserPhoneNumber = "567-890-1237", StationId = 35, BicycleId = 135 });
            Rentals.Add(new Rental() { Id = 36, StartTime = DateTime.Now.AddHours(-36), EndTime = DateTime.Now, UserPhoneNumber = "678-901-2348", StationId = 36, BicycleId = 136 });
            Rentals.Add(new Rental() { Id = 37, StartTime = DateTime.Now.AddHours(-37), EndTime = DateTime.Now.AddMinutes(-30), UserPhoneNumber = "789-012-3459", StationId = 37, BicycleId = 137 });
            Rentals.Add(new Rental() { Id = 38, StartTime = DateTime.Now.AddHours(-38), EndTime = DateTime.Now.AddMinutes(-20), UserPhoneNumber = "890-123-4570", StationId = 38, BicycleId = 138 });
            Rentals.Add(new Rental() { Id = 39, StartTime = DateTime.Now.AddHours(-39), EndTime = DateTime.Now.AddMinutes(-10), UserPhoneNumber = "901-234-5681", StationId = 39, BicycleId = 139 });
            Rentals.Add(new Rental() { Id = 40, StartTime = DateTime.Now.AddHours(-40), EndTime = DateTime.Now.AddMinutes(-5), UserPhoneNumber = "012-345-6782", StationId = 40, BicycleId = 140 });
            Rentals.Add(new Rental() { Id = 41, StartTime = DateTime.Now.AddHours(-41), EndTime = DateTime.Now.AddMinutes(-2), UserPhoneNumber = "123-456-7894", StationId = 41, BicycleId = 141 });
            Rentals.Add(new Rental() { Id = 42, StartTime = DateTime.Now.AddHours(-42), EndTime = DateTime.Now.AddMinutes(-1), UserPhoneNumber = "234-567-8905", StationId = 42, BicycleId = 142 });
            Rentals.Add(new Rental() { Id = 43, StartTime = DateTime.Now.AddHours(-43), EndTime = DateTime.Now, UserPhoneNumber = "345-678-9016", StationId = 43, BicycleId = 143 });
            Rentals.Add(new Rental() { Id = 44, StartTime = DateTime.Now.AddHours(-44), EndTime = DateTime.Now.AddMinutes(-30), UserPhoneNumber = "456-789-0127", StationId = 44, BicycleId = 144 });
            Rentals.Add(new Rental() { Id = 45, StartTime = DateTime.Now.AddHours(-45), EndTime = DateTime.Now.AddMinutes(-20), UserPhoneNumber = "567-890-1238", StationId = 45, BicycleId = 145 });
            Rentals.Add(new Rental() { Id = 46, StartTime = DateTime.Now.AddHours(-46), EndTime = DateTime.Now.AddMinutes(-10), UserPhoneNumber = "678-901-2349", StationId = 46, BicycleId = 146 });
            Rentals.Add(new Rental() { Id = 47, StartTime = DateTime.Now.AddHours(-47), EndTime = DateTime.Now.AddMinutes(-5), UserPhoneNumber = "789-012-3460", StationId = 47, BicycleId = 147 });
            Rentals.Add(new Rental() { Id = 48, StartTime = DateTime.Now.AddHours(-48), EndTime = DateTime.Now.AddMinutes(-2), UserPhoneNumber = "890-123-4571", StationId = 48, BicycleId = 148 });
            Rentals.Add(new Rental() { Id = 49, StartTime = DateTime.Now.AddHours(-49), EndTime = DateTime.Now.AddMinutes(-1), UserPhoneNumber = "901-234-5682", StationId = 49, BicycleId = 149 });
            Rentals.Add(new Rental() { Id = 50, StartTime = DateTime.Now.AddHours(-50), EndTime = DateTime.Now, UserPhoneNumber = "012-345-6783", StationId = 50, BicycleId = 150 });
            // foreach (var rentall in Rentals) {
            //     await _repository.AddRentalAsync(rentall);
            // }
            IsRentalsEmpty = Rentals == null || Rentals.Count == 0;
        }
    }
}