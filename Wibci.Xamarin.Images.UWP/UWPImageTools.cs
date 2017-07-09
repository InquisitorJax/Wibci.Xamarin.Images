using System.Threading.Tasks;

namespace Wibci.Xamarin.Images.UWP
{
    public class UWPImageTools : IImageTools
    {
        public async Task<ConvertImageResult> ConvertImageAsync(ConvertImageContext context)
        {
            var convertCommand = new UWPConvertImageCommand();
            return await convertCommand.ExecuteAsync(context);
        }

        public async Task<ResizeImageResult> ResizeImageAsync(ResizeImageContext context)
        {
            var resizeCommand = new UWPResizeImageCommand();
            return await resizeCommand.ExecuteAsync(context);
        }
    }
}