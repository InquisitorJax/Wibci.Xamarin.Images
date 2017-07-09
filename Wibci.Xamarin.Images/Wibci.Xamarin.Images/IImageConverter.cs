using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images
{
    public enum ImageFormat
    {
        Png,
        Jpg
    }

    public interface IImageConverterCommand : IAsyncLogicCommand<ImageConvertContext, ConvertImageResult>
    {
    }

    public class ConvertImageResult : TaskCommandResult
    {
        public byte[] ConvertedImage { get; set; }
    }

    public class ImageConvertContext
    {
        public ImageConvertContext(byte[] image, ImageFormat format = ImageFormat.Png, int quality = 100)
        {
            Format = format;
            OriginalImage = image;
            Quality = quality;
        }

        public ImageFormat Format { get; set; }
        public byte[] OriginalImage { get; set; }
        public int Quality { get; set; }
    }
}