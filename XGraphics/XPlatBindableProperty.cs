using System;

namespace XGraphics
{
    public class XPlatBindableProperty
    {
        public static XPlatBindableProperty Create(string propertyName, Type returnType, object? defaultValue)
        {
            return new XPlatBindableProperty(propertyName, returnType, defaultValue);
        }

        public string PropertyName { get;  }

        public Type PropertyType { get; }

        public object DefaultValue { get;  }

        public XPlatBindableProperty(string propertyName, Type propertyType, object defaultValue)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            DefaultValue = defaultValue;
        }
    }
}
