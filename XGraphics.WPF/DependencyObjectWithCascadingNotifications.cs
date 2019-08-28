using System.Windows;

namespace XGraphics.WPF
{
    public class DependencyObjectWithCascadingNotifications : DependencyObject, INotifyObjectOrSubobjectChanged
    {
        public event ObjectOrSubobjectChangedEventHandler Changed;

        public void OnChanged() => Changed?.Invoke();

        public void OnSubobjectChanged() => Changed?.Invoke();
    }
}
