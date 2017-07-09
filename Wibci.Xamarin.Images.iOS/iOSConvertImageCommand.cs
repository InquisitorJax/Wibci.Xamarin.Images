using System;
using System.Threading.Tasks;
using UIKit;
using Wibci.LogicCommand;

namespace Wibci.Xamarin.Images.iOS
{
    public class iOSConvertImageCommand : AsyncLogicCommand<ImageConvertContext, ConvertImageResult>, IImageConverterCommand
    {
        public override Task<ConvertImageResult> ExecuteAsync(ImageConvertContext request)
        {
            try
            {
                var retResult = new ConvertImageResult();

                if (request.OriginalImage != null && request.OriginalImage.Length > 0)
                {
                    UIImage uiImage = new UIImage(Foundation.NSData.FromArray(request.OriginalImage));
                    if (request.Format == ImageFormat.Png)
                    {
                        retResult.ConvertedImage = uiImage.AsPNG().ToArray();
                    }
                    else
                    {
                        nfloat fQaulity = request.Quality / 100;
                        retResult.ConvertedImage = uiImage.AsJPEG(fQaulity).ToArray();
                    }
                }
                else
                {
                    retResult.TaskResult = TaskResult.Canceled;
                    retResult.Notification.Add("Original Image not specified");
                }

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