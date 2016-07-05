using Conarh_2016.Application.Domain.PostData;
using System.Threading.Tasks;

namespace Conarh_2016.Core.Services
{
    public interface ILinkedinLogin
    {
        //void RenameFile(string oldPath, string newPath);

        void createUserLinkedin();
        void openPage();
        

    }
}
