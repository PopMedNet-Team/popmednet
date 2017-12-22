using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Lpp.Dns.DataMart.Model.Settings
{
    /// <summary>
    /// A class that defines a processor's setting.
    /// </summary>
    [DataContract, Serializable]
    public class ProcessorSetting
    {
        public ProcessorSetting()
        {
            ValueType = typeof(string);
            EditorSettings = null;
            ValidValues = null;
        }

        /// <summary>
        /// The title to apply to the setting.
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// The key for the setting in the settings collection.
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// The type of the setting value.
        /// </summary>
        [DataMember]
        public Type ValueType { get; set; }

        /// <summary>
        /// Indicates if the setting is required.
        /// </summary>
        [DataMember]
        public bool Required { get; set; }

        /// <summary>
        /// A default value for the setting to initialize a new setting will.
        /// </summary>
        [DataMember]
        public string DefaultValue { get; set; }

        /// <summary>
        /// A collection of valid values if the setting value needs to be limited.
        /// </summary>
        [DataMember]
        public IEnumerable<KeyValuePair<string, object>> ValidValues { get; set; }

        /// <summary>
        /// Specific default settings for the setting's editor.
        /// </summary>
        [DataMember]
        public object EditorSettings { get; set; }
    }

    /// <summary>
    /// Marker class to indicate the setting should use a file picker as it's editor.
    /// </summary>
    [DataContract, Serializable]
    public sealed class FilePickerEditor
    {
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Filter { get; set; }
        [DataMember]
        public bool Multiselect { get; set; }
    }

    /// <summary>
    /// Marker class to indicate the setting should use a folder picker as it's editor.
    /// </summary>
    [DataContract, Serializable]
    public sealed class FolderSelectorEditor
    {
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool ShowNewFolderButton { get; set; }
    }
}
