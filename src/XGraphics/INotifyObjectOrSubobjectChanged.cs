namespace XGraphics
{
    public delegate void ObjectOrSubobjectChangedEventHandler();

    public interface INotifyObjectOrSubobjectChanged
    {
        event ObjectOrSubobjectChangedEventHandler Changed;

        /// <summary>
        /// Call this to notify the parent object that it has changed, and it should thus notify its listeners,
        /// cascading the change notification.
        /// </summary>
        void OnChanged();

        /// <summary>
        /// Call this to notify the parent object that one of its subobjects have changed, and it should thus notify its listeners,
        /// cascading the change notification.
        /// </summary>
        void OnSubobjectChanged();
    }
}