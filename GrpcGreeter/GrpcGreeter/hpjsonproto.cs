using System.Reflection;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public static class JsonHelper
{
    public static T DeserializeWithMapping<T>(string json) where T : new()
    {
        T obj = new T();
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            foreach (var element in doc.RootElement.EnumerateObject())
            {
                SetPropertyValue(obj, element.Name, element.Value);
            }
        }
        return obj;
    }

    private static void SetPropertyValue(object obj, string propertyName, JsonElement value)
    {
        var property = obj.GetType().GetProperty(ToPascalCase(propertyName), BindingFlags.Public | BindingFlags.Instance);
        if (property != null && property.CanWrite)
        {
            var convertedValue = ConvertValue(property.PropertyType, value);
            property.SetValue(obj, convertedValue);
        }
    }

    private static object ConvertValue(Type propertyType, JsonElement value)
    {
        if (value.ValueKind == JsonValueKind.Null)
        {
            if (propertyType == typeof(string))
            {
                return string.Empty; // 
            }
            if (propertyType.IsValueType)
            {
                return Activator.CreateInstance(propertyType); //  
            }
            return null;
        }

        if (propertyType == typeof(string))
        {
            return value.GetString();
        }
        if (propertyType == typeof(int) || propertyType == typeof(int?))
        {
            return value.GetInt32();
        }
        if (propertyType == typeof(double) || propertyType == typeof(double?))
        {
            return value.GetDouble();
        }
        if (propertyType == typeof(bool) || propertyType == typeof(bool?))
        {
            return value.GetBoolean();
        }
        if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
        {
            return value.GetDateTime();
        }
        if (propertyType.IsEnum)
        {
            return Enum.Parse(propertyType, value.GetString());
        }

        if (propertyType.IsClass)
        {
            
            var nestedObject = Activator.CreateInstance(propertyType);
            foreach (var nestedElement in value.EnumerateObject())
            {
                SetPropertyValue(nestedObject, nestedElement.Name, nestedElement.Value);
            }
            return nestedObject;
        }

        if (propertyType == typeof(List<double>))
        {
            var list = new List<double>();
            foreach (var item in value.EnumerateArray())
            {
                list.Add(item.GetDouble());
            }
            return list;
        }

        return null;
    }

    private static string ToPascalCase(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        if (str.Length == 1)
        {
            return str.ToUpper();
        }

        return char.ToUpper(str[0]) + str.Substring(1).Replace("_", string.Empty);
    }
}
