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

            if (originalHeight > originalWidth) // Höhe (71 für Avatar) ist Master
            {
                resizeHeight = height;
                nfloat teiler = originalHeight / height;
                resizeWidth = originalWidth / teiler;
            }
            else // Breite (61 for Avatar) ist Master
            {
                resizeWidth = width;
                nfloat teiler = originalWidth / width;
                resizeHeight = originalHeight / teiler;
            }
            //
            float resizeWidthEx = (float)resizeWidth;
            float resizeHeightEx = (float)resizeHeight;

            UIGraphics.BeginImageContext(new SizeF(resizeWidthEx, resizeHeightEx));
            originalImage.Draw(new RectangleF(0, 0, resizeWidthEx, resizeHeightEx));
            var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            //
            var bytesImagen = resizedImage.AsJPEG().ToArray();
            resizedImage.Dispose();
            return Task.FromResult(new ResizeImageResult { ResizedImage = bytesImagen });
        }
    }
}