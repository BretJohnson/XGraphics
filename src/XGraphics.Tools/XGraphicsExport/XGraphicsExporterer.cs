using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using XGraphics.Converters;
using XGraphics.StandardModel;
using XGraphics.StandardModel.Brushes;

namespace XGraphics.Tools.XGraphicsExport
{
    public class XGraphicsExporter
    {
        private readonly XCanvas _canvas;

        public XGraphicsExporter(XCanvas canvas) => _canvas = canvas;

        public void Export(Stream outputStream)
        {
            XElement rootElement = ObjectToElement(_canvas);
            var document = new XDocument(rootElement);

            document.Save(outputStream);
        }

        private XElement ObjectToElement(object obj)
        {
            Type objectType = obj.GetType();
            string elementName = objectType.Name;

            XElement element = new XElement(elementName);

            foreach (Type interfaceType in GetGraphicsModelObjectInterfaces(objectType))
            {
                foreach (PropertyInfo propertyInfo in interfaceType.GetProperties())
                {
                    string propertyName = propertyInfo.Name;
                    Type propertyType = propertyInfo.PropertyType;
                    object? propertyValue = propertyInfo.GetValue(obj);

                    object? defaultValue = GetModelDefaultValue(propertyInfo);

                    if (propertyValue == null || propertyValue.Equals(defaultValue))
                        continue;

                    if (propertyType.IsPrimitive || propertyType.IsEnum)
                    {
                        XAttribute attribute = new XAttribute(propertyName, propertyValue.ToString());
                        element.Add(attribute);
                    }
                    else if (propertyValue is IEnumerable enumerable)
                    {
                        var elementWithChildren = new XElement(propertyName);

                        foreach (object? childObject in enumerable)
                        {
                            if (childObject == null)
                                throw new InvalidOperationException($"Child object null in list: {enumerable}");

                            XElement childElement = ObjectToElement(childObject);
                            elementWithChildren.Add(childElement);
                        }

                        element.Add(elementWithChildren);
                    }
                    else if (propertyValue is SolidColorBrush solidColorBrush)
                    {
                        string valueString = ColorConverter.ConvertToString(solidColorBrush.Color);
                        element.Add(new XAttribute(propertyName, valueString));
                    }
                    else if (propertyType.IsInterface)
                    {
                        XElement childElement = ObjectToElement(propertyValue);
                        element.Add(childElement);
                    }
                    else if (propertyValue is Color color)
                    {
                        string valueString = ColorConverter.ConvertToString(color);
                        element.Add(new XAttribute(propertyName, valueString));
                    }
                    else if (propertyValue is Points points)
                    {
                        string valueString = PointsConverter.ConvertToString(points);
                        element.Add(new XAttribute(propertyName, valueString));
                    }
                    else
                        throw new InvalidOperationException(
                            $"Unexpected property type for {propertyName}: {propertyType.Name}");
                }
            }

            return element;
        }

        private IEnumerable<Type> GetGraphicsModelObjectInterfaces(Type type) => type.GetInterfaces().Where(HasGraphicsModelObjectAttribute);

        private bool HasGraphicsModelObjectAttribute(Type intface) => intface.GetCustomAttributes<GraphicsModelObjectAttribute>().Any();

        private object? GetModelDefaultValue(PropertyInfo propertyInfo)
        {
            foreach (ModelDefaultValueAttribute defaultValueAttribute in propertyInfo.GetCustomAttributes<ModelDefaultValueAttribute>())
            {
                object? defaultValue = defaultValueAttribute.Value;
                return defaultValue;
            }

            return null;
        }
    }
}
