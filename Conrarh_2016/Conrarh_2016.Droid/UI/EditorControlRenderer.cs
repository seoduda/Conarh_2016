using Conarh_2016.Droid.UI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Conarh_2016.Application.UI.Shared;

[assembly: ExportRenderer(typeof(EditorControl), typeof(EditorControlRenderer))]
namespace Conarh_2016.Droid.UI
{
	public class EditorControlRenderer:EditorRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				Control.SetBackgroundColor(global::Android.Graphics.Color.White);
				Control.SetTextColor(global::Android.Graphics.Color.Black);

			}
		}
	}
}
