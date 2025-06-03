using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Contentstack.Utils.Converters;
using Contentstack.Utils.Enums;

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
