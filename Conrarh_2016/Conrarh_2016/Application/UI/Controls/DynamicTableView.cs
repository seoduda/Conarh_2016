using Xamarin.Forms;
using Conarh_2016.Application.Domain;
using System.Collections.Generic;
using System;
using Conarh_2016.Application.UI.Shared;

namespace Conarh_2016.Application.UI.Controls
{
	public sealed class DynamicTableView<T, S>:ContentView 
		where T:DynamicChildContentView
		where S:UpdatedUniqueItem
	{
		private readonly List<T> ItemViews = new List<T>();

		public DynamicListData<S> Model;

		public int ItemsInRow = 3;
		public int ItemPadding = 10;

		private readonly List<StackLayout> _parents;
		private readonly StackLayout _mainStackLayout;

		public DynamicTableView(DynamicListData<S> model)
		{
			_parents = new List<StackLayout> ();
			_mainStackLayout = new StackLayout {Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.Fill, Spacing = ItemPadding};

			Content = _mainStackLayout;

			Model = model;
			Model.CollectionChanged += OnCollectionChanged;

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
				GetParent(index).Children.Add (view);
			}
			else view = ItemViews [index];

			return view;
		}

		private StackLayout GetParent(int itemIndex)
		{
			int rowIndex = itemIndex / ItemsInRow;

			StackLayout stackLayout = rowIndex >= _parents.Count ? null : _parents [rowIndex];
			if (stackLayout == null) {
				stackLayout = new StackLayout {Orientation = StackOrientation.Horizontal, Spacing = ItemPadding};
				_mainStackLayout.Children.Add (stackLayout);
				_parents.Add (stackLayout);
			} 

			return stackLayout;
		}
	}
}

