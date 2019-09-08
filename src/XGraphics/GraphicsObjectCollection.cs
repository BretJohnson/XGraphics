using System;
using System.Collections;
using System.Collections.Generic;

namespace XGraphics
{
    public class GraphicsObjectCollection<TItem> : IList, IEnumerable<TItem>, INotifyObjectOrSubobjectChanged where TItem : INotifyObjectOrSubobjectChanged
    {
        private readonly List<TItem> items;

        public GraphicsObjectCollection()
        {
            items = new List<TItem>();
        }

        public event ObjectOrSubobjectChangedEventHandler Changed;

        public void OnChanged() => Changed?.Invoke();

        public void OnSubobjectChanged() => Changed?.Invoke();

        public int Count => items.Count;

        public object SyncRoot => ((ICollection) items).SyncRoot;

        public bool IsSynchronized => ((ICollection)items).IsSynchronized;

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        public IEnumerator<TItem> GetEnumerator() => items.GetEnumerator();

        public void CopyTo(Array array, int index) => ((IList)items).CopyTo(array, index);

        public object this[int index] {
            get => items[index];
            set {
                if (!(value is TItem item))
                    throw new InvalidOperationException($"Only {typeof(TItem)} subclasses can be added to this collection");

                OnItemRemoved(items[index]);

                items[index] = item;

                OnItemAdded(item);
            }
        }

        public int Add(object itemObject)
        {
            if (!(itemObject is TItem item))
                throw new InvalidOperationException($"Only {typeof(TItem)} subclasses can be added to this collection");

            int index = ((IList)items).Add(item);
            OnItemAdded(item);
            return index;
        }

        public bool Contains(object itemObject)
        {
            if (!(itemObject is TItem item))
                return false;

            return items.Contains(item);
        }

        public void Clear()
        {
            var temp = items.ToArray();
            foreach (var t in temp)
                OnItemRemoved(t);

            items.Clear();
        }

        public int IndexOf(object itemObject)
        {
            if (!(itemObject is TItem item))
                return -1;
            return items.IndexOf(item);
        }

        public void Insert(int index, object itemObject)
        {
            if (!(itemObject is TItem item))
                throw new InvalidOperationException($"Only {typeof(TItem)} subclasses can be added to this collection");

            items.Insert(index, item);
            OnItemAdded(item);
        }

        public void Remove(object itemObject)
        {
            if (!(itemObject is TItem item))
                return;

            var result = items.Remove(item);
            if (result)
                OnItemRemoved(item);
        }

        public void RemoveAt(int index)
        {
            OnItemRemoved(items[index]);
            items.RemoveAt(index);
        }

        private void OnItemAdded(TItem element)
        {
            element.Changed += OnSubobjectChanged;
            OnChanged();
        }

        private void OnItemRemoved(TItem element)
        {
            element.Changed -= OnSubobjectChanged;
            OnChanged();
        }
    }
}
