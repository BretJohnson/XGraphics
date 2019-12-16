namespace XGraphics.StandardModel
{
    public class ObjectWithCascadingNotifications : INotifyObjectOrSubobjectChanged
    {
        public event ObjectOrSubobjectChangedEventHandler? Changed;

        public void NotifySinceObjectChanged() => Changed?.Invoke();

        public void NotifySinceSubobjectChanged() => Changed?.Invoke();

        protected void SetProperty<T>(ref T storage, T value)
        {
            if (!Equals(storage, value))
            {
                storage = value;
                Changed?.Invoke();
            }
        }
    }
}
