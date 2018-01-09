using Android.Graphics;
using System.IO;

namespace Wibci.Xamarin.Images.Droid
{
    public static class AndroidImageHelper
    {
        public static byte[] ToByteArray(this Bitmap image, ImageFormat format, int quality)
        {
            byte[] retImage = null;
            using (MemoryStream outStream = new MemoryStream())
            {
                Bitmap.CompressFormat compressFormat = format == ImageFormat.Jpeg ? Bitmap.CompressFormat.Jpeg : Bitmap.CompressFormat.Png;
                image.Compress(compressFormat, quality, outStream);
                retImage = outStream.ToArray();
            };
            return retImage;
        }
    }
}