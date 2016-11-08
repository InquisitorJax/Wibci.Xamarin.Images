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
                    double widthRatio = (double)request.Width / decoder.PixelWidth;
                    double heightRatio = (double)request.Height / decoder.PixelHeight;

                    double scaleRatio = Math.Min(widthRatio, heightRatio);

                    if (request.Width == 0)
                        scaleRatio = heightRatio;

                    if (request.Height == 0)
                        scaleRatio = widthRatio;

                    uint aspectHeight = (uint)Math.Floor(decoder.PixelHeight * scaleRatio);
                    uint aspectWidth = (uint)Math.Floor(decoder.PixelWidth * scaleRatio);

                    encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Linear;

                    encoder.BitmapTransform.ScaledHeight = aspectHeight;
                    encoder.BitmapTransform.ScaledWidth = aspectWidth;

                    await encoder.FlushAsync();
                    resizedStream.Seek(0);
                    var outBuffer = new byte[resizedStream.Size];
                    await resizedStream.ReadAsync(outBuffer.AsBuffer(), (uint)resizedStream.Size, InputStreamOptions.None);
                    return new ResizeImageResult { ResizedImage = outBuffer };
                }
            }

            return new ResizeImageResult { ResizedImage = request.OriginalImage, TaskResult = TaskResult.Canceled };
        }
    }
}