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
    [Guid( "191A635C-7501-4BA2-B80D-4DC32DC9DBA3" )]
    [CodeGeneratorRegistration( typeof( ViewCodeGenerator ), "C# View Code Generator (.cshtml)", vsContextGuids.vsContextGuidVCSProject, GeneratesDesignTimeSource = true )]
    [ProvideObject( typeof( ViewCodeGenerator ) )]
    public class ViewCodeGenerator : BaseCodeGeneratorWithSite
    {
        protected override byte[] GenerateCode( string inputFileContent )
        {
            try
            {
                Progress( 0 );

                var dom = GenerateCodeByRazor( inputFileContent );
                if ( dom == null ) return null;
                Progress( 50 );

                dom.Namespaces[0].Types[0].IsPartial = true;

                var res = GenerateFile( dom );
                Progress( 100 );

                return res;
            }
            catch ( Exception e )
            {
                GeneratorError( 4, e.ToString(), 1, 1 );
            }

            return null;
        }

        private CodeCompileUnit GenerateCodeByRazor( string inputFileContent )
        {
            var projectPath = Path.GetDirectoryName( GetProject().FullName );
            var projectRelativePath = InputFilePath.Substring( projectPath.Length );
            var virtualPath = VirtualPathUtility.ToAppRelative( "~" + projectRelativePath );

            var config = WebConfigurationManager.OpenMappedWebConfiguration( new WebConfigurationFileMap { VirtualDirectories = { { "/", new VirtualDirectoryMapping( projectPath, true ) } } }, projectRelativePath );

            var sectGroup = new RazorWebSectionGroup
            {
                Host = (HostSection)config.GetSection( HostSection.SectionName ) ??
					new HostSection { FactoryType = typeof( MvcWebRazorHostFactory ).AssemblyQualifiedName },
                Pages = (RazorPagesSection)config.GetSection( RazorPagesSection.SectionName )
            };

            var host = projectRelativePath.IndexOf( "app_code", StringComparison.InvariantCultureIgnoreCase ) >= 0 ?
				/* Helper file:  */ new WebCodeRazorHost( virtualPath, InputFilePath ) :
				/* Regular view: */ WebRazorHostFactory.CreateHostFromConfig( sectGroup, virtualPath, InputFilePath );

            host.DefaultNamespace = FileNameSpace;
            host.DefaultDebugCompilation = string.Equals( GetVSProject().Project.ConfigurationManager.ActiveConfiguration.ConfigurationName, "debug", StringComparison.InvariantCultureIgnoreCase );
            host.DefaultClassName = Path.GetFileNameWithoutExtension( InputFilePath ).Replace( '.', '_' );

            var res = new RazorTemplateEngine( host ).GenerateCode( new StringReader( inputFileContent ), null, null, InputFilePath );

            foreach ( RazorError error in res.ParserErrors )
            {
                GeneratorError( 4, error.Message, (uint)error.Location.LineIndex + 1, (uint)error.Location.CharacterIndex + 1 );
            }

            return res.ParserErrors.Count > 0 ? null : res.GeneratedCode;
        }

        private byte[] GenerateFile( CodeCompileUnit dom )
        {
            var result = new MemoryStream();
            var writer = new StreamWriter( result, Encoding.UTF8 );

            foreach ( Reference3 r in GetVSProject().References )
            {
                dom.ReferencedAssemblies.Add( r.Path );
            }
            GetCodeProvider().GenerateCodeFromCompileUnit( dom, writer, new CodeGeneratorOptions() );
            writer.Flush();

            return result.GetBuffer().Take( (int)result.Length ).ToArray();
        }

        private void Progress( uint percent )
        {
            if ( this.CodeGeneratorProgress != null )
            {
                this.CodeGeneratorProgress.Progress( percent, 100 );
            }
        }
    }
}