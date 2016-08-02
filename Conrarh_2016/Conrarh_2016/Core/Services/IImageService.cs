namespace Conarh_2016.Core.Services
{
	public interface IImageCache
	{
		void Clear();
	}

	public interface IImageService
	{
		void CropAndResizeImage(string sourceFile, string targetFile, float size);
        //void SelectImage(string filename, bool allowCamera, int size);
	}
}

