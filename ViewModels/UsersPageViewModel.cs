using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BikeSystemAdminPanel.Models;
using BikeSystemAdminPanel.Database;

namespace BikeSystemAdminPanel.ViewModels
{
    public partial class UsersPageViewModel : ViewModelBase
    {
        private readonly IUserRepository _repository;

        public UsersPageViewModel()
        {
            var dbPath = "data.db";
            _repository = new SQLiteUserRepository(dbPath);
        }

        [ObservableProperty]
        private ObservableCollection<User> _users = new();

        [ObservableProperty]
        private User _newUser = new();

        [ObservableProperty]
        private string _statusMessage;

        [ObservableProperty]
        private bool _isStatusSuccess;

        public async Task LoadUsers()
        {
            var users = await _repository.GetAllUsersAsync().ConfigureAwait(false);

            Users.Clear();
            foreach (var user in users)
                Users.Add(user);
        }

        [RelayCommand]
        private async Task AddUser()
        {
            StatusMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(NewUser.PhoneNumber) ||
                string.IsNullOrWhiteSpace(NewUser.Name) ||
                string.IsNullOrWhiteSpace(NewUser.Surname))
            {
                StatusMessage = "All fields are required.";
                IsStatusSuccess = false;
                return;
            }

            if (Users.Any(u => u.PhoneNumber == NewUser.PhoneNumber))
            {
                StatusMessage = $"User with phone number {NewUser.PhoneNumber} already exists.";
                IsStatusSuccess = false;
                return;
            }

            var userToAdd = new User
            {
                PhoneNumber = NewUser.PhoneNumber,
                Name = NewUser.Name,
                Surname = NewUser.Surname
            };

            await _repository.AddUserAsync(userToAdd);

            Users.Add(userToAdd);

            StatusMessage = "User added successfully.";
            IsStatusSuccess = true;
            NewUser = new User();
        }
        
        [RelayCommand]
        private async Task DeleteUser(User user)
        {
            if (user == null) return;

            await _repository.DeleteUserAsync(user.PhoneNumber).ConfigureAwait(false);

            Users.Remove(user);
        }
    }
}
