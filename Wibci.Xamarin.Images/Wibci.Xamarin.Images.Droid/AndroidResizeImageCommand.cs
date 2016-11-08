using Android.Graphics;
using System.IO;
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

            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            //
            float resizeHeight = 0;
            float resizeWidth = 0;
            //
            var originalHeight = originalImage.Height;
            var originalWidth = originalImage.Width;
            //
            if (originalHeight > originalWidth) // Height is preferred to keep aspect
            {
                resizeHeight = height;
                float teiler = originalHeight / height;
                resizeWidth = originalWidth / teiler;
            }
            else // Width is preferred to keep aspect
            {
                resizeWidth = width;
                float teiler = originalWidth / width;
                resizeHeight = originalHeight / teiler;
            }
            //
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)resizeWidth, (int)resizeHeight, false);
            //
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return Task.FromResult(new ResizeImageResult { ResizedImage = ms.ToArray() });
            }
        }
    }
}