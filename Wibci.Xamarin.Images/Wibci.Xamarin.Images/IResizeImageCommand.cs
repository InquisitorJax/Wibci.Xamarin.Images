using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images
{
    public interface IResizeImageCommand : IAsyncLogicCommand<ResizeImageContext, ResizeImageResult>
    {
    }

    public class ResizeImageContext
    {
        public int Height { get; set; }
        public byte[] OriginalImage { get; set; }
        public int Width { get; set; }
    }

    public class ResizeImageResult : DeviceCommandResult
    {
        public byte[] ResizedImage { get; set; }
    }
}