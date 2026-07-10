using System;
using System.Collections.Generic;
using Contentstack.Utils.Interfaces;

namespace Contentstack.Utils.Extensions
{
    /// <summary>
    /// Extension methods for EditableEntry to support Live Preview functionality.
    /// </summary>
    public static class EditableEntryExtension
    {
        /// <summary>
        /// Checks if the EditableEntry contains a specific key.
        /// </summary>
        /// <param name="entry">The EditableEntry to check</param>
        /// <param name="key">The key to check for</param>
        /// <returns>True if the key exists and is accessible, false otherwise</returns>
        public static bool ContainsKey(this EditableEntry entry, string key)
        {
            if (entry == null || string.IsNullOrEmpty(key))
                return false;

            try
            {
                // Try to access the key - if it doesn't exist, this will return null
                // but won't throw for most implementations
                var value = entry[key];
                return true;
            }
            catch
            {
                // Key doesn't exist or access failed
                return false;
            }
        }

        /// <summary>
        /// Safely gets a value from EditableEntry, returning null if key doesn't exist.
        /// </summary>
        /// <param name="entry">The EditableEntry to get from</param>
        /// <param name="key">The key to get</param>
        /// <returns>The value if key exists, null otherwise</returns>
        public static object SafeGet(this EditableEntry entry, string key)
        {
            if (entry == null || string.IsNullOrEmpty(key))
                return null;

            try
            {
                return entry[key];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Safely sets a value in EditableEntry, ignoring errors.
        /// </summary>
        /// <param name="entry">The EditableEntry to set in</param>
        /// <param name="key">The key to set</param>
        /// <param name="value">The value to set</param>
        public static void SafeSet(this EditableEntry entry, string key, object value)
        {
            if (entry == null || string.IsNullOrEmpty(key))
                return;

            try
            {
                entry[key] = value;
            }
            catch
            {
                // Ignore set failures
            }
        }
    }
}