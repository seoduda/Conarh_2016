using Xamarin.Forms;
using XLabs.Forms.Controls;
using Conarh_2016.Core;

namespace Conarh_2016.Application.UI.Profile
{
	public sealed class RatingGridPage : ContentPage
	{
		private GridView _gridView;
		private readonly RatingListModel Model;

		public RatingGridPage ()
		{
			Model = new RatingListModel ();
			Model.ItemsChanged += OnItemsChanged;

			BackgroundColor = Color.White;
			Title = AppResources.ProfileRatingBtnHeader;

			int width = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Width) - 20;
			_gridView = new GridView {
				RowSpacing = 5,
				ColumnSpacing = 5,
				ContentPaddingBottom = 0,
				ContentPaddingTop = 20,
				ContentPaddingLeft = 10,
				ContentPaddingRight = 0,
				ItemWidth = width,
				ItemHeight = 40,
				ItemTemplate = new DataTemplate (typeof(RatingFastCell))
			};

			Content = _gridView;
			AppController.Instance.UpdateRating (Model.Users, null);
		}

		void OnItemsChanged ()
		{
			Device.BeginInvokeOnMainThread (() => {
				_gridView.ItemsSource = Model.Items;
			});
		}

	}

}