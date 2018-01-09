using System.Threading.Tasks;

namespace Wibci.Xamarin.Images.iOS
{
    public class iOSImageTools : IImageTools
    {
        public async Task<ConvertImageResult> ConvertImageAsync(ConvertImageContext context)
        {
            var convertCommand = new iOSConvertImageCommand();
            return await convertCommand.ExecuteAsync(context);
        }

        public async Task<ResizeImageResult> ResizeImageAsync(ResizeImageContext context)
        {
            var resizeCommand = new iOSImageResizeCommand();
            return await resizeCommand.ExecuteAsync(context);
        }
    }
}