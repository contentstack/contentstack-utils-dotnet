using System;
namespace Contentstack.Utils.Models
{
    public class TextNode: Node
    {
        public bool bold { get; set; }
        public bool italic { get; set; }
        public bool underline { get; set; }
        public bool strikethrough { get; set; }
        public bool inlineCode { get; set; }
        public bool subscript { get; set; }
        public bool superscript { get; set; }
        public string classname { get; set; }
        public string id { get; set; }
        public string text { get; set; }
    }
}
