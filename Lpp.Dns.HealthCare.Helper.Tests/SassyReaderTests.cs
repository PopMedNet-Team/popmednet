using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lpp.Dns.General.SassyReader;
using Common.Logging.Simple;
using Common.Logging;

namespace Lpp.Dns.HealthCare.Helper.Tests
{
    [TestClass]
    public class SassyReaderTests
    {
        [TestMethod]
        public void ReadModularProgramSignature()
        {
            SasReader reader = new SasReader(new SasFile("allrun_signature.sas7bdat"));
            reader.Logger = new ConsoleOutLogger("SAS7bDatReadExample", LogLevel.Debug, true, true, true, null);
            reader.read(new SasReaderCallback());
        }
    }

    class SasReaderCallback : ISasReaderCallback
    {
        private object[][] cols = new object[][]
                                  {
                                        new object[] {"Var", "NAME OF FORMER VARIABLE", SasColumnType.CHARACTER, 16 },
                                        new object[] {"VALUE", null, SasColumnType.CHARACTER, 36 }
                                  };

        private string[][] rows = new string[][] 
                                  { 
                                        new string[] {"DPID", "ms"},
                                        new string[] {"SITEID", "oc"},
                                        new string[] {"MSReqID", "test_mp8_wp1_v01"},
                                        new string[] {"MSProjID", "test"},
                                        new string[] {"MSWPType", "mp8"},
                                        new string[] {"MSWPID", "wp1"},
                                        new string[] {"MSVerID", "v01"},
                                        new string[] {"MP8Cycles", "2"},
                                        new string[] {"MP8Scenarios", "4"},
                                        new string[] {"NumCycle", "2"},
                                        new string[] {"NumScen", "4"},
                                        new string[] {"NCPU", "8"},
                                        new string[] {"OSAbbr", "WIN"},
                                        new string[] {"OSName", "W32_7PRO"},
                                        new string[] {"SASVersion", "9.3"},
                                        new string[] {"SASVersionLong", "9.03.01M0P060711"},
                                        new string[] {"SASAccessODBC", "Licensed"},
                                        new string[] {"SASAccessPC", "Licensed"},
                                        new string[] {"SASGraph", "Licensed"},
                                        new string[] {"SASIML", "Licensed"},
                                        new string[] {"SASStat", "Licensed"},
                                        new string[] {"TotalRequestTime", "0 h 2 m 23 s"}
                                  };

        public void column(int index, String name, String label, SasColumnType type, int length)
        {
            Assert.AreEqual(cols[index][0], name);
            Assert.AreEqual(cols[index][1], label);
            Assert.AreEqual(cols[index][2], type);
            Assert.AreEqual(cols[index][3], length);
        }

        public bool readData() { return true; }

        public bool row(int rowNumber, Object[] rowData)
        {
            Assert.AreEqual(rows[rowNumber - 1][0], rowData[0]);
            Assert.AreEqual(rows[rowNumber - 1][1], rowData[1]);
            return true;
        }

    }
}
