using System.Collections.Generic;
namespace Lpp.Mvc.Controls
{
    public interface IClientSettingsProvider
    {
        /// <summary>
        /// Returns the setting of a given type or null if there is no setting by the given key
        /// </summary>
        string GetSetting( string key );

        /// <summary>
        /// Sets the setting that this provider can set and returns the rest of them
        /// </summary>
        IDictionary<string,string> SetSettings( IDictionary<string,string> ss );
    }
}