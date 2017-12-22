using System;
using System.Collections;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Lpp.Composition;
using Lpp.Composition.Modules;
//using Lpp.Data;
//using Lpp.Data.Composition;

namespace Lpp.FileSystem
{
    public static class Files
    {
        public static IModule Module<TDomain>()
        {
            return new ModuleBuilder()
                //.Export<IPersistenceDefinition<TDomain>, FilePersistence<TDomain>>()
                //.Export<IPersistenceDefinition<TDomain>, FileSegmentPersistence<TDomain>>()
                .Export<IFileSystemService<TDomain>, FileSystemService<TDomain>>()
                .CreateModule();
        }
    }
}