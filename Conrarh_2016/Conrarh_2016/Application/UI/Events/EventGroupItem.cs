﻿using System;
using Xamarin.Forms;
using Conarh_2016.Application.Wrappers;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class EventGroupItem:ViewCell
	{
		private Label _label;

		public EventGroupItem ()
		{
			_label = new Label
			{
                TextColor = AppResources.MenuColor,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
				FontAttributes = FontAttributes.Bold
			};
			_label.SetBinding (Label.TextProperty, EventsDynamicObservableData.SectionNamePropertyName);

			var contentView = new ContentView {  
				Content = _label, 
				Padding = new Thickness(0, 10), 
				BackgroundColor = AppResources.AgendaDataBGColor
			};

			View = contentView;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			var model = BindingContext as EventsDynamicObservableData;

			if(model != null)
				_label.Text = model.SectionName;
		}
	}
}

