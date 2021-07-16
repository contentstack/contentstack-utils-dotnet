using System;
using System.Collections.Generic;
using Contentstack.Utils.Converters;
using Contentstack.Utils.Interfaces;
using Newtonsoft.Json;

namespace Contentstack.Utils.Models
{
    [Newtonsoft.Json.JsonConverter(typeof(RTEJsonConverter))]
    public class JsonRTENodes<T> where T : IEmbeddedObject
    {
        [JsonProperty("json")]
        public List<Node> Json { get; set; }
        [JsonProperty("embedded_itemsConnection.edges")]
        public List<IEdges<T>> Edges { get; set; }
    }
}
