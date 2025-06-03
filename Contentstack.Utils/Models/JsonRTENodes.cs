using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Contentstack.Utils.Converters;
using Contentstack.Utils.Interfaces;

namespace Contentstack.Utils.Models
{
    [JsonConverter(typeof(RTEJsonConverter))]
    public class JsonRTENodes<T> where T : IEmbeddedObject
    {
        [JsonPropertyName("json")]
        public List<Node> Json { get; set; }
        [JsonPropertyName("embedded_itemsConnection.edges")]
        public List<IEdges<T>> Edges { get; set; }
    }
}
