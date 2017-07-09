using System.Threading.Tasks;

namespace Wibci.Xamarin.Images.Droid
{
    public class AndroidImageTools : IImageTools
    {
        public async Task<ConvertImageResult> ConvertImageAsync(ConvertImageContext context)
        {
            var convertCommand = new AndroidConvertImageCommand();
            return await convertCommand.ExecuteAsync(context);
        }

        public async Task<ResizeImageResult> ResizeImageAsync(ResizeImageContext context)
        {
            var resizeCommand = new AndroidResizeImageCommand();
            return await resizeCommand.ExecuteAsync(context);
        }
    }
}