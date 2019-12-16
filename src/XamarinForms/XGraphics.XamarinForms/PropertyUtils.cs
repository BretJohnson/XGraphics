using System;
using System.Collections;
using System.Linq;
using Xamarin.Forms;
using XGraphics.Brushes;
using XGraphics.Transforms;
using XGraphics.XamarinForms.Brushes;
using XGraphics.XamarinForms.Transforms;

namespace XGraphics.XamarinForms
{
    public static class PropertyUtils
    {
        public static readonly Wrapper.Color DefaultColor = Wrapper.Color.Transparent;
        public static readonly Wrapper.Point DefaultPoint = Wrapper.Point.Default;
        public static readonly Wrapper.Size DefaultSize = Wrapper.Size.Default;

        public static IEnumerable Empty<T>()
        {
            return Enumerable.Empty<T>();
        }

        public static BindableProperty Create(string propertyName, Type propertyType, Type ownerType, object? defaultValue)
        {
            if (propertyType == typeof(IBrush))
                propertyType = typeof(Brush);
            else if (propertyType == typeof(ITransform))
                propertyType = typeof(Transform);
            else if (propertyType == typeof(Color))
            {
                propertyType = typeof(Wrapper.Color);
                defaultValue = Wrapper.Color.Transparent;
            }

            return BindableProperty.Create(propertyName, propertyType, ownerType, defaultValue: defaultValue,
                propertyChanged: OnPropertyChanged);
        }

        private static void OnPropertyChanged(BindableObject obj, object oldValue, object newValue)
        {
            if (!(obj is INotifyObjectOrSubobjectChanged parentObj))
                return;

            // The logic below cascades change notifications from subobjects up the object hierarchy, eventually causing the GraphicsCanvas
            // to be invalidated on any change
            if (oldValue is INotifyObjectOrSubobjectChanged oldChildObj)
                oldChildObj.Changed -= parentObj.NotifySinceSubobjectChanged;

            if (newValue is INotifyObjectOrSubobjectChanged newChildObj)
                newChildObj.Changed += parentObj.NotifySinceSubobjectChanged;

            parentObj.NotifySinceObjectChanged();
        }
    }
}
