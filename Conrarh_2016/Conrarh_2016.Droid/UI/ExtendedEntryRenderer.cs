using Xamarin.Forms;

using XLabs.Forms.Controls;
using Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace XLabs.Forms.Controls
{
	using System;

	using Android.Views;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	using Android.Text;

	/// <summary>
	/// Class ExtendedEntryRenderer.
	/// </summary>
	public class ExtendedEntryRenderer : EntryRenderer
	{
		/// <summary>
		/// The mi n_ distance
		/// </summary>
		private const int MIN_DISTANCE = 10;
	
		/// <summary>
		/// Called when [element changed].
		/// </summary>
		/// <param name="e">The e.</param>
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			var view = (ExtendedEntry)Element;

			SetFont(view);
			SetTextAlignment(view);
			SetBorder(view);
			SetPlaceholderTextColor(view);
			SetMaxLength(view);

			if (this.Control == null) return;

			this.Control.Background = new ColorDrawable (global::Android.Graphics.Color.Transparent);
		}

		/// <summary>
		/// Handles the <see cref="E:ElementPropertyChanged" /> event.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (ExtendedEntry)Element;

			if (e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
				SetFont(view);
			if (e.PropertyName == ExtendedEntry.XAlignProperty.PropertyName)
				SetTextAlignment(view);
			//if (e.PropertyName == ExtendedEntry.HasBorderProperty.PropertyName)
			//    SetBorder(view);
			if (e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
				SetPlaceholderTextColor(view);
		}

		/// <summary>
		/// Sets the border.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetBorder(ExtendedEntry view)
		{
			//NotCurrentlySupported: HasBorder peroperty not suported on Android
		}

		/// <summary>
		/// Sets the text alignment.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetTextAlignment(ExtendedEntry view)
		{
			switch (view.XAlign)
			{
			case Xamarin.Forms.TextAlignment.Center:
				Control.Gravity = GravityFlags.CenterHorizontal;
				break;
			case Xamarin.Forms.TextAlignment.End:
				Control.Gravity = GravityFlags.Bottom;
				break;
			case Xamarin.Forms.TextAlignment.Start:
				Control.Gravity = GravityFlags.Top;
				break;
			}
		}

		/// <summary>
		/// Sets the text alignment.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetVerticalTextAlignment(ExtendedEntry view)
		{
			
		}

		/// <summary>
		/// Sets the font.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetFont(ExtendedEntry view)
		{
			if(view.Font != Font.Default) {
				Control.TextSize = view.Font.ToScaledPixel();
			}
		}

		/// <summary>
		/// Sets the color of the placeholder text.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetPlaceholderTextColor(ExtendedEntry view){
			if(view.PlaceholderTextColor != Color.Default) 
				Control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());			
		}

		/// <summary>
		/// Sets the MaxLength characteres.
		/// </summary>
		/// <param name="view">The view.</param>
		private void SetMaxLength(ExtendedEntry view)
		{
			Control.SetFilters(new IInputFilter[] { new global::Android.Text.InputFilterLengthFilter(view.MaxLength) });
		}
	}
}

