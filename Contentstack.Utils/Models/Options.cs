using System;
using System.Collections.Generic;
using Contentstack.Utils.Enums;
using Contentstack.Utils.Interfaces;
using System.Text.Json;
using Contentstack.Utils;

namespace Contentstack.Utils.Models
{
    public class Options : IRenderable
    {
        #region #region Public
        public Options() { }

        public Options(IEntryEmbedable entry)
        {
            this.entry = entry;
        }

        public IEntryEmbedable entry;

        public virtual string RenderOption(IEmbeddedObject embeddedObject, Metadata metadata)
        {
            string uid = HtmlEscaper.Text(embeddedObject.Uid);
            string linkText = EmbeddedLinkText(metadata);
            switch (metadata.StyleType)
            {
                case Enums.StyleType.Block:
                    string renderString = "<div><p>" + uid + "</p>";
                    if (embeddedObject is IEmbeddedEntry)
                    {
                        renderString += "<p>Content type: <span>" + HtmlEscaper.Text(((IEmbeddedEntry)embeddedObject).Title) + "</span></p>";
                    }
                    else
                    {
                        renderString += "<p>Content type: <span>" + HtmlEscaper.Text(embeddedObject.ContentTypeUid) + "</span></p>";
                    }
                    renderString = renderString + "</div>";
                    return renderString;

                case Enums.StyleType.Inline:
                    if (embeddedObject is IEmbeddedEntry)
                    {
                        return "<span>" + HtmlEscaper.Text(((IEmbeddedEntry)embeddedObject).Title) + "</span>";
                    }
                    return "<span>" + uid + "</span>";

                case Enums.StyleType.Link:
                    if (embeddedObject is IEmbeddedEntry)
                    {
                        return "<a href=\"" + HtmlEscaper.LinkUrl(embeddedObject.Uid) + "\">" + (linkText ?? HtmlEscaper.Text(((IEmbeddedEntry)embeddedObject).Title)) + "</a>";
                    }
                    return "<a href=\"" + HtmlEscaper.LinkUrl(embeddedObject.Uid) + "\">" + (linkText ?? uid) + "</a>";

                case Enums.StyleType.Display:
                    if (embeddedObject is IEmbeddedAsset)
                    {
                        return "<img src=\"" + HtmlEscaper.ImageUrl(((IEmbeddedAsset)embeddedObject).Url) + "\" alt=\"" + HtmlEscaper.Attribute(((IEmbeddedAsset)embeddedObject).Title) + "\" />";
                    }
                    return "<img src=\"" + HtmlEscaper.ImageUrl(embeddedObject.Uid) + "\" alt=\"" + HtmlEscaper.Attribute(embeddedObject.Uid) + "\" />";

                case Enums.StyleType.Download:
                    if (embeddedObject is IEmbeddedAsset)
                    {
                        return "<a href=\"" + HtmlEscaper.LinkUrl(((IEmbeddedAsset)embeddedObject).Url) + "\">" + (linkText ?? HtmlEscaper.Text(((IEmbeddedAsset)embeddedObject).Title)) + "</a>";
                    }
                    return "<a href=\"" + HtmlEscaper.LinkUrl(embeddedObject.Uid) + "\">" + (linkText ?? uid) + "</a>";
            }
            return "";
        }

        /// <summary>
        /// Text shown inside an embedded link. For HTML RTE content the text comes from the parsed
        /// markup and may already contain entities, so it is decoded once before being encoded.
        /// Returns null when the metadata carries no text.
        /// </summary>
        private static string EmbeddedLinkText(Metadata metadata)
        {
            if (metadata.Text == null)
                return null;
            string text = string.IsNullOrEmpty(metadata.OuterHTML) ? metadata.Text : HtmlAgilityPack.HtmlEntity.DeEntitize(metadata.Text);
            return HtmlEscaper.Text(text);
        }

        public virtual string RenderMark(MarkType markType, string text, string className = "", string id = "")
        {
            switch (markType)
            {
                case MarkType.Bold:
                    return $"<strong>{text}</strong>";
                case MarkType.Italic:
                    return $"<em>{text}</em>";
                case MarkType.Underline:
                    return $"<u>{text}</u>";
                case MarkType.Strikethrough:
                    return $"<strike>{text}</strike>";
                case MarkType.InlineCode:
                    return $"<span>{text}</span>";
                case MarkType.Subscript:
                    return $"<sub>{text}</sub>";
                case MarkType.Superscript:
                    return $"<sup>{text}</sup>";
                case MarkType.Id:
                case MarkType.Class:
                    string classAttr = !string.IsNullOrEmpty(className) ? $" class=\"{HtmlEscaper.Attribute(className)}\"" : "";
                    string idAttr = !string.IsNullOrEmpty(id) ? $" id=\"{HtmlEscaper.Attribute(id)}\"" : "";
                    return $"<span{classAttr}{idAttr}>{text}</span>";
            }
            return text;
        }

