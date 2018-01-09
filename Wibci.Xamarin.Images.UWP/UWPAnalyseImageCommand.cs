using System;
using System.IO;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace Wibci.Xamarin.Images.UWP
{
    public class UWPAnalyseImageCommand : AsyncLogicCommand<AnalyseImageContext, AnalyseImageResult>, IAnalyseImageCommand
    {
        public override async Task<AnalyseImageResult> ExecuteAsync(AnalyseImageContext request)
        {
            AnalyseImageResult retResult = new AnalyseImageResult();
            try
            {
                var memStream = new MemoryStream(request.Image);

                IRandomAccessStream imageStream = memStream.AsRandomAccessStream();
                var decoder = await BitmapDecoder.CreateAsync(imageStream);

                retResult.Height = decoder.OrientedPixelHeight;
                retResult.Width = decoder.OrientedPixelWidth;
                retResult.SizeInKB = request.Image.SizeInKB();
                if (retResult.Width == retResult.Height)
                {
                    retResult.Orientaion = ImageOrientation.Square;
                }
                else
                {
                    retResult.Orientaion = retResult.Height > retResult.Width ? ImageOrientation.Portrait : ImageOrientation.Landscape;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#else
                retResult.Notification.Add("Analyse Image failed: " + ex.Message);
#endif
            }

            return retResult;
        }
    }
}