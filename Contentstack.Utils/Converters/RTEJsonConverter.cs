using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Contentstack.Utils.Converters
{
    public class RTEJsonConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var root = jsonDoc.RootElement;
                object targetObj = Activator.CreateInstance(typeToConvert);
                foreach (PropertyInfo prop in typeToConvert.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    var attr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                    string jsonPath = attr != null ? attr.Name : prop.Name;
                    JsonElement token = root;
                    bool found = false;
                    // Support nested property names like 'system.uid'
                    if (jsonPath.Contains("."))
                    {
                        var parts = jsonPath.Split('.');
                        JsonElement current = root;
                        foreach (var part in parts)
                        {
                            if (current.ValueKind == JsonValueKind.Object && current.TryGetProperty(part, out var next))
                            {
                                current = next;
                                found = true;
                            }
                            else
                            {
                                found = false;
                                break;
                            }
                        }
                        if (found)
                            token = current;
                    }
                    else if (root.TryGetProperty(jsonPath, out var directToken))
                    {
                        token = directToken;
                        found = true;
                    }
                    if (found)
                    {
                        object value = JsonSerializer.Deserialize(token.GetRawText(), prop.PropertyType, options);
                        prop.SetValue(targetObj, value);
                    }
                    else
                    {
                        // Set default value for missing properties
                        if (prop.PropertyType.IsValueType)
                            prop.SetValue(targetObj, Activator.CreateInstance(prop.PropertyType));
                        else
                            prop.SetValue(targetObj, null);
                    }
                }
                return targetObj;
            }
        }
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (PropertyInfo prop in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                string jsonPath = attr != null ? attr.Name : prop.Name;
                writer.WritePropertyName(jsonPath);
                JsonSerializer.Serialize(writer, prop.GetValue(value), options);
            }
            writer.WriteEndObject();
        }
    }
}
