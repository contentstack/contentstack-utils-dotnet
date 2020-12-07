using Contentstack.Utils.Models;
using Contentstack.Utils.Interfaces;
namespace Contentstack.Utils.Interfaces
{
    public interface IRenderable
    {
        string RenderOption(IEmbeddedObject entry, Metadata metadata);
    }
}
