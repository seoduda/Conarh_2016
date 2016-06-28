using System;
using System.IO;
using System.Threading.Tasks;

namespace Conarh_2016.Android.Services
{
	public sealed class IOManager:Core.Services.IIOManager
	{
		private readonly string documentPath;

		public IOManager()
		{
			documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		public void CreateDirectory(string path)
		{
			Directory.CreateDirectory(path);
		}

		public void SaveFile(byte[] content, string path)
		{
			CheckDirectory(path);
			File.WriteAllBytes(path, content);
		}

		public Task SaveFileAsync(byte[] content, string path)
		{
			CheckDirectory(path);

			using (FileStream SourceStream = File.Open(path, FileMode.OpenOrCreate))
			{
				SourceStream.Seek(0, SeekOrigin.End);
				return SourceStream.WriteAsync(content, 0, content.Length);
			}
		}

		public void SaveFile(string content, string path)
		{
			CheckDirectory(path);
			File.WriteAllText(path, content);
		}

		public bool FileExists(string path)
		{
			return File.Exists(path);
		}

		private void CheckDirectory(string path )
		{
			string directory = Path.GetDirectoryName(path);
			if(!DirectoryExists(directory))
				CreateDirectory(directory);
		}

		public bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		public string DocumentPath
		{
			get { return documentPath; }
		}

		public void DeleteFile(string path)
		{
			if(File.Exists(path))
				File.Delete(path);
		}

		public void RenameFile(string oldPath, string newPath)
		{
			if (File.Exists(oldPath))
			{
				if (File.Exists(newPath))
					File.Delete(newPath);

				File.Move(oldPath, newPath);
			}
		}

		public Stream GetStream(string path)
		{
			return new FileStream(path, FileMode.Open);
		}

		public void CloseStream(Stream stream)
		{
			stream.Close();
		}

		public long GetFileSize(string path)
		{
			if (FileExists(path))
			{
				return new FileInfo(path).Length;
			}
			return 0;
		}

		public string GetFileContent(string path)
		{
			if (FileExists(path))
			{
				return File.ReadAllText(path);
			}

			return null;
		}

		public byte[] GetBytesFileContent(string path)
		{
			if (FileExists(path))
			{
				return File.ReadAllBytes(path);
			}

			return null;
		}
	}
}

