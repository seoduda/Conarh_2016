using System;
using Xamarin.Forms;
using XLabs.Forms.Controls;
using Conarh_2016.Core;

namespace Conarh_2016.Application.UI.Shared
{
	public sealed class InputFieldView:ContentView
	{
		public class Parameters
		{
			public string PlaceholderValue;
			public int TopPadding;
			public bool IsPassword = false;
			public float AddionalBorder = 0;
			public float LeftBorder = 20;
			public string CurrentValue;
			public Keyboard Keyboard;
		}

		private ExtendedEntry _entry;
		private BoxView _horizontalBoxView;
		private BoxView _verticalBoxView;

		public InputFieldView(Parameters parameters)
		{
			var layout = new AbsoluteLayout { 
				Padding = new Thickness(parameters.LeftBorder, parameters.TopPadding, 0, 0) };

			int entryWidth = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Width - parameters.LeftBorder * 2 - parameters.AddionalBorder);
			_entry = new ExtendedEntry { 
				Placeholder = parameters.PlaceholderValue, 
				PlaceholderTextColor = AppResources.LoginNormalTextColor,
				TextColor = AppResources.LoginActiveTextColor, 
				BackgroundColor = Color.Transparent,
				HasBorder = false,
				IsPassword = parameters.IsPassword, 
				WidthRequest = entryWidth, 
				Font = Font.SystemFontOfSize(12),
				Text = parameters.CurrentValue,
				YAlign = TextAlignment.Start,
				Keyboard  = parameters.Keyboard
			};

			int width = AppProvider.Screen.ConvertPixelsToDp (AppProvider.Screen.Width - parameters.LeftBorder * 2 - parameters.AddionalBorder);
			_horizontalBoxView = new BoxView {
				BackgroundColor = AppResources.LoginNormalTextColor,
				HeightRequest = 0.5f,
				WidthRequest = width
			};
			_verticalBoxView = new BoxView {
				BackgroundColor = AppResources.LoginNormalTextColor,
				HeightRequest = 5,
				WidthRequest = 0.5f
			};

			Point entryPosition = Point.Zero;
			if (Device.OS == TargetPlatform.Android)
				entryPosition = new Point (0, -3);
			layout.Children.Add (_entry, entryPosition);

			layout.Children.Add (_horizontalBoxView, new Point(0, 20));
			layout.Children.Add (_verticalBoxView, new Point(0, 15));

			_entry.Focused += OnEntryFocused;

			Content = layout;
		}

		void OnEntryFocused (object sender, FocusEventArgs e)
		{
			Color settedColor = e.IsFocused ? AppResources.LoginActiveTextColor : AppResources.LoginNormalTextColor;
			_entry.PlaceholderTextColor = settedColor;
			_horizontalBoxView.BackgroundColor = settedColor;
			_verticalBoxView.BackgroundColor = settedColor;
		}

		public string Text
		{
			get 
			{
				return _entry.Text;
			}
		}
	}
}