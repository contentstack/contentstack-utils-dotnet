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
            string result = Utils.RenderContent(Constants.Constants.kAssetDisplay, new DefaultRenderMock(new EmbeddedModel("", embedAssetUID:"blt55f6d8cbd7e03a1f")));
            Assert.Equal("<img src=\"url\" alt=\"title\" />", result);
        }

        [Fact]
        public void testEntryBlock()
        {
            string result = Utils.RenderContent(Constants.Constants.kEntryBlock, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div>", result);
        }

        [Fact]
        public void testEntryInline()
        {
            string result = Utils.RenderContent(Constants.Constants.kEntryInline, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<span>blt55f6d8cbd7e03a1f</span>", result);
        }

        [Fact]
        public void testEntryLink()
        {
            string result = Utils.RenderContent(Constants.Constants.kEntryLink, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a>", result);
        }

        [Fact]
        public void testAssetAll ()
        {
            string result = Utils.RenderContent(Constants.Constants.kAssetEmbed, new DefaultRenderMock(new EmbeddedModel("", "blt55f6d8cbd7e03a1f")));
            Assert.Equal("<p></p><p></p>", result);

            var embModel = new EmbeddedModel("");
            embModel.embeddedItems = new Dictionary<string, List<IEmbeddedObject>>()
            {
                ["rte"] = new List<IEmbeddedObject> {
                    new EmbeddedAssetModel { Uid = "blt8d49bb742bcf2c83" },
                    new EmbeddedAssetModel { Uid = "blt120a5a04d91c9466" }
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

            result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}", new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div><a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a>", result);
        }

        [Fact]
        public void testEntryBlockLinkInline()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", defaultRender);
            Assert.Equal(" ", result);

            result = Utils.RenderContent($"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}  {Constants.Constants.kEntryInline}", new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div><a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a>  <span>blt55f6d8cbd7e03a1f</span>", result);
        }

        [Fact]
        public void testAllEmbedStyles()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", defaultRender);
            Assert.Equal(" ", result);

            result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div><a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a> <span>blt55f6d8cbd7e03a1f</span>", result);
        }

        [Fact]
        public void testAllEmbedStylesWithAllEmbedObject()
        {
            string result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", defaultRender);
            Assert.Equal(" ", result);

            result = Utils.RenderContent($"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}", new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article", embedAssetUID: "blt55f6d8cbd7e03a1f")));
            Assert.Equal("<img src=\"url\" alt=\"title\" /><div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div><a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a> <span>blt55f6d8cbd7e03a1f</span>", result);
        }
    }
}
