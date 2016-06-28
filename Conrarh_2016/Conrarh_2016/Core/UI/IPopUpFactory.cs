using System;

namespace Conarh_2016.Core.UI
{
    public interface IPopUpFactory
    {
        void ShowMessage(string message, string header, string buttonKey = "", Action action = null);
		void ShowConfirmation(string message, string header, Action yesAction, Action noAction, string yesBtnName, string noBtnName);
    }
}