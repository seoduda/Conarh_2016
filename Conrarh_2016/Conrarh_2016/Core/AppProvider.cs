using Conarh_2016.Core.Services;
using Conarh_2016.Core.UI;
using TwinTechs.Controls;
using XLabs.Platform.Services.Media;

namespace Conarh_2016.Core
{
    public static class AppProvider
    {
        public static ILog Log;

        public static IIOManager IOManager;

        public static IPopUpFactory PopUpFactory;

        public static IMediaPicker MediaPicker;

        public static IIOManager Manager;

        public static IScreenOptions Screen;

        public static IImageService ImageService;

        public static ShareService ShareService;

        public static IImageCache ImageCache;

        public static IFastCellCache FastCellCache;

        public static ILinkedinLogin LinkedinLogin;

        public static void Initialize()
        {
        }
    }
}