using System;
using System.Text.Json.Serialization;
using Contentstack.Utils.Converters;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;

namespace Contentstack.Utils.Tests.Mocks
{
    public class GQLModel<T> where T: IEmbeddedObject
    {
        [JsonConverter(typeof(RTEJsonConverter))]
        public JsonRTENodes<T> multiplerte { get; set; }
        public JsonRTENode<T> singlerte { get; set; }
    }

    [JsonConverter(typeof(RTEJsonConverter))]
    public class EntryModel : IEmbeddedEntry
    {
        [JsonPropertyName("system.uid")]
        public string Uid { get; set; }
        [JsonPropertyName("system.content_type_uid")]
        public string ContentTypeUid { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    [JsonConverter(typeof(RTEJsonConverter))]
    public class AssetModel : IEmbeddedAsset
    {
        [JsonPropertyName("system.uid")]
        public string Uid { get; set; }
        [JsonPropertyName("system.content_type_uid")]
        public string ContentTypeUid { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("filename")]
        public string FileName { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}

