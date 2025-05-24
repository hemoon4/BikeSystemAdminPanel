using System.Collections.Generic;
using System.Threading.Tasks;
using BikeSystemAdminPanel.Models;

namespace BikeSystemAdminPanel.Database
{
    public interface IBicycleRepository
    {
        Task<List<Bicycle>> GetAllBicyclesAsync();
        Task AddBicycleAsync(Bicycle bicycle);
        Task DeleteBicycleAsync(int id);
    }
}