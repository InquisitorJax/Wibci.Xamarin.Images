using Android.Graphics;
using System.IO;
using static Android.Graphics.Bitmap;

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

		public static Bitmap CropToRoundedImage(this Bitmap bitmap)
		{
			//doc: https://stackoverflow.com/questions/11932805/cropping-circular-area-from-bitmap-in-android

			int widthLight = bitmap.Width;
			int heightLight = bitmap.Height;

			Bitmap output = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, Config.Argb8888);

			Canvas canvas = new Canvas(output);
			Paint paintColor = new Paint
			{
				Flags = PaintFlags.AntiAlias
			};

			RectF rectF = new RectF(new Rect(0, 0, widthLight, heightLight));

			canvas.DrawRoundRect(rectF, widthLight / 2, heightLight / 2, paintColor);

			Paint paintImage = new Paint();
			paintImage.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcAtop));
			canvas.DrawBitmap(bitmap, 0, 0, paintImage);

			return output;
		}

	}
}