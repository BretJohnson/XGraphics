using System;
using System.Collections;
using System.Collections.Generic;

namespace XGraphics
{
    public class GraphicsObjectCollection<TItem> : IList, IEnumerable<TItem>, INotifyObjectOrSubobjectChanged where TItem : INotifyObjectOrSubobjectChanged
    {
        private readonly List<TItem> _items;

        public GraphicsObjectCollection()
        {
            _items = new List<TItem>();
        }

        public event ObjectOrSubobjectChangedEventHandler? Changed;

        public void OnChanged() => Changed?.Invoke();

        public void OnSubobjectChanged() => Changed?.Invoke();

        public int Count => _items.Count;

        public object SyncRoot => ((ICollection) _items).SyncRoot;

        public bool IsSynchronized => ((ICollection)_items).IsSynchronized;

        public bool IsReadOnly => false;

        public bool IsFixedSize => false;

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public IEnumerator<TItem> GetEnumerator() => _items.GetEnumerator();

        public void CopyTo(Array array, int index) => ((IList)_items).CopyTo(array, index);

        public object this[int index] {
            get => _items[index];
            set {
                if (!(value is TItem item))
                    throw new InvalidOperationException($"Only {typeof(TItem)} subclasses can be added to this collection");

                OnItemRemoved(_items[index]);

                _items[index] = item;

                OnItemAdded(item);
            }
        }

        public int Add(object itemObject)
        {
            if (!(itemObject is TItem item))
                throw new InvalidOperationException($"Only {typeof(TItem)} subclasses can be added to this collection");

            int index = ((IList)_items).Add(item);
            OnItemAdded(item);
            return index;
        }

        public bool Contains(object itemObject)
        {
            if (!(itemObject is TItem item))
                return false;

            return _items.Contains(item);
        }

        public void Clear()
        {
            var temp = _items.ToArray();
            foreach (var t in temp)
                OnItemRemoved(t);

            _items.Clear();
        }

        public int IndexOf(object itemObject)
        {
            if (!(itemObject is TItem item))
                return -1;
            return _items.IndexOf(item);
        }

        public void Insert(int index, object itemObject)
        {
            if (!(itemObject is TItem item))
                throw new InvalidOperationException($"Only {typeof(TItem)} subclasses can be added to this collection");

            _items.Insert(index, item);
            OnItemAdded(item);
        }

        public void Remove(object itemObject)
        {
            if (!(itemObject is TItem item))
                return;

            bool result = _items.Remove(item);
            if (result)
                OnItemRemoved(item);
        }

        public void RemoveAt(int index)
        {
            OnItemRemoved(_items[index]);
            _items.RemoveAt(index);
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
