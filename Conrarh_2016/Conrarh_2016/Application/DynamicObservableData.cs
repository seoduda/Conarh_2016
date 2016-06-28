using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Specialized;
using Conarh_2016.Core.Tools;
using Conarh_2016.Application.Domain;

namespace Conarh_2016.Application
{
	public class DynamicObservableData<T> : SortedObservableCollection<T> where T:UpdatedUniqueItem
	{
		public readonly List<string> ItemIds;

		public DynamicObservableData(bool isSorted):base(isSorted)
		{
			ItemIds = new List<string> ();
		}

		public void UpdateData(List<T> newItems)
		{
			if (newItems == null)
				return;

			foreach (T newItem in newItems) 
				AddItem (newItem);
		}

		private bool IsContains(T item)
		{
			return ItemIds.Contains (item.Id);
		}

		private void AddItem(T item)
		{
			Device.BeginInvokeOnMainThread (() => {
				if (!IsContains (item))
				{
					InsertItem(0, item);
					ItemIds.Add(item.Id);

					OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
				}
				else
				{
					var result = Items.Where(temp => temp.Id.Equals(item.Id));
					foreach(T existingItem in result)
					{
						if(existingItem.UpdatedAtTime < item.UpdatedAtTime)
							existingItem.UpdateWithItem(item);
					}
				}
			});
		}

		public void AddOne(T newItem)
		{
			AddItem (newItem);
		}
	
		public void ClearData ()
		{
			Device.BeginInvokeOnMainThread (() => {
				Items.Clear ();
				ItemIds.Clear ();

				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			});
		}

		public void DeleteOne (T data)
		{
			Device.BeginInvokeOnMainThread (() => {
				if (Contains (data)) {
					ItemIds.Remove (data.Id);
					Items.Remove (data);

					OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, data));
				}
			});
		}
	}
}

