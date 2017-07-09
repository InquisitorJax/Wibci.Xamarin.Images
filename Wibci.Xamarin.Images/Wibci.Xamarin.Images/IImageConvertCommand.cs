using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images
{
    public enum ImageFormat
    {
        Png,
        Jpeg
    }

    public interface IConvertImageCommand : IAsyncLogicCommand<ConvertImageContext, ConvertImageResult>
    {
    }

    public class ConvertImageContext
    {
        public ConvertImageContext(byte[] image, ImageFormat format = ImageFormat.Png, int quality = 100)
        {
            Format = format;
            OriginalImage = image;
            Quality = quality;
        }

        public ImageFormat Format { get; set; }
        public byte[] OriginalImage { get; set; }
        public int Quality { get; set; }
    }

    public class ConvertImageResult : TaskCommandResult
    {
        public byte[] ConvertedImage { get; set; }
    }
}