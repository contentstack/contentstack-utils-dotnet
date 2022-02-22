using System;
using Contentstack.Utils.Models;
using Newtonsoft.Json;

namespace Contentstack.Utils.Tests.Mocks
{
    public class AssetMetaModel
    {
        public string Uid { get; set; }
        public string ContentTypeUid { get; set; }
        public string Url { get; set; }
        [JsonProperty(PropertyName = "file_size")]
        public string FileSize { get; set; }
        [JsonProperty(PropertyName = "_metadata")]
        public AssetMetadata assetMetadata;
    }
}
