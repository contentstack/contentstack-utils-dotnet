using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Contentstack.Utils.Models;

namespace Contentstack.Utils.Converters
{
    public class NodeJsonConverter : JsonConverter<Node>
    {
        public override Node Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = doc.RootElement;
                bool hasType = root.TryGetProperty("type", out JsonElement typeEl) && typeEl.ValueKind != JsonValueKind.Null;

                if (!hasType)
                {
                    TextNode textNode = new TextNode();
                    textNode.type = "text";
                    PopulateTextNode(root, textNode, options);
                    return textNode;
                }

                Node node = new Node();
                PopulateNode(root, node, options);
                return node;
            }
        }

        public override void Write(Utf8JsonWriter writer, Node value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value?.GetType() ?? typeof(Node), CloneWithoutNodeConverter(options));
        }

        private static void PopulateNode(JsonElement root, Node node, JsonSerializerOptions options)
        {
            if (root.TryGetProperty("type", out JsonElement typeProp))
                node.type = typeProp.GetString();

            if (root.TryGetProperty("attrs", out JsonElement attrs))
                node.attrs = JsonSerializer.Deserialize<IDictionary<string, object>>(attrs.GetRawText(), options);

            if (root.TryGetProperty("children", out JsonElement children))
                node.children = JsonSerializer.Deserialize<List<Node>>(children.GetRawText(), options);
        }

        private static void PopulateTextNode(JsonElement root, TextNode node, JsonSerializerOptions options)
        {
            PopulateNode(root, node, options);

            if (root.TryGetProperty("text", out JsonElement text))
                node.text = text.GetString();

            TryBindBool(root, "bold", v => node.bold = v);
            TryBindBool(root, "italic", v => node.italic = v);
            TryBindBool(root, "underline", v => node.underline = v);
            TryBindBool(root, "strikethrough", v => node.strikethrough = v);
            TryBindBool(root, "inlineCode", v => node.inlineCode = v);
            TryBindBool(root, "subscript", v => node.subscript = v);
            TryBindBool(root, "superscript", v => node.superscript = v);

            if (root.TryGetProperty("classname", out JsonElement cn))
                node.classname = cn.ValueKind == JsonValueKind.Null ? null : cn.GetString();

            if (root.TryGetProperty("id", out JsonElement id))
                node.id = id.ValueKind == JsonValueKind.Null ? null : id.GetString();
        }

        private static void TryBindBool(JsonElement root, string name, Action<bool> set)
        {
            if (!root.TryGetProperty(name, out JsonElement el))
                return;
            if (el.ValueKind == JsonValueKind.True || el.ValueKind == JsonValueKind.False)
                set(el.GetBoolean());
        }

        private static JsonSerializerOptions CloneWithoutNodeConverter(JsonSerializerOptions options)
        {
            JsonSerializerOptions inner = new JsonSerializerOptions(options);
            for (int i = inner.Converters.Count - 1; i >= 0; i--)
            {
                if (inner.Converters[i] is NodeJsonConverter)
                    inner.Converters.RemoveAt(i);
            }

            return inner;
        }
    }
}
