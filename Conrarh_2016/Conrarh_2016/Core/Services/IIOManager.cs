using System.IO;
using System.Threading.Tasks;

namespace Conarh_2016.Core.Services
{
    public interface IIOManager
    {
        void CreateDirectory(string path);

        void SaveFile(byte[] content, string path);

        void SaveFile(string content, string path);

        void DeleteFile(string path);

        void RenameFile(string oldPath, string newPath);

        Task SaveFileAsync(byte[] content, string path);

        bool FileExists(string path);

        bool DirectoryExists(string path);

        string DocumentPath { get; }

        long GetFileSize(string path);

        string GetFileContent(string path);

        byte[] GetBytesFileContent(string path);

        Stream GetStream(string path);

        void CloseStream(Stream stream);
    }
}