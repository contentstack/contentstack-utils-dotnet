using Xunit;
using Contentstack.Utils.Tests.Mocks;
using System.Collections.Generic;
using Contentstack.Utils.Interfaces;

namespace Contentstack.Utils.Tests
{
    public class UtilsTest
    {
        DefaultRenderMock defaultRender = new DefaultRenderMock(new EmbeddedModel(""));
        [Fact]
        public void testRenderBlankString()
        {
            string result = Utils.RenderContent("", defaultRender);
            Assert.Equal("", result);
        }

        [Fact]
        public void testRenderString()
        {
            string renderString = "<h1>TEST </h2>";
            string result = Utils.RenderContent(renderString, defaultRender);
            Assert.Equal("<h1>TEST </h1>", result);
        }

        [Fact]
        public void testNonHtmlString()
        {
            string result = Utils.RenderContent(Constants.Constants.kNoHTML, defaultRender);
            Assert.Equal(Constants.Constants.kNoHTML, result);
        }

        [Fact]
        public void testHtmlString()
        {
            string result = Utils.RenderContent(Constants.Constants.kSimpleHTML, defaultRender);
            Assert.Equal(Constants.Constants.kSimpleHTML, result);
        }

        [Fact]
        public void testUnexpectedClose()
        {
            string result = Utils.RenderContent(Constants.Constants.kUnexpectedClose, defaultRender);
            Assert.Equal("<span>uid</span>", result);
        }

        [Fact]
        public void testNoChildmodel()
        {
            string result = Utils.RenderContent(Constants.Constants.kNoChildNode, defaultRender);
            Assert.Equal("<span>uid</span>", result);
        }

        [Fact]
        public void testAssetDisplay()
        {
            string result = Utils.RenderContent(Constants.Constants.kAssetDisplay, new DefaultRenderMock(new EmbeddedModel("", embedAssetUID:"UID_01")));
            Assert.Equal("<img src=\"url\" alt=\"title\" />", result);
        }

        [Fact]
        public void testEntryBlock()
        {
            string result = Utils.RenderContent(Constants.Constants.kEntryBlock, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<div><p>UID_01</p><p>Content type: <span>article</span></p></div>", result);
        }

        [Fact]
        public void testEntryInline()
        {
            string result = Utils.RenderContent(Constants.Constants.kEntryInline, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<span>UID_01</span>", result);
        }

        [Fact]
        public void testEntryLink()
        {
            string result = Utils.RenderContent(Constants.Constants.kEntryLink, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<a href=\"UID_01\">{{title}}</a>", result);
        }

        [Fact]
        public void testAssetAll ()
        {
            string result = Utils.RenderContent(Constants.Constants.kAssetEmbed, new DefaultRenderMock(new EmbeddedModel("", "UID_01")));
            Assert.Equal("<p></p><p></p>", result);

            var embModel = new EmbeddedModel("");
            embModel.embeddedItems = new Dictionary<string, List<IEmbeddedObject>>()
            {
                ["rte"] = new List<IEmbeddedObject> {
                    new EmbeddedAssetModel { Uid = "UID_02" },
                    new EmbeddedAssetModel { Uid = "UID_03" }
                }
            };

            result = Utils.RenderContent(Constants.Constants.kAssetEmbed, new DefaultRenderMock(embModel));
            Assert.Equal("<img src=\"url\" alt=\"title\" /><p></p><p></p><img src=\"url\" alt=\"title\" />", result);
        }

        [Fact]
        public void testEntryBlockLink()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}", defaultRender);
            Assert.Equal("", result);

            result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}", new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<div><p>UID_01</p><p>Content type: <span>article</span></p></div><a href=\"UID_01\">{{title}}</a>", result);
        }

        [Fact]
        public void testEntryBlockLinkInline()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", defaultRender);
            Assert.Equal(" ", result);

            result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}  {Constants.Constants.kEntryInline}", new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<div><p>UID_01</p><p>Content type: <span>article</span></p></div><a href=\"UID_01\">{{title}}</a>  <span>UID_01</span>", result);
        }

        [Fact]
        public void testAllEmbedStyles()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", defaultRender);
            Assert.Equal(" ", result);

            result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article")));
            Assert.Equal("<div><p>UID_01</p><p>Content type: <span>article</span></p></div><a href=\"UID_01\">{{title}}</a> <span>UID_01</span>", result);
        }

        [Fact]
        public void testAllEmbedStylesWithAllEmbedObject()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", defaultRender);
            Assert.Equal(" ", result);

            result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "UID_01", contentTypeUid: "article", embedAssetUID: "UID_01")));
            Assert.Equal("<img src=\"url\" alt=\"title\" /><div><p>UID_01</p><p>Content type: <span>article</span></p></div><a href=\"UID_01\">{{title}}</a> <span>UID_01</span>", result);
        }
    }
}
