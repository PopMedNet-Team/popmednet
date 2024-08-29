using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// This program takes two arguments: (1) SQL from DMC log and (2) Argument and Param List from DMC log and
/// apply the arguments to the SQL to produce a near executable SQL. It is a simple utility to help with debugging DMC generated SQL.
/// The program is simple minded and is not perfect.
/// </summary>
namespace Lpp.ViewSQL
{
	class Program
	{
		static void Main(string[] args)
		{
			String sqlFile = args[0];
			String paramsFile = args[1];
			String dbtype = args.Length > 2 ? args[2] : "";
			String sql;
			List<string[]> paramsList = new List<string[]>();

			using (StreamReader sr = new StreamReader(sqlFile))
			{
				sql = sr.ReadToEnd();
			}

			using (StreamReader sr = new StreamReader(paramsFile))
			{
				string[] sep = { ": ", "(" };
				String param = sr.ReadLine();
				string[] paramArgs = param.Substring(35).Split(sep, StringSplitOptions.None);
				paramArgs[0] = paramArgs[0].Substring(60);
				paramsList.Add(paramArgs);
				while (param != null)
				{
					//sql = sql.Replace("@"+paramArg[0], paramArg[1]);
					param = sr.ReadLine();
					if (param != null && param.Length == 0)
					{
						param = sr.ReadLine();
						paramArgs = param.Substring(35).Split(sep, StringSplitOptions.None);
						paramArgs[0] = paramArgs[0].Substring(60);

						paramsList.Add(paramArgs);
					}
				}
			}

			string pattern1 = @"([A-Za-z0-9]*), .* = (\d*)";
			string pattern2 = "= ([A-Za-z0-9]*)";
			Regex rgx1 = new Regex(pattern1, RegexOptions.IgnoreCase);
			Regex rgx2 = new Regex(pattern2, RegexOptions.IgnoreCase);

			String[][] paramsArray = paramsList.ToArray();

			for (int i = 0; i < paramsArray.Length; i++)
			{
				string vartype = "", varsize = "";
				string[] paramArg = paramsArray[i];
				MatchCollection matches = rgx1.Matches(paramArg[2]);
				if (matches.Count <= 0)
				{
					matches = rgx2.Matches(paramArg[2]);
					foreach (Match match in matches)
					{
						vartype = match.Groups[1].Value;
					}
					if (dbtype == "Oracle")
					{
						Console.WriteLine("VAR {0} {1}", paramArg[0], vartype);
						Console.WriteLine("EXEC :{0} := TO_DATE({1}, 'MM/DD/YYYY')", paramArg[0], paramArg[1]);
					}
					else // SQL
					{
						Console.WriteLine("DECLARE @{0} AS {1};", paramArg[0], vartype);
						Console.WriteLine("SET @{0} = {1};", paramArg[0], paramArg[1]);
					}
				}
				else
				{
					foreach (Match match in matches)
					{
						vartype = match.Groups[1].Value;
						varsize = match.Groups[2].Value;
					}
					if (dbtype == "Oracle")
					{
						vartype = vartype == "String" ? "VARCHAR2" : vartype;
						Console.WriteLine("VAR {0} {1}({2});", paramArg[0], vartype, varsize);
						Console.WriteLine("EXEC :{0} := {1};", paramArg[0], paramArg[1]);
					}
					else
					{
						vartype = vartype == "String" ? "VARCHAR" : vartype;
						vartype = vartype == "Double" ? "FLOAT" : vartype;
						vartype = vartype == "Int32" ? "INT" : vartype;

						if(varsize == "")
							Console.WriteLine("DECLARE @{0} AS {1}", paramArg[0], vartype, varsize);
						else
							Console.WriteLine("DECLARE @{0} AS {1}({2})", paramArg[0], vartype, varsize);
						Console.WriteLine("SET @{0} = {1}", paramArg[0], paramArg[1]);
					}
				}
			}

			//for (int i=paramsArray.Length; i > 0; i--)
			//{
			//	String[] paramArg = paramsArray[i - 1];
			//	sql = sql.Replace("@" + paramArg[0], (paramArg[2].Contains("DateTime") ? "TIMESTAMP " : "") + paramArg[1]); // Postgres only
			//	//sql = sql.Replace("@" + paramArg[0], paramArg[1]);
			//}
			//Console.WriteLine(sql);

		}
	}
}