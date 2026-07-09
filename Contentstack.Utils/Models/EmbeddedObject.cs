using System.Collections.Generic;
using Contentstack.Utils.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Contentstack.Utils.Models
{
    // Concrete class used by EmbeddedObjectConverter when deserializing _embedded_items.
    // Implements both IEmbeddedEntry and IEmbeddedAsset to cover entries and assets.
    public class EmbeddedObject : IEmbeddedEntry, IEmbeddedAsset
    {
        [JsonProperty("uid")]
        public string Uid { get; set; } = string.Empty;

        [JsonProperty("_content_type_uid")]
        public string ContentTypeUid { get; set; } = string.Empty;

        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("filename")]
        public string FileName { get; set; } = string.Empty;

        [JsonProperty("url")]
        public string Url { get; set; } = string.Empty;

        // Any field not explicitly declared above (custom fields, locale data, etc.)
        [JsonExtensionData]
        public IDictionary<string, JToken> Fields { get; set; } = new Dictionary<string, JToken>();
    }
}
