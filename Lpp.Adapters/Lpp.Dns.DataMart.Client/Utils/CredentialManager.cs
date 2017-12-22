using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Kerr;
using System.Security;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using log4net;

namespace Lpp.Dns.DataMart.Client.Utils
{
    /// <summary>
    /// Manages persisting, retrieving and deleting of credentials in the OS.
    /// </summary>
    public class CredentialManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Saves the username and password using NetworkSetting.CredentialKey as key. The persistence is for the local computer for the current Windows
        /// logon user. It persists between logon sessions.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="username"></param>
        /// <param name="password">Encrypted password</param>
        public static void SaveCredential(string key, string username, string password)
        {
            int errCode = -1;
            try
            {
                errCode = WriteCred(key, username, password ?? string.Empty);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Cannot save credential. Error code: {0}.", errCode), ex);
            }
        }

        /// <summary>
        /// Retrieves the username and password given the NetworkSetting.CredentialKey.
        /// Returns nulls if credential does not exist in the system.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="username"></param>
        /// <param name="password">Encrypted password</param>
        public static void GetCredential(string key, out string username, out string password)
        {
            int errCode = -1;
            username = password = null;
            try
            {
                errCode = ReadCred(key, out username, out password);
            }
            catch (ArgumentNullException nex)
            {
                log.Warn(string.Format("Credential is not accessible for key:{0}, argument null exception raised.", key));
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Cannot get credential. Error code: {0}.", errCode), ex);
            }
        }

        /// <summary>
        /// Deletes a credential given the NetworkSetting.CredentialKey.
        /// Silently ignores it if it does not exist.
        /// </summary>
        /// <param name="key"></param>
        public static void DeleteCredential(string key)
        {
            int errCode = -1;
            try
            {
                DeleteCred(key);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Cannot delete credential. Error code: {0}.", errCode), ex);
            }
        }

        #region CredMan

        public static int WriteCred(string key, string username, string secret)
        {
            // Validations.

            byte[] byteArray = Encoding.Unicode.GetBytes(secret);
            if (byteArray.Length > 512)
                throw new ArgumentOutOfRangeException("The secret message has exceeded 512 bytes.");

            // Go ahead with what we have are stuff it into the CredMan structures.
            Credential cred = new Credential();
            cred.TargetName = key;
            cred.UserName = username;
            cred.CredentialBlob = secret;
            cred.CredentialBlobSize = (UInt32)Encoding.Unicode.GetBytes(secret).Length;
            cred.AttributeCount = 0;
            cred.Attributes = IntPtr.Zero;
            cred.Comment = null;
            cred.TargetAlias = null;
            cred.Type = CRED_TYPE.GENERIC;
            cred.Persist = Persistence.CRED_PERSIST_LOCAL_MACHINE;
            NativeCredential ncred = NativeCredential.GetNativeCredential(cred);
            // Write the info into the CredMan storage.
            bool written = CredWrite(ref ncred, 0);
            int lastError = Marshal.GetLastWin32Error();

            if (written)
            {
                return 0;
            }
            else
            {
                return lastError;
            }
        }

        public static int ReadCred(string key, out string username, out string password)
        {
            // Validations.
            username = password = null;
            IntPtr nCredPtr;
            //string readPasswordText = null;

            // Make the API call using the P/Invoke signature
            bool read = CredRead(key, CRED_TYPE.GENERIC, 0, out nCredPtr);
            int lastError = Marshal.GetLastWin32Error();

            // If the API was successful then...
            if (read)
            {
                using (CriticalCredentialHandle critCred = new CriticalCredentialHandle(nCredPtr))
                {
                    Credential cred = critCred.GetCredential();
                    username = cred.UserName;
                    password = cred.CredentialBlob;                    
                }
                return 0;
            }
            else
            {
                return lastError;
            }

        }

        public static int DeleteCred(string key)
        {
            // Make the API call using the P/Invoke signature
            bool del = CredDelete(key, CRED_TYPE.GENERIC, 0);
            int lastError = Marshal.GetLastWin32Error();

            // If the API was successful then...
            if (del)
            {
                return 0;
            }
            else
            {
                return lastError;
            }

        }

