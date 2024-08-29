using Lpp.Dns.DataMart.Model.PCORIQueryBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Entity;
using System.Collections;
using Lpp.Dns.DataMart.Model.QueryComposer.Tests;

namespace Lpp.Dns.DataMart.Model.Processors.Tests.PCORIQueryBuilder
{
    [TestClass]
    public class PCORIQueryBuilderTests
    {
        static readonly string MSSqlConnectionString;

        static PCORIQueryBuilderTests()
        {
            MSSqlConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PCORNET"].ConnectionString;
        }

        [TestMethod]
        public void PCORI_QueryPatients()
        {
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString))
            {
                var results = db.Patients.Take(10).ToArray();
                foreach (var r in results)
                {
                    Console.WriteLine(r.ID);
                }

                DataTable dt = new DataTable();
                var asDataTable = results.Select(r => dt.LoadDataRow(new object[] { r.Hispanic, r.ID, r.Race }, LoadOption.OverwriteChanges));

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                var i = ds.Tables.Count;
            }


        }

        [TestMethod]
        public void PCORI_QueryDiagnoses()
        {
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString))
            {
                var results = db.Diagnoses.Take(10).ToArray();
                foreach (var r in results)
                {
                    Console.WriteLine(r.PatientID);
                }

                DataTable dt = new DataTable();
                var asDataTable = results.Select(r => dt.LoadDataRow(new object[] { r.Code, r.PatientID, r.CodeType }, LoadOption.OverwriteChanges));

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                var i = ds.Tables.Count;
            }


        }

        [TestMethod]
        public void PCORI_QueryEncounters()
        {
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString))
            {
                var results = db.Encounters.Take(10).ToArray();
                foreach (var r in results)
                {
                    Console.WriteLine(r.PatientID);
                }

                DataTable dt = new DataTable();
                var asDataTable = results.Select(r => dt.LoadDataRow(new object[] { r.ID, r.PatientID, r.EncounterType }, LoadOption.OverwriteChanges));

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                var i = ds.Tables.Count;
            }


        }

        [TestMethod]
        public void PCORI_QueryEnrollments()
        {
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString))
            {
                var results = db.Enrollments.Take(10).ToArray();
                foreach (var r in results)
                {
                    Console.WriteLine(r.PatientID);
                }

                DataTable dt = new DataTable();
                var asDataTable = results.Select(r => dt.LoadDataRow(new object[] { r.EncrollmentBasis, r.PatientID, r.StartedOn }, LoadOption.OverwriteChanges));

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                var i = ds.Tables.Count;
            }


        }

        [TestMethod]
        public void PCORI_QueryProcedures()
        {
            using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString))
            {
                var results = db.Procedures.Take(10).ToArray();
                foreach (var r in results)
                {
                    Console.WriteLine(r.PatientID);
                }

                DataTable dt = new DataTable();
                var asDataTable = results.Select(r => dt.LoadDataRow(new object[] { r.Code, r.PatientID, r.CodeType }, LoadOption.OverwriteChanges));

                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                var i = ds.Tables.Count;
            }


        }

        [TestMethod]
        public void PCORI_QueryVitals()
        {
            //using (var db = Helper.CreatePCORIDataContext(MSSqlConnectionString))
            //{
            //    var results = db.Vitals.Take(10).ToArray();
            //    foreach (var r in results)
            //    {
            //        Console.WriteLine(r.PatientID);
            //    }

            //    DataTable dt = new DataTable();
            //    var asDataTable = results.Select(r => dt.LoadDataRow(new object[] { r.Height, r.PatientID, r.Weight }, LoadOption.OverwriteChanges));

            //    DataSet ds = new DataSet();
            //    ds.Tables.Add(dt);

            //    var i = ds.Tables.Count;
            //}


        }

        [TestMethod]
        public void PCORI_ConfirmMappings()
        {
            using (DataContext db = Helper.CreatePCORIDataContext(MSSqlConnectionString))
            {
                var demographic = db.Patients.First();
                var diagnosis = db.Diagnoses.First();
                var encounter = db.Encounters.First();
                var enrollment = db.Enrollments.First();
                var procedure = db.Procedures.First();
                //var vital = db.Vitals.First();
            }
        }


    }
}
