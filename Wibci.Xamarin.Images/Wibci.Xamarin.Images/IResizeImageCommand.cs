using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images
{
    public interface IResizeImageCommand : IAsyncLogicCommand<ResizeImageContext, ResizeImageResult>
    {
    }

    public class ResizeImageContext
    {
        public ResizeImageContext()
        {
            Quality = 100;
        }

        public int Height { get; set; }
        public ImageFormat ImageFormat { get; set; }
        public byte[] OriginalImage { get; set; }
        public int Quality { get; set; }
        public bool UpScaleIfImageIsSmaller { get; set; }
        public int Width { get; set; }
    }

    public class ResizeImageResult : TaskCommandResult
    {
        public int ResizedHeight { get; set; }
        public byte[] ResizedImage { get; set; }
        public int ResizedWidth { get; set; }
    }
}