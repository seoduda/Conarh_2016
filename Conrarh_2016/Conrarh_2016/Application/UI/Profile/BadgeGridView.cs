using Xamarin.Forms;
using Conarh_2016.Core;
using Conarh_2016.Application.Domain;
using System.Collections.Generic;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class BadgeGridView : ContentView
	{
		public readonly UserModel Model;

		private List<BadgeCell> _badgesCells;
		private readonly Grid _badgeGrid;

		public BadgeGridView (UserModel model)
		{
			Model = model;

			//int px20Padding = AppProvider.Screen.ConvertPixelsToDp (20);
            int px40Padding = AppProvider.Screen.ConvertPixelsToDp(40);

            int badgeCellSize = AppProvider.Screen.ConvertPixelsToDp((AppProvider.Screen.Width - 100) / 3);
			_badgeGrid = new Grid
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto},
					new RowDefinition { Height = GridLength.Auto}
				},
				ColumnDefinitions = 
				{
					new ColumnDefinition { Width = new GridLength(badgeCellSize, GridUnitType.Absolute) },
					new ColumnDefinition { Width = new GridLength(badgeCellSize, GridUnitType.Absolute) },
					new ColumnDefinition { Width = new GridLength(badgeCellSize, GridUnitType.Absolute) },

				},
				Padding = new Thickness(px40Padding, px40Padding / 4, px40Padding, px40Padding / 4)
			};

			if (!AppModel.Instance.BadgeTypes.IsEmpty ())
				OnApplyBadgesTypes (AppModel.Instance.BadgeTypes.Items);
			else
				AppModel.Instance.BadgeTypes.CollectionChanged += OnApplyBadgesTypes; 
			
			Content = _badgeGrid;
		}

		void OnApplyBadgesTypes (List<BadgeType> badges)
		{
			Device.BeginInvokeOnMainThread (() => {
				if (badges.Count == 0)
					return;

				if (_badgesCells != null)
					return;

				_badgesCells = new List<BadgeCell> ();

				for (int i = 0; i < badges.Count; i++) {
					var cell = new BadgeCell (Model);
					_badgeGrid.Children.Add (cell, i % 3, i / 3);

					cell.BindingContext = badges [i];
					_badgesCells.Add (cell);
				}
			});
		}

	}
}