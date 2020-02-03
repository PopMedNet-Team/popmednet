using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Configuration;
using Lpp.Utilities.Legacy;

namespace Lpp.Dns.Portal
{
    public static class Password
    {
        public static string ComputeHash( string password )
        {
            //Contract.Requires( password != null );
            //Contract.Ensures( //Contract.Result<string>() != null );

            using ( var sha = new System.Security.Cryptography.SHA256Managed() )
            {
                sha.Initialize();
                var hash = sha.ComputeHash( Encoding.UTF8.GetBytes( password ) );
                return Convert.ToBase64String( hash );
            }
        }

        public static double Strength( string password )
        {
            var pwd = password.EmptyIfNull();
            var charPool = 0;
            if ( pwd.Any( char.IsDigit ) ) charPool += 10;
            if ( pwd.Any( char.IsLower ) ) charPool += 26;
            if ( pwd.Any( char.IsUpper ) ) charPool += 26;
            if ( pwd.Any( _specialChars.Contains ) ) charPool += _specialChars.Count;

            if ( charPool == 0 ) return 0;

            int pwdLength = (password ?? "").Length;
            /* If password is exceeds the pre-set length limit, set it to 1 so that it becomes weak*/
            int minLength = (from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMinLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 7;
            int maxLength = (from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMaxLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 14;

            if (pwdLength < minLength || pwdLength > maxLength) pwdLength = 1;


            var entropy = pwdLength * Math.Log( charPool );
            return Math.Min( 1, entropy / _maxEntropy );
        }

        const string _specialCharsString = "!,@,#,$,%,^,&,*,?,_,~,-,£,(,)";
        static readonly HashSet<char> _specialChars = new HashSet<char>( _specialCharsString );
        static double _maxEntropy = 10 * Math.Log( 10 + 26*2 + _specialCharsString.Length );

        public static class Rules
        {
            [Export] public static IPasswordRule Digits { get { return new PasswordRule( "Password should include at least one digit",
                p => (!((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordRequireNumericCharacter"]) from i in bool.Parse(cfg) select i).Catch().AsNullable() ?? true))
                || p.EmptyIfNull().Any(char.IsDigit));}}
            [Export] public static IPasswordRule Letters { get { return new PasswordRule( "Password should include at least one letter", 
                p => p.EmptyIfNull().Any( char.IsLetter ) ); } }
            [Export] public static IPasswordRule Ucase { get { return new PasswordRule("Password should include at least one upper case letter",
                p => (!((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordRequireMixedCaseCharacter"]) from i in bool.Parse(cfg) select i).Catch().AsNullable() ?? true))
                || p.EmptyIfNull().Any(char.IsUpper));}}
            [Export] public static IPasswordRule Lcase { get { return new PasswordRule("Password should include at least one lower case letter",
                p => (!((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordRequireMixedCaseCharacter"]) from i in bool.Parse(cfg) select i).Catch().AsNullable() ?? true))
                || p.EmptyIfNull().Any(char.IsLower));}}
            [Export] public static IPasswordRule Specials { get { return new PasswordRule("Password should contain at least one special character (" + _specialCharsString + ")",
                p => (!((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordRequireSpecialCharacter"]) from i in bool.Parse(cfg) select i).Catch().AsNullable() ?? true))
                || p.EmptyIfNull().Any(_specialChars.Contains));}}
            [Export] public static IPasswordRule MinLength { get { return new PasswordRule("Password should be at least " + ((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMinLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 7) + " characters long",
                p => {int passwordLength = (from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMinLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 7;
                    return p != null && p.Length >= passwordLength;}); } }
            [Export] public static IPasswordRule MaxLength { get { return new PasswordRule("Password should be maximum " + ((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMaxLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 14) + " characters long",
                p => { int passwordLength = (from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMaxLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 14;
                    return p != null && p.Length <= passwordLength;}); } }
            }
    }

    public interface IPasswordRule 
    {
        string Name { get; }
        bool Holds( string password );
    }

    public class PasswordRule : IPasswordRule
    {
        private readonly Func<string, bool> _holds;
        public string Name { get; private set; }
        public bool Holds( string password ) { return _holds( password ); }
        public PasswordRule( string name, Func<string,bool> holds )
        {
            //Contract.Requires( holds != null );
            //Contract.Requires( !String.IsNullOrEmpty( name ) );
            Name = name;
            _holds = holds;
        }
    }
}
