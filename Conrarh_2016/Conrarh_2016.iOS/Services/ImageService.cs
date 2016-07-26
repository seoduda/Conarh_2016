using System;
using Conarh_2016.Core.Services;
using System.IO;
using UIKit;
using CoreGraphics;
using System.Drawing;

namespace Conarh_2016.iOS.Services
{
	public class ImageServiceIOS: IImageService
	{
		public void CropAndResizeImage(string sourceFile, string targetFile, float size)
		{
			if (File.Exists (sourceFile) && !File.Exists (targetFile)) {

				using (UIImage sourceImage = UIImage.FromFile (sourceFile)) 
				{  
					UIImage newImage = ScaleAndCropImage (sourceImage, new SizeF (size, size));
					SaveImage (targetFile, newImage);
				}
			}
		}

		public UIImage ScaleAndCropImage(UIImage sourceImage, SizeF targetSize)
		{
			var imageSize = sourceImage.Size;
			UIImage newImage = null;
			var width = imageSize.Width;
			var height = imageSize.Height;
			var targetWidth = targetSize.Width;
			var targetHeight = targetSize.Height;
			var scaleFactor = 0.0f;
			var scaledWidth = targetWidth;
			var scaledHeight = targetHeight;
			var thumbnailPoint = PointF.Empty;
			if (imageSize != targetSize)
			{
				float widthFactor = (float)( targetWidth / width );
				float heightFactor = (float)(targetHeight / height);
				if (widthFactor > heightFactor)
				{
					scaleFactor = widthFactor;// scale to fit height
				}
				else
				{
					scaleFactor = heightFactor;// scale to fit width
				}
				scaledWidth = (float)(width * scaleFactor);
				scaledHeight = (float)(height * scaleFactor);
				// center the image
				if (widthFactor > heightFactor)
				{
					thumbnailPoint.Y = (targetHeight - scaledHeight) * 0.5f;
				}
				else
				{
					if (widthFactor < heightFactor)
					{
						thumbnailPoint.X = (targetWidth - scaledWidth) * 0.5f;
					}
				}
			}
			UIGraphics.BeginImageContextWithOptions(targetSize, false, 0.0f);
			var thumbnailRect = new RectangleF(thumbnailPoint, new SizeF(scaledWidth, scaledHeight));
			sourceImage.Draw(thumbnailRect);
			newImage = UIGraphics.GetImageFromCurrentImageContext();
			if (newImage == null)
			{
				Console.WriteLine("could not scale image");
			}
			//pop the context to get back to the default
			UIGraphics.EndImageContext();

			return newImage;
		}

		private void SaveImage(string targetFile, UIImage resultImage)
		{
			if (!Directory.Exists(Path.GetDirectoryName(targetFile)))
				Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

			resultImage.AsJPEG(0.75f).Save(targetFile, true);
		}
	}
}
