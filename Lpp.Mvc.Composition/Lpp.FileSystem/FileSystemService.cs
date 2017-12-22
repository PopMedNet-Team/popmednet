using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using Lpp.Composition;
//using Lpp.Data;
using System.Data.Entity.SqlServer;
using Lpp.Utilities.Legacy;

namespace Lpp.FileSystem
{
    [PartMetadata(ExportScope.Key, TransactionScope.Id)]
    class FileSystemService<TDomain> : IFileSystemService<TDomain>
    {
        //[Import] public IRepository<TDomain, File> Repo { get; set; }
        //[Import] public IRepository<TDomain, FileSegment> Segments { get; set; }
        [Import]
        public ICompositionScopingService Scoping { get; set; }
        //[ImportMany] public IEnumerable<IPersistentObjectReferenceRoot<TDomain, File>> References { get; set; }

        public IQueryable<File> Files { 
            get 
            {
                throw new Lpp.Utilities.CodeToBeUpdatedException();
                //return Repo.All; 
            } 
        }

        public File AddFile(File f) 
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //return Repo.Add(f); 
        }

        public void DeleteFile(File f) 
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();
            //Repo.Remove(f); 
        }

        public IObservable<Unit> DeleteUnreferencedFiles()
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //return Observable.Using(
            //    () => Scoping.OpenScope(TransactionScope.Id),
            //    scope => Observable.Start(() =>
            //    {
            //        var refs = scope.GetMany<IPersistentObjectReferenceRoot<TDomain, File>>();
            //        if (!refs.Any()) return false;

            //        var isReferenced = refs.Select(root => root.IsObjectReferenced).Fold(Expression.Or);
            //        var repo = scope.Get<IRepository<TDomain, File>>();
            //        var unrefFile = repo.All.FirstOrDefault(isReferenced);
            //        if (unrefFile == null) return false;

            //        repo.Remove(unrefFile);
            //        scope.Get<IUnitOfWork<TDomain>>().Commit();
            //        return true;
            //    }))
            //    .Repeat()
            //    .TakeWhile(removedAny => removedAny)
            //    .AsUnits();
        }

        public Stream OpenFile(int fileId)
        {
            return new FileStream<TDomain>(fileId, Scoping);
        }

        public void TruncateFiles(int Count)
        {
            throw new Lpp.Utilities.CodeToBeUpdatedException();

            //using (var scope = Scoping.OpenScope(TransactionScope.Id))
            //{
            //    var repoFiles = scope.Get<IRepository<TDomain, File>>();
            //    var repoSegments = scope.Get<IRepository<TDomain, FileSegment>>();
            //    (from f in repoFiles.All join s in repoSegments.All on f.Id equals s.FileId where f.LastSegmentFill < SqlFunctions.DataLength(s.Data) select f).Take(Count).ForEach(f =>
            //        {
            //            var segment = f.Segments.Last<FileSegment>();
            //            var buffer = new byte[f.LastSegmentFill];
            //            Buffer.BlockCopy(segment.Data, 0, buffer, 0, f.LastSegmentFill);
            //            segment.Data = buffer;
            //        });
            //    scope.Get<IUnitOfWork<TDomain>>().Commit();
            //}
        }
    }
}