using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Contentstack.Utils.Models;

namespace Contentstack.Utils.Converters
{
    public class NodeJsonConverter : JsonConverter<Node>
    {
        public override Node Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var root = jsonDoc.RootElement;
                Node node;
                if (!root.TryGetProperty("type", out _))
                {
                    var textNode = new TextNode();
                    textNode.type = "text";
                    if (root.TryGetProperty("text", out var textProp))
                        textNode.text = textProp.GetString() ?? string.Empty;
                    if (root.TryGetProperty("bold", out var boldProp))
                        textNode.bold = boldProp.GetBoolean();
                    if (root.TryGetProperty("italic", out var italicProp))
                        textNode.italic = italicProp.GetBoolean();
                    if (root.TryGetProperty("underline", out var underlineProp))
                        textNode.underline = underlineProp.GetBoolean();
                    if (root.TryGetProperty("strikethrough", out var strikeProp))
                        textNode.strikethrough = strikeProp.GetBoolean();
                    if (root.TryGetProperty("inlineCode", out var inlineCodeProp))
                        textNode.inlineCode = inlineCodeProp.GetBoolean();
                    if (root.TryGetProperty("subscript", out var subscriptProp))
                        textNode.subscript = subscriptProp.GetBoolean();
                    if (root.TryGetProperty("superscript", out var superscriptProp))
                        textNode.superscript = superscriptProp.GetBoolean();
                    if (root.TryGetProperty("classname", out var classnameProp))
                        textNode.classname = classnameProp.GetString();
                    if (root.TryGetProperty("id", out var idProp))
                        textNode.id = idProp.GetString();
                    node = textNode;
                }
                else
                {
                    node = new Node();
                }
                foreach (var prop in root.EnumerateObject())
                {
                    switch (prop.Name)
                    {
                        case "type":
                            node.type = prop.Value.GetString();
                            break;
                        case "attrs":
                            node.attrs = JsonSerializer.Deserialize<IDictionary<string, object>>(prop.Value.GetRawText(), options);
                            break;
                        case "children":
                            node.children = JsonSerializer.Deserialize<List<Node>>(prop.Value.GetRawText(), options);
                            break;
                    }
                }
                return node;
            }
        }
        public override void Write(Utf8JsonWriter writer, Node value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("type", value.type);
            writer.WritePropertyName("attrs");
            JsonSerializer.Serialize(writer, value.attrs, options);
            writer.WritePropertyName("children");
            JsonSerializer.Serialize(writer, value.children, options);
            writer.WriteEndObject();
        }
    }
}
