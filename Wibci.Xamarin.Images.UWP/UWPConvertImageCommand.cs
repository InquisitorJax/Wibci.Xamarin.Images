using System;
using System.IO;
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
            var memStream = new MemoryStream(request.OriginalImage);

            using (IRandomAccessStream imageStream = memStream.AsRandomAccessStream())
            {
                var decoder = await BitmapDecoder.CreateAsync(imageStream);
                var encodingStream = new InMemoryRandomAccessStream();
                Guid encoderId = request.Format == ImageFormat.Png ? BitmapEncoder.PngEncoderId : BitmapEncoder.JpegEncoderId;
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderId, imageStream);
            }

            return await bitmapImage.ToByteArrayAsync(ImageFormat.PNG);
        }
    }
}