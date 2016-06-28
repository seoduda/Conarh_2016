using Xamarin.Forms;
using Conarh_2016.Application.Wrappers;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ProfileHistoryView:ContentView
	{
		public ProfileHistoryView (ContactListWrapper contactList)
		{
			var label = new Label {
				FontSize = 22,
				Text = AppResources.ProfileHistoryHeader,
				TextColor = AppResources.AgendaCongressoColor,
				XAlign = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold,
				YAlign = TextAlignment.Center
			};

			var listView = new ListView {
				ItemTemplate = new DataTemplate (typeof(ProfileHistoryItemCell)),
				SeparatorVisibility = SeparatorVisibility.None,
				BackgroundColor = Color.Transparent,
				ItemsSource = contactList,
				RowHeight = 80
			};

			Content =  new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical,
				BackgroundColor = Color.White, 
				Padding = new Thickness(0, 5, 0, 0),
				Children = {label, listView}
			};
		}
	}
}

