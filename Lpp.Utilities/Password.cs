using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities
{
    public static class Password
    {
        public static string ComputeHash(this string password)
        {
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                sha.Initialize();
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        public static double Strength(string password)
        {
            var pwd = password.IsNullOrWhiteSpace() ? string.Empty : password;
            var charPool = 0;
            if (pwd.Any(char.IsDigit)) charPool += 10;
            if (pwd.Any(char.IsLower)) charPool += 26;
            if (pwd.Any(char.IsUpper)) charPool += 26;
            if (pwd.Any(_specialChars.Contains)) charPool += _specialChars.Count;

            if (charPool == 0) return 0;

            int pwdLength = (password ?? "").Length;
            /* If password is exceeds the pre-set length limit, set it to 1 so that it becomes weak*/
            int minLength = ConfigurationManager.AppSettings["PasswordMinLength"].ToNullableInt32() ?? 6;
            int maxLength = ConfigurationManager.AppSettings["PasswordMaxLength"].ToNullableInt32() ?? 14;

            if (pwdLength < minLength || pwdLength > maxLength) pwdLength = 1;
            
            var entropy = pwdLength * Math.Log(charPool);
            return Math.Min(1, entropy / _maxEntropy);
        }

        const string _specialCharsString = "`,!,@,#,$,%,^,&,*,?,_,~,-,£,(,)";
        static readonly HashSet<char> _specialChars = new HashSet<char>(_specialCharsString);
        static double _maxEntropy = 10 * Math.Log(10 + 26 * 2 + _specialCharsString.Length);

        //public static class Rules
        //{
        //    public static IPasswordRule Digits
        //    {
        //        get
        //        {
        //            return new PasswordRule("Password should include at least one digit",
        //                p =>  ConfigurationManager.AppSettings["PasswordRequireNumericCharacter"].ToBool()
        //                || p.EmptyIfNull().Any(char.IsDigit));
        //        }
        //    }

        //    public static IPasswordRule Letters
        //    {
        //        get
        //        {
        //            return new PasswordRule("Password should include at least one letter",
        //                p => p.EmptyIfNull().Any(char.IsLetter));
        //        }
        //    }
        //    public static IPasswordRule Ucase
        //    {
        //        get
        //        {
        //            return new PasswordRule("Password should include at least one upper case letter",
        //                p => (!((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordRequireMixedCaseCharacter"]) from i in bool.Parse(cfg) select i).Catch().AsNullable() ?? true))
        //                || p.EmptyIfNull().Any(char.IsUpper));
        //        }
        //    }
        //    public static IPasswordRule Lcase
        //    {
        //        get
        //        {
        //            return new PasswordRule("Password should include at least one lower case letter",
        //                p => (!((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordRequireMixedCaseCharacter"]) from i in bool.Parse(cfg) select i).Catch().AsNullable() ?? true))
        //                || p.EmptyIfNull().Any(char.IsLower));
        //        }
        //    }
        //    public static IPasswordRule Specials
        //    {
        //        get
        //        {
        //            return new PasswordRule("Password should contain at least one special character (" + _specialCharsString + ")",
        //                p => (!((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordRequireSpecialCharacter"]) from i in bool.Parse(cfg) select i).Catch().AsNullable() ?? true))
        //                || p.EmptyIfNull().Any(_specialChars.Contains));
        //        }
        //    }
        //    public static IPasswordRule MinLength
        //    {
        //        get
        //        {
        //            return new PasswordRule("Password should be at least " + ((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMinLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 7) + " characters long",
        //                p =>
        //                {
        //                    int passwordLength = (from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMinLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 7;
        //                    return p != null && p.Length >= passwordLength;
        //                });
        //        }
        //    }

        //    public static IPasswordRule MaxLength
        //    {
        //        get
        //        {
        //            return new PasswordRule("Password should be maximum " + ((from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMaxLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 14) + " characters long",
        //                p =>
        //                {
        //                    int passwordLength = (from cfg in Maybe.Value(ConfigurationManager.AppSettings["PasswordMaxLength"]) from i in int.Parse(cfg) select i).Catch().AsNullable() ?? 14;
        //                    return p != null && p.Length <= passwordLength;
        //                });
        //        }
        //    }
        //}
    }

    public interface IPasswordRule
    {
        string Name { get; }
        bool Holds(string password);
    }

    public class PasswordRule : IPasswordRule
    {
        private readonly Func<string, bool> _holds;
        public string Name { get; private set; }
        public bool Holds(string password) { return _holds(password); }
        public PasswordRule(string name, Func<string, bool> holds)
        {
            Name = name;
            _holds = holds;
        }
    }

}
