using Android.Graphics;
using System;
using System.Threading.Tasks;
using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images.Droid
{
    public class AndroidConvertImageCommand : AsyncLogicCommand<ImageConvertContext, ConvertImageResult>, IImageConverterCommand
    {
        public override Task<ConvertImageResult> ExecuteAsync(ImageConvertContext request)
        {
            try
            {
                ConvertImageResult retResult = new ConvertImageResult();

                Bitmap resultBitmap = BitmapFactory.DecodeByteArray(request.OriginalImage, 0, request.OriginalImage.Length);

                retResult.ConvertedImage = resultBitmap.ToByteArray(request.Format, request.Quality);

                resultBitmap.Recycle();
                GC.Collect();

                return Task.FromResult(retResult);
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