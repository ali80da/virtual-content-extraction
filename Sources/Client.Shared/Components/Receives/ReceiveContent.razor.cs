/*using Microsoft.AspNetCore.Components.Forms;

namespace Client.Shared.Components.Receives;

public partial class ReceiveContent
{

    private IBrowserFile selectedFile;

    private async Task UploadFiles(IEnumerable<IBrowserFile> files)
    {
        foreach (var file in files)
        {
            try
            {
                selectedFile = file;

                // نمایش پیام موفقیت در Snackbar
                Snackbar.Add($"File {file.Name} has been successfully selected.", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
            }
        }

        StateHasChanged(); // برای بروزرسانی UI بعد از انتخاب فایل
    }
}*/