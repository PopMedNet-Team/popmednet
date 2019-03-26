using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MigrationHelpers
{
    class Program
    {
        enum ExitCode : int
        {
            NotNeedsMigration = 1,
            NeedsMigration = 2,
        }

        static int Main(string[] args)
        {
            //#if DEBUG
            //            Console.WriteLine("Please attach your debugger and then press a key");
            //            Console.ReadKey();
            //#endif

            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (System.IO.File.Exists(System.IO.Path.Combine(path, "ConnectionStrings.config")))
            {
                Console.WriteLine("Deleting Old ConnectionStrings File into bin folder");
                System.IO.File.Delete(System.IO.Path.Combine(path, "ConnectionStrings.config"));
            }

            Console.WriteLine("Copying ConnectionStrings File into bin folder");

            System.IO.File.Copy(System.IO.Path.Combine(path, @"..\ConnectionStrings.config"), System.IO.Path.Combine(path, "ConnectionStrings.config"));

            List<string> appliedMigrations = new List<string>();

            Console.WriteLine("Loading Data Library");

            Assembly ass = Assembly.Load("Lpp.Dns.Data");

            Console.WriteLine("Connecting to Sql Server");

            using (var sql = new SqlConnection(ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString))
            {
                sql.Open();
                Console.WriteLine("Selecting Previously retrieved Migrations");
                using (var cmd = new SqlCommand("SELECT MigrationId from __MigrationHistory", sql))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        appliedMigrations.Add(reader.GetString(0));
                    }
                }
                sql.Close();
            }

            Console.WriteLine("Closed Connection to Sql Server");

            var migrationClasses = (from a in AppDomain.CurrentDomain.GetAssemblies()
                                    from t in GetLoadableTypes(a).Where(i => i.FullName.StartsWith("System.") == false)
                                    let interfaces = t.GetInterfaces().DefaultIfEmpty()
                                    where interfaces.Any(i => i == typeof(IMigrationMetadata))
                                    && !t.IsInterface
                                    select t).ToArray();


            Console.WriteLine("Getting all Migration Classes.");

            List<string> migrationsFromClass = new List<string>();

            foreach (Type type in migrationClasses.Where(t => t.GetConstructor(Type.EmptyTypes) != null))
            {
                var migration = (IMigrationMetadata)Activator.CreateInstance(type);

                if (migration != null)
                    migrationsFromClass.Add(migration.Id);
            }

            var notApplied = migrationsFromClass.Where(x => !appliedMigrations.Contains(x));

            Console.WriteLine("Comparing Applied Migrations and Migrations in Data Library");

            System.IO.File.Delete(@"ConnectionStrings.config");

            if (notApplied.Count() > 0)
                return (int)ExitCode.NeedsMigration;
            else
                return (int)ExitCode.NotNeedsMigration;
        }

        static IEnumerable<Type> GetLoadableTypes(System.Reflection.Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (System.Reflection.ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}
