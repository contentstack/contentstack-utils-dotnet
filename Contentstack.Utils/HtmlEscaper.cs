using System;
using System.Net;
using System.Text;

namespace Contentstack.Utils
{
    /// <summary>
    /// Escaping used by the default renderers. Mirrors the other Contentstack SDKs:
    /// attribute values use the platform HTML encoder (as Java uses StringEscapeUtils.escapeHtml4),
    /// text nodes escape &amp;, &lt; and &gt; (as the Java Utils SDK does), and URLs follow the scheme
    /// rule used by Studio (block javascript:, vbscript: and data:, allowing data:image/ only for images).
    /// </summary>
    internal static class HtmlEscaper
    {
        private static readonly string[] BlockedSchemes = { "javascript:", "vbscript:", "data:" };

        /// <summary>
        /// Escapes a value written as HTML text content.
        /// </summary>
        internal static string Text(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        /// <summary>
        /// Escapes a value written inside a quoted HTML attribute.
        /// </summary>
        internal static string Attribute(string value)
        {
            return WebUtility.HtmlEncode(value);
        }

        /// <summary>
        /// Escapes a URL for an attribute, or returns <paramref name="fallback"/> when the URL uses a
        /// blocked scheme: javascript:, vbscript: or data:. When <paramref name="isImage"/> is true,
        /// data:image/ URLs are allowed.
        /// </summary>
        internal static string Url(string url, string fallback, bool isImage = false)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            return IsBlockedUrl(url, isImage) ? fallback : Attribute(url);
        }

        private static bool IsBlockedUrl(string url, bool isImage)
        {
            // Like the RTE editor sanitizer, drop whitespace before reading the scheme, and also drop
            // other control characters. This is stricter than browsers, so it can only over-block.
            var normalized = new StringBuilder(url.Length);
            foreach (char c in url)
            {
                if (c > ' ')
                    normalized.Append(char.ToLowerInvariant(c));
            }
            string value = normalized.ToString();

            foreach (string scheme in BlockedSchemes)
            {
                if (value.StartsWith(scheme, StringComparison.Ordinal))
                    return !(isImage && scheme == "data:" && value.StartsWith("data:image/", StringComparison.Ordinal));
            }
            return false;
        }
    }
}
