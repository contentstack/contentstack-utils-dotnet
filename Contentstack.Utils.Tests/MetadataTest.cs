using Contentstack.Utils.Models;
using Xunit;
using HtmlAgilityPack;
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
                Assert.Empty(metadata.attributes);
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
                Assert.Equal(4, metadata.attributes.Count);
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
                Assert.Equal(4, metadata.attributes.Count);
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
                Assert.Equal(3, metadata.attributes.Count);
                Assert.Equal(doc.DocumentNode.OuterHtml, metadata.OuterHTML);
            }
        }
    }
}
