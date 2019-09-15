using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    public class BindableObjectWithCascadingNotifications : BindableObject, INotifyObjectOrSubobjectChanged
    {
        public event ObjectOrSubobjectChangedEventHandler? Changed;

        public void OnChanged() => Changed?.Invoke();

        public void OnSubobjectChanged() => Changed?.Invoke();
    }
}
