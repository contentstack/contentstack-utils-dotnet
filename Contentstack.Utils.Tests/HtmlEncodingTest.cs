using System.Collections.Generic;
using Contentstack.Utils.Enums;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Contentstack.Utils.Tests.Helpers;
using Contentstack.Utils.Tests.Mocks;
using Xunit;

namespace Contentstack.Utils.Tests
{
    /// <summary>
    /// Content values must be rendered as text or quoted attribute values, never as markup,
    /// and links/media may only use safe URL schemes.
    /// </summary>
    public class HtmlEncodingTest
    {
        private readonly Options options = new Options();

        private static string Doc(string children)
        {
            return "{ \"uid\":\"doc_uid\", \"type\":\"doc\", \"attrs\":{}, \"children\":[" + children + "] }";
        }

        private string Render(string children)
        {
            return Utils.JsonToHtml(NodeParser.parse(Doc(children)), options);
        }

        [Fact]
        public void Text_Is_Encoded()
        {
            var result = Render("{\"type\":\"p\",\"attrs\":{},\"children\":[{\"text\":\"<script>alert(1)</script> & co\"}]}");
            Assert.Equal("<p>&lt;script&gt;alert(1)&lt;/script&gt; &amp; co</p>", result);
        }

        [Fact]
        public void Text_With_Marks_Is_Encoded()
        {
            var result = Render("{\"type\":\"p\",\"attrs\":{},\"children\":[{\"text\":\"<b>x</b>\",\"bold\":true}]}");
            Assert.Equal("<p><strong>&lt;b&gt;x&lt;/b&gt;</strong></p>", result);
        }

        [Fact]
        public void Plain_Text_Is_Unchanged()
        {
            var result = Render("{\"type\":\"p\",\"attrs\":{},\"children\":[{\"text\":\"Don't \\\"quote\\\" me\"}]}");
            Assert.Equal("<p>Don't \"quote\" me</p>", result);
        }

        [Fact]
        public void ClassName_And_Id_Are_Attribute_Encoded()
        {
            var result = Render("{\"type\":\"p\",\"attrs\":{},\"children\":[{\"text\":\"t\",\"classname\":\"a\\\" onclick=\\\"x\",\"id\":\"i'd\"}]}");
            Assert.Equal("<p><span class=\"a&quot; onclick=&quot;x\" id=\"i&#39;d\">t</span></p>", result);
        }

        [Theory]
        [InlineData("javascript:alert(1)")]
        [InlineData("JaVaScRiPt:alert(1)")]
        [InlineData(" javascript:alert(1)")]
        [InlineData("java\\tscript:alert(1)")]
        [InlineData("java\\nscript:alert(1)")]
        [InlineData("\\u0001javascript:alert(1)")]
        [InlineData("vbscript:msgbox(1)")]
        [InlineData("data:text/html,<script>alert(1)</script>")]
        public void Link_With_Unsafe_Scheme_Is_Dropped(string url)
        {
            var result = Render("{\"type\":\"a\",\"attrs\":{\"url\":\"" + url + "\"},\"children\":[{\"text\":\"x\"}]}");
            Assert.Equal("<a href=\"#\"  target=\"\" title=\"\" >x</a>", result);
        }

        [Theory]
        [InlineData("https://example.com/?a=1&b=2", "https://example.com/?a=1&amp;b=2")]
        [InlineData("http://example.com", "http://example.com")]
        [InlineData("/relative/path", "/relative/path")]
        [InlineData("#section", "#section")]
        [InlineData("mailto:support@contentstack.com", "mailto:support@contentstack.com")]
        [InlineData("tel:+1-555-0100", "tel:+1-555-0100")]
        [InlineData("sms:+15550100", "sms:+15550100")]
        [InlineData("ftp://files.example.com/a.zip", "ftp://files.example.com/a.zip")]
        [InlineData("page:2", "#")]
        [InlineData("", "")]
        public void Link_Url_Is_Filtered_And_Encoded(string url, string expectedHref)
        {
            var result = Render("{\"type\":\"a\",\"attrs\":{\"url\":\"" + url + "\"},\"children\":[{\"text\":\"x\"}]}");
            Assert.Equal("<a href=\"" + expectedHref + "\"  target=\"\" title=\"\" >x</a>", result);
        }

