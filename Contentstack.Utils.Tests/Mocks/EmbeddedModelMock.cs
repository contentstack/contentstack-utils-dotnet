using System.Collections.Generic;
using Contentstack.Utils.Enums;
using Contentstack.Utils.Interfaces;
namespace Contentstack.Utils.Tests.Mocks
{
    public class EmbeddedModel : IEntryEmbedable
    {
        string rte;

        public Dictionary<string, List<IEmbeddedContentTypeUid>> embeddedEntries { get; set; }
        public Dictionary<string, List<IEmbeddedAsset>> embeddedAssets { get; set; }

        public EmbeddedModel(string rte, string embedContentUID = "uid", string embedAssetUID = "uid")
        {
            this.rte = rte;
            embeddedEntries = new Dictionary<string, List<IEmbeddedContentTypeUid>>()
            {
                ["rte"] = new List<IEmbeddedContentTypeUid> {
                    new EmbeddedContentTypeUidModel { Uid = embedContentUID }
                }
            };
            embeddedAssets = new Dictionary<string, List<IEmbeddedAsset>>()
            {
                ["rte"] = new List<IEmbeddedAsset> {
                    new EmbeddedAssetModel { Uid = embedAssetUID }
                }
            };
        }

    }

    class Embedded : IEmbeddedObject
    {
        public string Uid { get; set; }

        public string renderString(StyleType styleType, string text = null){
            switch (styleType) {
                case StyleType.Block:
                    return "<div><p>" + (this.Uid) + "</p></div>";
                case StyleType.Inline:
                    return "<span>" + this.Uid + "</span>";
                case StyleType.Link:
                    return "<a href=\"" + this.Uid + "\">" + (text ?? this.Uid) + "</a>";
                case StyleType.Display:
                    return "<img src=\""+ this.Uid + "\" alt=\"" + this.Uid + "\" />";
                case StyleType.Download:
                    return "<a href=\"" + this.Uid + "\">" + (text ?? this.Uid) + "</a>";
            }
            return "";
        }
    }


    public class EmbeddedContentTypeUidModel: IEmbeddedContentTypeUid
    {
        public string ContentTypeUid
        {
            get {
                return "contentTypeUid";
            }
            set
            {

            }
        }
        public string Uid { get; set; }

        public string renderString(StyleType styleType, string text = null)
        {
            switch (styleType)
            {
                case StyleType.Block:
                    return "<div><p>" + this.Uid + "</p><p>Content type: <span>" + this.ContentTypeUid + "</span></p></div>";
                case StyleType.Inline:
                    return "<span>" + this.Uid + "</span>";
                case StyleType.Link:
                    return "<a href=\"" + this.Uid + "\">" + (text ?? this.Uid) + "</a>";
                default:
                    break;
            }
            return "";
        }
    }

    public class EmbeddedEntryModel : IEmbeddedContentTypeUid, IEmbeddedEntry
    {
        public string Uid { get; set; }

        public string ContentTypeUid {
            get
            {
                return "contentTypeUid";
            }
            set
            {

            }
        }

        public string Title
        {
            get
            {
                return "title";
            }
            set
            {
            }
        }

        public string renderString(StyleType styleType, string text = null)
        {
            switch (styleType)
            {
                case StyleType.Block:
                    return "<div><p>" + this.Uid + "</p><p>Content type: <span>" + this.Title + "</span></p></div>";
                case StyleType.Inline:
                    return "<span>" + this.Title + "</span>";
                case StyleType.Link:
                    return "<a href=\"" + this.Uid + "\">" + (text ?? this.Title) + "</a>";
                default:
                    break;
            }
            return "";
        }
    }

    public class EmbeddedAssetModel : IEmbeddedAsset
    {
        public string Title
        {
            get
            {
                return "title";
            }
            set
            {
            }
        }

        public string FileName
        {
            get
            {
                return "filename";
            }
            set
            {
            }
        }

        public string Url
        {
            get
            {
                return "url";
            }
            set
            {
            }
        }

        public string Uid { get; set; }

        public string renderString(StyleType styleType, string text = null)
        {
            switch (styleType)
            {
                case StyleType.Display:
                    return "<img src=\"" + this.Url + "\" alt=\"" + this.Title + "\" />";
                case StyleType.Download:
                    return "<a href=\"" + this.Url + "\">" + (text ?? this.Title) + "</a>";
                default:
                    break;
            }
            return "";
        }
    }
}
