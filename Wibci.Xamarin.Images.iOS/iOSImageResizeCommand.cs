using System;
using System.Drawing;
using System.Threading.Tasks;
using UIKit;
using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images.iOS
{
    public class iOSImageResizeCommand : AsyncLogicCommand<ResizeImageContext, ResizeImageResult>, IResizeImageCommand
    {
        public override Task<ResizeImageResult> ExecuteAsync(ResizeImageContext request)
        {
            var retResult = new ResizeImageResult();

            var imageData = request.OriginalImage;
            var height = request.Height;
            var width = request.Width;

            try
            {
                // Load the bitmap
                if (imageData == null)
                {
                    retResult.TaskResult = TaskResult.Canceled;
                    return Task.FromResult(retResult);
                }
                //
                UIImage image;
                try
                {
                    image = new UIImage(Foundation.NSData.FromArray(imageData));
                }
                catch (Exception e)
                {
                    retResult.Notification.Add("Image load failed: " + e.Message);
                    retResult.TaskResult = TaskResult.Failed;
                    return Task.FromResult(retResult);
                }
                UIImage originalImage = image;

                //
                var originalHeight = originalImage.Size.Height;
                var originalWidth = originalImage.Size.Width;
                //
                nfloat resizeHeight = 0;
                nfloat resizeWidth = 0;
                //

                if (originalHeight > originalWidth) // Height used
                {
                    resizeHeight = height;
                    nfloat ratio = originalHeight / height;
                    resizeWidth = originalWidth / ratio;
                }
                else // Width used
                {
                    resizeWidth = width;
                    nfloat ratio = originalWidth / width;
                    resizeHeight = originalHeight / ratio;
                }
                //
                float resizeWidthEx = (float)resizeWidth;
                float resizeHeightEx = (float)resizeHeight;

                UIGraphics.BeginImageContext(new SizeF(resizeWidthEx, resizeHeightEx));
                originalImage.Draw(new RectangleF(0, 0, resizeWidthEx, resizeHeightEx));
                var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
                //
                var bytesImage = resizedImage.AsJPEG().ToArray();
                resizedImage.Dispose();
                var result = new ResizeImageResult
                {
                    ResizedImage = bytesImage,
                    ResizedHeight = (int)resizeHeightEx,
                    ResizedWidth = (int)resizeWidthEx
                };
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return Task.FromResult(new ResizeImageResult
                {
                    TaskResult = TaskResult.Failed,
                    Notification = new Notification(ex.Message)
                });
#endif
            }
        }
    }
}