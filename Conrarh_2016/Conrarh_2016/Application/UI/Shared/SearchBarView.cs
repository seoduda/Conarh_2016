using System;
using Xamarin.Forms;
using Conarh_2016.Core;
using XLabs.Forms.Controls;

namespace Conarh_2016.Application.UI.Shared
{
	public class SearchBarView:ContentView
	{
		private readonly ExtendedEntry _searchEntry;

		public event Action<string> Search;
		public event Action Clear;

		public SearchBarView ()
		{
			Grid grid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = 
				{
					new ColumnDefinition { Width = new GridLength(30, GridUnitType.Absolute) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
				},
				Padding = new Thickness(10, 2),
				BackgroundColor = AppResources.AgendaExpoColor
			};

			var searchIcon = new Image {
				WidthRequest = 20,
				HeightRequest = 20,
				Source = ImageLoader.Instance.GetImage(AppResources.ExhibitorsSearchImage, true)
			};

			grid.Children.Add(searchIcon, 0, 0);
			_searchEntry = new ExtendedEntry { 
				Placeholder = AppResources.ExhibitorsSearchText, 
				TextColor = AppResources.MenuColor, 
				BackgroundColor = Color.Transparent, 
				Font = Font.SystemFontOfSize(16),
				YAlign = TextAlignment.Center,

			};
			_searchEntry.HasBorder = false;

			grid.Children.Add(_searchEntry, 1, 0);
			_searchEntry.Completed += OnSearchClicked;
			Content = grid;
		}

		void OnSearchClicked (object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (_searchEntry.Text)) {
				if (Clear != null)
					Clear ();
			} else {
				if (Search != null)
					Search (_searchEntry.Text);
			}
		}

	}
}

