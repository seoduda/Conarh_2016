/*
 * Copyright (C) 2013 @JamesMontemagno http://www.montemagno.com http://www.refractored.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * Ported from Xamarin Sample App
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Java.IO;
using File = Java.IO.File;

namespace MonoDroidToolkit.ImageLoader
{

	public class ImageLoader
	{
		public static void CopyStream (Stream inputStream, OutputStream os)
		{
			int buffer_size = 1024;
			try {
				byte[] bytes = new byte[buffer_size];
				for (;;) {
					int count = inputStream.Read (bytes, 0, buffer_size);
					if (count <= 0)
						break;
					os.Write (bytes, 0, count);
				}
			} catch {
			}
		}

		private static Bitmap DecodeFile (string url, int requiredSize)
		{
			try {
				var bitmap = BitmapFactory.DecodeFile(url);

				//Find the correct scale value. It should be the power of 2.
				int tempWidth = bitmap.Width;
				int tempHeight = bitmap.Height;

				var scale = bitmap.Width / requiredSize;

				//decode with inSampleSize
				BitmapFactory.Options options2 = new BitmapFactory.Options { InSampleSize = scale,
					InScaled = true};

				bitmap = BitmapFactory.DecodeFile (url, options2);

				return bitmap;
			} 
			catch
			{
			}

			return null;
		}

		internal class PhotoToLoad
		{
			public String Url;
			public ImageView ImageView;
			public int Size;

			public PhotoToLoad (String url, ImageView imageView, int size)
			{
				Url = url;
				Size = size;
				ImageView = imageView;
			}
		}

		private MemoryCache m_MemoryCache = new MemoryCache ();

		private IDictionary<ImageView, String> m_ImageViews = new ConcurrentDictionary<ImageView, String> ();
		private int m_StubID = -1;

		private readonly int m_MaxImages = 10;

		public ImageLoader (Context context)
		{
		}

		public void DisplayImage (string url, ImageView imageView, int defaultResourceId, int size)
		{
			m_StubID = defaultResourceId;
			if (m_ImageViews.ContainsKey (imageView)) {
				m_ImageViews.Remove (imageView);
			}

			m_MemoryCache.PopCache (m_MaxImages);

			m_ImageViews.Add (imageView, url);


			var bitmap = m_MemoryCache.Get (url);
			if (bitmap != null) {
				var activity = (Activity)imageView.Context;
				activity.RunOnUiThread (() => imageView.SetImageBitmap (bitmap));
			} else {
				QueueImage (url, imageView, size);
			}
		}

		public void QueueImage (string url, ImageView imageView, int size)
		{
			var photoToUpload = new PhotoToLoad (url, imageView, size);
			LoadPhoto (photoToUpload);
		}

		private Bitmap GetBitmap (string url, int size)
		{
			return DecodeFile(url, size);
		}

		internal bool ImageViewReused (PhotoToLoad photoToLoad)
		{
			try {
				if (!m_ImageViews.ContainsKey (photoToLoad.ImageView))
					return true;

				if (!m_ImageViews [photoToLoad.ImageView].Equals (photoToLoad.Url))
					return true;
			} catch (Exception) {
			}

			return false;
		}

		public void LoadPhoto (object param)
		{
			var photoToLoad = param as PhotoToLoad;
			if (photoToLoad == null)
				return;

			if (ImageViewReused (photoToLoad))
				return;

			Bitmap bitmap = GetBitmap (photoToLoad.Url, photoToLoad.Size);
			m_MemoryCache.Put (photoToLoad.Url, bitmap);
			if (ImageViewReused (photoToLoad))
				return;

			BitmapDisplayer (bitmap, photoToLoad);
		}

		internal void BitmapDisplayer (Bitmap bitmap, PhotoToLoad photoToLoad)
		{
			var activity = (Activity)photoToLoad.ImageView.Context;
			activity.RunOnUiThread (() => {
				if (ImageViewReused (photoToLoad))
					return;
				photoToLoad.ImageView.Visibility = Android.Views.ViewStates.Visible;
				if (bitmap != null)
					photoToLoad.ImageView.SetImageBitmap (bitmap);
				else if (m_StubID != -1)
					photoToLoad.ImageView.SetImageResource (m_StubID);
			});
		}

		public void ClearCache ()
		{
			m_MemoryCache.Clear ();
			m_ImageViews.Clear ();
		}
	}
}