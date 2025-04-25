using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;
using Tesseract;

namespace Client.Core.Services.Extraction
{
    public class TextExtractionService : ITextExtractionService
    {
        // تبدیل تصویر به سیاه و سفید با استفاده از ImageSharp
        public Image<Rgba32> ConvertToBlackAndWhite(Image<Rgba32> image)
        {
            //image.Mutate(x => x.Grayscale()); // تبدیل تصویر به سیاه و سفید
            image.Mutate(x => x.Grayscale().Contrast(1.5f)); // تبدیل به سیاه و سفید و افزایش کنتراست
            return image;
        }

        // تبدیل فایل به Stream برای پردازش
        public async Task<Stream> ConvertImageToStream(IBrowserFile file)
        {
            // افزایش حد مجاز اندازه فایل (مثلاً 5MB)
            var maxAllowedSize = 5 * 1024 * 1024; // 5MB

            // باز کردن جریان فایل با اندازه حداکثر مجاز
            var stream = file.OpenReadStream(maxAllowedSize);
            return stream;
        }

        // پردازش تصویر با استفاده از Tesseract برای استخراج متن
        public async Task<string> ProcessImageWithTesseract(Stream imageStream)
        {
            try
            {
                // مسیر کامل به پوشه tessdata در پروژه Client.Core
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // مسیر به پوشه Client.Core و tessdata در آن
                string tessdataPath = Path.Combine(baseDirectory, "..", "..", "..", "..", "Client.Core", "tessdata");

                // راه‌اندازی Tesseract engine
                var ocr = new TesseractEngine(tessdataPath, "eng", EngineMode.Default);

                // تنظیمات اضافی Tesseract برای بهبود دقت
                ocr.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"); // فقط حروف و اعداد
                ocr.SetVariable("classify_bln_numeric_mode", "1");

                // تنظیم OEM (OCR Engine Mode) برای استفاده از مدل‌های LSTM
                ocr.SetVariable("tessedit_ocr_engine_mode", "3"); // OEM.LstmOnly (عدد 3 برای LSTM)

                // تنظیم PSM (Page Segmentation Mode) برای پردازش بهتر متن
                ocr.SetVariable("tessedit_pageseg_mode", "6"); // PSM 6 برای متن‌های ساده

                // تبدیل Stream به byte array
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    await imageStream.CopyToAsync(ms);  // استفاده از CopyToAsync به‌طور غیر همزمان
                    imageBytes = ms.ToArray();
                }

                // پردازش تصویر با استفاده از Tesseract
                using (var img = Pix.LoadFromMemory(imageBytes))
                {
                    var result = ocr.Process(img);
                    return result.GetText(); // استخراج متن از تصویر
                }
            }
            catch (Exception ex)
            {
                return $"Error occurred: {ex.Message}";
            }
        }

        // تبدیل Stream به byte array
        private async Task<byte[]> GetStreamBytes(Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms);  // استفاده از CopyToAsync به‌طور غیر همزمان
                return ms.ToArray();
            }
        }

        // متد اصلی برای استخراج متن از تصویر
        public async Task<string> ExtractTextFromPicture(IBrowserFile file)
        {
            try
            {
                // تبدیل فایل به Stream به‌طور غیر همزمان
                var stream = await ConvertImageToStream(file);

                // استفاده از ImageSharp برای پردازش تصویر
                using (var image = await Image.LoadAsync<Rgba32>(stream)) // استفاده از Image.LoadAsync<Rgba32>
                {
                    // پیش‌پردازش تصویر: تبدیل به سیاه و سفید
                    var binarizedImage = ConvertToBlackAndWhite(image);

                    // ذخیره تصویر باینری به حافظه
                    using (var ms = new MemoryStream())
                    {
                        await binarizedImage.SaveAsync(ms, new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder());
                        ms.Seek(0, SeekOrigin.Begin);

                        // پردازش تصویر با استفاده از Tesseract
                        return await ProcessImageWithTesseract(ms);
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error occurred: {ex.Message}";
            }
        }












        public async Task<string> ExtractTextWithAzureFromPicAsync(string imagePath)
        {
            throw new NotImplementedException();
        }






    }
}
