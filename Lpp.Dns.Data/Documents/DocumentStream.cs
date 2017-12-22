﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lpp.Dns.Data.Documents
{
    public class DocumentStream : Stream, IDisposable
    {
        private DataContext context;
        private Guid documentID;
        private Stream inner = null;
        private SqlCommand cmd = null;
        private SqlDataReader reader = null;

        public DocumentStream(DataContext db, Guid documentId)
        {
            this.context = db;
            this.documentID = documentId;
        }

        

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            
        }

        /// <summary>
        /// Copy data from a stream using a specified buffer size if the source allows seeking
        /// </summary>
        /// <param name="source"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public async Task CopyFromStreamAsync(Stream source, int bufferSize)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (this == null)
                throw new ArgumentNullException("destination");

            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException("bufferSize", "bufferSize must be greater than zero");

            //The source stream may not support seeking; e.g. a stream returned by ZipArchiveEntry.Open() or a network stream.
            var size = bufferSize;
            var canSeek = source.CanSeek;

            if (canSeek)
            {
                try
                {
                    size = Convert.ToInt32(Math.Min(bufferSize, source.Length));
                }
                catch (NotSupportedException) { canSeek = false; }
            }

            var buffer = new byte[size];
            var remaining = canSeek ? source.Length : 0;

            //If the stream is seekable, seek through it until all bytes are read.
            //If we read less than the expected number of bytes, it indicates an error, so throw the appropriate exception.
            //If the stream is not seekable, loop until we read 0 bytes. (It’s not an error in this case.)
            while (!canSeek || remaining > 0)
            {
                var read = await source.ReadAsync(buffer, 0, size);

                if (read <= 0)
                {
                    if (canSeek)
                        throw new EndOfStreamException(
                            string.Format("End of stream reached, but { 0 } remained to be read.",
                              remaining));
                    else
                        break;
                }

                await this.WriteAsync(buffer, 0, read);
                remaining -= canSeek ? read : 0;
            }
        }

        public override long Length
        {
            get
            {
                if (context.Database.Connection.State != ConnectionState.Open)
                    context.Database.Connection.Open();

                using (var cmd = new SqlCommand("SELECT DATALENGTH(Data) FROM Documents WHERE ID = @id",
                    (SqlConnection) context.Database.Connection))
                {
                    cmd.Parameters.AddWithValue("id", documentID);

                    object r = cmd.ExecuteScalar();
                    if (r == null || r == DBNull.Value)
                        return 0;

                    return (long) r;
                }
            }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (inner != null)
                return inner.Read(buffer, offset, count);


            if (context.Database.Connection.State != ConnectionState.Open)
                context.Database.Connection.Open();

            cmd = new SqlCommand("SELECT Data FROM Documents WHERE ID = @id",
                (SqlConnection) context.Database.Connection);
            

            cmd.Parameters.AddWithValue("id", documentID);

            reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
            if (reader.Read())
            {
                

                if (offset == 0)
                {
                    inner = reader.GetStream(0);
                    return inner.Read(buffer, offset, count);
                }

                //If the offset to start is greater than 0, need to use GetBytes to get specific bytes from the stream
                //http://msdn.microsoft.com/en-us/library/system.data.commandbehavior(v=vs.110).aspx
                //http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqldatareader.getbytes(v=vs.110).aspx

                return Convert.ToInt32(reader.GetBytes(0, offset, buffer, 0, count));
            }
            else
            {
                throw new ObjectNotFoundException("The document could not be found");
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        private void ResetStream()
        {
            if (inner != null)
            {
                inner.Dispose();
                inner = null;
            }

            if (reader != null)
            {
                if (!reader.IsClosed)
                    reader.Close();
                reader.Dispose();
                reader = null;
            }

            if (cmd != null)
            {
                cmd.Dispose();
                cmd = null;
            }
        }

        /// <summary>
        /// Allows you to write data directly to the database. Use this to overwrite the current data in the database.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (context.Database.Connection.State != ConnectionState.Open)
                context.Database.Connection.Open();

            using (
    var cmd =
        new SqlCommand(
            "UPDATE Documents SET Data = CASE WHEN Data IS NULL THEN @data ELSE Data + @data END WHERE ID = @id",
            (SqlConnection)context.Database.Connection))
            {

                cmd.Parameters.Add("@data", SqlDbType.Binary, -1).Value = buffer.Skip(offset).Take(count).ToArray();
                cmd.Parameters.AddWithValue("id", documentID);

                try
                {
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    ResetStream();
                }
            }
        }


        public override System.Threading.Tasks.Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return Task.Run(() => this.Write(buffer, offset, count), cancellationToken);
        }        
        public async Task<int> AppendAsync(byte[] buffer, int offset, int count)
        {
            if (context.Database.Connection.State != ConnectionState.Open)
                await context.Database.Connection.OpenAsync();

            using (
                var cmd =
                    new SqlCommand(
                        "UPDATE Documents SET Data = CASE WHEN Data IS NULL THEN @data ELSE Data + @data END WHERE ID = @id",
                        (SqlConnection) context.Database.Connection))
            {

                cmd.Parameters.Add("@data", SqlDbType.Binary, -1).Value = buffer.Skip(offset).Take(count);
                cmd.Parameters.AddWithValue("id", documentID);

                try
                {
                    return await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    ResetStream();
                }
            }
        }

        /// <summary>
        /// This will write the stream to the database and overwrite any existing data in the database.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<int> WriteStreamAsync(Stream stream)
        {
            if (context.Database.Connection.State != ConnectionState.Open)
                await context.Database.Connection.OpenAsync();

            using (var cmd = new SqlCommand("UPDATE Documents SET Data = @data WHERE ID = @id",
                (SqlConnection) context.Database.Connection))
            {

                cmd.Parameters.Add("@data", SqlDbType.Binary, -1).Value = stream;
                cmd.Parameters.AddWithValue("id", documentID);

                try
                {
                    return await cmd.ExecuteNonQueryAsync();                    
                }
                finally
                {
                    ResetStream();
                }
            }
        }

        /// <summary>
        /// Appends the data in the stream to the existing data in the database
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<int> AppendStreamAsync(Stream stream)
        {
            if (context.Database.Connection.State != ConnectionState.Open)
                await context.Database.Connection.OpenAsync();

            using (
                var cmd =
                    new SqlCommand(
                        "UPDATE Documents SET Data = CASE WHEN Data IS NULL THEN @data ELSE Data + @data END WHERE ID = @id",
                        (SqlConnection) context.Database.Connection))
            {

                cmd.Parameters.Add("@data", SqlDbType.Binary, -1).Value = stream;
                cmd.Parameters.AddWithValue("id", documentID);

                try
                {
                    return await cmd.ExecuteNonQueryAsync();
                }
                finally
                {
                    ResetStream();
                }
            }


        }

        void IDisposable.Dispose()
        {
            ResetStream();
        }
    }
}
