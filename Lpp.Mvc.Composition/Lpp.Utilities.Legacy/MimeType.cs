using System.Collections.Concurrent;
using System.IO;
using System.Net.Mime;

namespace Lpp.Utilities.Legacy
{
    public static class MimeType
    {
        static readonly ConcurrentDictionary<string, string> _knownMimeTypes = new ConcurrentDictionary<string, string>();

        public static string FromFileName( string rname )
        {
            var ext = Path.GetExtension( rname );
            if ( string.IsNullOrEmpty( ext ) ) return MediaTypeNames.Application.Octet;

            return _knownMimeTypes.GetOrAdd( ext, GetContentTypeFromRegistry );
        }

        static string GetContentTypeFromRegistry( string ext )
        {
            using ( var key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( ext ) )
            {
                return (key == null ? null : key.GetValue( "Content Type" ) as string) ?? MediaTypeNames.Application.Octet;
            }
        }
    }
}