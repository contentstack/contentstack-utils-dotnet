using Contentstack.Utils.Enums;
using Contentstack.Utils.Interfaces;

namespace Contentstack.Utils.Models
{
    public class Options: IRenderable
    {
        #region #region Public
        public Options() {}
        
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
                    } else {
                        renderString += "<p>Content type: <span>" + embeddedObject.ContentTypeUid + "</span></p>";
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

        public string RenderMark(MarkType markType, string text)
        {
            switch (markType) {
                case MarkType.Bold:
                    return "<strong>"+text+"</strong>";
                case MarkType.Italic:
                    return "<em>"+text+"</em>";
                case MarkType.Underline:
                    return "<u>"+text+"</u>";
                case MarkType.Strikethrough:
                    return "<strike>"+text+"</strike>";
                case MarkType.InlineCode:
                    return "<span>"+text+"</span>";
                case MarkType.Subscript:
                    return "<sub>" + text + "</sub>";
                case MarkType.Superscript:
                    return "<sup>"+text+"</sup>";
            }
            return text;
        }
        #endregion
    }
}
