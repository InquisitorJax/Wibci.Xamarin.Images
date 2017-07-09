using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace Wibci.Xamarin.Images.UWP
{
    public class UWPConvertImageCommand : AsyncLogicCommand<ImageConvertContext, ConvertImageResult>, IImageConverterCommand
    {
        public override async Task<ConvertImageResult> ExecuteAsync(ImageConvertContext request)
        {
            try
            {

        
                var memStream = new MemoryStream(request.OriginalImage);
                byte[] retConvertedImage;

                using (IRandomAccessStream imageStream = memStream.AsRandomAccessStream())
                {
                    var decoder = await BitmapDecoder.CreateAsync(imageStream);
                    var encodingStream = new InMemoryRandomAccessStream();
                    Guid encoderId = request.Format == ImageFormat.Png ? BitmapEncoder.PngEncoderId : BitmapEncoder.JpegEncoderId;
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderId, imageStream);
                    retConvertedImage = new byte[imageStream.Size];
                    await imageStream.ReadAsync(retConvertedImage.AsBuffer(), (uint)imageStream.Size, InputStreamOptions.None);

                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                           BitmapAlphaMode.Ignore,
                           (uint)decoder.PixelWidth,
                           (uint)decoder.PixelHeight,
                           96,
                           96,
                           retConvertedImage);

                    await encoder.FlushAsync();
               
                }

                return new ConvertImageResult { ConvertedImage = retConvertedImage };
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