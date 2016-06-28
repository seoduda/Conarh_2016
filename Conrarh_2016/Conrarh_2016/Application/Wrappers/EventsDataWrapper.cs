using System.Collections.Generic;
using System.Collections.ObjectModel;
using Conarh_2016.Application.Domain;
using Xamarin.Forms;
using System.Collections.Specialized;
using System;

namespace Conarh_2016.Application.Wrappers
{
	public sealed class EventsDynamicObservableData:DynamicObservableData<EventData>
	{
		public const string SectionNamePropertyName = "SectionName";
		public readonly DateTime Date;

		public EventsDynamicObservableData(DateTime eventDate) : base(false)
		{
			Date = eventDate;
		}

		public string SectionName
		{
			get 
			{
				return String.Format("{0:dd/MM/yyyy}", Date);   
			}
		}
	}

	public sealed class EventsDataWrapper:ObservableCollection<EventsDynamicObservableData>
	{
		public readonly DynamicListData<EventData> Events;

		public readonly Dictionary<DateTime, EventsDynamicObservableData> EventsDates;

		public EventsDataWrapper( DynamicListData<EventData> events)
		{
			EventsDates = new Dictionary<DateTime, EventsDynamicObservableData> ();
			Events = events;

			if (!Events.IsEmpty ())
				OnEventsChanged (Events.Items);

			Events.CollectionChanged += OnEventsChanged;
		}

		private void OnEventsChanged (List<EventData> events)
		{
			Dictionary<DateTime, List<EventData>> eventsByDate = new Dictionary<DateTime, List<EventData>> ();

			foreach (EventData eventData in events) 
			{
				DateTime date = new DateTime (eventData.Date.Year, eventData.Date.Month, eventData.Date.Day);
				if (!eventsByDate.ContainsKey (date))
					eventsByDate.Add (date, new List<EventData> ());

				eventsByDate [date].Add (eventData);
			}

			Device.BeginInvokeOnMainThread (() => {
				foreach (DateTime date in eventsByDate.Keys) 
				{
					if (!EventsDates.ContainsKey (date))
					{
						var data = new EventsDynamicObservableData (date);
						Items.Add(data);
						EventsDates.Add (date, data);

						OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, data));
					}

					EventsDates [date].UpdateData (eventsByDate [date]);
				}
			});
		}
	}
}

