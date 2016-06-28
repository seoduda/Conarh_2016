using XLabs.Forms.Controls;

namespace Conarh_2016.Application.UI.Events
{
	public sealed class EventCell: ExtendedViewCell
	{
		private readonly EventView _view;

		public EventCell ()
		{
			ShowDisclousure = false;
			_view = new EventView ();
			View = _view;
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			_view.BindingContext = BindingContext;
		}

	}
}