using System;
using Contentstack.Utils.Converters;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Newtonsoft.Json;

namespace Contentstack.Utils.Tests.Mocks
{
    public class GQLModel<T> where T: IEmbeddedObject
    {
        [Newtonsoft.Json.JsonConverter(typeof(RTEJsonConverter))]
        public JsonRTENodes<T> multiplerte { get; set; }
        public JsonRTENode<T> singlerte { get; set; }
    }

    [Newtonsoft.Json.JsonConverter(typeof(RTEJsonConverter))]
    public class EntryModel : IEmbeddedEntry
    {
        [JsonProperty("system.uid")]
        public string Uid
        {
            get;
            set;
        }
        [JsonProperty("system.content_type_uid")]
        public string ContentTypeUid
        {
            get;
            set;
        }
        [JsonProperty("title")]
        public string Title
        {
            get;
            set;
        }
    }

    [Newtonsoft.Json.JsonConverter(typeof(RTEJsonConverter))]
    public class AssetModel : IEmbeddedAsset
    {
        [JsonProperty("system.uid")]
        public string Uid
        {
            get;
            set;
        }
        [JsonProperty("system.content_type_uid")]
        public string ContentTypeUid
        {
            get;
            set;
        }
        [JsonProperty("title")]
        public string Title
        {
            get;
            set;
        }
        [JsonProperty("filename")]
        public string FileName
        {
            get;
            set;
        }
        [JsonProperty("url")]
        public string Url
        {
            get;
            set;
        }
    }
}

