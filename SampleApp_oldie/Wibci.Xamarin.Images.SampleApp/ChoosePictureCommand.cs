using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Threading.Tasks;
using Wibci.LogicCommand;
using DS = Xamarin.Forms.DependencyService;

namespace Wibci.Xamarin.Images.SampleApp
{
    public interface IChoosePictureCommand : IAsyncLogicCommand<ChoosePictureRequest, SelectPictureResult>
    {
    }

    public class ChoosePictureCommand : AsyncLogicCommand<ChoosePictureRequest, SelectPictureResult>, IChoosePictureCommand
    {
        private IMedia MediaPicker
        {
            get { return CrossMedia.Current; }
        }

        public override async Task<SelectPictureResult> ExecuteAsync(ChoosePictureRequest request)
        {
            var retResult = new SelectPictureResult();

            //NOTE: send suspend event BEFORE page_disappearing event fires to page is not removed from the view stack
            //...   resume will be called by generic life-cycle

            if (!MediaPicker.IsPickPhotoSupported)
            {
                retResult.Notification.Add("No camera available :(");
                retResult.TaskResult = TaskResult.Failed;
                return retResult;
            }

            var options = new PickMediaOptions();
            options.CompressionQuality = 100;

            var mediaFile = await MediaPicker.PickPhotoAsync(options);

            if (mediaFile != null)
            {
                byte[] image = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    mediaFile.GetStream().CopyTo(ms);
                    image = ms.ToArray();
                }

                if (request.MaxPixelDimension.HasValue)
                {
                    IResizeImageCommand resizeCommand = DS.Get<IResizeImageCommand>();

                    ResizeImageContext context = new ResizeImageContext
                    {
                        Height = request.MaxPixelDimension.Value,
                        Width = request.MaxPixelDimension.Value,
                        OriginalImage = image,
                        UpScaleIfImageIsSmaller = request.UpscaleImageIfSmaller
                    };
                    var resizeResult = await resizeCommand.ExecuteAsync(context);
                    if (resizeResult.IsValid())
                    {
                        image = resizeResult.ResizedImage;
                    }
                }

                retResult.Image = image;

                retResult.TaskResult = TaskResult.Success;
            }
            else
            {
                retResult.TaskResult = TaskResult.Canceled;
                retResult.Notification.Add("Select picture canceled");
            }

            return retResult;
        }
    }

    public class ChoosePictureRequest
    {
        public int? MaxPixelDimension { get; set; }
        public bool UpscaleImageIfSmaller { get; set; }
    }

    public class SelectPictureResult : TaskCommandResult
    {
        public byte[] Image { get; set; }
    }
}