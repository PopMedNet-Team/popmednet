using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Lpp.Dns.Tests;
using Lpp.Data;
using Lpp.Composition;
using System.ComponentModel.Composition;
using Xunit.Extensions;

namespace Lpp.Dns.Model.Test
{
    public class Document : TestBase
    {
        [Fact]
        public void Document_ReadChunks()
        {
            int docId;

            using ( var scope = Composition.Scoping.OpenScope( TransactionScope.Id ) )
            {
                var repo = scope.GetExportedValue<IRepository<DnsDomain, Model.Document>>();
                var uow = scope.GetExportedValue<IUnitOfWork<DnsDomain>>();
                var doc = repo.Add( new Lpp.Dns.Model.Document
                {
                    Name = "",
                    Mimetype = "",
                    Segments = { new DocumentSegment { Index = 0, Data = Enumerable.Range( 0, 20 ).Select( i => (byte)i ).ToArray() } },
                    NumberOfSegments = 1,
                    LastSegmentFill = 20,
                    SegmentSize = 20
                } );
                uow.Commit();

                docId = doc.Id;
            }

            using ( var scope = Composition.Scoping.OpenScope( TransactionScope.Id ) )
            {
                var repo = scope.GetExportedValue<IRepository<DnsDomain, Model.Document>>();
                Action<int, int, int, int> assertRange = ( ofs, size, expectedOfs, expectedSize ) =>
                    Assert.Equal(
                        Enumerable.Range( expectedOfs, expectedSize ).Select( i => (byte)i ).ToArray(), 
                        GetDocumentSegment( docId, ofs, size ) );

                assertRange( 0, 5,      0, 5 );
                assertRange( 5, 15,     5, 15 );

                assertRange( 6, 15,     6, 14 );
                assertRange( 0, 25,     0, 20 );
                assertRange( 20, 1,     0, 0 );
                assertRange( 19, 1,     19, 1 );
                assertRange( 25, 5,     0, 0 );

                assertRange( -1, 5,     0, 5 );
                assertRange( -10, 10,   0, 10 );
            }
        }

        private byte[] GetDocumentSegment( int docId, int ofs, int size )
        {
            var buf = new byte[size];
            using ( var str = new DocumentStream( docId, Composition.Scoping ) )
            {
                str.Position = Math.Max( 0, ofs );
                var total = 0;
                while ( total < size )
                {
                    var read = str.Read( buf, total, size-total );
                    if ( read == 0 ) break;
                    total += read;
                }

                if ( total < size ) Array.Resize( ref buf, total );
                return buf;
            }
        }

        [Theory]
        [InlineData( 10, 3, 0 )]
        [InlineData( 10, 3, 1 )]
        [InlineData( 10, 3, 5 )]
        [InlineData( 10, 5, 9 )]
        [InlineData( 10, 3, 10 )]
        [InlineData( 10, 3, 11 )]
        [InlineData( 10, 3, 15 )]
        [InlineData( 10, 3, 20 )]
        [InlineData( 10, 30, 0 )]
        [InlineData( 10, 30, 1 )]
        [InlineData( 10, 30, 5 )]
        [InlineData( 10, 30, 10 )]
        [InlineData( 10, 30, 15 )]
        [InlineData( 10, 30, 20 )]
        [InlineData( 30, 10, 20 )]
        public void Document_WriteSegments( int initialBlockSize, int writeSegmentSize, int writeAtOffset )
        {
            var initiaData = Enumerable.Range( 0, initialBlockSize ).Select( i => (byte)i ).ToArray();
            var writeData = Enumerable.Range( 1000, writeSegmentSize ).Select( i => (byte)i ).ToArray();
            var expectedData = 
                initiaData.Concat( EnumerableEx.Repeat( (byte)0 ) )
                .Zip(
                EnumerableEx.Concat(
                    Enumerable.Repeat( (byte?)null, writeAtOffset ),
                    writeData.Select( b => (byte?)b ),
                    Enumerable.Repeat( (byte?)null, Math.Max( writeAtOffset + writeSegmentSize, initialBlockSize ) - writeAtOffset - writeSegmentSize )
                ),
                ( a, b ) => (b ?? a) )
                .ToArray();

            int docId;

            using ( var scope = Composition.Scoping.OpenScope( TransactionScope.Id ) )
            {
                var repo = scope.GetExportedValue<IRepository<DnsDomain, Model.Document>>();
                var uow = scope.GetExportedValue<IUnitOfWork<DnsDomain>>();
                var doc = repo.Add( new Model.Document
                {
                    Segments = { new DocumentSegment { Index = 0, Data = initiaData } },
                    NumberOfSegments = 1, SegmentSize = initialBlockSize, LastSegmentFill = initialBlockSize,
                    Name = "", Mimetype = ""
                } );
                uow.Commit();

                docId = doc.Id;
            }

            using( var str = new DocumentStream( docId, Composition.Scoping ) )
            {
                str.Position = writeAtOffset;
                str.Write( writeData, 0, writeData.Length );
            }

            Assert.Equal( expectedData, GetDocumentSegment( docId, 0, expectedData.Length ) );
        }
    }
}