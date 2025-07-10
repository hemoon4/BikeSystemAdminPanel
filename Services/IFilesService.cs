using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace BikeSystemAdminPanel.Services;

public interface IFilesService
{
    public Task<IStorageFile?> SaveFileAsync();
}