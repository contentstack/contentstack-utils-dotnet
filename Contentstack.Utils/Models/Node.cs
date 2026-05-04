using System.Collections.Generic;
using Contentstack.Utils.Converters;
using System.Text.Json.Serialization;

namespace Contentstack.Utils.Models
{
    [JsonConverter(typeof(NodeJsonConverter))]
    public class Node
    {
        public string type { get; set; }

        public IDictionary<string, object> attrs { get; set; }

        public List<Node> children { get; set; }
    }
}
