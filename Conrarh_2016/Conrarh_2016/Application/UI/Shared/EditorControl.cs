using System;
using Xamarin.Forms;

namespace Conarh_2016.Application.UI.Shared
{
	public sealed class EditorControl:Editor
	{
		public readonly string PlaceHolderText;
		
		public EditorControl(string placeholder)
		{
			PlaceHolderText = placeholder;
			Text = PlaceHolderText;
			this.Focused += EditorControl_Focused;
			this.Completed += EditorControl_Completed;
		}

		void EditorControl_Completed (object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(Text))
				Text = PlaceHolderText;
		}

		void EditorControl_Focused (object sender, FocusEventArgs e)
		{
			if (e.IsFocused) {
				if (Text.Equals (PlaceHolderText))
					Text = string.Empty;
			} else {
				if (string.IsNullOrEmpty(Text))
					Text = PlaceHolderText;
			}
		}


	}
}

