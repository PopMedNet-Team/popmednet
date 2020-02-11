using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PopMedNet.Adapters.AcceptanceTests
{
    [TestClass]
    public abstract class BaseQueryTest<T>
    {
        protected readonly log4net.ILog Logger;
        protected bool _saveErrorResponse = false;

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
            using (var adapter = CreateModelAdapter(requestJsonFilename))
            {
                request.Header.SubmittedOn = DateTime.UtcNow.Date;
                return adapter.Execute(request, false);
            }
        }

        protected abstract Lpp.Dns.DataMart.Model.QueryComposer.IModelAdapter CreateModelAdapter(string testname);

        protected virtual Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO LoadRequest(string filename)
        {
            var json = System.IO.File.ReadAllText(System.IO.Path.Combine(RootFolderPath, filename + ".json"));
            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;
            Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO request = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerRequestDTO>(json, jsonSettings);

            return request;
        }

        protected virtual Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO LoadResponse(string filename)
        {
            var json = System.IO.File.ReadAllText(System.IO.Path.Combine(RootFolderPath, filename + ".json"));
            Newtonsoft.Json.JsonSerializerSettings jsonSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSettings.Converters.Add(new Lpp.Dns.DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());
            jsonSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.IgnoreAndPopulate;
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO>(json, jsonSettings);
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
        /// Confirms that the actual response matches the response provided in the expected response file. The number of rows, and the values of the rows must match.
        /// </summary>
        /// <param name="expected">The expected <see cref="Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO"/>.</param>
        /// <param name="result">The <see cref="Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO"/> of the actual query executed.</param>
        /// <param name="errorOutputFile">The filename, including path, of the response to save the result to if there are errors.</param>
        /// <param name="skipProperties">The properties to skip when doing validation.</param>
        protected virtual void ConfirmResponse(Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO expected, Lpp.Dns.DTO.QueryComposer.QueryComposerResponseDTO result, string errorOutputFile, string[] skipProperties = null)
        {
            //confirm the result does not have any errors
            Assert.IsTrue(result.Errors == null || result.Errors.Count() == 0, "There were errors in the response and should not have been.");

            var tableActual = result.Results.ToArray().FirstOrDefault();
            var tableExpected = expected.Results.ToArray().FirstOrDefault();

            if(tableActual == null && tableExpected == null)
            {
                return;
            }

            try
            {
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

                var propertyDefinitions = expected.Properties;
                foreach (var rowExpected in tableExpected)
                {
                    bool rowFound = false;
                    foreach (var rowActual in tableActual)
                    {
                        if (AllValuesMatch(rowExpected, rowActual, skipProperties, propertyDefinitions))
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

                if (_saveErrorResponse && System.IO.File.Exists(errorOutputFile))
                {
                    System.IO.File.Delete(errorOutputFile);
                }
            }
            catch(AssertFailedException ex)
            {
                if (_saveErrorResponse)
                {
                    result.Errors = new[] { new Lpp.Dns.DTO.QueryComposer.QueryComposerResponseErrorDTO { Description = ex.Message } };

                    System.IO.File.WriteAllText(errorOutputFile, Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));
                }

                throw ex;
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

                if(pair.Value == null && actualValue == null)
                {
                    return true;
                }

                if((actualValue == null && pair.Value != null) || (actualValue != null && pair.Value == null))
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
    }
}
