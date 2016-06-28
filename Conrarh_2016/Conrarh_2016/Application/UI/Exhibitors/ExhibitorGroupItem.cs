using System;
using Xamarin.Forms;
using Conarh_2016.Application.Wrappers;

namespace Conarh_2016.Application.UI.Exhibitors
{
	public sealed class ExhibitorGroupItem:ViewCell
	{
		private readonly Label _label;
		private readonly ContentView _contentView;
		
		public ExhibitorGroupItem ()
		{
			_label = new Label
			{ 
				TextColor = Color.White, 
				FontSize = 16,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center
			};

			_contentView = new ContentView {  Content = _label, Padding = new Thickness(0, 10), HeightRequest = 40};

			View = _contentView;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			var model = BindingContext as ExhibitorsDynamicObservableData;
			if (model != null) {
				_label.Text = model.SectionName;
				_contentView.BackgroundColor = model.SectionColor;
			}
		}
	}
}