        [Fact]
        public void Link_Title_And_Target_Cannot_Break_Out_Of_Attribute()
        {
            var result = Render("{\"type\":\"a\",\"attrs\":{\"url\":\"https://example.com\",\"title\":\"\\\" onmouseover=\\\"alert(1)\",\"target\":\"_blank\\\" onfocus=\\\"alert(1)\"},\"children\":[{\"text\":\"x\"}]}");
            Assert.Equal("<a href=\"https://example.com\"  target=\"_blank&quot; onfocus=&quot;alert(1)\" title=\"&quot; onmouseover=&quot;alert(1)\" >x</a>", result);
        }

        [Fact]
        public void Image_Url_Cannot_Inject_Attributes()
        {
            var result = Render("{\"type\":\"img\",\"attrs\":{\"url\":\"x\\\" onerror=\\\"alert(1)\"},\"children\":[{\"text\":\"\"}]}");
            Assert.Equal("<img src=\"x&quot; onerror=&quot;alert(1)\" />", result);
        }

        [Theory]
        [InlineData("javascript:alert(1)", "")]
        [InlineData("https://images.contentstack.io/v3/a.png", "https://images.contentstack.io/v3/a.png")]
        [InlineData("data:image/png;base64,iVBORw0KGgo=", "data:image/png;base64,iVBORw0KGgo=")]
        [InlineData("data:text/html;base64,PHNjcmlwdD4=", "")]
        public void Image_Url_Scheme_Is_Filtered(string url, string expectedSrc)
        {
            var result = Render("{\"type\":\"img\",\"attrs\":{\"url\":\"" + url + "\"},\"children\":[{\"text\":\"\"}]}");
            Assert.Equal("<img src=\"" + expectedSrc + "\" />", result);
        }

        [Theory]
        [InlineData("javascript:alert(1)", "")]
        [InlineData("data:text/html,<script>alert(1)</script>", "")]
        [InlineData("https://www.youtube.com/embed/abc", "https://www.youtube.com/embed/abc")]
        public void Embed_Url_Scheme_Is_Filtered(string url, string expectedSrc)
        {
            var result = Render("{\"type\":\"embed\",\"attrs\":{\"url\":\"" + url + "\"},\"children\":[{\"text\":\"\"}]}");
            Assert.Equal("<iframe src=\"" + expectedSrc + "\"></iframe>", result);
        }

        [Fact]
        public void Style_String_Cannot_Break_Out_Of_Attribute()
        {
            var paragraph = new Node
            {
                type = "p",
                attrs = new Dictionary<string, object> { ["style"] = "color:red\" onmouseover=\"alert(1)" },
                children = new List<Node> { new TextNode { type = "text", text = "s" } }
            };

            var result = options.RenderNode("p", paragraph, nodes => "s");
            Assert.Equal("<p style=\"color:red&quot; onmouseover=&quot;alert(1)\">s</p>", result);
        }

        [Fact]
        public void Style_Object_Cannot_Break_Out_Of_Attribute()
        {
            var result = Render("{\"type\":\"p\",\"attrs\":{\"style\":{\"color\":\"red\\\" onmouseover=\\\"alert(1)\"}},\"children\":[{\"text\":\"s\"}]}");
            Assert.Equal("<p style=\"color:red&quot; onmouseover=&quot;alert(1);\">s</p>", result);
        }

