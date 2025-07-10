using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Collections.Generic;

namespace BikeSystemAdminPanel.Services;

public class FilesService : IFilesService
{
    private readonly Window _target;

    public FilesService(Window target)
    {
        _target = target;
    }

    public async Task<IStorageFile?> SaveFileAsync()
    {
        return await _target.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = "Save monthly report",
            SuggestedFileName="rentals_report",
            DefaultExtension = ".csv",
            ShowOverwritePrompt = true,
            FileTypeChoices = new List<FilePickerFileType>
            {
                new FilePickerFileType("CSV Files") { Patterns = new[] { "*.csv" } }
            }
        });
    }
}