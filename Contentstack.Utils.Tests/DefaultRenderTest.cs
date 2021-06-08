using Contentstack.Utils.Models;
using Xunit;
using Contentstack.Utils.Tests.Mocks;
using Contentstack.Utils.Enums;

namespace Contentstack.Utils.Tests
{
    public class DefaultRenderTest
    {
        Options defaultRender = new Options(new EmbeddedModel(""));
        Embedded embedded = new Embedded { Uid = "uid" };
        EmbeddedContentTypeUidModel embeddedContentType = new EmbeddedContentTypeUidModel();
        EmbeddedAssetModel embeddedAsset = new EmbeddedAssetModel();
        EmbeddedEntryModel embeddedEntryModel = new EmbeddedEntryModel();
        string text = "Text To set Link";

        Metadata getMetadata(EmbedItemType embedItemType = EmbedItemType.Entry,
            StyleType styleType = StyleType.Block,
            string text = null)
        {
            return new Metadata { ItemType = embedItemType, StyleType = styleType, ItemUid = "", ContentTypeUid = "", Text = text };
        }

        [Fact]
        public void testEmbedded()
        {
            string blockString = defaultRender.RenderOption(embedded, getMetadata());
            Assert.Equal(embedded.renderString(StyleType.Block), blockString);

            string inlineString = defaultRender.RenderOption(embedded, getMetadata(styleType: StyleType.Inline));
            Assert.Equal(embedded.renderString(StyleType.Inline), inlineString);

            string linkString = defaultRender.RenderOption(embedded, getMetadata(styleType: StyleType.Link));
            Assert.Equal(embedded.renderString(StyleType.Link), linkString);

            string displayString = defaultRender.RenderOption(embedded, getMetadata(styleType: StyleType.Display));
            Assert.Equal(embedded.renderString(StyleType.Display), displayString);

            string downloadString = defaultRender.RenderOption(embedded, getMetadata(styleType: StyleType.Download));
            Assert.Equal(embedded.renderString(StyleType.Download), downloadString);

        }

        [Fact]
        public void testEmbeddedWithText()
        {
            string blockString = defaultRender.RenderOption(embedded, getMetadata(text: text));
            Assert.Equal(embedded.renderString(StyleType.Block, text), blockString);

            string inlineString = defaultRender.RenderOption(embedded, getMetadata(styleType: StyleType.Inline, text: text));
            Assert.Equal(embedded.renderString(StyleType.Inline, text), inlineString);

            string linkString = defaultRender.RenderOption(embedded, getMetadata(styleType: StyleType.Link, text: text));
            Assert.Equal(embedded.renderString(StyleType.Link, text), linkString);

            string displayString = defaultRender.RenderOption(embedded, getMetadata(styleType: StyleType.Display, text: text));
            Assert.Equal(embedded.renderString(StyleType.Display, text), displayString);

            string downloadString = defaultRender.RenderOption(embedded, getMetadata(styleType: StyleType.Download, text: text));
            Assert.Equal(embedded.renderString(StyleType.Download, text), downloadString);

        }

        [Fact]
        public void testEmbeddedContentTypeEntry()
        {
            string blockString = defaultRender.RenderOption(embeddedContentType, getMetadata());
            Assert.Equal(embeddedContentType.renderString(StyleType.Block), blockString);

            string inlineString = defaultRender.RenderOption(embeddedContentType, getMetadata(styleType: StyleType.Inline));
            Assert.Equal(embeddedContentType.renderString(StyleType.Inline), inlineString);

            string linkString = defaultRender.RenderOption(embeddedContentType, getMetadata(styleType: StyleType.Link));
            Assert.Equal(embeddedContentType.renderString(StyleType.Link), linkString);

        }

