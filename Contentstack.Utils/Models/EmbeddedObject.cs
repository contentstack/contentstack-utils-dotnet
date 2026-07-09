using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Contentstack.Utils.Interfaces;
namespace Contentstack.Utils.Models
{
    // Concrete class used by EmbeddedObjectConverter when deserializing _embedded_items.
    // Implements both IEmbeddedEntry and IEmbeddedAsset to cover entries and assets.
    public class EmbeddedObject : IEmbeddedEntry, IEmbeddedAsset
    {
        [JsonPropertyName("uid")]
        public string Uid { get; set; } = string.Empty;

        [JsonPropertyName("_content_type_uid")]
        public string ContentTypeUid { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("filename")]
        public string FileName { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        // Any field not explicitly declared above (custom fields, locale data, etc.)
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Fields { get; set; } = new Dictionary<string, JsonElement>();
    }
}
