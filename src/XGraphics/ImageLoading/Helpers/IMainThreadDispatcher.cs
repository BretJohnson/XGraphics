using System;
using System.Threading.Tasks;

namespace XGraphics.ImageLoading.Helpers
{
    public interface IMainThreadDispatcher
    {
        // void Post(Action action);

        Task PostAsync(Action action);

        Task PostAsync(Func<Task> action);
    }
}

