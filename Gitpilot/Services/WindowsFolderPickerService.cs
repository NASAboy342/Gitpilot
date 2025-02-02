using CommunityToolkit.Maui.Storage;
using Gitpilot.Models;
using Gitpilot.Services.Interfaces;

namespace Gitpilot.Services
{
    public class WindowsFolderPickerService : IFolderPickerService
    {
        public async Task<FolderResult> PickFolderAsync()
        {
            var selectedFolder = await FolderPicker.Default.PickAsync();
            if (selectedFolder.IsSuccessful)
            {
                return new FolderResult
                {
                    FolderPath = selectedFolder.Folder.Path,
                    FolderName = selectedFolder.Folder.Name,
                    IsOpent = true
                };
            }
            return new FolderResult
            {
                FolderPath = "",
                FolderName = "",
                IsOpent = false
            };
        }
    }
}
