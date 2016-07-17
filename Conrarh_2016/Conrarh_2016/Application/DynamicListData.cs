using Conarh_2016.Application.Domain;
using System;
using System.Collections.Generic;

namespace Conarh_2016.Application
{
    public class DynamicListData<T> where T : UpdatedUniqueItem
    {
        public readonly List<T> Items;

        public event Action<List<T>> CollectionChanged;

        public DynamicListData()
        {
            Items = new List<T>();
        }

        public bool IsEmpty()
        {
            return Items.Count == 0;
        }

        public void UpdateData(List<T> newItems)
        {
            if (newItems == null)
                return;

            bool isAdded = false;
            foreach (T newItem in newItems)
            {
                bool isAdd = AddItem(newItem);

                if (!isAdded && isAdd)
                    isAdded = true;
            }

            if (isAdded)
                RaiseCollectionChanged();
        }

        public T Find(string id)
        {
            string s = " ";
            if (!String.IsNullOrEmpty(id))
                s = id;
            T tmp = Items.Find(temp => temp.Id.Equals(s));
            return tmp;
        }

        private bool AddItem(T item)
        {
            bool isChanged = false;

            T currentItemData = Find(item.Id);

            if (currentItemData != null)
            {
                if (currentItemData.UpdatedAtTime < item.UpdatedAtTime || currentItemData is User)
                {
                    currentItemData.UpdateWithItem(item);
                    isChanged = true;
                }
            }
            else
            {
                Items.Add(item);
                isChanged = true;
            }

            return isChanged;
        }

        public void AddOne(T newItem)
        {
            if (AddItem(newItem))
                RaiseCollectionChanged();
        }

        public void RaiseCollectionChanged()
        {
            if (CollectionChanged != null)
                CollectionChanged(Items);
        }

        public void ClearData()
        {
            Items.Clear();
            RaiseCollectionChanged();
        }

        public void DeleteOne(T data)
        {
            T item = Find(data.Id);
            if (item != null)
            {
                Items.Remove(item);
                RaiseCollectionChanged();
            }
        }
    }
}