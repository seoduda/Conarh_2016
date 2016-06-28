using Xamarin.Forms;
using XLabs.Forms.Controls;
using Conarh_2016.Application.Wrappers;
using Conarh_2016.Core;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ProfileHistoryFastItemCell:FastGridCell
	{
		#region implemented abstract members of FastGridCell

		private StackLayout _layout;
		private Label _contactAddLabel;
		private Label _dateLabel;
		private ConnectionModel Model;

		protected override void InitializeCell ()
		{
			_layout = new StackLayout {
				Orientation = StackOrientation.Vertical, 
				Padding = new Thickness(0, 10)
			};
			_layout.BackgroundColor = AppResources.ProfileHistoryItemLightColor;

			_contactAddLabel = new Label { 
				XAlign = TextAlignment.End, 
				HeightRequest = 40,
				WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 10
			};
			_layout.Children.Add (_contactAddLabel);

			_dateLabel  = new Label { 
				FontSize = 13,
				TextColor = Color.White,
				XAlign = TextAlignment.End,
				HeightRequest = 15,
				WidthRequest = AppProvider.Screen.ConvertPixelsToDp(AppProvider.Screen.Width) - 10
			};
			_layout.Children.Add (_dateLabel);

			View = _layout;
		}

		protected override void SetupCell (bool isRecycled)
		{
			if(Model != null)
				Model.IndexChanged -= OnIndexChanged;

			Model = BindingContext as ConnectionModel;

			if(Model != null)
			{
				_contactAddLabel.FormattedText = Model.HeaderLabel;
				_dateLabel.Text = Model.DateLabel;

				Model.IndexChanged += OnIndexChanged;
				OnIndexChanged(Model.Index);
			}
		}

		#endregion


		void OnIndexChanged (int index)
		{
			Device.BeginInvokeOnMainThread(() => {
				Color color = Model.Index % 2 == 1 ? AppResources.ProfileHistoryItemLightColor : AppResources.ProfileHistoryItemDarkColor;
				_layout.BackgroundColor = color;
			});
		}
	}
}

