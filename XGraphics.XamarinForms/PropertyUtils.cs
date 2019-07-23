using System;
using Xamarin.Forms;

namespace XGraphics.XamarinForms
{
    public static class PropertyUtils
    {
        public static BindableProperty CreateProperty(XPlatBindableProperty xplatProperty, Type ownerType,
            Type? propertyType = null)
        {
            return BindableProperty.Create(xplatProperty.PropertyName, propertyType ?? xplatProperty.PropertyType,
                    ownerType, xplatProperty.DefaultValue);
        }
    }
}
