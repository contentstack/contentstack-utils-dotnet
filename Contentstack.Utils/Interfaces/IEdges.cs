using System;
using System.Text.Json.Serialization;

namespace Contentstack.Utils.Interfaces
{
    public class IEdges<T> where T: IEmbeddedObject
    {
        [JsonPropertyName("node")]
        public T Node { get; set; }
    }
}


