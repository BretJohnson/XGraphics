using System.Linq;
using XGraphics.ImageLoading.Concurrency.FastPriorityQueue;
using XGraphics.ImageLoading.Work;

namespace XGraphics.ImageLoading.Concurrency
{
    public class PendingTasksQueue : SimplePriorityQueue<IImageLoaderTask, int>
    {
        public IImageLoaderTask FirstOrDefaultByRawKey(string rawKey)
        {
            lock (_queue)
            {
                return _queue.FirstOrDefault(v => v.Data?.KeyRaw == rawKey)?.Data;
            }
        }
    }
}
