using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Client.Core.Services.Extraction;

public interface ITextExtractionService
{

    Image<Rgba32> ConvertToBlackAndWhite(Image<Rgba32> image);
    Task<Stream> ConvertImageToStream(IBrowserFile file);
    //Bitmap ConvertToBlackAndWhite(Bitmap image);
    Task<string> ProcessImageWithTesseract(Stream imageStream);
    Task<string> ExtractTextFromPicture(IBrowserFile file);

}