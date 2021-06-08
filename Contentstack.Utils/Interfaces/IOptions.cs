using Contentstack.Utils.Models;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Enums;

namespace Contentstack.Utils.Interfaces
{
    public interface IRenderable
    {
        string RenderOption(IEmbeddedObject entry, Metadata metadata);
        string RenderMark(MarkType markType, string text);
    }
}
