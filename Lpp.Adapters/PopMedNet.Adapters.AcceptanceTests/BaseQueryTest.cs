using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace PopMedNet.Adapters.AcceptanceTests
{
    [TestClass]
    public abstract class BaseQueryTest<T>
    {
        protected readonly log4net.ILog Logger;
        protected bool _saveErrorResponse = true;

        public BaseQueryTest()
        {
            log4net.Config.XmlConfigurator.Configure();
            Logger = log4net.LogManager.GetLogger(typeof(T));

            if (_saveErrorResponse && !System.IO.Directory.Exists(ErrorOutputFolder))
            {
                System.IO.Directory.CreateDirectory(ErrorOutputFolder);
            }
        }

        protected abstract string RootFolderPath { get; }

        protected abstract string ErrorOutputFolder { get; }

        protected virtual Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunRequest(string requestJsonFilename)
        {
            var request = LoadRequest(requestJsonFilename);
            return RunRequest(requestJsonFilename, request);
        }

        protected virtual Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO RunRequest(string requestJsonFilename, Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO request)
        {
            request.Header.SubmittedOn = DateTime.UtcNow.Date;

            var response = new Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO();
            response.Header = new Lpp.Dns.DTO.QueryComposer.QueryComposerResponseHeaderDTO
            {
                ID = Guid.NewGuid(),
                RequestID = request.Header.ID,
                DocumentID = Guid.NewGuid()
            };

            List<Lpp.Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO> queryResults = new List<Lpp.Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO>();
            using (var adapter = CreateModelAdapter(requestJsonFilename))
            {
                foreach (var query in request.Queries)
                {
                    queryResults.AddRange(adapter.Execute(query, false));
                }
            }

            response.Queries = queryResults;
            response.RefreshQueryDates();
            response.RefreshErrors();

            return response;
        }

        /// <summary>
        /// Executes a manual sql query, and populates a QueryComposerResponseDTO's results collection.
        /// The collection objects will be created based on the defined properties, and the column names of the sql response must
        /// match the defined property names.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="expectedResponse"></param>
        protected void ManualQueryForExpectedResults(string sql, Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO expectedResponse)
        {
            var queryResult = expectedResponse.Queries.FirstOrDefault();

            var properties = queryResult.Properties.Select(p => p.As).ToArray();

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();


            using (var conn = GetDbConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    queryResult.QueryStart = DateTimeOffset.UtcNow;

                    cmd.CommandText = sql;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            foreach (string propertyName in properties)
                            {
                                int propertyOrdinal = reader.GetOrdinal(propertyName);
                                if (propertyOrdinal >= 0)
                                {
                                    row.Add(propertyName, reader.IsDBNull(propertyOrdinal) ? null : reader.GetFieldValue<object>(propertyOrdinal));
                                }
                            }
                            if (row.Count > 0)
                            {
                                rows.Add(row);
                            }
                        }
                    }

                    queryResult.QueryEnd = DateTimeOffset.UtcNow;

                }

            }
            queryResult.Results = new[] { rows };
        }

        protected abstract Lpp.Dns.DataMart.Model.QueryComposer.IModelAdapter CreateModelAdapter(string testname);

        protected virtual Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO LoadRequest(string filename)
        {
            var json = System.IO.File.ReadAllText(System.IO.Path.Combine(RootFolderPath, filename + ".json"));
            var request = Lpp.Dns.DTO.QueryComposer.QueryComposerDTOHelpers.DeserializeRequest(json);

            return request;
        }

        protected virtual Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO LoadResponse(string filename)
        {
            var json = System.IO.File.ReadAllText(System.IO.Path.Combine(RootFolderPath, filename + ".json"));
            var response = Lpp.Dns.DTO.QueryComposer.QueryComposerDTOHelpers.DeserializeResponse(json);

            return response;
        }

        /// <summary>
        /// Confirms that the actual response matches the response provided in the expected response file. The number of rows, and the values of the rows must match.
        /// </summary>
        /// <param name="filename">The filename without extension of the response file containing the expected results.</param>
        /// <param name="result">The <see cref="Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO"/> of the actual query executed.</param>
        /// <param name="skipProperties">The properties to skip when doing validation.</param>
        /// <returns>The expected response as a <see cref="Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO"/>.</returns>
        protected virtual Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO ConfirmResponse(string filename, Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO result, string[] skipProperties = null)
        {
            var expected = LoadResponse(filename);
            ConfirmResponse(expected, result, System.IO.Path.Combine(ErrorOutputFolder, filename + ".json"), skipProperties);
            return expected;
        }

        /// <summary>
        /// Confirms that the actual response matches the response provided in the expected response file. The number of queries, rows, and the values of the rows must match.
        /// </summary>
        /// <param name="expected">The expected <see cref="Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO"/>.</param>
        /// <param name="result">The <see cref="Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO"/> of the actual query executed.</param>
        /// <param name="errorOutputFile">The filename, including path, of the response to save the result to if there are errors.</param>
        /// <param name="skipProperties">The properties to skip when doing validation.</param>
        protected virtual void ConfirmResponse(Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO expected, Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO result, string errorOutputFile, string[] skipProperties = null)
        {
            if (_saveErrorResponse && System.IO.File.Exists(errorOutputFile))
            {
                System.IO.File.Delete(errorOutputFile);
            }

            //confirm the result does not have any errors
            Assert.IsTrue(result.Errors == null || result.Errors.Count() == 0, "There were errors in the response and should not have been.");

            var expectedQueryResults = expected.Queries.ToArray();
            var actualQueryResults = result.Queries.ToArray();

            Assert.AreEqual(expectedQueryResults.Length, actualQueryResults.Length, $"The number of query results in the responses do not match. Expected:{ expectedQueryResults.Length }, Actual: { actualQueryResults.Length }");

            for (int i = 0; i < expectedQueryResults.Length; i++)
            {
                try
                {
                    ConfirmQueryResult(expectedQueryResults[i], actualQueryResults[i], skipProperties);
                }
                catch (AssertFailedException ex)
                {
                    if (_saveErrorResponse)
                    {
                        result.Errors = new[] { new Lpp.Dns.DTO.QueryComposer.QueryComposerResponseErrorDTO { Description = ex.Message } };

                        System.IO.File.WriteAllText(errorOutputFile, Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));
                    }

                    throw;
                }
            }
        }

        /// <summary>
        /// Confirms the actual query result matches the expected query result. The number of rows, columns, and values must match for success.
        /// </summary>
        /// <param name="expected">The expected query result.</param>
        /// <param name="result">The actual query result.</param>
        /// <param name="skipProperties">The properties to skip for value comparison.</param>
        void ConfirmQueryResult(Lpp.Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO expected, Lpp.Dns.DTO.QueryComposer.QueryComposerResponseQueryResultDTO result, string[] skipProperties)
        {
            var tableActual = result.Results.ToArray().FirstOrDefault();
            var tableExpected = expected.Results.ToArray().FirstOrDefault();

            if (tableActual == null && tableExpected == null)
            {
                return;
            }


            if ((tableActual == null && tableExpected != null) || (tableActual != null && tableExpected == null))
            {
                Assert.Fail("The results do not match, one of the results is missing.");
            }

            if (tableExpected.Count() == 0 && tableActual.Count() == 0)
            {
                return;
            }

            Assert.AreEqual(tableExpected.Count(), tableActual.Count(), "The number of rows in the result is different.");

            if (skipProperties == null)
                skipProperties = Array.Empty<string>();

            //compare the results are the same
            var expectedPropertyNames = string.Join(", ", Sort(tableExpected.First().Keys.Where(k => skipProperties.Contains(k, StringComparer.OrdinalIgnoreCase) == false)));
            var actualPropertyNames = string.Join(", ", Sort(tableActual.First().Keys.Where(k => skipProperties.Contains(k, StringComparer.OrdinalIgnoreCase) == false)));
            Assert.AreEqual(expectedPropertyNames, actualPropertyNames, true, "The properties are not the same.");

            foreach (var rowExpected in tableExpected)
            {
                bool rowFound = false;
                foreach (var rowActual in tableActual)
                {
                    if (AllValuesMatch(rowExpected, rowActual, skipProperties, expected.Properties))
                    {
                        rowFound = true;
                        break;
                    }
                }

                if (!rowFound)
                {
                    string expectedString = "[{" + string.Join(",", rowExpected.Where(k => skipProperties.Contains(k.Key, StringComparer.OrdinalIgnoreCase) == false).Select(k => string.Format("\"{0}\":{1}", k.Key, k.Value == null ? "null" : "\"" + k.Value.ToString() + "\"")).ToArray()) + "}]";
                    string resultJSON = Newtonsoft.Json.JsonConvert.SerializeObject(tableActual);
                    Assert.Fail("Could not find a matching expected result from the expected: " + expectedString + Environment.NewLine + "Actual result: " + resultJSON);
                }
            }
        }

        internal static bool AllValuesMatch(Dictionary<string, object> expected, Dictionary<string, object> actual, string[] skipProperties, IEnumerable<Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO> properties)
        {
            foreach (var pair in expected)
            {
                if (skipProperties.Contains(pair.Key, StringComparer.OrdinalIgnoreCase))
                    continue;

                object actualValue;
                if (!actual.TryGetValue(pair.Key, out actualValue))
                {
                    return false;
                }

                if (pair.Value == null && actualValue == null)
                {
                    return true;
                }

                if ((actualValue == null && pair.Value != null) || (actualValue != null && pair.Value == null))
                {
                    return false;
                }

                //var propDefinition = properties.First(p => string.Equals(p.As, pair.Key, StringComparison.OrdinalIgnoreCase));
                //Type expectedType = propDefinition.AsType();
                //if (actualValue.GetType() != expectedType) {
                //    try
                //    {
                //        actualValue = Convert.ChangeType(actualValue, expectedType);
                //    } catch (InvalidCastException cex)
                //    {
                //        string msg = "Could not cast the value for the property:" + propDefinition.As + ". Expected type:" + expectedType.Name + ", Actual type: " + actualValue.GetType().Name;
                //        throw new InvalidCastException(msg, cex);
                //    }
                //}
                //var expectedValue = pair.Value.GetType() == expectedType ? pair.Value : Convert.ChangeType(pair.Value, expectedType);

                //if (!expectedValue.Equals(actualValue))
                //{
                //    return false;
                //}

                //changed from trying to convert to matching types due to complications with deserialization, sometimes running into issues where the expected type is a nullable but the actual type is a non-nullable. This borks the type conversion for comparison.

                return string.Equals(pair.Value.ToString(), actualValue.ToString(), StringComparison.Ordinal);

            }

            return true;
        }

        internal static string[] Sort(IEnumerable<string> values)
        {
            var v = values.ToArray();
            Array.Sort(v);
            return v;
        }

        protected abstract DbConnection GetDbConnection();
    }
}
