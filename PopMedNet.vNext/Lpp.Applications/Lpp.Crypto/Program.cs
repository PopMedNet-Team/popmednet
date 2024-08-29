using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lpp.Utilities;

namespace Lpp.Cryptography
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var password = "Dmc$10$ynC15Us3r2020#$";
            Console.WriteLine("Base64 Encrypted String: " + password.EncryptString());
            Console.WriteLine("AES Encrypted String: " + Crypto.EncryptStringAES(password, "DMCS-Encrypted-Password", "DC882C35-C7DE-4C92-A004-FEA2CD052866"));
            Console.WriteLine("Hashed String: " + Password.ComputeHash(password));
            Console.ReadLine();
        }
    }
}
