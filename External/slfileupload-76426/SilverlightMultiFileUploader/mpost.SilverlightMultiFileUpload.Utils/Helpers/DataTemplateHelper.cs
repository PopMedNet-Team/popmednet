using System;
using System.Windows;
using System.Windows.Markup;

namespace mpost.SilverlightMultiFileUpload.Utils.Helpers
{
    public static class DataTemplateHelper
    {
        public static DataTemplate CreateDataTemplate(Type type)
        {
            string xaml = @"<DataTemplate  
                xmlns=""http://schemas.microsoft.com/client/2007"" 
                xmlns:controls=""clr-namespace:" + type.Namespace + @";assembly=" + type.Namespace + @"""> 
                <controls:" + type.Name + @"/></DataTemplate>";
            return (DataTemplate)XamlReader.Load(xaml);
        }
    }
}