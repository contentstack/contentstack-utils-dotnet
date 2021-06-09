using Contentstack.Utils.Enums;
using Contentstack.Utils.Interfaces;

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

        public virtual string RenderMark(MarkType markType, string text)
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
            }
            return text;
        }

        public virtual string RenderNode(NodeType nodeType, Node node, NodeChildrenCallBack callBack)
        {
            string href = "";

            switch (nodeType)
            {
                case NodeType.Paragraph:
                    return $"<p>{callBack(node.children)}</p>";
                case NodeType.Link:
                    if (node.attrs.ContainsKey("url"))
                    {
                        href = (string)node.attrs["url"];
                    }
                    return $"<a href=\"{href}\">{callBack(node.children)}</a>";
                case NodeType.Image:
                    if (node.attrs.ContainsKey("url"))
                    {
                        href = (string)node.attrs["url"];
                    }
                    return $"<img src=\"{href}\" />{callBack(node.children)}";
                case NodeType.Embed:
                    if (node.attrs.ContainsKey("url"))
                    {
                        href = (string)node.attrs["url"];
                    }
                    return $"<iframe src=\"{href}\">{callBack(node.children)}</iframe>";
                case NodeType.Heading_1:
                    return $"<h1>{callBack(node.children)}</h1>";
                case NodeType.Heading_2:
                    return $"<h2>{callBack(node.children)}</h2>";
                case NodeType.Heading_3:
                    return $"<h3>{callBack(node.children)}</h3>";
                case NodeType.Heading_4:
                    return $"<h4>{callBack(node.children)}</h4>";
                case NodeType.Heading_5:
                    return $"<h5>{callBack(node.children)}</h5>";
                case NodeType.Heading_6:
                    return $"<h6>{callBack(node.children)}</h6>";
                case NodeType.OrderList:
                    return $"<ol>{callBack(node.children)}</ol>";
                case NodeType.UnOrderList:
                    return $"<ul>{callBack(node.children)}</ul>";
                case NodeType.ListItem:
                    return $"<li>{callBack(node.children)}</li>";
                case NodeType.Hr:
                    return $"<hr>";
                case NodeType.Table:
                    return $"<table>{callBack(node.children)}</table>";
                case NodeType.TableHeader:
                    return $"<thead>{callBack(node.children)}</thead>";
                case NodeType.TableBody:
                    return $"<tbody>{callBack(node.children)}</tbody>";
                case NodeType.TableFooter:
                    return $"<tfoot>{callBack(node.children)}</tfoot>";
                case NodeType.TableRow:
                    return $"<tr>{callBack(node.children)}</tr>";
                case NodeType.TableHead:
                    return $"<th>{callBack(node.children)}</th>";
                case NodeType.TableData:
                    return $"<td>{callBack(node.children)}</td>";
                case NodeType.BlockQuote:
                    return $"<blockquote>{callBack(node.children)}</blockquote>";
                case NodeType.Code:
                    return $"<code>{callBack(node.children)}</code>";
                default:
                    return callBack(node.children);
            }
            #endregion
        }
    }
}
