# Wibci.Xamarin.Images
Some handy xamarin cross platform (Android / iOS / UWP) commands for byte[] image manipulation.
See the Sample App for example usages.

- Available on NuGet: [![NuGet](https://img.shields.io/nuget/v/Wibci.Xamarin.Images.svg?label=NuGet)](https://www.nuget.org/packages/Wibci.Xamarin.Images/)

- ResizeImageCommand: resize an image with specific width / height. Maintains aspect ratio.
- AnalyseImageCommand: Get the height, width and size of a given image
- ConvertImageCommand: Convert images between jpeg and png formats
- ImageTools: Methods for all 3 above

[Read more about it here](http://inquisitorjax.blogspot.co.za/2017/07/mobile-cross-platform-image.html)

Sample code: Resize an image

```C#
var resizeImage = DependencyService.Get<IResizeImageCommand>();

var resizeResult = await resizeImage.ExecuteAsync(new ResizeImageContext { Height = 130, Width = 280, OriginalImage = pictureResult.Image });

if (resizeResult.TaskResult == TaskResult.Success)
{
  Model.Logo = resizeResult.ResizedImage;
}
```

Platform implementations:

Android:
`DependencyService.Register<IResizeImageCommand, AndroidResizeImageCommand>();`

iOS
`DependencyService.Register<IResizeImageCommand, iOSImageResizeCommand>();`

UWP:
`DependencyService.Register<IResizeImageCommand, UWPResizeImageCommand>();`

