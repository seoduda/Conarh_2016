using System;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls;
using Xamarin.Forms;
using Android.Views;
using Android.Content;
using TwinTechs.Droid.Helper;
using Android.OS;

namespace XLabs.Forms.Controls
{
	public class GridViewCellContainer : ViewGroup
	{
		FastGridCell _viewCell;

		public FastGridCell ViewCell { get { return _viewCell; } }

		ViewGroup _nativeView;

		public GridViewCellContainer (Context context, FastGridCell fastGridCell, global::Android.Views.View parent, Size initialCellSize) : base (context)
		{
			using (var h = new Handler (Looper.MainLooper)) {
				h.Post (() => {
					_viewCell = fastGridCell;
					fastGridCell.PrepareCell (initialCellSize);
//					_viewCell.View.BackgroundColor = Xamarin.Forms.Color.Green;
					var renderer = RendererHelper.GetOrCreateRenderer (fastGridCell.View);
					_nativeView = renderer.ViewGroup;
//					SetBackgroundColor (Android.Graphics.Color.Yellow);
					AddView (_nativeView);
				});
			}

		}

		public void Update (object bindingContext)
		{
			using (var h = new Handler (Looper.MainLooper)) {
				h.Post (() => {
					_viewCell.BindingContext = bindingContext;
				});
			}
		}
	
		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			using (var h = new Handler (Looper.MainLooper)) {
				h.Post (() => {
					double width = base.Context.FromPixels ((double)(r - l));
					double height = base.Context.FromPixels ((double)(b - t));
					var msw = MeasureSpec.MakeMeasureSpec (r - l, MeasureSpecMode.Exactly);
					var msh = MeasureSpec.MakeMeasureSpec (b - t, MeasureSpecMode.Exactly);
					_nativeView.Measure (msw, msh);
					_nativeView.Layout (0, 0, r - l, b - t);

					var layout = _viewCell.View as Layout<Xamarin.Forms.View>;
					if (layout != null) {
						layout.Layout (new Rectangle (0, 0, width, height));
						layout.ForceLayout ();
						FixChildLayouts (layout);
					}
				});
			}
		}

		void FixChildLayouts (Layout<Xamarin.Forms.View> layout)
		{
			foreach (var child in layout.Children) {
				if (child is Layout<Xamarin.Forms.View>) {
					((Layout<Xamarin.Forms.View>)child).ForceLayout ();
					FixChildLayouts (child as Layout<Xamarin.Forms.View>);
				}
			}
		}
	}
}

