using System.Collections.Generic;
using Contentstack.Utils.Converters;
using Contentstack.Utils.Interfaces;
using System.Text.Json.Serialization;

namespace Contentstack.Utils.Models
{
    [JsonConverter(typeof(RTEJsonConverterFactory))]
    public class JsonRTENodes<T> where T : IEmbeddedObject
    {
        [JsonPropertyName("json")]
        public List<Node> Json { get; set; }
        [JsonPropertyName("embedded_itemsConnection.edges")]
        public List<IEdges<T>> Edges { get; set; }
    }
}
