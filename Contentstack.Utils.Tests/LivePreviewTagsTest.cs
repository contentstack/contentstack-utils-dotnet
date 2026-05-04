using System;
using System.Collections.Generic;
using Xunit;
using Contentstack.Utils.Models;
using Contentstack.Utils.Interfaces;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Contentstack.Utils.Tests
{
    /// <summary>
    /// Comprehensive test suite for Live Preview editable tags functionality.
    /// Tests match the JavaScript SDK test patterns for complete parity.
    /// </summary>
    public class LivePreviewTagsTest
    {
        #region Test Data and Helpers

        private static JObject ReadJsonRoot(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Resources", fileName);
            return JObject.Parse(File.ReadAllText(path));
        }

        private Dictionary<string, object> CreateBasicEntry()
        {
            return new Dictionary<string, object>
            {
                ["_version"] = 10,
                ["locale"] = "en-us",
                ["uid"] = "entry_uid_1",
                ["ACL"] = new Dictionary<string, object>(),
                ["rich_text_editor"] = "<p>Content with text</p>",
                ["rich_text_editor_multiple"] = new List<object> { "<p>Multiple content</p>" }
            };
        }

        private Dictionary<string, object> CreateModularBlockEntry()
        {
            return new Dictionary<string, object>
            {
                ["_version"] = 10,
                ["locale"] = "en-us",
                ["uid"] = "entry_uid_1",
                ["ACL"] = new Dictionary<string, object>(),
                ["modular_blocks"] = new object[]
                {
                    new Dictionary<string, object>
                    {
                        ["rich_text_inmodular"] = new Dictionary<string, object>
                        {
                            ["rich_text_editor"] = "<p>Modular content 1</p>",
                            ["rich_text_editor_multiple"] = new List<object> { "<p>Modular multiple 1</p>" },
                            ["_metadata"] = new Dictionary<string, object> { ["uid"] = "metadata_uid_1" }
                        }
                    },
                    new Dictionary<string, object>
                    {
                        ["global_modular"] = new Dictionary<string, object>
                        {
                            ["rich_text_editor"] = "<p>Global modular content</p>",
                            ["rich_text_editor_multiple"] = new List<object> { "<p>Global modular multiple</p>" },
                            ["group"] = new Dictionary<string, object>
                            {
                                ["rich_text_editor"] = "<p>Nested group content</p>",
                                ["rich_text_editor_multiple"] = new List<object> { "<p>Nested group multiple</p>" }
                            },
                            ["_metadata"] = new Dictionary<string, object> { ["uid"] = "metadata_uid_2" }
                        }
                    }
                }
            };
        }

        private Dictionary<string, object> CreateReferenceEntry()
        {
            return new Dictionary<string, object>
            {
                ["_version"] = 10,
                ["locale"] = "en-us",
                ["uid"] = "entry_uid_1",
                ["ACL"] = new Dictionary<string, object>(),
                ["reference"] = new object[]
                {
                    new Dictionary<string, object>
                    {
                        ["uid"] = "entry_uid_11",
                        ["_content_type_uid"] = "embed_entry",
                        ["title"] = "Reference Entry Title",
                        ["rich_text_editor"] = "<p>Reference content</p>",
                        ["rich_text_editor_multiple"] = new List<object> { "<p>Reference multiple</p>" }
                    }
                }
            };
        }

        private Dictionary<string, object> CreateVariantEntry()
        {
            return new Dictionary<string, object>
            {
                ["_version"] = 10,
                ["locale"] = "en-us",
                ["uid"] = "entry_uid_1",
                ["ACL"] = new Dictionary<string, object>(),
                ["_applied_variants"] = new Dictionary<string, object>
                {
                    ["rich_text_editor"] = "variant_1",
                    ["nested.field"] = "variant_2",
                    ["modular_blocks.content_from_variant.metadata_uid_2"] = "variant_3"
                },
                ["rich_text_editor"] = "<p>Content with variant</p>",
                ["rich_text_editor_multiple"] = new List<object> { "<p>Multiple content</p>" },
                ["nested"] = new Dictionary<string, object>
                {
                    ["field"] = "nested field content",
                    ["other_field"] = "other nested content"
                }
            };
        }

        private EditableEntryMock CreateEditableEntry(Dictionary<string, object> data)
        {
            return new EditableEntryMock(data);
        }

        /// <summary>
        /// Mock implementation of EditableEntry for testing purposes.
        /// </summary>
        public class EditableEntryMock : EditableEntry
        {
            private readonly Dictionary<string, object> _data;

            public EditableEntryMock(Dictionary<string, object> data)
            {
                _data = new Dictionary<string, object>(data);
            }

            public string Uid 
            { 
                get => _data.ContainsKey("uid") ? _data["uid"]?.ToString() : null;
                set => _data["uid"] = value;
            }

            public string ContentTypeUid 
            { 
                get => _data.ContainsKey("_content_type_uid") ? _data["_content_type_uid"]?.ToString() : null;
                set => _data["_content_type_uid"] = value;
            }

            public string Title 
            { 
                get => _data.ContainsKey("title") ? _data["title"]?.ToString() : null;
                set => _data["title"] = value;
            }

            public string Locale 
            { 
                get => _data.ContainsKey("locale") ? _data["locale"]?.ToString() : null;
                set => _data["locale"] = value;
            }

            public object this[string key]
            {
                get => _data.ContainsKey(key) ? _data[key] : null;
                set => _data[key] = value;
            }

            public Dictionary<string, object> GetData() => _data;

            // Add ContainsKey method for extensions
            public bool ContainsKey(string key) => _data.ContainsKey(key);
        }

        #endregion

        #region Basic Functionality Tests

        [Fact]
        public void AddEditableTags_BasicEntry_StringTags_GeneratesCorrectTags()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateBasicEntry());

            // Act
            Utils.addEditableTags(entry, "entry_asset", false);

            // Debug output
            var dollarKey = entry["$"];
            Assert.NotNull(dollarKey); // First check if $ key exists
            
            var tags = (Dictionary<string, object>)dollarKey;
            Assert.NotNull(tags); // Check if tags object exists
            Assert.True(tags.Count > 0); // Check if tags has any entries
            
            // Assert the specific tags exist
            Assert.True(tags.ContainsKey("rich_text_editor"), $"Missing rich_text_editor key. Available keys: {string.Join(", ", tags.Keys)}");
            Assert.Equal("data-cslp=entry_asset.entry_uid_1.en-us.rich_text_editor", tags["rich_text_editor"]);
            Assert.Equal("data-cslp=entry_asset.entry_uid_1.en-us.rich_text_editor_multiple", tags["rich_text_editor_multiple"]);
        }

        [Fact]
        public void AddEditableTags_BasicEntry_ObjectTags_GeneratesCorrectTags()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateBasicEntry());

            // Act
            Utils.addEditableTags(entry, "entry_asset", true);

            // Assert
            var tags = (Dictionary<string, object>)entry["$"];
            var richTextTag = (Dictionary<string, object>)tags["rich_text_editor"];
            var richTextMultipleTag = (Dictionary<string, object>)tags["rich_text_editor_multiple"];

            Assert.Equal("entry_asset.entry_uid_1.en-us.rich_text_editor", richTextTag["data-cslp"]);
            Assert.Equal("entry_asset.entry_uid_1.en-us.rich_text_editor_multiple", richTextMultipleTag["data-cslp"]);
        }

        [Fact]
        public void AddEditableTags_NullEntry_DoesNotThrow()
        {
            // Act & Assert - should not throw
            Utils.addEditableTags(null, "entry_asset", false);
        }

        [Fact]
        public void AddEditableTags_EmptyEntry_DoesNotThrow()
        {
            // Arrange
            var entry = CreateEditableEntry(new Dictionary<string, object> { ["uid"] = "test", ["locale"] = "en-us" });

            // Act & Assert - should not throw
            Utils.addEditableTags(entry, "entry_asset", false);
            
            // Should have empty tags object
            var tags = (Dictionary<string, object>)entry["$"];
            Assert.NotNull(tags);
        }

        #endregion

        #region Modular Block Tests

        [Fact]
        public void AddEditableTags_ModularBlocks_StringTags_GeneratesCorrectNestedTags()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateModularBlockEntry());

            // Act
            Utils.addEditableTags(entry, "entry_multiple_content", false);

            // Assert - Check deeply nested modular block tags
            var modularBlocks = (object[])entry.GetData()["modular_blocks"];
            var firstBlock = (Dictionary<string, object>)modularBlocks[0];
            var richTextInModular = (Dictionary<string, object>)firstBlock["rich_text_inmodular"];
            var tags = (Dictionary<string, object>)richTextInModular["$"];

            Assert.Equal("data-cslp=entry_multiple_content.entry_uid_1.en-us.modular_blocks.0.rich_text_inmodular.rich_text_editor", 
                tags["rich_text_editor"]);
            Assert.Equal("data-cslp=entry_multiple_content.entry_uid_1.en-us.modular_blocks.0.rich_text_inmodular.rich_text_editor_multiple", 
                tags["rich_text_editor_multiple"]);
        }

        [Fact]
        public void AddEditableTags_ModularBlocks_ObjectTags_GeneratesCorrectNestedTags()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateModularBlockEntry());

            // Act
            Utils.addEditableTags(entry, "entry_multiple_content", true);

            // Assert - Check object format for nested tags
            var modularBlocks = (object[])entry.GetData()["modular_blocks"];
            var secondBlock = (Dictionary<string, object>)modularBlocks[1];
            var globalModular = (Dictionary<string, object>)secondBlock["global_modular"];
            var tags = (Dictionary<string, object>)globalModular["$"];
            var richTextTag = (Dictionary<string, object>)tags["rich_text_editor"];

            Assert.Equal("entry_multiple_content.entry_uid_1.en-us.modular_blocks.1.global_modular.rich_text_editor", 
                richTextTag["data-cslp"]);
        }

        #endregion

        #region Reference Entry Tests

        [Fact]
        public void AddEditableTags_ReferenceEntry_StringTags_UsesReferenceContentType()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateReferenceEntry());

            // Act
            Utils.addEditableTags(entry, "entry_asset", false);

            // Assert - Reference should use its own content type
            var reference = (object[])entry.GetData()["reference"];
            var refEntry = (Dictionary<string, object>)reference[0];
            var tags = (Dictionary<string, object>)refEntry["$"];

            Assert.Equal("data-cslp=embed_entry.entry_uid_11.en-us.rich_text_editor", tags["rich_text_editor"]);
            Assert.Equal("data-cslp=embed_entry.entry_uid_11.en-us.rich_text_editor_multiple", tags["rich_text_editor_multiple"]);
        }

        [Fact]
        public void AddEditableTags_ReferenceEntry_ObjectTags_UsesReferenceContentType()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateReferenceEntry());

            // Act
            Utils.addEditableTags(entry, "entry_asset", true);

            // Assert - Reference should use its own content type in object format
            var reference = (object[])entry.GetData()["reference"];
            var refEntry = (Dictionary<string, object>)reference[0];
            var tags = (Dictionary<string, object>)refEntry["$"];
            var richTextTag = (Dictionary<string, object>)tags["rich_text_editor"];

            Assert.Equal("embed_entry.entry_uid_11.en-us.rich_text_editor", richTextTag["data-cslp"]);
        }

        #endregion

        #region Array Processing Tests

        [Fact]
        public void AddEditableTags_ArrayWithNullElements_SkipsNullElements()
        {
            // Arrange
            var entryData = new Dictionary<string, object>
            {
                ["locale"] = "en-us",
                ["uid"] = "uid",
                ["items"] = new object[] 
                { 
                    null, 
                    new Dictionary<string, object> { ["title"] = "valid item" }, 
                    null 
                }
            };
            var entry = CreateEditableEntry(entryData);

            // Act & Assert - should not throw
            Utils.addEditableTags(entry, "content_type", false);

            // Check that valid item got tagged
            var items = (object[])entry.GetData()["items"];
            var validItem = (Dictionary<string, object>)items[1];
            var tags = (Dictionary<string, object>)validItem["$"];
            
            Assert.Equal("data-cslp=content_type.uid.en-us.items.1.title", tags["title"]);
        }

        [Fact]
        public void AddEditableTags_ArrayElements_GeneratesIndexAndParentTags()
        {
            // Arrange
            var entryData = new Dictionary<string, object>
            {
                ["locale"] = "en-us",
                ["uid"] = "uid",
                ["items"] = new object[] 
                { 
                    new Dictionary<string, object> { ["title"] = "item 1" },
                    new Dictionary<string, object> { ["title"] = "item 2" }
                }
            };
            var entry = CreateEditableEntry(entryData);

            // Act
            Utils.addEditableTags(entry, "content_type", false);

            // Assert - Check field__index and field__parent patterns
            var tags = (Dictionary<string, object>)entry["$"];
            
            Assert.Contains("items__0", tags.Keys);
            Assert.Contains("items__1", tags.Keys);
            Assert.Contains("items__parent", tags.Keys);
            
            Assert.Equal("data-cslp=content_type.uid.en-us.items.0", tags["items__0"]);
            Assert.Equal("data-cslp=content_type.uid.en-us.items.1", tags["items__1"]);
            Assert.Equal("data-cslp-parent-field=content_type.uid.en-us.items", tags["items__parent"]);
        }

        #endregion

        #region Variant Support Tests

        [Fact]
        public void AddEditableTags_WithVariants_StringTags_AppliesV2PrefixAndVariantSuffix()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateVariantEntry());

            // Act
            Utils.addEditableTags(entry, "entry_asset", false);

            // Assert - Field with direct variant match should get v2 prefix and variant suffix
            var tags = (Dictionary<string, object>)entry["$"];
            Assert.Equal("data-cslp=v2:entry_asset.entry_uid_1_variant_1.en-us.rich_text_editor", tags["rich_text_editor"]);
            
            // Field without variant should not have v2 prefix
            Assert.Equal("data-cslp=entry_asset.entry_uid_1.en-us.rich_text_editor_multiple", tags["rich_text_editor_multiple"]);
        }

        [Fact]
        public void AddEditableTags_WithVariants_ObjectTags_AppliesV2PrefixAndVariantSuffix()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateVariantEntry());

            // Act
            Utils.addEditableTags(entry, "entry_asset", true);

            // Assert - Field with direct variant match should get v2 prefix and variant suffix as object
            var tags = (Dictionary<string, object>)entry["$"];
            var richTextTag = (Dictionary<string, object>)tags["rich_text_editor"];
            var richTextMultipleTag = (Dictionary<string, object>)tags["rich_text_editor_multiple"];

            Assert.Equal("v2:entry_asset.entry_uid_1_variant_1.en-us.rich_text_editor", richTextTag["data-cslp"]);
            Assert.Equal("entry_asset.entry_uid_1.en-us.rich_text_editor_multiple", richTextMultipleTag["data-cslp"]);
        }

        [Fact]
        public void AddEditableTags_WithNestedVariants_AppliesCorrectVariants()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateVariantEntry());

            // Act
            Utils.addEditableTags(entry, "entry_asset", false);

            // Assert - Nested field with direct variant match
            var nested = (Dictionary<string, object>)entry.GetData()["nested"];
            var nestedTags = (Dictionary<string, object>)nested["$"];
            
            Assert.Equal("data-cslp=v2:entry_asset.entry_uid_1_variant_2.en-us.nested.field", nestedTags["field"]);
            Assert.Equal("data-cslp=entry_asset.entry_uid_1.en-us.nested.other_field", nestedTags["other_field"]);
        }

        [Fact]
        public void AddEditableTags_EmptyVariants_WorksNormally()
        {
            // Arrange
            var entryData = CreateBasicEntry();
            entryData["_applied_variants"] = new Dictionary<string, object>(); // Empty variants
            var entry = CreateEditableEntry(entryData);

            // Act
            Utils.addEditableTags(entry, "entry_asset", false);

            // Assert - Should not have v2 prefix when variants object is empty
            var tags = (Dictionary<string, object>)entry["$"];
            Assert.Equal("data-cslp=entry_asset.entry_uid_1.en-us.rich_text_editor", tags["rich_text_editor"]);
        }

        #endregion

        #region Locale and Case Sensitivity Tests

        [Fact]
        public void AddEditableTags_UseLowerCaseLocaleTrue_LowercasesLocale()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateBasicEntry());
            var options = new AddEditableTagsOptions { UseLowerCaseLocale = true };

            // Act
            Utils.addEditableTags(entry, "TEST_CONTENT_TYPE", false, "en-US", options);

            // Assert - Both content type and locale should be lowercased
            var tags = (Dictionary<string, object>)entry["$"];
            var tag = (string)tags["rich_text_editor"];
            Assert.Contains("test_content_type.entry_uid_1.en-us.rich_text_editor", tag);
        }

        [Fact]
        public void AddEditableTags_UseLowerCaseLocaleFalse_PreservesLocaleCase()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateBasicEntry());
            var options = new AddEditableTagsOptions { UseLowerCaseLocale = false };

            // Act
            Utils.addEditableTags(entry, "TEST_CONTENT_TYPE", false, "en-US", options);

            // Assert - Content type lowercased but locale case preserved
            var tags = (Dictionary<string, object>)entry["$"];
            var tag = (string)tags["rich_text_editor"];
            Assert.Contains("test_content_type.entry_uid_1.en-US.rich_text_editor", tag);
        }

        [Fact]
        public void AddEditableTags_DefaultOptions_LowercasesLocale()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateBasicEntry());

            // Act
            Utils.addEditableTags(entry, "TEST_CONTENT_TYPE", false, "en-US");

            // Assert - Default behavior should lowercase locale
            var tags = (Dictionary<string, object>)entry["$"];
            var tag = (string)tags["rich_text_editor"];
            Assert.Contains("test_content_type.entry_uid_1.en-us.rich_text_editor", tag);
        }

        #endregion

        #region Alias Method Tests

        [Fact]
        public void AddTags_Alias_WorksCorrectly()
        {
            // Arrange
            var entry = CreateEditableEntry(CreateBasicEntry());

            // Act
            Utils.addTags(entry, "entry_asset", false);

            // Assert - Should work identically to addEditableTags
            var tags = (Dictionary<string, object>)entry["$"];
            Assert.Equal("data-cslp=entry_asset.entry_uid_1.en-us.rich_text_editor", tags["rich_text_editor"]);
        }

        #endregion

        #region Error Handling and Edge Cases

        [Fact]
        public void AddEditableTags_NullFieldValue_DoesNotThrow()
        {
            // Arrange
            var entryData = new Dictionary<string, object>
            {
                ["uid"] = "entry_uid_null",
                ["locale"] = "en-us",
                ["title"] = "Valid title",
                ["description"] = null
            };
            var entry = CreateEditableEntry(entryData);

            // Act & Assert - should not throw
            Utils.addEditableTags(entry, "content_type", false);
            
            var tags = (Dictionary<string, object>)entry["$"];
            Assert.Equal("data-cslp=content_type.entry_uid_null.en-us.title", tags["title"]);
        }

        [Fact]
        public void AddEditableTags_ComplexNestedStructure_HandlesGracefully()
        {
            // Arrange
            var entryData = new Dictionary<string, object>
            {
                ["locale"] = "en-us",
                ["uid"] = "uid",
                ["blocks"] = new object[]
                {
                    new Dictionary<string, object>
                    {
                        ["hero"] = new Dictionary<string, object>
                        {
                            ["title"] = "Hero title",
                            ["items"] = new object[] { null, new Dictionary<string, object> { ["name"] = "Item name" }, null }
                        }
                    },
                    null,
                    new Dictionary<string, object>
                    {
                        ["content"] = "Content text"
                    }
                }
            };
            var entry = CreateEditableEntry(entryData);

            // Act & Assert - should not throw
            Utils.addEditableTags(entry, "content_type", true);
            
            // Check that valid nested content got tagged
            var blocks = (object[])entry.GetData()["blocks"];
            var firstBlock = (Dictionary<string, object>)blocks[0];
            var hero = (Dictionary<string, object>)firstBlock["hero"];
            var heroTags = (Dictionary<string, object>)hero["$"];
            var titleTag = (Dictionary<string, object>)heroTags["title"];
            
            Assert.Equal("content_type.uid.en-us.blocks.0.hero.title", titleTag["data-cslp"]);
        }

        #endregion
    }

}