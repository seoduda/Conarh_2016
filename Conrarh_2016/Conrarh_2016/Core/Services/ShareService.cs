using Acr.UserDialogs;
using Conarh_2016.Application;
using System;

namespace Conarh_2016.Core.Services
{
    public interface IShareService
    {
        void ShareLink(string title, string status, string link, Action onShareDone);
    }

    public abstract class ShareService : IShareService
    {
        #region IShareService implementation

        public void ShareLink(string title, string status, string link, Action onShareDone)
        {
            var config = new ActionSheetConfig();
            config.Add(AppResources.Facebook, () => ShareFacebook(title, status, link, onShareDone));
            config.Add(AppResources.Twitter, () => ShareTwitter(title, status, link, onShareDone));
            config.Add(AppResources.Cancel);
            UserDialogs.Instance.ActionSheet(config);
        }

        #endregion IShareService implementation

        public abstract void ShareFacebook(string title, string status, string link, Action onShareDone);

        public abstract void ShareTwitter(string title, string status, string link, Action onShareDone);
    }
}