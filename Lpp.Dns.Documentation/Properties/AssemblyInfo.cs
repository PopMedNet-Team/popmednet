// NOTE: This project does not include the common file CommonAssemblyInfo.cs that other projects do,
// because it is inefficient to rebuild it with with every minor change, which is for two reasons:
//    1) This DLL is way too large to copy over every time
//    2) This DLL is very unlikely to change together with the rest of the code; in a sense, it is independent of the rest of the application.

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle( "Distributed Network Solution Documentation Package" )]
[assembly: AssemblyCompany( "Lincoln Peak Partners" )]
[assembly: AssemblyProduct( "Distributed Network Solution" )]
[assembly: AssemblyCopyright( "Copyright © Lincoln Peak Partners 2011-2012" )]
[assembly: ComVisible( false )]

[assembly: AssemblyVersion( "0.9" )]
[assembly: AssemblyFileVersion( "0.9" )]