using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Renci.SshNet;

namespace Lpp.Dns.DataMart.Client.Utils
{
    public class SFTPNotConnectedException : Exception
    {
        public SFTPNotConnectedException(String Message) : base(Message)
        {
        }
    }

    public class SFTPClient : IDisposable
    {
        // Copyright (c) 2010, RENCI
        // All rights reserved.
        // Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
        // * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
        // * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
        // * Neither the name of RENCI nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
        // THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

        public SFTPClient()
        {
        }

        public SFTPClient(String Server, int Port, String Account, String Password)
        {
            Connect(Server, Port, Account, Password);
        }

        public SftpClient Client { get; private set; }

        public void Dispose()
        {
            if (Client != null)
            {
                if (Client.IsConnected)
                    Client.Disconnect();
                Client.Dispose();
            }
        }
        
        public void Connect(String Server, int Port, String Account, String Password)
        {
            try
            {
                Client = new SftpClient(new PasswordConnectionInfo(Server, Port, Account, Password));
                Client.Connect();
                if (!Client.IsConnected)
                    throw new SFTPNotConnectedException("Cannot connect to server");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Disconnect()
        {
            if (Client.IsConnected)
                Client.Disconnect();
        }

        public Stream FileStream(String ServerFilePath)
        {
            Stream fileStream = null;
            try
            {
                if (!Client.IsConnected)
                    Client.Connect();
                if (Client.IsConnected)
                {
                     fileStream = Client.OpenRead(ServerFilePath);
                }
                else
                {
                    throw new SFTPNotConnectedException("Cannot connect to server");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return fileStream;
        }

        public void DownloadFile(String ServerFilePath, Stream LocalFile)
        {
            try
            {
                if (!Client.IsConnected)
                    Client.Connect();
                if (Client.IsConnected)
                {
                    //Client.UploadFile(LocalFile, ServerFilePath);
                    using (Renci.SshNet.Sftp.SftpFileStream remote = Client.OpenRead(ServerFilePath))
                    {
                        remote.CopyTo(LocalFile, (int)Client.BufferSize);
                    }
                }
                else
                {
                    throw new SFTPNotConnectedException("Cannot connect to server");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public void UploadFile(Stream LocalFile, String ServerFilePath)
        {
            try
            {
                if (!Client.IsConnected)
                    Client.Connect();
                if (Client.IsConnected)
                {
                    Client.UploadFile(LocalFile, ServerFilePath, true);
                    //using (Renci.SshNet.Sftp.SftpFileStream remote = Client.Create(ServerFilePath))
                    //{
                    //    LocalFile.CopyTo(remote, (int)Client.BufferSize);
                   // }
                }
                else
                {
                    throw new SFTPNotConnectedException("Cannot connect to server");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}
