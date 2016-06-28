using System.Collections.Generic;
using Conarh_2016.Application.Domain;

namespace Conarh_2016.Application.Wrappers
{
	public sealed class EventsGridWrapper:DynamicObservableData<EventData>
	{
		public readonly DynamicListData<EventData> Events;

		public EventsGridWrapper( DynamicListData<EventData> events):base(false)
		{
			Events = events;

			if (!Events.IsEmpty ())
				OnChanged (Events.Items);

			Events.CollectionChanged += OnChanged;
		}

		private void OnChanged (List<EventData> items)
		{
			UpdateData (items);
		}
	}
}

