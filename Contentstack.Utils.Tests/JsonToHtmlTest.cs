using Xunit;
using Contentstack.Utils.Tests.Mocks;
using System.Collections.Generic;
using Contentstack.Utils.Interfaces;
using Contentstack.Utils.Models;
using Contentstack.Utils.Tests.Helpers;
using Contentstack.Utils.Tests.Constants;

namespace Contentstack.Utils.Tests
{
    public class JsonToHtmlTest
    {
        DefaultRenderMock defaultRender = new DefaultRenderMock(null);

        [Fact]
        public void Should_Return_Empty_String_For_EmptyNode()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kBlankDocument);

            var result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal("", result);
        }

        [Fact]
        public void Should_Return_Empty_List_String_For_EmptyNode_Array()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kBlankDocument);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { "" }, result);
        }

        [Fact]
        public void Should_Return_Result_PlainText_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kPlainTextJson);

            var result = Utils.JsonToHtml(node, defaultRender);

            Assert.Equal(JsonToHtmlResultConstants.kPlainTextHtml, result);
        }

        [Fact]
        public void Should_Return_Result_Array_PlainText_Document()
        {
            Node node = NodeParser.parse(JsonToHtmlConstants.kPlainTextJson);

            var result = Utils.JsonToHtml(new List<Node>() { node }, defaultRender);

            Assert.Equal(new List<string>() { JsonToHtmlResultConstants.kPlainTextHtml }, result);
        }
    }
}
