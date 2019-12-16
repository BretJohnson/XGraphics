using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    public class BindableObjectWithCascadingNotifications : BindableObject, INotifyObjectOrSubobjectChanged
    {
        public event ObjectOrSubobjectChangedEventHandler? Changed;

        public void NotifySinceObjectChanged() => Changed?.Invoke();

        public void NotifySinceSubobjectChanged() => Changed?.Invoke();
    }
}
