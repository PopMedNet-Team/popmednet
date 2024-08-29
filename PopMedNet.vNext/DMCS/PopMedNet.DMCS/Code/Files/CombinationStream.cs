using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PopMedNet.DMCS.Code.Files
{
    public class CombinationStream : Stream
    {
        private readonly IList<Stream> _streams;
        private readonly IList<int> _streamsToDispose;
        private int _currentStreamIndex;
        private Stream _currentStream;
        private long _length = -1;
        private long _postion;

        public CombinationStream(IList<Stream> streams)
            : this(streams, null)
        {
        }

        public CombinationStream(IList<Stream> streams, IList<int> streamsToDispose)
        {
            _streams = streams ?? throw new ArgumentNullException("streams");

            _streamsToDispose = streamsToDispose;
            if (streams.Count > 0)
                _currentStream = streams[_currentStreamIndex++];
        }

        public IList<Stream> InternalStreams { get { return _streams; } }

        public override void Flush()
        {
            if (_currentStream != null)
                _currentStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException("Stream is not seekable.");
        }

        public override void SetLength(long value)
        {
            _length = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int result = 0;
            int buffPostion = offset;

            while (count > 0)
            {
                int bytesRead = _currentStream.Read(buffer, buffPostion, count);
                result += bytesRead;
                buffPostion += bytesRead;
                _postion += bytesRead;

                if (bytesRead <= count)
                    count -= bytesRead;

                if (count > 0)
                {
                    if (_currentStreamIndex >= _streams.Count)
                        break;

                    _currentStream = _streams[_currentStreamIndex++];
                }
            }

            return result;
        }

        public async override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int result = 0;
            int buffPostion = offset;

            while (count > 0)
            {
                int bytesRead = await _currentStream.ReadAsync(buffer, buffPostion, count, cancellationToken);
                result += bytesRead;
                buffPostion += bytesRead;
                _postion += bytesRead;

                if (bytesRead <= count)
                    count -= bytesRead;

                if (count > 0)
                {
                    if (_currentStreamIndex >= _streams.Count)
                        break;

                    _currentStream = _streams[_currentStreamIndex++];
                }
            }

            return result;
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Stream is not writable");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("Stream is not writable");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_streamsToDispose == null)
            {
                foreach (var stream in InternalStreams)
                    stream.Dispose();
            }
            else
            {
                int i;
                for (i = 0; i < InternalStreams.Count; i++)
                    InternalStreams[i].Dispose();
            }
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
            get { return false; }
        }

        public override long Length
        {
            get
            {
                if (_length == -1)
                {
                    _length = 0;
                    foreach (var stream in _streams)
                        _length += stream.Length;
                }

                return _length;
            }
        }

        public override long Position
        {
            get { return _postion; }
            set { throw new NotImplementedException(); }
        }
    }
}
