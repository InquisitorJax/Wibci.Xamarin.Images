using System.Threading.Tasks;

namespace Wibci.Xamarin.Images
{
    public interface IImageTools
    {
        Task<ConvertImageResult> ConvertImageAsync(ConvertImageContext context);

        Task<ResizeImageResult> ResizeImageAsync(ResizeImageContext context);
    }
}