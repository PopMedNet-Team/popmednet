﻿using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace PopMedNet.Utilities.WebSites.Models
{
    public class LoginResponseModel : System.Security.Principal.IIdentity
    {
        const string EncryptionSalt = "07809072-4AB9-4998-A19C-855287983782";

        protected LoginResponseModel()
        {
            AuthenticationType = "Basic";
        }

        //public LoginResponseModel(HttpContext httpContext, PopMedNet.Utilities.Security.IUser user, string password, Guid? employerID, DateTime? passwordExpiration, int sessionExpiration)
        //    : this(httpContext, user.ID, user.UserName, password, employerID, passwordExpiration, sessionExpiration)
        //{
        //}

        public LoginResponseModel(HttpContext httpContext, Guid? id, string username, string password, Guid? employerID, DateTime? passwordExpiration, int sessionExpiration)
        {
            ID = id;
            UserName = username;
            EmployerID = employerID;
            AuthenticationType = "PopMedNet";
            Authorization = Crypto.EncryptStringAES(string.Format("{0}:{1}:{2}", username, password, DateTime.UtcNow.Ticks), "PopMedNet Authorization", EncryptionSalt);
            Token = Crypto.EncryptStringAES((sessionExpiration <= 0 ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddMinutes(sessionExpiration)).ToString("s"), httpContext.Request.Host.Value, EncryptionSalt);
            PasswordExpiration = passwordExpiration;
            SessionExpireMinutes = sessionExpiration;
        }

        [DataMember]
        public Guid? ID { get; set; }
        [DataMember]
        public string? UserName { get; set; }
        [DataMember]
        public string? Name { get; set; }
        [DataMember]
        public Guid? EmployerID { get; set; }
        [DataMember]
        public string? Authorization { get; set; }
        [DataMember]
        public string? Token { get; set; }
        [DataMember]
        public DateTime? PasswordExpiration { get; set; }
        [DataMember]
        public int SessionExpireMinutes { get; set; }
        [DataMember]
        public bool isAdmin { get; set; }
        [DataMember]
        public string AuthenticationType { get; set; }

        bool System.Security.Principal.IIdentity.IsAuthenticated
        {
            get { return true; }
        }

        string System.Security.Principal.IIdentity.Name
        {
            get { return UserName!; }
        }

        [DataMember]
        public IEnumerable<Relationship<Guid>>? Employers { get; set; }

        public bool TokenIsValid(HttpContext httpContext)
        {
            var goodUntil = DateTime.Parse(Crypto.DecryptStringAES(Token, httpContext.Request.Host.Value, EncryptionSalt));
            return !string.IsNullOrEmpty(Token) && (goodUntil > DateTime.UtcNow);
        }

        public static void DecryptCredentials(string token, out string username, out string password)
        {
            var decrypted = Crypto.DecryptStringAES(token, "PopMedNet Authorization", EncryptionSalt).Split(':');
            username = decrypted[0];
            password = decrypted[1];
        }
    }

    [DataContract]
    public class Relationship<K> where K : struct, IComparable
    {        
        [DataMember]
        public K ID { get; set; }
        [DataMember]
        public string Name { get; set; } = string.Empty;
    }
}