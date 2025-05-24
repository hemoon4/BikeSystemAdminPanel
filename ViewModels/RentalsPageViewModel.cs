using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BikeSystemAdminPanel.Models;
using BikeSystemAdminPanel.Database;

namespace BikeSystemAdminPanel.ViewModels
{
    public partial class RentalsPageViewModel : ViewModelBase
    {
        private readonly IRentalRepository _repository;

        public RentalsPageViewModel()
        {
            var dbPath = "data.db";
            _repository = new SqliteRentalRepository(dbPath);
        }

        [ObservableProperty]
        private ObservableCollection<Rental> _rentals = new();

        [ObservableProperty]
        private Rental _newRental = new();

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private bool _isStatusSuccess;

        public async Task LoadRentals()
        {
            var rentals = await _repository.GetAllRentalsAsync().ConfigureAwait(false);

            Rentals.Clear();
            foreach (var rental in rentals)
                Rentals.Add(rental);
        }

        [RelayCommand]
        private async Task AddRental()
        {
            StatusMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewRental.UserPhoneNumber) ||
                NewRental.StationId <= 0 ||
                NewRental.BicycleId <= 0 ||
                NewRental.StartTime == default)
            {
                StatusMessage = "All fields are required, and IDs must be positive.";
                IsStatusSuccess = false;
                return;
            }

            var rentalToAdd = new Rental
            {
                StartTime = NewRental.StartTime,
                EndTime = NewRental.EndTime,
                UserPhoneNumber = NewRental.UserPhoneNumber,
                StationId = NewRental.StationId,
                BicycleId = NewRental.BicycleId
            };

            await _repository.AddRentalAsync(rentalToAdd);

            Rentals.Add(rentalToAdd);

            StatusMessage = "Rental added successfully.";
            IsStatusSuccess = true;
            NewRental = new Rental();
        }

        [RelayCommand]
        private async Task DeleteRental(Rental rental)
        {
            if (rental == null) return;

            await _repository.DeleteRentalAsync(rental.Id).ConfigureAwait(false);

            Rentals.Remove(rental);
        }
    }
}