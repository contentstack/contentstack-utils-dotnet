using Contentstack.Utils.Interfaces;

namespace Contentstack.Utils.Models
{
    public class Options: IRenderable
    {
        #region Internal Constructors
        internal Options()
        {

        }
        #endregion

        #region Public
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
                    string renderString = "<div><p>" + embeddedObject.Uid +"</p>";
                    if (embeddedObject is IEmbeddedEntry) {
                        renderString += "<p>Content type: <span>" + ((IEmbeddedEntry)embeddedObject).Title + "</span></p>";
                    } else if (embeddedObject is IEmbeddedContentTypeUid) {
                        renderString += "<p>Content type: <span>" + ((IEmbeddedContentTypeUid)embeddedObject).ContentTypeUid + "</span></p>";
                    }
                    renderString = renderString + "</div>";
                    return renderString;

                case Enums.StyleType.Inline:
                    if (embeddedObject is IEmbeddedEntry) {
                        return "<span>" + ((IEmbeddedEntry)embeddedObject).Title + "</span>";
                    }
                    return "<span>" + embeddedObject.Uid + "</span>";

                case Enums.StyleType.Link:
                    if (embeddedObject is IEmbeddedEntry) {
                        return "<a href=\"" + embeddedObject.Uid +"\">" + (metadata.Text ?? ((IEmbeddedEntry)embeddedObject).Title) + "</a>";
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
                    return "<a href=\"" + embeddedObject.Uid + "\">" + (metadata.Text ?? embeddedObject.Uid)+ "</a>";
            }
            return "";
        }
        #endregion
    }
}
