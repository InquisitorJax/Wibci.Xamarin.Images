using System.Threading.Tasks;

namespace Wibci.Xamarin.Images
{
    public interface IImageTools
    {
        Task<ConvertImageResult> ConvertImageAsync(ImageConvertContext context);

        Task<ResizeImageResult> ResizeImageAsync(ResizeImageContext context);
    }
}