        public virtual string RenderNode(string nodeType, Node node, NodeChildrenCallBack callBack)
        {
            string href = "";
            string styleAttrs = "";
            string target = "";
            string title = "";

            if (node.attrs?.ContainsKey("style") == true)
            {
                var styleVal = node.attrs["style"];
                if (styleVal != null)
                {
                    if (styleVal is string styleStr)
                    {
                        styleAttrs = $" style=\"{HtmlEscaper.Attribute(styleStr)}\"";
                    }
                    else if (JsonAttrValue.TryGetStyleObject(styleVal, out Dictionary<string, string> styleDictionary) && styleDictionary != null)
                    {
                        styleAttrs = " style=\"";
                        foreach (var pair in styleDictionary)
                        {
                            styleAttrs += $"{HtmlEscaper.Attribute(pair.Key)}:{HtmlEscaper.Attribute(pair.Value)};";
                        }
                        styleAttrs += "\"";
                    }
                }
            }
            switch (nodeType)
            {
                case "p":
                    return $"<p{styleAttrs}>{callBack(node.children)}</p>";
                case "a":
                    if (node.attrs?.ContainsKey("url")==true)
                    {
                        href = HtmlEscaper.LinkUrl(JsonAttrValue.AsString(node.attrs["url"]));
                    }
                    if (node.attrs?.ContainsKey("target") == true)
                    {
                        target = HtmlEscaper.Attribute(JsonAttrValue.AsString(node.attrs["target"]));
                    }
                    if (node.attrs?.ContainsKey("title") == true)
                    {
                        title = HtmlEscaper.Attribute(JsonAttrValue.AsString(node.attrs["title"]));
                    }
                    return $"<a href=\"{href}\"  target=\"{target}\" title=\"{title}\" {styleAttrs}>{callBack(node.children)}</a>";

                case "img":
                    if (node.attrs.ContainsKey("url"))
                    {
                        href = HtmlEscaper.ImageUrl(JsonAttrValue.AsString(node.attrs["url"]));
                    }
                    return $"<img{styleAttrs} src=\"{href}\" />{callBack(node.children)}";
                case "embed":
                    if (node.attrs.ContainsKey("url"))
                    {
                        href = HtmlEscaper.FrameUrl(JsonAttrValue.AsString(node.attrs["url"]));
                    }
                    return $"<iframe{styleAttrs} src=\"{href}\">{callBack(node.children)}</iframe>";
                case "fragment":
                    return $"<fragment{styleAttrs}>{callBack(node.children)}</fragment>";
                case "h1":
                    return $"<h1{styleAttrs}>{callBack(node.children)}</h1>";
                case "h2":
                    return $"<h2{styleAttrs}>{callBack(node.children)}</h2>";
                case "h3":
                    return $"<h3{styleAttrs}>{callBack(node.children)}</h3>";
                case "h4":
                    return $"<h4{styleAttrs}>{callBack(node.children)}</h4>";
                case "h5":
                    return $"<h5{styleAttrs}>{callBack(node.children)}</h5>";
                case "h6":
                    return $"<h6{styleAttrs}>{callBack(node.children)}</h6>";
                case "ol":
                    return $"<ol{styleAttrs}>{callBack(node.children)}</ol>";
                case "ul":
                    return $"<ul{styleAttrs}>{callBack(node.children)}</ul>";
                case "li":
                    return $"<li{styleAttrs}>{callBack(node.children)}</li>";
                case "hr":
                    return $"<hr>";
                case "table":
                    return $"<table{styleAttrs}>{callBack(node.children)}</table>";
                case "thead":
                    return $"<thead{styleAttrs}>{callBack(node.children)}</thead>";
                case "tbody":
                    return $"<tbody{styleAttrs}>{callBack(node.children)}</tbody>";
                case "tfoot":
                    return $"<tfoot{styleAttrs}>{callBack(node.children)}</tfoot>";
                case "tr":
                    return $"<tr{styleAttrs}>{callBack(node.children)}</tr>";
                case "th":
                    return $"<th{styleAttrs}>{callBack(node.children)}</th>";
                case "td":
                    return $"<td{styleAttrs}>{callBack(node.children)}</td>";
                case "blockquote":
                    return $"<blockquote>{callBack(node.children)}</blockquote>";
                case "code":
                    return $"<code{styleAttrs}>{callBack(node.children)}</code>";
                default:
                    return callBack(node.children);
            }
            #endregion
        }
    }
}
