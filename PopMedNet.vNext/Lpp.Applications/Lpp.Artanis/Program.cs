using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Artanis
{
    class Program
    {
        static void Main(string[] args)
        {
            bool Api = false;
            bool Interface = false;
            bool ViewModel = false;
            bool Netclient = false;
            var Path = "";
            var Config = "";
            foreach (string arg in args)
            {
                switch (arg.Substring(0,arg.IndexOf("=")))
                {
                    case "/Api":
                        Api = true;
                        break;
                    //Actual Argument Flag is /Interfaces
                    case "/Interface":
                        Interface = true;
                        break;
                    //Actual Argument Flag is /ViewModels
                    case "/ViewModel":
                        ViewModel = true;
                        break;
                    case "/NetClient":
                        Netclient = true;
                        break;
                    /// Actual Argument Flag is /Path
                    case "/Path":
                        Path = arg.Substring(6);
                        break;
                    case "/Config":
                        Config = arg.Substring(8);
                        break;
                }
            }
            if(!Api && !Interface && !ViewModel && !Netclient)
            {
                Console.Error.Write("The Parameter /Api or /DTO was not provided.  Closing Application");
                Environment.Exit(1);
            }
            if (Path == "")
            {
                Console.Error.Write("The Parameter /Path was not provided.  Closing Application");
                Environment.Exit(1);
            }
            if (Config == "")
            {
                Console.Error.Write("The Parameter /Config was not provided.  Closing Application");
                Environment.Exit(1);
            }

            if (Interface)
            {
                Interfaces.GenTS(Path, Config);
            }
            else if (ViewModel)
            {
                ViewModels.GenTS(Path, Config);
            }
            else if(Api)
            {
                WebApi.GenTS(Path, Config);
            }
            else if (Netclient)
            {
                NetClient.GenTS(Path, Config);
            }
        }
    }
}
