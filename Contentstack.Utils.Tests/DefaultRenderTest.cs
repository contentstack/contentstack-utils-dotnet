using Contentstack.Utils.Models;
using Xunit;
using Contentstack.Utils.Tests.Mocks;
using Contentstack.Utils.Enums;
using Contentstack.Utils.Tests.Helpers;
using Contentstack.Utils.Tests.Constants;

namespace Contentstack.Utils.Tests
{
    public class DefaultRenderTest
    {
        Options defaultRender = new Options(new EmbeddedModel(""));
        Embedded embedded = new Embedded { Uid = "uid" };
        EmbeddedContentTypeUidModel embeddedContentType = new EmbeddedContentTypeUidModel();
        EmbeddedAssetModel embeddedAsset = new EmbeddedAssetModel();
        EmbeddedEntryModel embeddedEntryModel = new EmbeddedEntryModel();

        Node node = NodeParser.parse(JsonToHtmlConstants.kBlankDocument);

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

        [Fact]
        public void testParagraphDocument ()
        {
            string result = defaultRender.RenderNode(NodeType.Paragraph, node, (nodes) => { return text; });

            Assert.Equal($"<p>{text}</p>", result);
        }

        [Fact]
        public void testLinkhDocument()
        {
            Node nodeLink = NodeParser.parse(JsonToHtmlConstants.kLinkInPJson).children[0].children[1];

            string result = defaultRender.RenderNode(NodeType.Link, nodeLink, (nodes) => { return text; });

            Assert.Equal($"<a href=\"{nodeLink.attrs["url"]}\">{text}</a>", result);
        }

        [Fact]
        public void testImageDocument()
        {
            string result = defaultRender.RenderNode(NodeType.Image, node, (nodes) => { return text; });

            Assert.Equal($"<img src=\"\" />{text}", result);
        }

        [Fact]
        public void testEmbedDocument()
        {
            string result = defaultRender.RenderNode(NodeType.Embed, node, (nodes) => { return text; });

            Assert.Equal($"<iframe src=\"\">{text}</iframe>", result);
        }

        [Fact]
        public void testH1Document()
        {
            string result = defaultRender.RenderNode(NodeType.Heading_1, node, (nodes) => { return text; });

            Assert.Equal($"<h1>{text}</h1>", result);
        }

        [Fact]
        public void testH2Document()
        {
            string result = defaultRender.RenderNode(NodeType.Heading_2, node, (nodes) => { return text; });

            Assert.Equal($"<h2>{text}</h2>", result);
        }

        [Fact]
        public void testH3Document()
        {
            string result = defaultRender.RenderNode(NodeType.Heading_3, node, (nodes) => { return text; });

            Assert.Equal($"<h3>{text}</h3>", result);
        }

        [Fact]
        public void testH4Document()
        {
            string result = defaultRender.RenderNode(NodeType.Heading_4, node, (nodes) => { return text; });

            Assert.Equal($"<h4>{text}</h4>", result);
        }

        [Fact]
        public void testH5Document()
        {
            string result = defaultRender.RenderNode(NodeType.Heading_5, node, (nodes) => { return text; });

            Assert.Equal($"<h5>{text}</h5>", result);
        }

        [Fact]
        public void testH6Document()
        {
            string result = defaultRender.RenderNode(NodeType.Heading_6, node, (nodes) => { return text; });

            Assert.Equal($"<h6>{text}</h6>", result);
        }

        [Fact]
        public void testOrderListDocument()
        {
            string result = defaultRender.RenderNode(NodeType.OrderList, node, (nodes) => { return text; });

            Assert.Equal($"<ol>{text}</ol>", result);
        }

        [Fact]
        public void testUnOrderListDocument()
        {
            string result = defaultRender.RenderNode(NodeType.UnOrderList, node, (nodes) => { return text; });

            Assert.Equal($"<ul>{text}</ul>", result);
        }

        [Fact]
        public void testListItemDocument()
        {
            string result = defaultRender.RenderNode(NodeType.ListItem, node, (nodes) => { return text; });

            Assert.Equal($"<li>{text}</li>", result);
        }

        [Fact]
        public void testHRDocument()
        {
            string result = defaultRender.RenderNode(NodeType.Hr, node, (nodes) => { return text; });

            Assert.Equal($"<hr>", result);
        }

        [Fact]
        public void testTableDocument()
        {
            string result = defaultRender.RenderNode(NodeType.Table, node, (nodes) => { return text; });

            Assert.Equal($"<table>{text}</table>", result);
        }

        [Fact]
        public void testTableHeaderDocument()
        {
            string result = defaultRender.RenderNode(NodeType.TableHeader, node, (nodes) => { return text; });

            Assert.Equal($"<thead>{text}</thead>", result);
        }

        [Fact]
        public void testTableFooterDocument()
        {
            string result = defaultRender.RenderNode(NodeType.TableFooter, node, (nodes) => { return text; });

            Assert.Equal($"<tfoot>{text}</tfoot>", result);
        }

        [Fact]
        public void testTableBodyDocument()
        {
            string result = defaultRender.RenderNode(NodeType.TableBody, node, (nodes) => { return text; });

            Assert.Equal($"<tbody>{text}</tbody>", result);
        }

        [Fact]
        public void testTableRowDocument()
        {
            string result = defaultRender.RenderNode(NodeType.TableRow, node, (nodes) => { return text; });

            Assert.Equal($"<tr>{text}</tr>", result);
        }

        [Fact]
        public void testTableHeadDocument()
        {
            string result = defaultRender.RenderNode(NodeType.TableHead, node, (nodes) => { return text; });

            Assert.Equal($"<th>{text}</th>", result);
        }

        [Fact]
        public void testTableDataDocument()
        {
            string result = defaultRender.RenderNode(NodeType.TableData, node, (nodes) => { return text; });

            Assert.Equal($"<td>{text}</td>", result);
        }

        [Fact]
        public void testBlockQuoteDocument()
        {
            string result = defaultRender.RenderNode(NodeType.BlockQuote, node, (nodes) => { return text; });

            Assert.Equal($"<blockquote>{text}</blockquote>", result);
        }

        [Fact]
        public void testCodeDocument()
        {
            string result = defaultRender.RenderNode(NodeType.Code, node, (nodes) => { return text; });

            Assert.Equal($"<code>{text}</code>", result);
        }

        [Fact]
        public void testDocument()
        {
            string result = defaultRender.RenderNode(NodeType.Document, node, (nodes) => { return text; });

            Assert.Equal(text, result);
        }
    }
}