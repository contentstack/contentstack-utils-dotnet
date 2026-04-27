using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Contentstack.Utils.Interfaces;

namespace Contentstack.Utils.Converters
{
    /// <summary>
    /// Factory for JSON path-based deserialization used by <see cref="Models.JsonRTENode{T}"/> and <see cref="Models.JsonRTENodes{T}"/>.
    /// </summary>
    public sealed class RTEJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            Type def = typeToConvert.GetGenericTypeDefinition();
            return def == typeof(Models.JsonRTENode<>) || def == typeof(Models.JsonRTENodes<>);
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type embeddedArg = typeToConvert.GetGenericArguments()[0];
            Type def = typeToConvert.GetGenericTypeDefinition();

            if (def == typeof(Models.JsonRTENode<>))
            {
                Type converterType = typeof(RTEJsonConverterImpl<>).MakeGenericType(embeddedArg);
                return (JsonConverter)Activator.CreateInstance(converterType);
            }

            if (def == typeof(Models.JsonRTENodes<>))
            {
                Type converterType = typeof(RTEJsonNodesConverterImpl<>).MakeGenericType(embeddedArg);
                return (JsonConverter)Activator.CreateInstance(converterType);
            }

            throw new InvalidOperationException($"Unsupported type for RTE JSON converter: {typeToConvert}");
        }
    }

    /// <summary>
    /// Deserializes objects whose properties map to dotted JSON paths (same idea as Newtonsoft <c>SelectToken</c> with <see cref="JsonPropertyNameAttribute"/>).
    /// </summary>
    public sealed class PathMappedJsonConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonObject jo = JsonNode.Parse(ref reader) as JsonObject ?? new JsonObject();
            T targetObj = Activator.CreateInstance<T>();

            foreach (PropertyInfo prop in typeof(T).GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                JsonPropertyNameAttribute att = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                string jsonPath = att?.Name ?? prop.Name;
                JsonNode token = RTEJsonPath.SelectPath(jo, jsonPath);

                if (token != null && !RTEJsonPath.IsJsonNull(token))
                {
                    object value = RTEJsonPath.DeserializeNode(token, prop.PropertyType, options);
                    prop.SetValue(targetObj, value);
                }
            }

            return targetObj;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Serialization is not supported for path-mapped RTE models.");
        }
    }

    internal static class RTEJsonPath
    {
        internal static JsonNode SelectPath(JsonObject root, string path)
        {
            if (root == null || string.IsNullOrEmpty(path))
                return null;

            string[] segments = path.Split('.');
            JsonNode current = root;
            foreach (string segment in segments)
            {
                if (current is JsonObject jo && jo.TryGetPropertyValue(segment, out JsonNode next))
                    current = next;
                else
                    return null;
            }

            return current;
        }

        internal static bool IsJsonNull(JsonNode node)
        {
            if (node == null)
                return true;
            return node is JsonValue jv && jv.GetValueKind() == JsonValueKind.Null;
        }

        internal static object DeserializeNode(JsonNode token, Type propertyType, JsonSerializerOptions options)
        {
            string json = token.ToJsonString(options);
            return JsonSerializer.Deserialize(json, propertyType, options);
        }
    }

    internal sealed class RTEJsonConverterImpl<T> : JsonConverter<Models.JsonRTENode<T>> where T : IEmbeddedObject
    {
        public override Models.JsonRTENode<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonObject jo = JsonNode.Parse(ref reader) as JsonObject ?? new JsonObject();
            var targetObj = new Models.JsonRTENode<T>();

            foreach (PropertyInfo prop in typeof(Models.JsonRTENode<T>).GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                JsonPropertyNameAttribute att = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                string jsonPath = att?.Name ?? prop.Name;
                JsonNode token = RTEJsonPath.SelectPath(jo, jsonPath);

                if (token != null && !RTEJsonPath.IsJsonNull(token))
                {
                    object value = RTEJsonPath.DeserializeNode(token, prop.PropertyType, options);
                    prop.SetValue(targetObj, value);
                }
            }

            return targetObj;
        }

        public override void Write(Utf8JsonWriter writer, Models.JsonRTENode<T> value, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Serialization is not supported for JsonRTENode.");
        }
    }

    internal sealed class RTEJsonNodesConverterImpl<T> : JsonConverter<Models.JsonRTENodes<T>> where T : IEmbeddedObject
    {
        public override Models.JsonRTENodes<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            JsonObject jo = JsonNode.Parse(ref reader) as JsonObject ?? new JsonObject();
            var targetObj = new Models.JsonRTENodes<T>();

            foreach (PropertyInfo prop in typeof(Models.JsonRTENodes<T>).GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                JsonPropertyNameAttribute att = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                string jsonPath = att?.Name ?? prop.Name;
                JsonNode token = RTEJsonPath.SelectPath(jo, jsonPath);

                if (token != null && !RTEJsonPath.IsJsonNull(token))
                {
                    object val = RTEJsonPath.DeserializeNode(token, prop.PropertyType, options);
                    prop.SetValue(targetObj, val);
                }
            }

            return targetObj;
        }

        public override void Write(Utf8JsonWriter writer, Models.JsonRTENodes<T> value, JsonSerializerOptions options)
        {
            throw new NotSupportedException("Serialization is not supported for JsonRTENodes.");
        }
    }
}
