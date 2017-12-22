using System;
using System.Collections;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using Lpp.Composition;
//using Lpp.Data;

namespace Lpp.FileSystem
{
    class FileStream<TDomain> : Stream
    {
        private readonly int _documentId;
        private readonly ICompositionScopingService _scopes;
        private WorkingSet _currentWorkingSet;

        public const string DocumentMustBeSavedErrorMessage = "File must be saved to the database before a stream on it can be opened";
        public const int MaxWorkingSetSize = 20;

        sealed class WorkingSet : IDisposable
        {
            //private readonly IRepository<TDomain, File> _documents;
            //private readonly IRepository<TDomain, FileSegment> _segments;
            //private readonly IPersistenceMagic<TDomain> _magic;
            //private readonly IUnitOfWork<TDomain> _unitOfWork;
            private readonly CompositionContainer _composition;
            private readonly File _document;
            private readonly Hashtable _set = new Hashtable();

            public File Document { get { return _document; } }
            public IQueryable<FileSegment> AllSegments 
            { 
                get {
                    throw new Lpp.Utilities.CodeToBeUpdatedException();
                    //return _segments.All.Where( s => s.FileId == _document.Id ); 
                } 
            }
            public FileSegment Segment( int index, bool create )
            {
                throw new Lpp.Utilities.CodeToBeUpdatedException();

                //var res = _set[index] as FileSegment;
                //if ( res == null )
                //{
                //    var data = _segments.All.Where( s => s.FileId == _document.Id && s.Index == index ).Select( s => s.Data ).FirstOrDefault();
                //    if ( data == null && !create ) return null;
                    
                //    res = new FileSegment { FileId = _document.Id, Index = index };
                //    if ( data == null ) _segments.Add( res ); else { _magic.AttachEntity( res ); }
                //    res.Data = data ?? new byte[_document.SegmentSize];

                //    _set[index] = res;
                //}

                //return res;
            }
            public bool IsFull { get { return _set.Count > MaxWorkingSetSize; } }

            public WorkingSet( int docId, CompositionContainer comp )
            {
                throw new Lpp.Utilities.CodeToBeUpdatedException();

                //_composition = comp;
                //_documents = comp.Get<IRepository<TDomain, File>>();
                //_segments = comp.Get<IRepository<TDomain, FileSegment>>();
                //_magic = comp.Get<IPersistenceMagic<TDomain>>();
                //_unitOfWork = comp.Get<IUnitOfWork<TDomain>>();
                //_document = _documents.Find( docId );
                //if ( _document == null ) throw new InvalidOperationException( DocumentMustBeSavedErrorMessage );
            }

            public void Dispose()
            {
                _composition.Dispose();
            }

            public void Commit()
            {
                throw new Lpp.Utilities.CodeToBeUpdatedException();

                //_unitOfWork.Commit();
            }

            public void RemoveSegment( FileSegment s )
            {
                throw new Lpp.Utilities.CodeToBeUpdatedException();

                //_segments.Remove( s );
                //_set.Remove( s.Index );
                //s.Data = null; // Let GC reclaim the memory
            }
        }

        public FileStream( int docId, ICompositionScopingService scopes )
        {
            //Contract.Requires( docId > 0, DocumentMustBeSavedErrorMessage );
            //Contract.Requires( scopes != null );
            _documentId = docId;
            _scopes = scopes;
        }

        protected override void Dispose( bool disposing )
        {
            Flush();
        }

        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return true; } }

        public override long Length { get { return GetWorkingSet().Document.Size; } }
        public override long Position { get; set; }

        public override void Flush()
        {
            var ws = _currentWorkingSet;
            if ( ws != null )
            {
                _currentWorkingSet = null;
                ws.Commit();
                ws.Dispose();
            }
        }

        public override int Read( byte[] buffer, int offset, int count )
        {
            var totalBytesRead = 0;
            var segmentSize = GetWorkingSet().Document.SegmentSize;

            while ( count > 0 && Position < Length )
            {
                var idx = (int)(Position / segmentSize);
                var seg = GetWorkingSet().Segment( idx, false );
                var relPos = (int)(Position - idx * segmentSize);
                var bytesToRead = (int)Math.Min( Math.Min( count, segmentSize - relPos ), Length - Position );

                var data = GetData( seg );
                if ( data == null ) Array.Clear( buffer, offset, bytesToRead );
                else Buffer.BlockCopy( data, relPos, buffer, offset, bytesToRead );

                Position += bytesToRead;
                offset += bytesToRead;
                count -= bytesToRead;
                totalBytesRead += bytesToRead;
            }

            return totalBytesRead;
        }

        public override long Seek( long offset, SeekOrigin origin )
        {
            switch ( origin )
            {
                case SeekOrigin.Begin: Position = offset; break;
                case SeekOrigin.Current: Position = Math.Max( 0, Position + offset ); break;
                case SeekOrigin.End: Position = Math.Max( 0, Length - offset ); break;
            }

            return Position;
        }

        public override void SetLength( long value )
        {
            if ( value > Length ) return;

            var ws = GetWorkingSet();
            var doc = ws.Document;
            var even = value % doc.SegmentSize == 0;
            var oldNumberOfSegments = doc.NumberOfSegments;
            doc.NumberOfSegments = (int)(value / doc.SegmentSize + (even ? 0 : 1));
            doc.LastSegmentFill = (int)(even ? 0 : value % doc.SegmentSize);
            ws.AllSegments
                .Where( s => s.Index >= doc.NumberOfSegments )
                .ToList()
                .ForEach( ws.RemoveSegment );
        }

        public override void Write( byte[] buffer, int offset, int count )
        {
            var segmentSize = GetWorkingSet().Document.SegmentSize;

            while ( count > 0 )
            {
                var idx = (int)(Position / segmentSize);
                var seg = GetWorkingSet().Segment( idx, true );
                var relPos = (int)(Position - idx * segmentSize);
                var bytesToWrite = (int)Math.Min( count, segmentSize - relPos );
                Buffer.BlockCopy( buffer, offset, EnsureData( seg, segmentSize ), relPos, bytesToWrite );

                var doc = GetWorkingSet().Document;
                if ( idx >= doc.NumberOfSegments ) doc.NumberOfSegments = idx+1;
                if ( idx == doc.NumberOfSegments-1 && doc.LastSegmentFill < relPos + bytesToWrite ) doc.LastSegmentFill = relPos + bytesToWrite;

                Position += bytesToWrite;
                offset += bytesToWrite;
                count -= bytesToWrite;
            }
        }

        private byte[] EnsureData( FileSegment seg, int segmentSize )
        {
            return seg.Data ?? (seg.Data = new byte[segmentSize]);
        }

        private byte[] GetData( FileSegment seg )
        {
            return seg == null ? null : seg.Data == null ? null : seg.Data;
        }

        private WorkingSet GetWorkingSet()
        {
            var ws = _currentWorkingSet;
            if ( ws != null && ws.IsFull ) 
            {
                ws.Commit();
                ws.Dispose();
                _currentWorkingSet = ws = null;
            }

            if ( ws == null )
            {
                _currentWorkingSet = ws = new WorkingSet( _documentId, _scopes.OpenScope( TransactionScope.Id ) );
            }

            return ws;
        }
    }
}