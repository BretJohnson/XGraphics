using System.Windows;

namespace XGraphics.WPF
{
    public class DependencyObjectWithCascadingNotifications : DependencyObject, INotifyObjectOrSubobjectChanged
    {
        public event ObjectOrSubobjectChangedEventHandler? Changed;

        public void NotifySinceObjectChanged() => Changed?.Invoke();

        public void NotifySinceSubobjectChanged() => Changed?.Invoke();
    }
}
