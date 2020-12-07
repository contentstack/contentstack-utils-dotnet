using Xunit;
using Contentstack.Utils.Extensions;
using Contentstack.Utils.Models;
using HtmlAgilityPack;
using System;

namespace Contentstack.Utils.Tests
{
    public class HtmlDocumentExtensionTest
    {
        HtmlDocument doc = new HtmlDocument();

        [Fact]
        public void testBlankString()
        {
            doc.LoadHtml(Constants.Constants.kBlankString);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                Assert.True(false, "Should not call on non xml string");
            });
        }

        [Fact]
        public void testNonHtmlString()
        {
            doc.LoadHtml(Constants.Constants.kNoHTML);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                Assert.True(false, "Should not call on non xml string");
            });
        }

        [Fact]
        public void testHtmlString()
        {
            doc.LoadHtml(Constants.Constants.kSimpleHTML);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                Assert.True(false, "Should not call on non xml string");
            });
        }

        [Fact]
        public void testUnexpectedClose()
        {
            doc.LoadHtml(Constants.Constants.kUnexpectedClose);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                Assert.Equal(Enums.EmbedItemType.Asset, metadata.ItemType);
                Assert.Equal("uid", metadata.ItemUid);
                Assert.Equal("data-sys-content-type-uid", metadata.ContentTypeUid);
                Assert.Equal(Enums.StyleType.Inline, metadata.StyleType);
                Assert.Equal("", metadata.Text);
            });
        }

        [Fact]
        public void testNoChildmodel()
        {
            doc.LoadHtml(Constants.Constants.kNoChildNode);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                Assert.Equal(Enums.EmbedItemType.Asset, metadata.ItemType);
                Assert.Equal("uid", metadata.ItemUid);
                Assert.Equal("data-sys-content-type-uid", metadata.ContentTypeUid);
                Assert.Equal(Enums.StyleType.Inline, metadata.StyleType);
                Assert.Equal("", metadata.Text);
            });
        }

        [Fact]
        public void testAssetDisplay()
        {
            doc.LoadHtml(Constants.Constants.kAssetDisplay);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                Assert.Equal(Enums.EmbedItemType.Asset, metadata.ItemType);
                Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                Assert.Equal("", metadata.ContentTypeUid);
                Assert.Equal(Enums.StyleType.Display, metadata.StyleType);
                Assert.Equal("", metadata.Text);
            });
        }

        [Fact]
        public void kEntryBlock()
        {
            doc.LoadHtml(Constants.Constants.kEntryBlock);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                Assert.Equal("article", metadata.ContentTypeUid);
                Assert.Equal(Enums.StyleType.Block, metadata.StyleType);
                Assert.Equal("{{title}}", metadata.Text);
            });
        }

        [Fact]
        public void testEntryInline()
        {
            doc.LoadHtml(Constants.Constants.kEntryInline);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                Assert.Equal("article", metadata.ContentTypeUid);
                Assert.Equal(Enums.StyleType.Inline, metadata.StyleType);
                Assert.Equal("{{title}}", metadata.Text);
            });
        }

        [Fact]
        public void testEntryLink()
        {
            doc.LoadHtml(Constants.Constants.kEntryLink);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                Assert.Equal("article", metadata.ContentTypeUid);
                Assert.Equal(Enums.StyleType.Link, metadata.StyleType);
                Assert.Equal("{{title}}", metadata.Text);
            });
        }

        [Fact]
        public void testAsset()
        {
            doc.LoadHtml(Constants.Constants.kAssetEmbed);
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                if (metadata.ItemUid == "blt8d49bb742bcf2c83")
                {
                    Assert.Equal(Enums.EmbedItemType.Asset, metadata.ItemType);
                    Assert.Equal("blt8d49bb742bcf2c83", metadata.ItemUid);
                    Assert.Equal("", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Display, metadata.StyleType);
                    Assert.Equal("", metadata.Text);
                    Assert.Equal("blt8d49bb742bcf2c83", metadata.attributes["data-sys-asset-uid"].Value);
                    Assert.Equal("Cuvier-67_Autruche_d_Afrique.jpg", metadata.attributes["data-sys-asset-alt"].Value);
                    Assert.Equal("http://abc.com", metadata.attributes["data-sys-asset-link"].Value);
                    Assert.Equal("https://images.contentstack.com/v3/assets/blt77263d300aee3e6b/blt8d49bb742bcf2c83/5f744bfcb3d3d20813386c10/clitud.jpeg", metadata.attributes["data-sys-asset-filelink"].Value);
                    Assert.Equal("center", metadata.attributes["data-sys-asset-position"].Value);
                    Assert.Equal("somecaption", metadata.attributes["data-sys-asset-caption"].Value);
                    Assert.Equal("image/jpeg", metadata.attributes["data-sys-asset-contenttype"].Value);
                    Assert.Equal("true", metadata.attributes["data-sys-asset-isnewtab"].Value);
                    Assert.Equal("Cuvier-67_Autruche_d_Afrique.jpg", metadata.attributes["data-sys-asset-filename"].Value);
                }
            });
        }

        [Fact]
        public void testEntryBlockLink()
        {
            int callbackCount = 0;
            doc.LoadHtml($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}");
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                callbackCount++;
                if (metadata.StyleType == Enums.StyleType.Link)
                {
                    Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                    Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                    Assert.Equal("article", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Link, metadata.StyleType);
                    Assert.Equal("{{title}}", metadata.Text);
                }
                else
                {
                    Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                    Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                    Assert.Equal("article", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Block, metadata.StyleType);
                    Assert.Equal("{{title}}", metadata.Text);
                }
            });
            Assert.Equal(2, callbackCount);
        }

        [Fact]
        public void testEntryBlockLinkInline()
        {
            int callbackCount = 0;
            doc.LoadHtml($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}{Constants.Constants.kEntryInline}");
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                callbackCount++;
                if (metadata.StyleType == Enums.StyleType.Link)
                {
                    Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                    Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                    Assert.Equal("article", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Link, metadata.StyleType);
                    Assert.Equal("{{title}}", metadata.Text);
                }
                else if (metadata.StyleType == Enums.StyleType.Inline)
                {
                    Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                    Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                    Assert.Equal("article", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Inline, metadata.StyleType);
                    Assert.Equal("{{title}}", metadata.Text);
                }
                else
                {
                    Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                    Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                    Assert.Equal("article", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Block, metadata.StyleType);
                    Assert.Equal("{{title}}", metadata.Text);
                }
            });
            Assert.Equal(3, callbackCount);
        }

        [Fact]
        public void testAllEmbedStyles()
        {
            int callbackCount = 0;
            doc.LoadHtml($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}{Constants.Constants.kEntryInline}");
            doc.FindEmbeddedObject((Metadata metadata) =>
            {
                callbackCount++;
                if (metadata.StyleType == Enums.StyleType.Display)
                {
                    Assert.Equal(Enums.EmbedItemType.Asset, metadata.ItemType);
                    Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                    Assert.Equal("", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Display, metadata.StyleType);
                    Assert.Equal("", metadata.Text);
                }
                else if(metadata.StyleType == Enums.StyleType.Link)
                {
                    Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                    Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                    Assert.Equal("article", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Link, metadata.StyleType);
                    Assert.Equal("{{title}}", metadata.Text);
                }
                else if (metadata.StyleType == Enums.StyleType.Inline)
                {
                    Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                    Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                    Assert.Equal("article", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Inline, metadata.StyleType);
                    Assert.Equal("{{title}}", metadata.Text);
                }
                else
                {
                    Assert.Equal(Enums.EmbedItemType.Entry, metadata.ItemType);
                    Assert.Equal("blt55f6d8cbd7e03a1f", metadata.ItemUid);
                    Assert.Equal("article", metadata.ContentTypeUid);
                    Assert.Equal(Enums.StyleType.Block, metadata.StyleType);
                    Assert.Equal("{{title}}", metadata.Text);
                }
            });
            Assert.Equal(4, callbackCount);
        }
    }
}
