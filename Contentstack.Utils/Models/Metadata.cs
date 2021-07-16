using System;
using System.Runtime.CompilerServices;
using Contentstack.Utils.Enums;
using HtmlAgilityPack;

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
                Text = node.InnerText ?? "",
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
            if (!node.attrs.ContainsKey("display-type") || !(Enum.TryParse((string)node.attrs["display-type"], true, out styleType)))
            {
                styleType = StyleType.Block;
            }

            EmbedItemType embedItemType;
            if (!node.attrs.ContainsKey("type") || !(Enum.TryParse((string)node.attrs["type"], true, out embedItemType)))
            {
                embedItemType = EmbedItemType.Entry;
            }
            string text = "";
            if (node.children != null && node.children.Count > 0 && node.children[0].GetType() == typeof(TextNode))
            {
                text = ((TextNode)node.children[0]).text;
            }
            string itemUID = "";
            if (node.attrs.ContainsKey("entry-uid"))
            {
                itemUID = (string)node.attrs["entry-uid"];
            }else if (node.attrs.ContainsKey("asset-uid"))
            {
                itemUID = (string)node.attrs["asset-uid"];
            }

            return new Metadata()
            {
                Text = text,
                OuterHTML = "",
                StyleType = styleType,
                ItemType = embedItemType,
                ItemUid = itemUID,
                ContentTypeUid = node.attrs.ContainsKey("content-type-uid") ? (string)node.attrs["content-type-uid"] : "",
                attributes = node.attrs
            };

        }
    }
}