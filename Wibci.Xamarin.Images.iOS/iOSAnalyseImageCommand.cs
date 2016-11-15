using System;
using System.Threading.Tasks;
using UIKit;
using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images.iOS
{
    public class iOSAnalyseImageCommand : AsyncLogicCommand<AnalyseImageContext, AnalyseImageResult>, IAnalyseImageCommand
    {
        public override Task<AnalyseImageResult> ExecuteAsync(AnalyseImageContext request)
        {
            AnalyseImageResult retResult = new AnalyseImageResult();
            var imageData = request.Image;

            UIImage image;
            try
            {
                image = new UIImage(Foundation.NSData.FromArray(imageData));

                retResult.Height = (uint)image.Size.Height;
                retResult.Width = (uint)image.Size.Width;
                retResult.Orientaion = retResult.Height > retResult.Width ? ImageOrientation.Portrait : ImageOrientation.Landscape;

                image = null;
            }
            catch (Exception e)
            {
                retResult.Notification.Add("Analyse Image failed: " + e.Message);
                return Task.FromResult(retResult);
            }

            return Task.FromResult(retResult);
        }
    }
}