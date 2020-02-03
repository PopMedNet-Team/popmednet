using System;
using System.Collections.Generic;
namespace Lpp.Dns.DataMart.Lib.Css
{
    /// <summary>
    /// Object used to parse CSS Files.
    /// This can also be used to minify a CSS file though I
    /// doubt this will pass all the same tests as YUI compressor
    /// or some other tool
    /// </summary>
    public interface ICSSParser : IEnumerable<KeyValuePair<String, List<KeyValuePair<String, String>>>>
    {
        /// <summary>
        /// Original Style Sheet loaded
        /// </summary>
        String StyleSheet { get; set; }
        
        /// <summary>
        /// Gets all styles in an Immutable collection
        /// </summary>
        IEnumerable<KeyValuePair<String, List<KeyValuePair<String, String>>>> Styles { get; }       

        /// <summary>
        /// Gets the CSS classes.
        /// </summary>
        Dictionary<String, Dictionary<String, String>> Classes { get; }
     
        /// <summary>
        /// Gets the elements.
        /// </summary>
        Dictionary<String, Dictionary<String, String>> Elements { get; }

        /// <summary>
        /// Removes all elements from the <see cref="CSSParser"></see>.
        /// </summary>
        new void Clear();

        /// <summary>
        /// Reads the specified cascading style sheet.
        /// </summary>
        /// <param name="CascadingStyleSheet">The cascading style sheet.</param>
        void Read(String CascadingStyleSheet);

        /// <summary>
        /// Reads the CSS file.
        /// </summary>
        /// <param name="Path">The path.</param>
        void ReadCSSFile(String Path);        
    }
}
