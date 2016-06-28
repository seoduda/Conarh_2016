using Xamarin.Forms;
using System;
using Conarh_2016.Application.Domain;
using Conarh_2016.Application.UI.Controls;
using Conarh_2016.Core;
using XLabs.Forms.Controls;

namespace Conarh_2016.Application.UI.Exhibitors
{
	public sealed class ExhibitorViewCell: ExtendedViewCell
	{
		private Label _nameLabel;
		private Label _descLabel;
		private DownloadedImage _exhibitorImage;

		public ExhibitorViewCell()
		{
			var layout = new StackLayout () { Orientation = StackOrientation.Horizontal, Padding = new Thickness(10)};

			_exhibitorImage = new DownloadedImage(AppResources.DefaultExhibitorImage) 
			{
				WidthRequest = 40,
				HeightRequest = 40
			};
			layout.Children.Add (_exhibitorImage);

			var dataLayout = new StackLayout () {Orientation = StackOrientation.Vertical, Padding = new Thickness(5, 0, 0, 0)};

			_nameLabel = new Label () {
				FontAttributes = FontAttributes.Bold,
				FontSize = 15,
				TextColor = AppResources.ExhibitorNameTextColor,
				XAlign = TextAlignment.Start,
				HeightRequest = 20,
				WidthRequest = AppProvider.Screen.Width - 100
			};
			dataLayout.Children.Add (_nameLabel);

			_descLabel = new Label () 
			{
				FontSize = 13,
				TextColor = AppResources.ExhibitorDescTextColor,
				XAlign = TextAlignment.Start,
				HeightRequest = 20,
				WidthRequest = AppProvider.Screen.Width - 100
			};
			dataLayout.Children.Add (_descLabel);

			layout.Children.Add (dataLayout);

			TapGestureRecognizer cellTapRecongizer = new TapGestureRecognizer ();
			cellTapRecongizer.Tapped += OnExhibitorClick;
			layout.GestureRecognizers.Add (cellTapRecongizer);

			View = layout;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			var exhibitorModel = BindingContext as Exhibitor;
			if (exhibitorModel != null) {
				_nameLabel.Text = exhibitorModel.Title;
				_descLabel.Text = exhibitorModel.Description;
				_exhibitorImage.UpdateDefault ();
				_exhibitorImage.UpdateAtTime = exhibitorModel.UpdatedAtTime;
				_exhibitorImage.ServerImagePath = exhibitorModel.Icon;
			}
		}

		void OnExhibitorClick (object sender, EventArgs e)
		{
			AppController.Instance.AppRootPage.NavigateTo (Main.MainMenuItemData.MapPage, false);
		}
	}
}