        [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CredRead(string target, CRED_TYPE type, int reservedFlag, out IntPtr CredentialPtr);

        [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CredWrite([In] ref NativeCredential userCredential, [In] UInt32 flags);

        [DllImport("Advapi32.dll", EntryPoint = "CredDeleteW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool CredDelete(string target, CRED_TYPE type, [In] UInt32 flags);

        [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
        static extern bool CredFree([In] IntPtr cred);

        [DllImport("kernel32.dll", EntryPoint = "RtlFillMemory", SetLastError = false)]
        static extern void FillMemory(IntPtr destination, uint length, byte fill);

        public enum CredUIReturnCodes
        {
            NoError = 0x00,
            Cancelled = 1223,
            NoSuchLogonSession = 1312,
            NotFound = 1168,
            InvalidAccountName = 1315,
            InsufficientBuffer = 122,
            InvalidParameter = 87,
            InvalidFlags = 1004
        }

        public enum CRED_TYPE : uint
        {
            GENERIC = 1,
            DOMAIN_PASSWORD = 2,
            DOMAIN_CERTIFICATE = 3,
            DOMAIN_VISIBLE_PASSWORD = 4,
            GENERIC_CERTIFICATE = 5,
            DOMAIN_EXTENDED = 6,
            MAXIMUM = 7,      // Maximum supported cred type
            MAXIMUM_EX = (MAXIMUM + 1000),  // Allow new applications to run on old OSes
        }

        public enum Persistence
        {
            CRED_PERSIST_SESSION = 0x1,
            CRED_PERSIST_LOCAL_MACHINE = 0x2,
            CRED_PERSIST_ENTERPRISE = 0x3
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct NativeCredential
        {
            public UInt32 Flags;
            public CRED_TYPE Type;
            public IntPtr TargetName;
            public IntPtr Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public UInt32 CredentialBlobSize;
            public IntPtr CredentialBlob;
            public UInt32 Persist;
            public UInt32 AttributeCount;
            public IntPtr Attributes;
            public IntPtr TargetAlias;
            public IntPtr UserName;

            /// <summary>
            /// This method derives a NativeCredential instance from a given Credential instance.
            /// </summary>
            /// <param name="cred">The managed Credential counterpart containing data to be stored.</param>
            /// <returns>A NativeCredential instance that is derived from the given Credential
            /// instance.</returns>
            internal static NativeCredential GetNativeCredential(Credential cred)
            {
                NativeCredential ncred = new NativeCredential();
                ncred.AttributeCount = 0;
                ncred.Attributes = IntPtr.Zero;
                ncred.Comment = IntPtr.Zero;
                ncred.TargetAlias = IntPtr.Zero;
                ncred.Type = CRED_TYPE.GENERIC;
                ncred.Persist = (UInt32)Persistence.CRED_PERSIST_LOCAL_MACHINE;
                ncred.CredentialBlobSize = (UInt32)cred.CredentialBlobSize;
                ncred.TargetName = Marshal.StringToCoTaskMemUni(cred.TargetName);
                ncred.CredentialBlob = Marshal.StringToCoTaskMemUni(cred.CredentialBlob);
                ncred.UserName = Marshal.StringToCoTaskMemUni(cred.UserName);
                return ncred;
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct Credential
        {
            public UInt32 Flags;
            public CRED_TYPE Type;
            public string TargetName;
            public string Comment;
            public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
            public UInt32 CredentialBlobSize;
            public string CredentialBlob;
            public Persistence Persist;
            public UInt32 AttributeCount;
            public IntPtr Attributes;
            public string TargetAlias;
            public string UserName;
        }

        #endregion

        #region Critical Handle Type definition

        sealed class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
        {
            // Set the handle.
            internal CriticalCredentialHandle(IntPtr preexistingHandle)
            {
                SetHandle(preexistingHandle);
            }

            internal Credential GetCredential()
            {
                if (!IsInvalid)
                {
                    // Get the Credential from the mem location
                    NativeCredential ncred = (NativeCredential)Marshal.PtrToStructure(handle,
                          typeof(NativeCredential));

                    // Create a managed Credential type and fill it with data from the native counterpart.
                    Credential cred = new Credential();
                    cred.CredentialBlobSize = ncred.CredentialBlobSize;
                    cred.CredentialBlob = Marshal.PtrToStringUni(ncred.CredentialBlob,
                          (int)ncred.CredentialBlobSize / 2);
                    cred.UserName = Marshal.PtrToStringUni(ncred.UserName);
                    cred.TargetName = Marshal.PtrToStringUni(ncred.TargetName);
                    cred.TargetAlias = Marshal.PtrToStringUni(ncred.TargetAlias);
                    cred.Type = ncred.Type;
                    cred.Flags = ncred.Flags;
                    cred.Persist = (Persistence)ncred.Persist;
                    return cred;
                }
                else
                {
                    throw new InvalidOperationException("Invalid CriticalHandle!");
                }
            }

            // Perform any specific actions to release the handle in the ReleaseHandle method.
            // Often, you need to use Pinvoke to make a call into the Win32 API to release the 
            // handle. In this case, however, we can use the Marshal class to release the unmanaged memory.

            override protected bool ReleaseHandle()
            {
                // If the handle was set, free it. Return success.
                if (!IsInvalid)
                {
                    // ZERO out the memory allocated to the handle, before free'ing it
                    // so there are no traces of the sensitive data left in memory.
                    NativeCredential ncred = (NativeCredential)Marshal.PtrToStructure(handle, typeof(NativeCredential));
                    string username = Marshal.PtrToStringUni(ncred.UserName);
                    string target = Marshal.PtrToStringUni(ncred.TargetName);
                    string targetAlias = Marshal.PtrToStringUni(ncred.TargetAlias);

                    if (targetAlias != null)
                        FillMemory(ncred.TargetAlias, (uint)targetAlias.Length * sizeof(char), 0);
                    if (target != null)
                        FillMemory(ncred.TargetName, (uint)target.Length * sizeof(char), 0);
                    if (username != null)
                        FillMemory(ncred.UserName, (uint)username.Length * sizeof(char), 0);
                    if (ncred.CredentialBlob != null)
                        FillMemory(ncred.CredentialBlob, ncred.CredentialBlobSize, 0);
                    FillMemory(handle, (uint)Marshal.SizeOf(typeof(NativeCredential)), 0);
                    CredFree(handle);
                    // Mark the handle as invalid for future users.
                    SetHandleAsInvalid();
                    return true;
                }
                // Return false. 
                return false;
            }
        }

        #endregion
    }
}
