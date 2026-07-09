using System.Collections.Generic;

namespace Contentstack.Utils.Models
{
    /// <summary>
    /// Options for addEditableTags method to control Live Preview tag generation behavior.
    /// </summary>
    public class AddEditableTagsOptions
    {
        /// <summary>
        /// Whether to convert the locale to lowercase in generated tags. Defaults to true.
        /// </summary>
        public bool UseLowerCaseLocale { get; set; } = true;
    }

    /// <summary>
    /// Container for applied variant information used during Live Preview tag generation.
    /// </summary>
    public class AppliedVariants
    {
        /// <summary>
        /// Dictionary mapping field paths to variant identifiers.
        /// </summary>
        public Dictionary<string, string> _applied_variants { get; set; }

        /// <summary>
        /// Whether variant processing should be applied based on the presence of applied variants.
        /// </summary>
        public bool shouldApplyVariant { get; set; }

        /// <summary>
        /// The current field path being processed, used for variant inheritance lookups.
        /// </summary>
        public string metaKey { get; set; } = "";

        public AppliedVariants()
        {
            _applied_variants = new Dictionary<string, string>();
            shouldApplyVariant = false;
        }

        public AppliedVariants(Dictionary<string, string> appliedVariants, string metaKey = "")
        {
            _applied_variants = appliedVariants ?? new Dictionary<string, string>();
            shouldApplyVariant = _applied_variants != null && _applied_variants.Count > 0;
            this.metaKey = metaKey;
        }
    }
}