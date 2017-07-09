using Android.Graphics;
using System;
using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images.Droid
{
    public class AndroidResizeImageCommand : AsyncLogicCommand<ResizeImageContext, ResizeImageResult>, IResizeImageCommand
    {
        public override Task<ResizeImageResult> ExecuteAsync(ResizeImageContext request)
        {
            //see: https://forums.xamarin.com/discussion/37681/how-to-resize-an-image-in-xamarin-forms-ios-android-and-wp

            var imageData = request.OriginalImage;
            var height = request.Height;
            var width = request.Width;

            try
            {
                // Load the bitmap
                Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

                var originalDim = new Dimension { Height = (uint)originalImage.Height, Width = (uint)originalImage.Width };
                var resizeRequestDim = new Dimension { Height = (uint)request.Height, Width = (uint)request.Width };
                var newDimensions = ImageHelper.Resize(originalDim, resizeRequestDim, request.UpScaleIfImageIsSmaller);

                Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newDimensions.Width, (int)newDimensions.Height, false);

                var image = resizedImage.ToByteArray(request.ImageFormat, request.Quality);

                resizedImage.Recycle();
                GC.Collect();

                var result = new ResizeImageResult
                {
                    ResizedImage = image,
                    ResizedHeight = (int)newDimensions.Height,
                    ResizedWidth = (int)newDimensions.Width
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