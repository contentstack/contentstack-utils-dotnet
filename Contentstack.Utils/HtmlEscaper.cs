using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Contentstack.Utils
{
    /// <summary>
    /// Encoding helpers used by the default JSON RTE / embedded-object renderers so that
    /// content values are always emitted as text or as quoted attribute values, never as markup.
    /// </summary>
    internal static class HtmlEscaper
    {
        private static readonly Regex SchemePattern = new Regex(@"^([a-zA-Z][a-zA-Z0-9+.\-]*):", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private static readonly string[] LinkSchemes = { "http", "https", "mailto", "tel", "sms", "ftp" };
        private static readonly string[] MediaSchemes = { "http", "https" };

        /// <summary>
        /// Encodes a value for use as HTML text content (between tags).
        /// </summary>
        internal static string Text(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            StringBuilder sb = null;
            for (int i = 0; i < value.Length; i++)
            {
                string replacement;
                switch (value[i])
                {
                    case '&': replacement = "&amp;"; break;
                    case '<': replacement = "&lt;"; break;
                    case '>': replacement = "&gt;"; break;
                    default: replacement = null; break;
                }
                if (replacement != null)
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder(value.Length + 16);
                        sb.Append(value, 0, i);
                    }
                    sb.Append(replacement);
                }
                else if (sb != null)
                {
                    sb.Append(value[i]);
                }
            }
            return sb == null ? value : sb.ToString();
        }

        /// <summary>
        /// Encodes a value for use inside a double- or single-quoted HTML attribute.
        /// </summary>
        internal static string Attribute(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            StringBuilder sb = null;
            for (int i = 0; i < value.Length; i++)
            {
                string replacement;
                switch (value[i])
                {
                    case '&': replacement = "&amp;"; break;
                    case '<': replacement = "&lt;"; break;
                    case '>': replacement = "&gt;"; break;
                    case '"': replacement = "&quot;"; break;
                    case '\'': replacement = "&#39;"; break;
                    default: replacement = null; break;
                }
                if (replacement != null)
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder(value.Length + 16);
                        sb.Append(value, 0, i);
                    }
                    sb.Append(replacement);
                }
                else if (sb != null)
                {
                    sb.Append(value[i]);
                }
            }
            return sb == null ? value : sb.ToString();
        }

        /// <summary>
        /// Attribute-encodes a link target (a href). Relative URLs and http, https, mailto, tel, sms and ftp
        /// are kept; any other scheme (for example javascript: or data:) is replaced with "#".
        /// </summary>
        internal static string LinkUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return url;
            return Attribute(FilterUrl(url, LinkSchemes, allowDataImage: false) ?? "#");
        }

        /// <summary>
        /// Attribute-encodes an image source. Relative URLs, http, https and data:image/ are kept.
        /// </summary>
        internal static string ImageUrl(string url)
        {
            return Attribute(FilterUrl(url, MediaSchemes, allowDataImage: true) ?? "");
        }

        /// <summary>
        /// Attribute-encodes an iframe/embed source. Only relative URLs, http and https are kept.
        /// </summary>
        internal static string FrameUrl(string url)
        {
            return Attribute(FilterUrl(url, MediaSchemes, allowDataImage: false) ?? "");
        }

        /// <summary>
        /// Returns the URL unchanged when it is relative or uses an allowed scheme, otherwise null.
        /// </summary>
        private static string FilterUrl(string url, string[] allowedSchemes, bool allowDataImage)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            // Mirror how browsers read a URL before resolving its scheme: leading C0 controls and
            // spaces are ignored, and ASCII tab / LF / CR are removed anywhere in the string.
            StringBuilder normalized = new StringBuilder(url.Length);
            foreach (char c in url)
            {
                if (c == '\t' || c == '\n' || c == '\r')
                    continue;
                if (normalized.Length == 0 && c <= ' ')
                    continue;
                normalized.Append(c);
            }

            Match match = SchemePattern.Match(normalized.ToString());
            if (!match.Success)
                return url; // relative, fragment or query-only URL

            string scheme = match.Groups[1].Value.ToLowerInvariant();
            if (Array.IndexOf(allowedSchemes, scheme) >= 0)
                return url;

            if (allowDataImage && scheme == "data"
                && normalized.ToString().StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                return url;

            return null;
        }
    }
}
