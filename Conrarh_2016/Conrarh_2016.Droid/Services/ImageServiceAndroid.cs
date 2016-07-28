using Android.Graphics;
using Conarh_2016.Core.Services;
using System;
using System.Drawing;
using System.IO;

namespace Conarh_2016.Android.Services
{
    public class ImageServiceAndroid : IImageService
    {
        public void CropAndResizeImage(string sourceFile, string targetFile, float size)
        {
            System.Threading.Thread.Sleep(500);

            if (File.Exists(sourceFile) && !File.Exists(targetFile))
            {
                //AppProvider.PopUpFactory.ShowMessage(sourceFile, "file verifi");

                // First decode with inJustDecodeBounds=true to check dimensions
                var options = new BitmapFactory.Options()
                {
                    InJustDecodeBounds = false,
                    InPurgeable = true,
                };

                using (var sourceImage = BitmapFactory.DecodeFile(sourceFile, options))
                {
                    Bitmap newImage = ScaleImage(sourceImage, new SizeF(size, size));
                    SaveImage(targetFile, newImage);
                }
            }
        }

        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);

            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }

        private Bitmap ScaleAndCropImage(Bitmap sourceImage, SizeF targetSize)
        {
            var width = sourceImage.GetBitmapInfo().Width;
            var height = sourceImage.GetBitmapInfo().Height;

            var targetWidth = (int)targetSize.Width;
            var targetHeight = (int)targetSize.Height;

            var scaleFactor = 0.0f;
            float scaledWidth = targetWidth;
            float scaledHeight = targetHeight;

            if (width != targetWidth)
            {
                float widthFactor = targetWidth * 1.0f / width;
                float heightFactor = targetHeight * 1.0f / height;
                if (widthFactor > heightFactor)
                {
                    scaleFactor = widthFactor;// scale to fit height
                }
                else
                {
                    scaleFactor = heightFactor;// scale to fit width
                }
                scaledWidth = width * scaleFactor;
                scaledHeight = height * scaleFactor;
            }

            using (var scaledBitmap = Bitmap.CreateScaledBitmap(sourceImage, (int)scaledWidth, (int)scaledHeight, true))
            {
                int size = Math.Min(scaledBitmap.Width, scaledBitmap.Height);
                var image = Bitmap.CreateBitmap(scaledBitmap, 0, 0, size, size);
                scaledBitmap.Recycle();
                return image;
            }
        }

        private Bitmap ScaleImage(Bitmap sourceImage, SizeF targetSize)
        {
            var width = sourceImage.GetBitmapInfo().Width;
            var height = sourceImage.GetBitmapInfo().Height;

            var targetWidth = (int)targetSize.Width;
            var targetHeight = (int)targetSize.Height;

            var scaleFactor = 0.0f;
            float scaledWidth = targetWidth;
            float scaledHeight = targetHeight;

            if (width != targetWidth)
            {
                float widthFactor = targetWidth * 1.0f / width;
                float heightFactor = targetHeight * 1.0f / height;
                //if (widthFactor > heightFactor)
                /* TODO - Mudei para ficar proporcional*/
                if (widthFactor < heightFactor)
                {
                    scaleFactor = widthFactor;// scale to fit height
                }
                else
                {
                    scaleFactor = heightFactor;// scale to fit width
                }
                scaledWidth = width * scaleFactor;
                scaledHeight = height * scaleFactor;
            }

            using (var scaledBitmap = Bitmap.CreateScaledBitmap(sourceImage, (int)scaledWidth, (int)scaledHeight, true))
            {   /* TODO - Mudei para ficar proporcional*/
                int size = Math.Min(scaledBitmap.Width, scaledBitmap.Height);

                var image = Bitmap.CreateBitmap(scaledBitmap, 0, 0, scaledBitmap.Width, scaledBitmap.Height);
                scaledBitmap.Recycle();
                return image;
            }
        }

        private void SaveImage(string targetFile, Bitmap resultImage)
        {
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(targetFile)))
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(targetFile));
            //  int bc = resultImage.ByteCount;

            using (resultImage)
            {
                using (Stream outStream = File.Create(targetFile))
                {
                    resultImage.Compress(Bitmap.CompressFormat.Jpeg, 90, outStream);
                    /*
                    if (targetFile.ToLower().EndsWith("png"))
                    {
                        resultImage.Compress(Bitmap.CompressFormat.Png, 100, outStream);
                    }
                    else
                        resultImage.Compress(Bitmap.CompressFormat.Jpeg, 95, outStream);
                        */
                    resultImage.Recycle();
                }
            }
        }

        public void ResizeImage(string sourceFile, string targetFile, float maxWidth, float maxHeight)
        {
            if (!File.Exists(targetFile) && File.Exists(sourceFile))
            {
                // First decode with inJustDecodeBounds=true to check dimensions
                var options = new BitmapFactory.Options()
                {
                    InJustDecodeBounds = false,
                    InPurgeable = true,
                };

                using (var image = BitmapFactory.DecodeFile(sourceFile, options))
                {
                    if (image != null)
                    {
                        var sourceSize = new Size((int)image.GetBitmapInfo().Height, (int)image.GetBitmapInfo().Width);

                        var maxResizeFactor = Math.Min(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

                        string targetDir = System.IO.Path.GetDirectoryName(targetFile);
                        if (!Directory.Exists(targetDir))
                            Directory.CreateDirectory(targetDir);

                        if (maxResizeFactor > 0.9)
                        {
                            File.Copy(sourceFile, targetFile);
                        }
                        else
                        {
                            var width = (int)(maxResizeFactor * sourceSize.Width);
                            var height = (int)(maxResizeFactor * sourceSize.Height);

                            using (var bitmapScaled = Bitmap.CreateScaledBitmap(image, height, width, true))
                            {
                                bitmapScaled.Recycle();
                            }
                        }

                        image.Recycle();
                    }
                }
            }
        }
    }
}