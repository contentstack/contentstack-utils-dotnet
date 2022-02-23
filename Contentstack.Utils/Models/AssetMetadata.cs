using System;
using System.Collections.Generic;
using Contentstack.Utils.Converters;
using Newtonsoft.Json;

namespace Contentstack.Utils.Models
{
    [JsonConverter(typeof(AssetMetadataConverter))]
    public class AssetMetadata
    {
        public List<AssetExtension> Extensions;
    }

    public class AssetExtension
    {
        public string Uid { get; set; }
        public List<ExtensionMetadata> ExtensionMetadata;
       
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
