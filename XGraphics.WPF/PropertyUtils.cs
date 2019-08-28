using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Windows;
using XGraphics.Transforms;
using XGraphics.WPF.Transforms;

namespace XGraphics.WPF
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

        public static DependencyProperty Create(string propertyName, Type propertyType, Type ownerType, object? defaultValue)
        {
            Enumerable.Empty<DependencyProperty>();
            new List<DependencyProperty>();


            if (propertyType == typeof(IBrush))
                propertyType = typeof(Brush);
            else if (propertyType == typeof(ITransform))
                propertyType = typeof(Transform);
            else if (propertyType == typeof(Color))
            {
                propertyType = typeof(global::XGraphics.WPF.Wrapper.Color);
                defaultValue = Wrapper.Color.Transparent;
            }

            var propertyMetadata = new PropertyMetadata(defaultValue, OnPropertyChanged);
            return DependencyProperty.Register(propertyName, propertyType, ownerType, propertyMetadata);
        }

        private static void OnPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is INotifyObjectOrSubobjectChanged parentObj))
                return;

            // The logic below cascades change notifications from subobjects up the object hierarchy, eventually causing the GraphicsCanvas
            // to be invalidated on any change
            if (e.OldValue is INotifyObjectOrSubobjectChanged oldChildObj)
                oldChildObj.Changed -= parentObj.OnSubobjectChanged;

            if (e.NewValue is INotifyObjectOrSubobjectChanged newChildObj)
                newChildObj.Changed += parentObj.OnSubobjectChanged;

            parentObj.OnChanged();
        }
    }
}
