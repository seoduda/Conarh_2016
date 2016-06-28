using Xamarin.Forms;
using Conarh_2016.Application.UI.Shared;
using System.Collections.Specialized;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ContactListUIWrapper: PullRefreshListWrapper
	{
		public override void OnAction ()
		{
			if (IsBusy)
				return;

			IsBusy = true;

			UserController.Instance.DownloadConnections (Done, true);
		}
	}


	public sealed class ContactListPage : ContentPage
	{
		private readonly Label _emptyList = null;
		private ListView _listView;

		public ContactListPage ()
		{
			BackgroundColor = AppResources.UserBackgroundColor;
			Title = AppResources.ContactList;

			var wrapper = new ContactListUIWrapper ();

			_listView = new ListView {
				HasUnevenRows = false,
				RowHeight = 60,
				ItemTemplate = new DataTemplate (typeof(UserCell)),
				SeparatorVisibility = SeparatorVisibility.None,
				BackgroundColor = Color.Transparent,
				ItemsSource = AppModel.Instance.CurrentContactListWrapper,

				RefreshCommand = wrapper.RefreshCommand,
				IsPullToRefreshEnabled = true,
				BindingContext = wrapper
			};

			_listView.SetBinding<ContactListUIWrapper> (ListView.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);

			AppModel.Instance.CurrentContactListWrapper.CollectionChanged += OnItemsChanged;

			_emptyList = new Label { 
				FontSize = 24,
				XAlign = TextAlignment.Center,
				TextColor = Color.White,
				Text = AppResources.ContactListIsEmpty,
				IsVisible = AppModel.Instance.CurrentContactListWrapper.IsEmpty
			};

			Content = new StackLayout { 
				Padding = new Thickness(10),
				Children = { _emptyList, _listView }
			};
		}


		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			if (AppProvider.ImageCache != null)
				AppProvider.ImageCache.Clear ();

			if (AppProvider.FastCellCache != null)
				AppProvider.FastCellCache.FlushAllCaches ();
		}


		void OnItemsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			if(AppModel.Instance.CurrentContactListWrapper != null)
				_emptyList.IsVisible = AppModel.Instance.CurrentContactListWrapper.IsEmpty;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_listView.BeginRefresh ();
		}
	}

}