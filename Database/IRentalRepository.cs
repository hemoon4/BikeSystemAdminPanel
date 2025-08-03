using System.Collections.Generic;
using System.Threading.Tasks;
using BikeSystemAdminPanel.Models;

namespace BikeSystemAdminPanel.Database
{
    public interface IRentalRepository
    {
        Task<List<Rental>> GetAllRentalsAsync();
        Task<int> AddRentalAsync(Rental rental);
        Task DeleteRentalAsync(int id);
    }
}