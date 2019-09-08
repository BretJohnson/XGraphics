using System.Collections;
using System.Collections.Generic;

namespace XGraphics
{
    public class PointCollection : IList<Point>, INotifyObjectOrSubobjectChanged
    {
        private readonly List<Point> _items;

        public PointCollection()
        {
            _items = new List<Point>();
        }

        public event ObjectOrSubobjectChangedEventHandler Changed;

        public void OnChanged() => Changed?.Invoke();

        public void OnSubobjectChanged() => Changed?.Invoke();

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public IEnumerator<Point> GetEnumerator() => _items.GetEnumerator();

        public Point this[int index] {
            get => _items[index];
            set {
                _items[index] = value;
                OnChanged();
            }
        }

        public void AddRange(IEnumerable<Point> points)
        {
            _items.AddRange(points);
            OnChanged();
        }

        public void Add(Point point)
        {
            _items.Add(point);
            OnChanged();
        }

        public bool Contains(Point point)
        {
            return _items.Contains(point);
        }

        public void CopyTo(Point[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public void Clear()
        {
            _items.Clear();
            OnChanged();
        }

        public int IndexOf(Point point) => _items.IndexOf(point);

        public void Insert(int index, Point point)
        {
            _items.Insert(index, point);
            OnChanged();
        }

        public bool Remove(Point point)
        {
            bool found = _items.Remove(point);
            if (found)
                OnChanged();
            return found;
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
            OnChanged();
        }
    }
}
