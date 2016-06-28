using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

[assembly: 
	InternalsVisibleTo("XLabs.Forms.Droid"),
	InternalsVisibleTo("XLabs.Forms.iOS"),
	InternalsVisibleTo("XLabs.Forms.WP8")]

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// An extended entry control that allows the Font and text X alignment to be set
	/// </summary>
	public class ExtendedEntry : Entry
	{
		/// <summary>
		/// The font property
		/// </summary>
		public static readonly BindableProperty FontProperty =
			BindableProperty.Create("Font", typeof(Font), typeof(ExtendedEntry), new Font());

		/// <summary>
		/// The XAlign property
		/// </summary>
		public static readonly BindableProperty XAlignProperty =
			BindableProperty.Create("XAlign", typeof(TextAlignment), typeof(ExtendedEntry), 
				TextAlignment.Start);

		/// <summary>
		/// The XAlign property
		/// </summary>
		public static readonly BindableProperty YAlignProperty =
			BindableProperty.Create("YAlign", typeof(TextAlignment), typeof(ExtendedEntry), 
				TextAlignment.Start);
		

		/// <summary>
		/// The HasBorder property
		/// </summary>
		public static readonly BindableProperty HasBorderProperty =
			BindableProperty.Create("HasBorder", typeof(bool), typeof(ExtendedEntry), true);

		/// <summary>
		/// The PlaceholderTextColor property
		/// </summary>
		public static readonly BindableProperty PlaceholderTextColorProperty =
			BindableProperty.Create("PlaceholderTextColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

		/// <summary>
		/// The MaxLength property
		/// </summary>
		public static readonly BindableProperty MaxLengthProperty =
			BindableProperty.Create("MaxLength", typeof(int), typeof(ExtendedEntry), int.MaxValue);

		/// <summary>
		/// The AutoCorrection property
		/// </summary>
		public static readonly BindableProperty AutoCorrectionProperty =
			BindableProperty.Create("AutoCorrection", typeof(bool), typeof(ExtendedEntry), false);
		
		/// <summary>
		/// Gets or sets the AutoCorrection
		/// </summary>
		public bool AutoCorrection
		{
			get { return (bool)this.GetValue(AutoCorrectionProperty);}
			set { this.SetValue(AutoCorrectionProperty, value); }
		}

		/// <summary>
		/// Gets or sets the MaxLength
		/// </summary>
		public int MaxLength
		{
			get { return (int)this.GetValue(MaxLengthProperty);}
			set { this.SetValue(MaxLengthProperty, value); }
		}

		/// <summary>
		/// Gets or sets the Font
		/// </summary>
		public Font Font
		{
			get { return (Font)GetValue(FontProperty); }
			set { SetValue(FontProperty, value); }
		}

		/// <summary>
		/// Gets or sets the X alignment of the text
		/// </summary>
		public TextAlignment XAlign
		{
			get { return (TextAlignment)GetValue(XAlignProperty); }
			set { SetValue(XAlignProperty, value); }
		}

		/// <summary>
		/// Gets or sets the X alignment of the text
		/// </summary>
		public TextAlignment YAlign
		{
			get { return (TextAlignment)GetValue(YAlignProperty); }
			set { SetValue(YAlignProperty, value); }
		}


		/// <summary>
		/// Gets or sets if the border should be shown or not
		/// </summary>
		public bool HasBorder
		{
			get { return (bool)GetValue(HasBorderProperty); }
			set { SetValue(HasBorderProperty, value); }
		}

		/// <summary>
		/// Sets color for placeholder text
		/// </summary>
		public Color PlaceholderTextColor
		{
			get { return (Color)GetValue(PlaceholderTextColorProperty); }
			set { SetValue(PlaceholderTextColorProperty, value); }
		}

		public EventHandler LeftSwipe;
		public EventHandler RightSwipe;

		internal void OnLeftSwipe(object sender, EventArgs e)
		{
			var handler = this.LeftSwipe;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		internal void OnRightSwipe(object sender, EventArgs e)
		{
			var handler = this.RightSwipe;
			if (handler != null)
			{
				handler(this, e);
			}
		}
	}
}