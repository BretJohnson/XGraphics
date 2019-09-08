using System;

namespace XGraphics
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ModelDefaultValueAttribute : Attribute
    {
        public object? Value { get; }

        public ModelDefaultValueAttribute(object? value)
        {
            Value = value;
        }
    }
}