using Android.Graphics;
using Java.Lang;
using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images.Droid
{
    public class AndroidAnalyseImageCommand : AsyncLogicCommand<AnalyseImageContext, AnalyseImageResult>, IAnalyseImageCommand
    {
        public override Task<AnalyseImageResult> ExecuteAsync(AnalyseImageContext request)
        {
            AnalyseImageResult retResult = new AnalyseImageResult();
            var imageData = request.Image;

            try
            {
                Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
                retResult.Height = (uint)originalImage.Height;
                retResult.Width = (uint)originalImage.Width;

                if (originalImage.Width == originalImage.Height)
                {
                    retResult.Orientaion = ImageOrientation.Square;
                }
                else
                {
                    retResult.Orientaion = originalImage.Height > originalImage.Width ? ImageOrientation.Portrait : ImageOrientation.Landscape;
                }

                originalImage.Recycle();
                originalImage = null;
            }
            catch (Exception ex)
            {
                retResult.Notification.Add("Analyse Image failed: " + ex.Message);
            }

            return Task.FromResult(retResult);
        }
    }
}