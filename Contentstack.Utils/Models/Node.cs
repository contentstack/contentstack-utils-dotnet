using System;
using System.Collections.Generic;
using Contentstack.Utils.Converters;
using Contentstack.Utils.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Contentstack.Utils.Models
{
    [JsonConverter(typeof(NodeJsonConverter))]
    public class Node
    {
        public NodeType type { get; set; }

        public IDictionary<string, object> attrs { get; set; }

        public List<Node> children { get; set; }
    }
}
