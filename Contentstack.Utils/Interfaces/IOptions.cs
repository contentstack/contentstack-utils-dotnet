using Contentstack.Utils.Models;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Enums;
using System.Collections.Generic;

namespace Contentstack.Utils.Interfaces
{
    public delegate string NodeChildrenCallBack(List<Node> nodes);

    public interface IRenderable
    {
        string RenderOption(IEmbeddedObject entry, Metadata metadata);
        string RenderMark(MarkType markType, string text);
        string RenderNode(NodeType nodeType, Node node, NodeChildrenCallBack callBack);
    }
}
