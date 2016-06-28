using Xamarin.Forms;
using Conarh_2016.Application.Domain;
using System.Collections.Generic;
using System;
using Conarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Controls
{
	public sealed class DynamicListView<T, S>:ContentView 
		where T:DynamicChildContentView
		where S:UpdatedUniqueItem
	{
		private readonly List<T> ItemViews = new List<T>();

		public DynamicListData<S> Model;

		public float RowHeight = 0;

		private StackLayout _parent;

		public DynamicListView(DynamicListData<S> model, double spacing)
		{
			_parent = new StackLayout () {Spacing = spacing};
			Content = _parent;

			Model = model;
			Model.CollectionChanged += OnCollectionChanged;

			if (Model.Items.Count > 0)
				OnCollectionChanged (Model.Items);
		}

		private void OnCollectionChanged (List<S> itemsData)
		{
			Device.BeginInvokeOnMainThread (() => {
				for (int i = 0; i < itemsData.Count; i++) {
					T view = GetCell (i);
					view.BindingContext = itemsData [i];
					view.IsVisible = true;
				}

				for (int i = itemsData.Count; i < ItemViews.Count; i++) {
					T view = GetCell (i);
					view.IsVisible = false;
				}
			});
		}

		private T GetCell(int index)
		{
			T view = null;

			if (index >= ItemViews.Count) 
			{
				view = (T)Activator.CreateInstance (typeof(T), index);
				ItemViews.Add (view);
				_parent.Children.Add (view);
			}
			else view = ItemViews [index];

			return view;
		}
	}
}