        [Fact]
        public void testEmbeddedContentTypeEntryWithText()
        {
            string blockString = defaultRender.RenderOption(embeddedContentType, getMetadata(text: text));
            Assert.Equal(embeddedContentType.renderString(StyleType.Block, text), blockString);

            string inlineString = defaultRender.RenderOption(embeddedContentType, getMetadata(styleType: StyleType.Inline, text: text));
            Assert.Equal(embeddedContentType.renderString(StyleType.Inline, text), inlineString);

            string linkString = defaultRender.RenderOption(embeddedContentType, getMetadata(styleType: StyleType.Link, text: text));
            Assert.Equal(embeddedContentType.renderString(StyleType.Link, text), linkString);

        }

        [Fact]
        public void testEmbeddedEntry()
        {
            string blockString = defaultRender.RenderOption(embeddedEntryModel, getMetadata());
            Assert.Equal(embeddedEntryModel.renderString(StyleType.Block), blockString);

            string inlineString = defaultRender.RenderOption(embeddedEntryModel, getMetadata(styleType: StyleType.Inline));
            Assert.Equal(embeddedEntryModel.renderString(StyleType.Inline), inlineString);

            string linkString = defaultRender.RenderOption(embeddedEntryModel, getMetadata(styleType: StyleType.Link));
            Assert.Equal(embeddedEntryModel.renderString(StyleType.Link), linkString);

        }

        [Fact]
        public void testEmbeddedEntryWithText()
        {
            string blockString = defaultRender.RenderOption(embeddedEntryModel, getMetadata(text: text));
            Assert.Equal(embeddedEntryModel.renderString(StyleType.Block, text), blockString);

            string inlineString = defaultRender.RenderOption(embeddedEntryModel, getMetadata(styleType: StyleType.Inline, text: text));
            Assert.Equal(embeddedEntryModel.renderString(StyleType.Inline, text), inlineString);

            string linkString = defaultRender.RenderOption(embeddedEntryModel, getMetadata(styleType: StyleType.Link, text: text));
            Assert.Equal(embeddedEntryModel.renderString(StyleType.Link, text), linkString);

        }


        [Fact]
        public void testEmbeddedAsset()
        {
            string displayString = defaultRender.RenderOption(embeddedAsset, getMetadata(styleType: StyleType.Display));
            Assert.Equal(embeddedAsset.renderString(StyleType.Display), displayString);

            string downloadString = defaultRender.RenderOption(embeddedAsset, getMetadata(styleType: StyleType.Download));
            Assert.Equal(embeddedAsset.renderString(StyleType.Download), downloadString);
        }

        [Fact]
        public void testEmbeddedAssetWithText()
        {
            string displayString = defaultRender.RenderOption(embeddedAsset, getMetadata(styleType: StyleType.Display, text: text));
            Assert.Equal(embeddedAsset.renderString(StyleType.Display, text), displayString);

            string downloadString = defaultRender.RenderOption(embeddedAsset, getMetadata(styleType: StyleType.Download, text: text));
            Assert.Equal(embeddedAsset.renderString(StyleType.Download, text), downloadString);
        }

        [Fact]
        public void testRenderMark()
        {
            string boldString = defaultRender.RenderMark(MarkType.Bold, text);
            Assert.Equal("<strong>"+text+"</strong>", boldString);
            string italicString = defaultRender.RenderMark(MarkType.Italic, text: text);
            Assert.Equal("<em>"+text+"</em>", italicString);
            string underlineString = defaultRender.RenderMark(MarkType.Underline, text: text);
            Assert.Equal("<u>"+text+"</u>", underlineString);
            string strickthroughString = defaultRender.RenderMark(MarkType.Strikethrough, text: text);
            Assert.Equal("<strike>"+text+"</strike>", strickthroughString);
            string inlineCodeString = defaultRender.RenderMark(MarkType.InlineCode, text: text);
            Assert.Equal("<span>"+text+"</span>", inlineCodeString);
            string subscriptString = defaultRender.RenderMark(MarkType.Subscript, text: text);
            Assert.Equal("<sub>"+text+"</sub>", subscriptString);
            string superscriptString = defaultRender.RenderMark(MarkType.Superscript, text: text);
            Assert.Equal("<sup>"+text+"</sup>", superscriptString);
        }
    }
}