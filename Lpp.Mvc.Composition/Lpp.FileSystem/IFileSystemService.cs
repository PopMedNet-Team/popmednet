using System;
using System.IO;
using System.Linq;
using System.Reactive;

namespace Lpp.FileSystem
{
    public interface IFileSystemService<TDomain>
    {
        IQueryable<File> Files { get; }
        File AddFile( File f );
        void DeleteFile( File f );
        IObservable<Unit> DeleteUnreferencedFiles();
        Stream OpenFile( int fileId );
        void TruncateFiles(int count);
    }
}