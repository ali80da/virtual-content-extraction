using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace Client.Shared.Components.Receives;

public partial class ReceiveContent
{

    private IBrowserFile selectedFile;
    private string extractedText;
    private bool isLoading = false;  // متغیر برای کنترل وضعیت لودینگ
    private bool isError = false;  // متغیر برای کنترل وضعیت خطا


    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }

    private async Task UploadFiles(IEnumerable<IBrowserFile> files)
    {
        foreach (var file in files)
        {
            try
            {
                selectedFile = file;
                isLoading = true; // فعال کردن لودینگ
                isError = false; // غیرفعال کردن خطا

                // ارسال به سرویس OCR برای استخراج متن
                extractedText = await TextExtractionService.ExtractTextFromPicture(file);

                Snackbar.Add($"File {file.Name} has been successfully selected and processed.", Severity.Success);
            }
            catch (Exception ex)
            {
                isError = true; // فعال کردن خطا
                Snackbar.Add($"An error occurred: {ex.Message}", Severity.Error);
            }
            finally
            {
                await Task.Delay(5000);
                isLoading = false; // غیرفعال کردن لودینگ پس از پردازش
            }
        }

        StateHasChanged(); // بروزرسانی UI
    }

    private async Task DownloadTextFile()
    {
        // ایجاد فایل متنی با استفاده از متن استخراج‌شده
        var fileName = "extracted-text.txt";
        var content = extractedText;

        // دانلود فایل از طریق JavaScript
        await JSRuntime.InvokeVoidAsync("downloadFile", fileName, content);
    }










}