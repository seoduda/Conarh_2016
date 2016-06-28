using System.ComponentModel;
using Xamarin.Forms;

namespace Conarh_2016.Application.Wrappers
{
	public abstract class PullRefreshListWrapper: INotifyPropertyChanged
	{
		private bool isBusy = false;
		public bool IsBusy
		{
			get { return isBusy; }
			set 
			{
				if (isBusy == value)
					return;

				isBusy = value;
				OnPropertyChanged ("IsBusy");
			}
		}

		public abstract void OnAction ();

		public void Done()
		{
			IsBusy = false;
		}

		private Command refreshCommand;
		public Command RefreshCommand
		{
			get { return refreshCommand ?? (refreshCommand = new Command (OnAction)); }
		}
	
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
		}
		
	}
}

