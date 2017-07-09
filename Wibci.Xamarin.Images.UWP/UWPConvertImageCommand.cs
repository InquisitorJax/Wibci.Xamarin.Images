using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace Wibci.Xamarin.Images.UWP
{
    public class UWPConvertImageCommand : AsyncLogicCommand<ConvertImageContext, ConvertImageResult>, IConvertImageCommand
    {
        public override async Task<ConvertImageResult> ExecuteAsync(ConvertImageContext request)
        {
            try
            {

               

                //var memStream = new MemoryStream(request.OriginalImage);
                byte[] retConvertedImage;

                using (IRandomAccessStream imageStream = new InMemoryRandomAccessStream())
                {
                    byte[] detachedPixelData;

                    BitmapDecoder decoder;
                    using (var stream = new InMemoryRandomAccessStream())
                    {
                        await stream.WriteAsync(request.OriginalImage.AsBuffer());
                        decoder = await BitmapDecoder.CreateAsync(stream);
                        var pixelData = await decoder.GetPixelDataAsync();
                        detachedPixelData = pixelData.DetachPixelData();
                    }

                    Guid formatId = request.Format == ImageFormat.Png ? BitmapEncoder.PngEncoderId : BitmapEncoder.JpegEncoderId;

                    BitmapEncoder encoder;
                    if (request.Format == ImageFormat.Png)
                    {
                        encoder = await BitmapEncoder.CreateAsync(formatId, imageStream);

                    }
                    else
                    {
                        BitmapPropertySet propertySet = new BitmapPropertySet();
                        var qualityValue = new BitmapTypedValue(request.Quality * .01, Windows.Foundation.PropertyType.Single);
                        propertySet.Add("ImageQuality", qualityValue);
                       
                        encoder = await BitmapEncoder.CreateAsync(formatId, imageStream, propertySet); 
                    }
                    encoder.SetPixelData(decoder.BitmapPixelFormat, decoder.BitmapAlphaMode, decoder.OrientedPixelWidth, decoder.OrientedPixelHeight, decoder.DpiX, decoder.DpiY, detachedPixelData);

                    await encoder.FlushAsync();
                    retConvertedImage = new byte[imageStream.AsStream().Length];
                    await imageStream.AsStream().ReadAsync(retConvertedImage, 0, retConvertedImage.Length);

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