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
        public HtmlAttributeCollection attributes;

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
    }
}