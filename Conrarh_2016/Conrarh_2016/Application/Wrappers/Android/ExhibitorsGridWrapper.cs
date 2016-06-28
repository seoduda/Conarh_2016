using System.Collections.Generic;
using Conarh_2016.Application.Domain;

namespace Conarh_2016.Application.Wrappers
{
	public sealed class ExhibitorsGridWrapper:DynamicObservableData<Exhibitor>
	{
		public readonly DynamicListData<Exhibitor> Exhibitors;

		public ExhibitorsGridWrapper( DynamicListData<Exhibitor> exhibitors):base(false)
		{
			Exhibitors = exhibitors;

			if (!Exhibitors.IsEmpty ())
				OnChanged (Exhibitors.Items);

			Exhibitors.CollectionChanged += OnChanged;
		}

		private void OnChanged (List<Exhibitor> items)
		{
			UpdateData (items);
		}
	}
}

