using System.Collections.Generic;

namespace XGraphics.SvgImporter
{
    public class StyleableProperties
    {
        private readonly Dictionary<string, string> _propertyValues;
        private readonly StyleableProperties? _baseProperties;

        public StyleableProperties(Dictionary<string, string> propertyValues, StyleableProperties? baseProperties)
        {
            _baseProperties = baseProperties;
            _propertyValues = propertyValues;
        }

        public string? GetValue(string property)
        {
            if (_propertyValues.TryGetValue(property, out string value))
                return value.Trim();

            if (_baseProperties != null)
                return _baseProperties.GetValue(property);

            return null;
        }

        public bool HasValue(string property) => GetValue(property) != null;

        public string? GetString(string name)
        {
            if (_propertyValues.TryGetValue(name, out string localValue))
                return localValue.Trim();

            if (_baseProperties != null)
                return _baseProperties.GetString(name);

            return null;
        }
    }
}
