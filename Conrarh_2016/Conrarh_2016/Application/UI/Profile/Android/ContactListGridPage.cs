using Xamarin.Forms;
using Conarh_2016.Application.UI.Shared;
using System.Collections.Specialized;
using Conarh_2016.Core;
using XLabs.Forms.Controls;
using TwinTechs.Controls;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ContactListGridPage : ContentPage
	{
		private Label _emptyList = null;
		private GridView _gridView;

		public ContactListGridPage ()
		{
			BackgroundColor = AppResources.UserBackgroundColor;
			Title = AppResources.ContactList;
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

			AppModel.Instance.CurrentContactListWrapper.CollectionChanged += OnItemsChanged;

			_emptyList = new Label { 
				FontSize = 24,
				XAlign = TextAlignment.Center,
				TextColor = Color.White,
				Text = AppResources.ContactListIsEmpty,
				IsVisible = AppModel.Instance.CurrentContactListWrapper.IsEmpty
			};
	
			int width = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Width) - 10;
			_gridView = new GridView {
				RowSpacing = 5,
				ColumnSpacing = 5,
				ContentPaddingBottom = 0,
				ContentPaddingTop = 0,
				ContentPaddingLeft = 0,
				ContentPaddingRight = 0,
				ItemWidth = width,
				ItemHeight = 60,
				ItemsSource = AppModel.Instance.CurrentContactListWrapper,
				ItemTemplate = new DataTemplate (typeof(UserFastCell)),
			};

			var container = new PageViewContainer {
				HorizontalOptions = LayoutOptions.StartAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = new ContentPage {Content = _gridView}
			};

			Content = new StackLayout {
				Orientation = StackOrientation.Vertical,
				Children = { _emptyList, container }
			};
		}
	}

}