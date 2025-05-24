using System.Collections.Generic;
using System.Threading.Tasks;
using BikeSystemAdminPanel.Models;

namespace BikeSystemAdminPanel.Database
{
    public interface IStationRepository
    {
        Task<List<Station>> GetAllStationsAsync();
        Task AddStationAsync(Station station);
        Task DeleteStationAsync(int id);
    }
}