        [Fact]
        public void Embedded_Entry_Title_Is_Encoded()
        {
            var entry = new UnsafeEntry { Uid = "uid", ContentTypeUid = "ct", Title = "<img src=x onerror=alert(1)>" };

            Assert.Equal("<span>&lt;img src=x onerror=alert(1)&gt;</span>",
                options.RenderOption(entry, new Metadata { StyleType = StyleType.Inline }));
            Assert.Equal("<div><p>uid</p><p>Content type: <span>&lt;img src=x onerror=alert(1)&gt;</span></p></div>",
                options.RenderOption(entry, new Metadata { StyleType = StyleType.Block }));
            Assert.Equal("<a href=\"uid\">&lt;img src=x onerror=alert(1)&gt;</a>",
                options.RenderOption(entry, new Metadata { StyleType = StyleType.Link }));
        }

        [Fact]
        public void Embedded_Link_Text_Is_Encoded()
        {
            var entry = new UnsafeEntry { Uid = "uid", ContentTypeUid = "ct", Title = "t" };
            Assert.Equal("<a href=\"uid\">&lt;script&gt;alert(1)&lt;/script&gt;</a>",
                options.RenderOption(entry, new Metadata { StyleType = StyleType.Link, Text = "<script>alert(1)</script>" }));
        }

        [Fact]
        public void Embedded_Asset_Url_And_Title_Are_Encoded()
        {
            var asset = new UnsafeAsset { Uid = "asset_uid", ContentTypeUid = "sys_assets", Title = "\"><script>alert(1)</script>", Url = "javascript:alert(1)" };

            Assert.Equal("<img src=\"\" alt=\"&quot;&gt;&lt;script&gt;alert(1)&lt;/script&gt;\" />",
                options.RenderOption(asset, new Metadata { StyleType = StyleType.Display }));
            Assert.Equal("<a href=\"#\">\"&gt;&lt;script&gt;alert(1)&lt;/script&gt;</a>",
                options.RenderOption(asset, new Metadata { StyleType = StyleType.Download }));
        }

        [Fact]
        public void Embedded_Asset_Safe_Url_Is_Kept()
        {
            var asset = new UnsafeAsset { Uid = "asset_uid", ContentTypeUid = "sys_assets", Title = "photo", Url = "https://images.contentstack.io/v3/a.png?w=10&h=10" };

            Assert.Equal("<img src=\"https://images.contentstack.io/v3/a.png?w=10&amp;h=10\" alt=\"photo\" />",
                options.RenderOption(asset, new Metadata { StyleType = StyleType.Display }));
        }

        [Fact]
        public void Html_Rte_Embedded_Link_Text_Is_Not_Double_Encoded()
        {
            var html = "<p><a class=\"embedded-entry\" type=\"entry\" sys-style-type=\"link\" data-sys-entry-uid=\"uid\" data-sys-content-type-uid=\"data-sys-content-type-uid\">Tom &amp; Jerry</a></p>";
            var result = Utils.RenderContent(html, new Options(new EmbeddedModel(html)));

            Assert.Contains("<a href=\"uid\">Tom &amp; Jerry</a>", result);
            Assert.DoesNotContain("&amp;amp;", result);
        }

        [Fact]
        public void Html_Rte_Embedded_Link_Text_Cannot_Inject_Markup()
        {
            var html = "<p><a class=\"embedded-entry\" type=\"entry\" sys-style-type=\"link\" data-sys-entry-uid=\"uid\" data-sys-content-type-uid=\"data-sys-content-type-uid\">&lt;script&gt;alert(1)&lt;/script&gt;</a></p>";
            var result = Utils.RenderContent(html, new Options(new EmbeddedModel(html)));

            Assert.Contains("<a href=\"uid\">&lt;script&gt;alert(1)&lt;/script&gt;</a>", result);
            Assert.DoesNotContain("<script>", result);
        }

        private class UnsafeEntry : IEmbeddedEntry
        {
            public string Uid { get; set; }
            public string ContentTypeUid { get; set; }
            public string Title { get; set; }
        }

        private class UnsafeAsset : IEmbeddedAsset
        {
            public string Uid { get; set; }
            public string ContentTypeUid { get; set; }
            public string Title { get; set; }
            public string FileName { get; set; }
            public string Url { get; set; }
        }
    }
}
