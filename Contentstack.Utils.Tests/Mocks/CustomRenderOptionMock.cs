using System.Collections.Generic;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using HtmlAgilityPack;

namespace Contentstack.Utils.Tests.Mocks
{
    public class CustomRenderOptionMock : Options
    {
        public CustomRenderOptionMock(IEntryEmbedable entry) : base(entry)
        {
        }

        public override string RenderNode(string nodeType, Node node, NodeChildrenCallBack callBack)
        {
            switch (nodeType)
            {
                case "a":
                    if (node.attrs.ContainsKey("target"))
                    {
                        return $"<a href=\"{(string)node.attrs["url"]}\" target=\"{(string)node.attrs["target"]}\">{callBack(node.children)}</a>";
                    }
                    return $"<a href=\"{(string)node.attrs["url"]}\">{callBack(node.children)}</a>";
            }
            return base.RenderNode(nodeType, node, callBack);
        }

        public override string RenderOption(IEmbeddedObject embeddedObject, Metadata metadata)
        {
            var attributeStringList = new List<string>();
            foreach (var attribute in (HtmlAttributeCollection)metadata.attributes)
            {
                attributeStringList.Add($" {attribute.Name}=\"{attribute.Value}\"");
            }
            var attributeString = string.Join(" ", attributeStringList);
            switch (metadata.StyleType)
            {
                case Enums.StyleType.Block:
                    string renderString = "";
                    if (embeddedObject is IEmbeddedEntry)
                    {
                        renderString += $"<div {attributeString}> <b>{((IEmbeddedEntry)embeddedObject).Title}</b></div>";
                    }
                    else
                    {
                        renderString += $"<div {attributeString}> <b>{embeddedObject.Uid}</b></div>";
                    }
                    return renderString;

                case Enums.StyleType.Inline:
                    if (embeddedObject is IEmbeddedEntry)
                    {
                        return $"<span {attributeString}><b>{((IEmbeddedEntry)embeddedObject).Title}</b></span>";
                    }
                    else
                    {
                        return $"<span {attributeString}><b>{embeddedObject.Uid}</b></span>";
                    }
                case Enums.StyleType.Link:
                    if (embeddedObject is IEmbeddedEntry)
                    {
                        return $"<span> Please find link to: <a {attributeString}><b>{metadata.Text ?? ((IEmbeddedEntry)embeddedObject).Title}</b></a></span>";
                    }
                    else
                    {
                        return $"<span> Please find link to: <a {attributeString}><b>{metadata.Text ?? embeddedObject.Uid}</b></a></span>";
                    }
                case Enums.StyleType.Display:
                    if (embeddedObject is IEmbeddedAsset)
                    {
                        return $"<b>{((IEmbeddedAsset)embeddedObject).Title}</b><p>{((IEmbeddedAsset)embeddedObject).FileName} image: <img {attributeString} /></p>";
                    }
                    return "<img src=\"" + embeddedObject.Uid + "\" alt=\"" + embeddedObject.Uid + "\" />";

                case Enums.StyleType.Download:
                    if (embeddedObject is IEmbeddedAsset)
                    {
                        return "<span> Please find link to: <a href=\"" + ((IEmbeddedAsset)embeddedObject).Url + "\">" + (metadata.Text ?? ((IEmbeddedAsset)embeddedObject).Title) + "</a></span>";
                    }
                    return "<a href=\"" + embeddedObject.Uid + "\">" + (metadata.Text ?? embeddedObject.Uid) + "</a>";
            }
            return base.RenderOption(embeddedObject, metadata);
        }
    }
}
