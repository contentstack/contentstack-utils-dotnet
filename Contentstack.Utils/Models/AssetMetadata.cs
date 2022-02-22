using System;
using System.Collections.Generic;
using Contentstack.Utils.Converters;
using Newtonsoft.Json;

namespace Contentstack.Utils.Models
{
    [JsonConverter(typeof(AssetMetadataConverter))]
    public class AssetMetadata
    {
        public List<AssetExtension> extensions;
    }

    public class AssetExtension
    {
        public string Uid { get; set; }
        [JsonProperty(PropertyName = "local_metadata")]
        public ExtensionMetadata LocalMetadata;
        [JsonProperty(PropertyName = "global_metadata")]
        public ExtensionMetadata GlobalMetadata;
    }

    [JsonConverter(typeof(ExtensionMetadataConverter))]
    public class ExtensionMetadata
    {
        public List<Preset> Presets { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }

    public class Preset
    {
        public string Uid { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> Options { get; set; }
    }
}
