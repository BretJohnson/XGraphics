using System.Collections.Generic;

namespace XGraphics.ImageLoading.Concurrency.FastPriorityQueue
{
    public class QueueComparer<TPriority> : Comparer<TPriority>
    {
        private readonly Comparer<TPriority> _comparer = Comparer<TPriority>.Default;

        public override int Compare(TPriority x, TPriority y)
        {
            return _comparer.Compare(x, y) * -1;
        }
    }
}
