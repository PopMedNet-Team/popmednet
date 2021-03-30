using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DTO.QueryComposer
{
    /// <summary>
    /// QueryComposer Response PropertyDefinitionDTO
    /// </summary>
    [DataContract]
    public class QueryComposerResponsePropertyDefinitionDTO : Lpp.Objects.Dynamic.IPropertyDefinition
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// The name of the type of the property. This should be a defined type.
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        string _as = null;
        /// <summary>
        /// The name of the property to be used in a select or aggregation.
        /// </summary>
        [DataMember]
        public string As
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_as))
                    return this.Name;

                return _as;
            }
            set
            {
                _as = value;
            }
        }
        /// <summary>
        /// The name of an applicable aggregation to apply when aggregating the results. (Sum, Average, Count, etc.)
        /// </summary>
        [DataMember]
        public string Aggregate { get; set; }

        /// <summary>
        /// Returns the Type property as a System.Type.
        /// </summary>
        /// <returns></returns>
        public Type AsType()
        {
            return System.Type.GetType(this.Type);
        }
    }
}
