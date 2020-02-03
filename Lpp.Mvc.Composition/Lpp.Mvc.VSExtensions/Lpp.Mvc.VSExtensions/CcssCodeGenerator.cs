using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Razor;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.WebPages.Razor;
using System.Web.WebPages.Razor.Configuration;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;

namespace Lpp.Mvc
{
    [ComVisible( true )]
    [Guid( "62B1506D-24B9-43A3-A487-8CEE57CC540D" )]
    [CodeGeneratorRegistration( typeof( CcssCodeGenerator ), "CCSS to CSS compiler (.ccss)", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( CcssCodeGenerator ) )]
    public class CcssCodeGenerator : BaseCodeGeneratorWithSite
    {
        protected override byte[] GenerateCode( string inputFileContent )
        {
            try
            {
                Progress( 0 );
                var res = Lpp.Ccss.Compiler.Compile( inputFileContent );
                Progress( 100 );
                if ( res.Success ) return Encoding.UTF8.GetBytes( res.Result );

                GeneratorError( 4, string.Join( "", res.ErrorMessages ), (uint)res.Line, (uint)res.Column ); 
            }
            catch ( Exception e )
            {
                GeneratorError( 4, e.ToString(), 1, 1 );
            }

            return null;
        }

        protected override string GetDefaultExtension()
        {
            return ".css";
        }

        private void Progress( uint percent )
        {
            if ( this.CodeGeneratorProgress != null ) this.CodeGeneratorProgress.Progress( percent, 100 );
        }
    }
}