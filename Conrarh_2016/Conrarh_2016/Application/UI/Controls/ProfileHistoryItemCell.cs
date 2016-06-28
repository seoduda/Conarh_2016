using Xamarin.Forms;
using Conarh_2016.Application.Domain;
using XLabs.Forms.Controls;
using Conarh_2016.Application.Wrappers;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class ProfileHistoryItemCell:ExtendedViewCell
	{
		private StackLayout _layout;
		private Label _contactAddLabel;
		private Label _dateLabel;

		public ProfileHistoryItemCell ()
		{
			_layout = new StackLayout {Orientation = StackOrientation.Vertical, Padding = new Thickness(0, 10)};
			_layout.BackgroundColor = AppResources.ProfileHistoryItemLightColor;

			_contactAddLabel = new Label { XAlign = TextAlignment.End};
			_layout.Children.Add (_contactAddLabel);

			_dateLabel  = new Label { FontSize = 13, TextColor = Color.White, XAlign = TextAlignment.End };
			_layout.Children.Add (_dateLabel);

			View = _layout;
		}

		private ConnectionModel Model;
		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

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

		void OnIndexChanged (int index)
		{
			Device.BeginInvokeOnMainThread(() => {
				if(Model != null)
				{
					Color color = Model.Index % 2 == 1 ? AppResources.ProfileHistoryItemLightColor : AppResources.ProfileHistoryItemDarkColor;
					_layout.BackgroundColor = color;
				}
			});
		}
	}
}

 