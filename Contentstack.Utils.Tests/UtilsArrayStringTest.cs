using Xunit;
using Contentstack.Utils.Tests.Mocks;
using System.Collections.Generic;
using Contentstack.Utils.Interfaces;

namespace Contentstack.Utils.Tests
{
    public class UtilsArrayStringTest
    {
        DefaultRenderMock defaultRender = new DefaultRenderMock(new EmbeddedModel(""));
        [Fact]
        public void testRenderBlankString()
        {
            List<string> result = Utils.RenderContent(new List<string>() { "" }, defaultRender);
            Assert.Equal("", result[0]);
        }

        [Fact]
        public void testRenderString()
        {
            string renderString = "<h1>TEST </h2>";
            List<string> result = Utils.RenderContent(new List<string>() { renderString }, defaultRender);
            Assert.Equal("<h1>TEST </h1>", result[0]);
        }

        [Fact]
        public void testNonHtmlString()
        {
            List<string> result = Utils.RenderContent(new List<string>() { Constants.Constants.kNoHTML }, defaultRender);
            Assert.Equal(Constants.Constants.kNoHTML, result[0]);
        }

        [Fact]
        public void testHtmlString()
        {
            List<string> result = Utils.RenderContent(new List<string>() { Constants.Constants.kSimpleHTML }, defaultRender);
            Assert.Equal(Constants.Constants.kSimpleHTML, result[0]);
        }

        [Fact]
        public void testUnexpectedClose()
        {
            List<string> result = Utils.RenderContent(new List<string>() { Constants.Constants.kUnexpectedClose }, defaultRender);
            Assert.Equal("<span>uid</span>", result[0]);
        }

        [Fact]
        public void testNoChildmodel()
        {
            List<string> result = Utils.RenderContent(new List<string>() { Constants.Constants.kNoChildNode }, defaultRender);
            Assert.Equal("<span>uid</span>", result[0]);
        }

        [Fact]
        public void testAssetDisplay()
        {
            List<string> result = Utils.RenderContent(new List<string>() { Constants.Constants.kAssetDisplay }, new DefaultRenderMock(new EmbeddedModel("", embedAssetUID: "blt55f6d8cbd7e03a1f")));
            Assert.Equal("<img src=\"url\" alt=\"title\" />", result[0]);
        }

        [Fact]
        public void testEntryBlock()
        {
            List<string> result = Utils.RenderContent(new List<string>() { Constants.Constants.kEntryBlock }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div>", result[0]);
        }

        [Fact]
        public void testEntryInline()
        {
            List<string> result = Utils.RenderContent(new List<string>() { Constants.Constants.kEntryInline }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<span>blt55f6d8cbd7e03a1f</span>", result[0]);
        }

        [Fact]
        public void testEntryLink()
        {
            List<string> result = Utils.RenderContent(new List<string>() { Constants.Constants.kEntryLink }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a>", result[0]);
        }

        [Fact]
        public void testAssetAll()
        {
            List<string> result = Utils.RenderContent(new List<string>() { Constants.Constants.kAssetEmbed }, new DefaultRenderMock(new EmbeddedModel("", "blt55f6d8cbd7e03a1f")));
            Assert.Equal("<p></p><p></p>", result[0]);

            var embModel = new EmbeddedModel("");
            embModel.embeddedItems = new Dictionary<string, List<Interfaces.IEmbeddedObject>>()
            {
                ["rte"] = new List<IEmbeddedObject> {
                    new EmbeddedAssetModel { Uid = "blt8d49bb742bcf2c83" },
                    new EmbeddedAssetModel { Uid = "blt120a5a04d91c9466" }
                }
            };

            result = Utils.RenderContent(new List<string>() { Constants.Constants.kAssetEmbed }, new DefaultRenderMock(embModel));
            Assert.Equal("<img src=\"url\" alt=\"title\" /><p></p><p></p><img src=\"url\" alt=\"title\" />", result[0]);
        }

        [Fact]
        public void testEntryBlockLink()
        {
            List<string> result = Utils.RenderContent(new List<string>() { $"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}" }, defaultRender);
            Assert.Equal("", result[0]);

            result = Utils.RenderContent(new List<string>() { $"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}" }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div><a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a>", result[0]);
        }

        [Fact]
        public void testEntryBlockLinkInline()
        {
            List<string> result = Utils.RenderContent(new List<string>() { $"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}" }, defaultRender);
            Assert.Equal(" ", result[0]);

            result = Utils.RenderContent(new List<string>() { $"{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink}  {Constants.Constants.kEntryInline}" }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div><a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a>  <span>blt55f6d8cbd7e03a1f</span>", result[0]);
        }

        [Fact]
        public void testAllEmbedStyles()
        {
            List<string> result = Utils.RenderContent(new List<string>() { $"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}" }, defaultRender);
            Assert.Equal(" ", result[0]);

            result = Utils.RenderContent(new List<string>() { $"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}" }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div><a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a> <span>blt55f6d8cbd7e03a1f</span>", result[0]);
        }

        [Fact]
        public void testAllEmbedStylesWithAllEmbedObject()
        {
            List<string> result = Utils.RenderContent(new List<string>() { $"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}" }, defaultRender);
            Assert.Equal(" ", result[0]);

            result = Utils.RenderContent(new List<string>() { $"{Constants.Constants.kAssetDisplay}{Constants.Constants.kEntryBlock}{Constants.Constants.kEntryLink} {Constants.Constants.kEntryInline}" }, new DefaultRenderMock(new EmbeddedModel("", embedContentUID: "blt55f6d8cbd7e03a1f", embedAssetUID: "blt55f6d8cbd7e03a1f", contentTypeUid: "article")));
            Assert.Equal("<img src=\"url\" alt=\"title\" /><div><p>blt55f6d8cbd7e03a1f</p><p>Content type: <span>article</span></p></div><a href=\"blt55f6d8cbd7e03a1f\">{{title}}</a> <span>blt55f6d8cbd7e03a1f</span>", result[0]);
        }
    }
}
