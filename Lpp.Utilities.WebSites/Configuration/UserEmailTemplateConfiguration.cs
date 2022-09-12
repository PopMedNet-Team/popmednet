using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities.WebSites.Configuration
{
    public class UserEmailTemplateSection : ConfigurationSection
    {
        [ConfigurationProperty("queryToolName", DefaultValue = "", IsRequired = true)]
        public string QueryToolName
        {
            get
            {
                return (string)this["queryToolName"];
            }
            set
            {
                this["queryToolName"] = value;
            }
        }

        [ConfigurationProperty("emailTemplates", IsDefaultCollection = true)]
        public UserEmailTemplateCollection Templates
        {
            get
            {
                return (UserEmailTemplateCollection)this["emailTemplates"];
            }
            set
            {
                this["emailTemplates"] = value;
            }
        }
    }

    [ConfigurationCollection(typeof(UserEmailTemplateElement), AddItemName = "template")]
    public class UserEmailTemplateCollection : ConfigurationElementCollection
    {
        //public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;

        protected override ConfigurationElement CreateNewElement()
        {
            return new UserEmailTemplateElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UserEmailTemplateElement)element).TemplateTypeID;
        }

        protected override string ElementName => "template";

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals("template", StringComparison.InvariantCultureIgnoreCase);
        }

        public UserEmailTemplateElement this[int idx]
        {
            get { return (UserEmailTemplateElement)BaseGet(idx); }
        }

        public UserEmailTemplateElement FindByTemplateID(int id)
        {
            for(int i = 0; i < this.Count; i++)
            {
                var el = this[i];
                if (el.TemplateTypeID == id)
                    return el;
            }
            return null;
        }
    }

    public class UserEmailTemplateElement : ConfigurationElement
    {
        [ConfigurationProperty("templateTypeID", DefaultValue = "1", IsRequired = true, IsKey = true)]
        public int TemplateTypeID
        {
            get
            {
                return (int)this["templateTypeID"];
            }
            set
            {
                this["templateTypeID"] = value;
            }
        }

        [ConfigurationProperty("templateName", DefaultValue = "", IsRequired = true)]
        public string TemplateName
        {
            get
            {
                return (string)this["templateName"];
            }
            set
            {
                this["templateName"] = value;
            }
        }

        [ConfigurationProperty("subject", DefaultValue = "", IsRequired = true)]
        public string Subject
        {
            get
            {
                return (string)this["subject"];
            }
            set
            {
                this["subject"] = value;
            }
        }

        [ConfigurationProperty("htmlTemplatePath", DefaultValue = "", IsRequired = true)]
        public string HtmlTemplatePath
        {
            get
            {
                return (string)this["htmlTemplatePath"];
            }
            set
            {
                this["htmlTemplatePath"] = value;
            }
        }

        [ConfigurationProperty("textTemplatePath", DefaultValue = "", IsRequired = true)]
        public string TextTemplatePath
        {
            get
            {
                return (string)this["textTemplatePath"];
            }
            set
            {
                this["textTemplatePath"] = value;
            }
        }


    }
}
