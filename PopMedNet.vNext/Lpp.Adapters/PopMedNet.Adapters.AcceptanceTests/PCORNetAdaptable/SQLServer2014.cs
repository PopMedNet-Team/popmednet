﻿using Lpp.Dns.DataMart.Model.QueryComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace PopMedNet.Adapters.AcceptanceTests.PCORNetAdaptable
{
    [TestClass]
    public class SQLServer2014 : PCORNetAdaptable<SQLServer2014>
    {
        protected override string ErrorOutputFolder => @".\Error Output\PCORNet Adaptable SQLServer2014";

        Dictionary<string, object> adapterSettings = null;

        //setup configuration settings and database specific tests
        public SQLServer2014() : base("PCORNET_SQLServer2014")
        {
        }

        protected override Dictionary<string, object> ProvideAdapterSettings()
        {
            if (adapterSettings == null)
            {
                var connectionStringBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);

                adapterSettings = new Dictionary<string, object>(){
                    {"Server", connectionStringBuilder.DataSource },
                    {"UserID", connectionStringBuilder.UserID },
                    {"Password", connectionStringBuilder.Password },
                    {"Database", connectionStringBuilder.InitialCatalog },
                    {"DataProvider", Lpp.Dns.DataMart.Model.Settings.SQLProvider.SQLServer.ToString()}
                };
            }

            return adapterSettings;
        }

        protected override DbConnection GetDbConnection()
        {
            var mssmsConn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            mssmsConn.Open();
            return mssmsConn;
        }
    }
}