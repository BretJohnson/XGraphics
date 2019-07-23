using System;
using System.Windows;

namespace XGraphics.WPF
{
    public static class PropertyUtils
    {
        public static DependencyProperty CreateProperty(XPlatBindableProperty xplatProperty, Type ownerType,
            Type? propertyType = null)
        {
            var propertyMetadata = new PropertyMetadata(xplatProperty.DefaultValue);
            return DependencyProperty.Register(xplatProperty.PropertyName, propertyType ?? xplatProperty.PropertyType, ownerType,
                propertyMetadata);
        }
    }
}
