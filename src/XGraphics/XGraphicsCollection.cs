using System;
using System.Collections;
using System.Collections.Generic;

namespace XGraphics
{
    public class XGraphicsCollection<TItem> : IList, IList<TItem>, IEnumerable<TItem>, INotifyObjectOrSubobjectChanged where TItem : INotifyObjectOrSubobjectChanged
    {
        private readonly List<TItem> _items;


        public XGraphicsCollection()
        {
            _items = new List<TItem>();
        }

        public event ObjectOrSubobjectChangedEventHandler? Changed;

        public void NotifySinceObjectChanged() => Changed?.Invoke();

        public void NotifySinceSubobjectChanged() => Changed?.Invoke();

        public int Count => _items.Count;

        public int IndexOf(TItem item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, TItem item)
        {
            _items.Insert(index, item);
            OnItemAdded(item);
        }

        public void RemoveAt(int index)
        {
            OnItemRemoved(_items[index]);
            _items.RemoveAt(index);
        }

        public TItem this[int index]
        {
            get => _items[index];
            set
            {
                OnItemRemoved(_items[index]);
                _items[index] = value;
                OnItemAdded(value);
            }
        }

        public void Clear()
        {
            var temp = _items.ToArray();
            foreach (var t in temp)
                OnItemRemoved(t);

            _items.Clear();
        }

        public void Add(TItem item)
        {
            _items.Add(item);
            OnItemAdded(item);
        }

        public bool Contains(TItem item) => _items.Contains(item);

        public void CopyTo(TItem[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public bool Remove(TItem item)
        {
            bool result = _items.Remove(item);
            if (result)
                OnItemRemoved(item);
            return result;
        }

        public bool IsReadOnly => false;

        public IEnumerator<TItem> GetEnumerator() => _items.GetEnumerator();

        private void OnItemAdded(TItem element)
        {
            element.Changed += NotifySinceSubobjectChanged;
            Changed?.Invoke();
        }

        private void OnItemRemoved(TItem element)
        {
            element.Changed -= NotifySinceSubobjectChanged;
            Changed?.Invoke();
        }

        #region Nongeneric IList members

        object ICollection.SyncRoot => ((ICollection)_items).SyncRoot;

        bool ICollection.IsSynchronized => ((ICollection)_items).IsSynchronized;

        bool IList.IsFixedSize => false;

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => ((IList)_items).CopyTo(array, index);

        object IList.this[int index] {
            get => _items[index];
            set {
                if (!(value is TItem item))
                    throw new InvalidOperationException($"Only {typeof(TItem)} subclasses can be added to this collection");

                OnItemRemoved(_items[index]);

                _items[index] = item;

                OnItemAdded(item);
            }
        }

        int IList.Add(object itemObject)
        {
            if (!(itemObject is TItem item))
                throw new InvalidOperationException($"Only {typeof(TItem)} subclasses can be added to this collection");

            int index = ((IList)_items).Add(item);
            OnItemAdded(item);
            return index;
        }

        bool IList.Contains(object itemObject)
        {
            if (!(itemObject is TItem item))
                return false;

            return _items.Contains(item);
        }

        int IList.IndexOf(object itemObject)
        {
            if (!(itemObject is TItem item))
                return -1;
            return _items.IndexOf(item);
        }

        void IList.Insert(int index, object itemObject)
        {
            if (!(itemObject is TItem item))
                throw new InvalidOperationException($"Only {typeof(TItem)} subclasses can be added to this collection");

            _items.Insert(index, item);
            OnItemAdded(item);
        }

        void IList.Remove(object itemObject)
        {
            if (!(itemObject is TItem item))
                return;

            bool result = _items.Remove(item);
            if (result)
                OnItemRemoved(item);
        }

        #endregion
    }
}
