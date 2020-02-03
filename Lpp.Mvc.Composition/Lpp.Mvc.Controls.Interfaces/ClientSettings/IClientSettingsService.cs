using System.Collections.Generic;

namespace Lpp.Mvc.Controls
{
    public interface IClientSettingsService
    {
        string GetSetting( string key );
        void SetSettings( IDictionary<string, string> ss );

        /// <summary>
        /// Return URL of a JS module that is an object with one member, "set", which is a function( key, value )
        /// </summary>
        string ClientSetSettingScriptReferenceUrl { get; }
    }
}