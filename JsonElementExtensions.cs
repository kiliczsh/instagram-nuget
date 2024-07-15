using System.Text.Json;

namespace Instagram
{
    public static class JsonElementExtensions
    {
        public static string GetPropertyAsString(this JsonElement element, string propertyName)
        {
            return element.GetProperty(propertyName).GetString()!;
        }

        public static int GetPropertyAsInt(this JsonElement element, string propertyName)
        {
            return element.GetProperty(propertyName).GetInt32();
        }

        public static bool TryGetPropertyAsString(this JsonElement element, string propertyName, out string? value)
        {
            if (element.TryGetProperty(propertyName, out JsonElement propertyElement) && propertyElement.ValueKind != JsonValueKind.Null)
            {
                value = propertyElement.GetString();
                return true;
            }
            value = null;
            return false;
        }

        public static bool TryGetProperty(this JsonElement element, string propertyName, out JsonElement value)
        {
            return element.TryGetProperty(propertyName, out value);
        }
        
        public static string GetNestedPropertyAsString(this JsonElement element, params string[] propertyNames)
        {
            JsonElement currentElement = element;
            foreach (var propertyName in propertyNames)
            {
                if (currentElement.TryGetProperty(propertyName, out JsonElement nestedElement))
                {
                    currentElement = nestedElement;
                }
                else
                {
                    throw new KeyNotFoundException($"Property '{propertyName}' not found.");
                }
            }
            return currentElement.GetString()!;
        }

        public static int GetNestedPropertyAsInt(this JsonElement element, params string[] propertyNames)
        {
            JsonElement currentElement = element;
            foreach (var propertyName in propertyNames)
            {
                if (currentElement.TryGetProperty(propertyName, out JsonElement nestedElement))
                {
                    currentElement = nestedElement;
                }
                else
                {
                    throw new KeyNotFoundException($"Property '{propertyName}' not found.");
                }
            }
            return currentElement.GetInt32();
        }
    }
}