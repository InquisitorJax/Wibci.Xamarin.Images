using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace Wibci.Xamarin.Images.UWP
{
    public class UWPResizeImageCommand : AsyncLogicCommand<ResizeImageContext, ResizeImageResult>, IResizeImageCommand
    {
        public override async Task<ResizeImageResult> ExecuteAsync(ResizeImageContext request)
        {
            try
            {
                //http://stackoverflow.com/questions/36009021/uwp-how-to-resize-an-image
                var memStream = new MemoryStream(request.OriginalImage);

                IRandomAccessStream imageStream = memStream.AsRandomAccessStream();
                var decoder = await BitmapDecoder.CreateAsync(imageStream);
                if (decoder.PixelHeight > request.Height || decoder.PixelWidth > request.Width)
                {
                    using (imageStream)
                    {
                        var resizedStream = new InMemoryRandomAccessStream();

                        BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync(resizedStream, decoder);
                      
                        var originalDim = new Dimension { Height = (uint)decoder.PixelHeight, Width = (uint)decoder.PixelWidth };
                        var resizeRequestDim = new Dimension { Height = (uint)request.Height, Width = (uint)request.Width };
                        var newDimensions = ImageHelper.Resize(originalDim, resizeRequestDim, request.UpScaleIfImageIsSmaller);

                        encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Linear;

                        encoder.BitmapTransform.ScaledHeight = newDimensions.Height;
                        encoder.BitmapTransform.ScaledWidth = newDimensions.Width;

                        await encoder.FlushAsync();
                        resizedStream.Seek(0);
                        var outBuffer = new byte[resizedStream.Size];
                        await resizedStream.ReadAsync(outBuffer.AsBuffer(), (uint)resizedStream.Size, InputStreamOptions.None);

                        var result = new ResizeImageResult
                        {
                            ResizedImage = outBuffer,
                            ResizedHeight = (int)newDimensions.Height,
                            ResizedWidth = (int)newDimensions.Width,
                        };

                        return result;
                    }
                }

                return new ResizeImageResult { ResizedImage = request.OriginalImage, TaskResult = TaskResult.Canceled };
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return new ResizeImageResult
                {
                    TaskResult = TaskResult.Failed,
                    Notification = new Notification(ex.Message)
                };
#endif
            }
        }
    }
}