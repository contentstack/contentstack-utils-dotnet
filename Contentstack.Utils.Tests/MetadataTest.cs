using Contentstack.Utils.Models;
using Xunit;
using HtmlAgilityPack;
using Contentstack.Utils.Tests.Helpers;
using Contentstack.Utils.Tests.Constants;
using System.Collections.Generic;

namespace Contentstack.Utils.Tests
{
    public class MetadataTest
    {
        HtmlDocument doc = new HtmlDocument();

        [Fact]
        public void testBlankAttributes()
        {
            var html = "<h1>TEST</h1>";
            doc.LoadHtml(html);
            var htmlNode = doc.DocumentNode.SelectNodes("//h1");
            foreach(var node in htmlNode)
            {
                Metadata metadata = node;
                Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                Assert.Equal(Enums.StyleType.Block, metadata.StyleType);
                Assert.Equal("", metadata.ItemUid);
                Assert.Equal("", metadata.ContentTypeUid);
                Assert.Equal("TEST", metadata.Text);
                Assert.Empty(((HtmlAttributeCollection)metadata.attributes));
                Assert.Equal(doc.DocumentNode.OuterHtml, metadata.OuterHTML);
            }
        }

        [Fact]
        public void testWrongAttributes()
        {
            var html = "<h1 type=\"\" sys-style-type=\"\" data-sys-entry-uid=\"\" data-sys-content-type-uid=\"\">TEST</h1>";
            doc.LoadHtml(html);
            var htmlNode = doc.DocumentNode.SelectNodes("//h1");
            foreach (var node in htmlNode)
            {
                Metadata metadata = node;
                Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                Assert.Equal(Enums.StyleType.Block, metadata.StyleType);
                Assert.Equal("", metadata.ItemUid);
                Assert.Equal("", metadata.ContentTypeUid);
                Assert.Equal("TEST", metadata.Text);
                Assert.Equal(4, ((HtmlAttributeCollection)metadata.attributes).Count);
                Assert.Equal(doc.DocumentNode.OuterHtml, metadata.OuterHTML);
            }
        }

        [Fact]
        public void testAttributes()
        {
            var html = "<h1 type=\"entry\" sys-style-type=\"inline\" data-sys-entry-uid=\"uid\" data-sys-content-type-uid=\"contentType\">"
                       + "TEST </ h1 >";
            doc.LoadHtml(html);
            var htmlNode = doc.DocumentNode.SelectNodes("//h1");
            foreach (var node in htmlNode)
            {
                Metadata metadata = node;
                Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                Assert.Equal(Enums.StyleType.Inline, metadata.StyleType);
                Assert.Equal("uid", metadata.ItemUid);
                Assert.Equal("contentType", metadata.ContentTypeUid);
                Assert.Equal("TEST ", metadata.Text);
                Assert.Equal(4, ((HtmlAttributeCollection)metadata.attributes).Count);
                Assert.Equal(doc.DocumentNode.OuterHtml, metadata.OuterHTML);
            }
        }

        [Fact]
        public void testAssetUidAttributes()
        {
            var html = "<h1 type=\"asset\" sys-style-type=\"display\" data-sys-asset-uid=\"assetuid\" >"
                       + "TEST </ h1 >";
            doc.LoadHtml(html);
            var htmlNode = doc.DocumentNode.SelectNodes("//h1");
            foreach (var node in htmlNode)
            {
                Metadata metadata = node;
                Assert.Equal(Enums.EmbedItemType.Asset, metadata.ItemType);
                Assert.Equal(Enums.StyleType.Display, metadata.StyleType);
                Assert.Equal("assetuid", metadata.ItemUid);
                Assert.Equal("", metadata.ContentTypeUid);
                Assert.Equal("TEST ", metadata.Text);
                Assert.Equal(3, ((HtmlAttributeCollection)metadata.attributes).Count);
                Assert.Equal(doc.DocumentNode.OuterHtml, metadata.OuterHTML);
            }
        }

        [Fact]
        public void Should_Parse_Reference_Asset_Node_To_Metadata ()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kAssetReferenceJson).children[0];
            Metadata metadata = node;
            Assert.Equal(Enums.EmbedItemType.Asset, metadata.ItemType);
            Assert.Equal(Enums.StyleType.Display, metadata.StyleType);
            Assert.Equal("blt44asset", metadata.ItemUid);
            Assert.Equal("sys_assets", metadata.ContentTypeUid);
            Assert.Equal("", metadata.Text);
            Assert.Equal(11, ((IDictionary<string, object>)metadata.attributes).Count);
            Assert.Equal("", metadata.OuterHTML);
        }
        [Fact]
        public void Should_Parse_Node_Doc_To_Metadata()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kAssetReferenceJson);
            Metadata metadata = node;
            Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
            Assert.Equal(Enums.StyleType.Block, metadata.StyleType);
            Assert.Equal("", metadata.ItemUid);
            Assert.Equal("", metadata.ContentTypeUid);
            Assert.Equal("", metadata.Text);
            Assert.Equal(0, ((IDictionary<string, object>)metadata.attributes).Count);
            Assert.Equal("", metadata.OuterHTML);
        }

        [Fact]
        public void Should_Parse_Reference_Entry_Block_Node_To_Metadata()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceBlockJson).children[0];
            Metadata metadata = node;
            Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
            Assert.Equal(Enums.StyleType.Block, metadata.StyleType);
            Assert.Equal("blttitleuid", metadata.ItemUid);
            Assert.Equal("content_block", metadata.ContentTypeUid);
            Assert.Equal("", metadata.Text);
            Assert.Equal(6, ((IDictionary<string, object>)metadata.attributes).Count);
            Assert.Equal("", metadata.OuterHTML);
        }

        [Fact]
        public void Should_Parse_Reference_Entry_Link_Node_To_Metadata()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceLinkJson).children[0];
            Metadata metadata = node;
            Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
            Assert.Equal(Enums.StyleType.Link, metadata.StyleType);
            Assert.Equal("bltemmbedEntryuid", metadata.ItemUid);
            Assert.Equal("embeddedrte", metadata.ContentTypeUid);
            Assert.Equal("/copy-of-entry-final-02", metadata.Text);
            Assert.Equal(8, ((IDictionary<string, object>)metadata.attributes).Count);
            Assert.Equal("", metadata.OuterHTML);
        }

        [Fact]
        public void Should_Parse_Reference_Entry_Inline_Node_To_Metadata()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kEntryReferenceInlineJson).children[0];
            Metadata metadata = node;
            Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
            Assert.Equal(Enums.StyleType.Inline, metadata.StyleType);
            Assert.Equal("blttitleUpdateuid", metadata.ItemUid);
            Assert.Equal("embeddedrte", metadata.ContentTypeUid);
            Assert.Equal("", metadata.Text);
            Assert.Equal(6, ((IDictionary<string, object>)metadata.attributes).Count);
            Assert.Equal("", metadata.OuterHTML);
        }
    }
}
