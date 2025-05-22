using System.Collections.Generic;
using System.Threading.Tasks;
using BikeSystemAdminPanel.Models;

namespace BikeSystemAdminPanel.Database
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string phoneNumber);
    }
}
