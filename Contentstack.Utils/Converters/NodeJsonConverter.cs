using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Contentstack.Utils.Models;

namespace Contentstack.Utils.Converters
{
    public class NodeJsonConverter : JsonConverter<Node>
    {
        public override Node ReadJson(JsonReader reader, Type objectType, Node existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Node node = null;
            JObject jObject = JObject.Load(reader);
            if (jObject["type"] == null)
            {
                node = new TextNode();
                node.type = Enums.NodeType.Text;
            }else
            {
                node = new Node();
            }
            serializer.Populate(jObject.CreateReader(), node);
            return node;
        }

        public override void WriteJson(JsonWriter writer, Node value, JsonSerializer serializer)
        {
            
        }
    }
}
