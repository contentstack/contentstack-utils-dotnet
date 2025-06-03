using System;
using System.Runtime.CompilerServices;
using Contentstack.Utils.Enums;
using HtmlAgilityPack;
using System.Text.Json;

[assembly: InternalsVisibleTo("Contentstack.Utils.Tests")]
namespace Contentstack.Utils.Models
{
    public struct Metadata
    {
        /// <summary>
        /// This will specify the type of Embedded object 
        /// </summary>
        public EmbedItemType ItemType;

        /// <summary>
        /// This will specify the style type of Embedded object 
        /// </summary>
        public StyleType StyleType;

        /// <summary>
        /// Uid of Embedded object 
        /// </summary>
        public string ItemUid;

        /// <summary>
        /// Content type for the Embedded object
        /// </summary>
        public string ContentTypeUid;

        /// <summary>
        /// Text wrapped in embed tag
        /// </summary>
        public string Text;

        /// <summary>
        /// Attributes collection for embed tag
        /// </summary>
        public object attributes;

        /// <summary>
        /// Html string of embed tag
        /// </summary>
        internal string OuterHTML;

        public static implicit operator Metadata(HtmlNode node)
        {
            StyleType styleType;
            if (node.Attributes["sys-style-type"] == null || !(Enum.TryParse(node.Attributes["sys-style-type"].Value, true, out styleType)))
            {
                styleType = StyleType.Block;
            }

            EmbedItemType embedItemType;
            if (node.Attributes["type"] == null || !(Enum.TryParse(node.Attributes["type"].Value, true, out embedItemType)))
            {
                embedItemType = EmbedItemType.Entry;
            }

            return new Metadata() {
                Text = node.InnerText ?? string.Empty,
                OuterHTML = node.OuterHtml ?? "",
                StyleType = styleType,
                ItemType = embedItemType,
                ItemUid = node.Attributes["data-sys-entry-uid"] != null ? node.Attributes["data-sys-entry-uid"].Value : (node.Attributes["data-sys-asset-uid"] != null ? node.Attributes["data-sys-asset-uid"].Value : ""),
                ContentTypeUid = node.Attributes["data-sys-content-type-uid"] != null ? node.Attributes["data-sys-content-type-uid"].Value : "",
                attributes = node.Attributes
            };
        }

        public static implicit operator Metadata(Node node) 
        {
            StyleType styleType;
            string GetAttrString(string key)
            {
                if (!node.attrs.ContainsKey(key) || node.attrs[key] == null) return "";
                var val = node.attrs[key];
                if (val is string s) return s;
                if (val is JsonElement je && je.ValueKind == JsonValueKind.String) return je.GetString();
                return val.ToString();
            }
            if (!node.attrs.ContainsKey("display-type") || !(Enum.TryParse(GetAttrString("display-type"), true, out styleType)))
            {
                styleType = StyleType.Block;
            }
            EmbedItemType embedItemType;
            if (!node.attrs.ContainsKey("type") || !(Enum.TryParse(GetAttrString("type"), true, out embedItemType)))
            {
                embedItemType = EmbedItemType.Entry;
            }
            string text = "";
            if (node.children != null && node.children.Count > 0 && node.children[0].GetType() == typeof(TextNode))
            {
                text = ((TextNode)node.children[0]).text ?? string.Empty;
            }
            string itemUID = "";
            if (node.attrs.ContainsKey("entry-uid"))
            {
                itemUID = GetAttrString("entry-uid");
            }else if (node.attrs.ContainsKey("asset-uid"))
            {
                itemUID = GetAttrString("asset-uid");
            }
            return new Metadata()
            {
                Text = text,
                OuterHTML = "",
                StyleType = styleType,
                ItemType = embedItemType,
                ItemUid = itemUID,
                ContentTypeUid = node.attrs.ContainsKey("content-type-uid") ? GetAttrString("content-type-uid") : "",
                attributes = node.attrs
            };

        }
    }
}