using System;
using System.Collections.Generic;
using Contentstack.Utils.Enums;
using Contentstack.Utils.Interfaces;
using Newtonsoft.Json.Linq;

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
            switch (metadata.StyleType)
            {
                case Enums.StyleType.Block:
                    string renderString = "<div><p>" + embeddedObject.Uid + "</p>";
                    if (embeddedObject is IEmbeddedEntry)
                    {
                        renderString += "<p>Content type: <span>" + ((IEmbeddedEntry)embeddedObject).Title + "</span></p>";
                    }
                    else
                    {
                        renderString += "<p>Content type: <span>" + embeddedObject.ContentTypeUid + "</span></p>";
                    }
                    renderString = renderString + "</div>";
                    return renderString;

                case Enums.StyleType.Inline:
                    if (embeddedObject is IEmbeddedEntry)
                    {
                        return "<span>" + ((IEmbeddedEntry)embeddedObject).Title + "</span>";
                    }
                    return "<span>" + embeddedObject.Uid + "</span>";

                case Enums.StyleType.Link:
                    if (embeddedObject is IEmbeddedEntry)
                    {
                        return "<a href=\"" + embeddedObject.Uid + "\">" + (metadata.Text ?? ((IEmbeddedEntry)embeddedObject).Title) + "</a>";
                    }
                    return "<a href=\"" + embeddedObject.Uid + "\">" + (metadata.Text ?? embeddedObject.Uid) + "</a>";

                case Enums.StyleType.Display:
                    if (embeddedObject is IEmbeddedAsset)
                    {
                        return "<img src=\"" + ((IEmbeddedAsset)embeddedObject).Url + "\" alt=\"" + ((IEmbeddedAsset)embeddedObject).Title + "\" />";
                    }
                    return "<img src=\"" + embeddedObject.Uid + "\" alt=\"" + embeddedObject.Uid + "\" />";

                case Enums.StyleType.Download:
                    if (embeddedObject is IEmbeddedAsset)
                    {
                        return "<a href=\"" + ((IEmbeddedAsset)embeddedObject).Url + "\">" + (metadata.Text ?? ((IEmbeddedAsset)embeddedObject).Title) + "</a>";
                    }
                    return "<a href=\"" + embeddedObject.Uid + "\">" + (metadata.Text ?? embeddedObject.Uid) + "</a>";
            }
            return "";
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
                    string classAttr = !string.IsNullOrEmpty(className) ? $" class=\"{className}\"" : "";
                    string idAttr = !string.IsNullOrEmpty(id) ? $" id=\"{id}\"" : "";
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
                    if (styleVal is string)
                    {
                        styleAttrs = $" style=\"{styleVal}\"";
                    }
                    else if (styleVal is JObject)
                    {
                        var styleObject = (JObject)styleVal;
                        var styleDictionary = styleObject.ToObject<Dictionary<string, string>>();
                        styleAttrs = " style=\"";
                        foreach (var pair in styleDictionary)
                        {
                            styleAttrs += $"{pair.Key}:{pair.Value};";
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
                        href = (string)node.attrs["url"];
                    }
                    if (node.attrs?.ContainsKey("target") == true)
                    {
                        target = (string)node.attrs["target"];
                    }
                    if (node.attrs?.ContainsKey("title") == true)
                    {
                        title = (string)node.attrs["title"];
                    }
                    return $"<a href=\"{href}\"  target=\"{target}\" title=\"{title}\" {styleAttrs}>{callBack(node.children)}</a>";

                case "img":
                    if (node.attrs.ContainsKey("url"))
                    {
                        href = (string)node.attrs["url"];
                    }
                    return $"<img{styleAttrs} src=\"{href}\" />{callBack(node.children)}";
                case "embed":
                    if (node.attrs.ContainsKey("url"))
                    {
                        href = (string)node.attrs["url"];
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
