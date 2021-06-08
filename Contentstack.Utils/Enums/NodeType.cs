using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Contentstack.Utils.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum NodeType
    {
        [EnumMember(Value = "doc")]
        Document,
        [EnumMember(Value = "p")]
        Paragraph,

        [EnumMember(Value = "a")]
        Link,
        [EnumMember(Value = "img")]
        Image,
        [EnumMember(Value = "embed")]
        Embed,

        [EnumMember(Value = "h1")]
        Heading_1,
        [EnumMember(Value = "h2")]
        Heading_2,
        [EnumMember(Value = "h3")]
        Heading_3,
        [EnumMember(Value = "h4")]
        Heading_4,
        [EnumMember(Value = "h5")]
        Heading_5,
        [EnumMember(Value = "h6")]
        Heading_6,

        [EnumMember(Value = "ol")]
        OrderList,
        [EnumMember(Value = "ul")]
        UnOrderList,
        [EnumMember(Value = "li")]
        ListItem,

        [EnumMember(Value = "dhroc")]
        Hr,

        [EnumMember(Value = "table")]
        Table,
        [EnumMember(Value = "thead")]
        TableHeader,
        [EnumMember(Value = "tbody")]
        TableBody,
        [EnumMember(Value = "tfoot")]
        TableFooter,
        [EnumMember(Value = "tr")]
        TableRow,
        [EnumMember(Value = "th")]
        TableHead,
        [EnumMember(Value = "td")]
        TableData,

        [EnumMember(Value = "blockquote")]
        BlockQuote,
        [EnumMember(Value = "code")]
        Code,

        [EnumMember(Value = "text")]
        Text,
        [EnumMember(Value = "reference")]
        Reference,

        Unknown
    }
}
