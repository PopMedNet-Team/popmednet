using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Lpp.Dns.DataMart.Lib.Css
{
    /// <summary>
    /// Object used to parse CSS Files.
    /// This can also be used to minify a CSS file though I
    /// doubt this will pass all the same tests as YUI compressor
    /// or some other tool
    /// </summary>
    [Serializable]
    public partial class CSSParser : List<KeyValuePair<String, List<KeyValuePair<String, String>>>>, ICSSParser
    {
        private const String SelectorKey = "selector";
        private const String NameKey = "name";
        private const String ValueKey = "value";
        /// <summary>
        /// Regular expression to parse the Stylesheet
        /// </summary>
        [NonSerialized]
        private Regex rStyles = new Regex(RegularExpressionLibrary.CSSGroups, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private string stylesheet = String.Empty;
        private Dictionary<String, Dictionary<String, String>> classes;
        private Dictionary<String, Dictionary<String, String>> elements;



        /// <summary>
        /// Original Style Sheet loaded
        /// </summary>
        public String StyleSheet
        {
            get
            {
                return this.stylesheet;
            }
            set
            {
                //If the style sheet changes we will clean out any dependant data
                this.stylesheet = value;
                this.Clear();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingStyleSheet"/> class.
        /// </summary>
        public CSSParser()
        {
            this.StyleSheet = String.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CascadingStyleSheet"/> class.
        /// </summary>
        /// <param name="CascadingStyleSheet">The cascading style sheet.</param>
        public CSSParser(String CascadingStyleSheet)
        {
            this.Read(CascadingStyleSheet);
        }

        /// <summary>
        /// Reads the CSS file.
        /// </summary>
        /// <param name="Path">The path.</param>
        public void ReadCSSFile(String Path)
        {
            this.StyleSheet = File.ReadAllText(Path);
            this.Read(StyleSheet);
        }

        /// <summary>
        /// Reads the specified cascading style sheet.
        /// </summary>
        /// <param name="CascadingStyleSheet">The cascading style sheet.</param>
        public void Read(String CascadingStyleSheet)
        {
            this.StyleSheet = CascadingStyleSheet;

            if (!String.IsNullOrEmpty(CascadingStyleSheet))
            {
                //Remove comments before parsing the CSS. Don't want any comments in the collection. Don't know how iTextSharp would react to CSS Comments
                MatchCollection MatchList = rStyles.Matches(Regex.Replace(CascadingStyleSheet, RegularExpressionLibrary.CSSComments, String.Empty));
                foreach (Match item in MatchList)
                {
                    //Check for nulls
                    if (item != null && item.Groups != null && item.Groups[SelectorKey] != null && item.Groups[SelectorKey].Captures != null && item.Groups[SelectorKey].Captures[0] != null && !String.IsNullOrEmpty(item.Groups[SelectorKey].Value))
                    {
                        String strSelector = item.Groups[SelectorKey].Captures[0].Value.Trim();
                        var style = new List<KeyValuePair<String, String>>();

                        for (int i = 0; i < item.Groups[NameKey].Captures.Count; i++)
                        {
                            String className = item.Groups[NameKey].Captures[i].Value;
                            String value = item.Groups[ValueKey].Captures[i].Value;
                            //Check for null values in the properies
                            if (!String.IsNullOrEmpty(className) && !String.IsNullOrEmpty(value))
                            {
                                className = className.TrimWhiteSpace();
                                value = value.TrimWhiteSpace();
                                //One more check to be sure we are only pulling valid css values
                                if (!String.IsNullOrEmpty(className) && !String.IsNullOrEmpty(value))
                                {
                                    style.Add(new KeyValuePair<String, String>(className, value));
                                }
                            }
                        }
                        this.Add(new KeyValuePair<String, List<KeyValuePair<String, String>>>(strSelector, style));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the CSS classes.
        /// </summary>
        public Dictionary<String, Dictionary<String, String>> Classes
        {
            get
            {
                if (classes == null || classes.Count == 0)
                {
                    this.classes = this.Where(cl => cl.Key.StartsWith(".")).ToDictionary(cl => cl.Key.Trim(new Char[] { '.' }), cl => cl.Value.ToDictionary(p => p.Key, p => p.Value));
                }

                return classes;
            }
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        public Dictionary<String, Dictionary<String, String>> Elements
        {
            get
            {
                if (elements == null || elements.Count == 0)
                {
                    elements = this.Where(el => !el.Key.StartsWith(".")).ToDictionary(el => el.Key, el => el.Value.ToDictionary(p => p.Key, p => p.Value));
                }
                return elements;
            }
        }

        /// <summary>
        /// Gets all styles in an Immutable collection
        /// </summary>
        public IEnumerable<KeyValuePair<String, List<KeyValuePair<String, String>>>> Styles
        {
            get
            {
                return this.ToArray();
            }
        }

        /// <summary>
        /// Removes all elements from the <see cref="CSSParser"></see>.
        /// </summary>
        new public void Clear()
        {
            base.Clear();
            this.classes = null;
            this.elements = null;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> the CSS that was entered as it is stored internally.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder strb = new StringBuilder(this.StyleSheet.Length);
            foreach (var item in this)
            {
                strb.Append(item.Key).Append("{");
                foreach (var property in item.Value)
                {
                    strb.Append(property.Key).Append(":").Append(property.Value).Append(";");
                }
                strb.Append("}");
            }



            return strb.ToString();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return StyleSheet == null ? 0 : StyleSheet.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        ///   </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            CSSParser o = obj as CSSParser;
            return this.StyleSheet.Equals(o.StyleSheet);
        }
    }
}