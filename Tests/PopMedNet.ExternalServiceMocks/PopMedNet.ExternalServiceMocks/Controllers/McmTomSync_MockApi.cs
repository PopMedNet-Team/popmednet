using Microsoft.AspNetCore.Mvc;

namespace PopMedNet.ExternalServiceMocks.Controllers
{
    [ApiController]
    [Route("ErrorResponses")]
    public class MockSyncController : Controller
    {
        /// <summary>
        /// For checking NotFound response handling
        /// </summary>
        /// <returns></returns>
        [HttpGet("notFound")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        /// <summary>
        /// For checking Unauthorized response handling
        /// </summary>
        /// <returns></returns>
        [HttpGet("unauthorized/{*url}")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        /// <summary>
        /// For checking JSON deserialization when there's a bad name.
        /// Versions object has "link" property instead of "href"
        /// </summary>
        /// <returns></returns>

        [HttpGet("MCM/BadNameInJson/{*url}")]
        public IActionResult GetMcmObjectWithBadName()
        {
            return Ok(@"
                {
                  ""meta"": { ""last_updated"": ""08/08/2022""},
                  ""results"": 
                    {
                        ""category_name"": ""rx"",
                        ""class"": ""fdb-etc"",
                        ""id"": ""1.rx"",
                        ""latest"": 
                        {
                            ""link"": ""https://mcm.sentinelsystem.org/resource/terminology/rx/fdb/fdb-etc/20220804.1"",
                            ""id"": ""20220804.1""
                        },
                        ""source"": ""fdb"",
                        ""versions"": 
                        [
                            {
                                ""link"": ""https://mcm.sentinelsystem.org/resource/terminology/rx/fdb/fdb-etc/20220428.1"",
                                ""id"": ""20220428.1""
                            },
                            {
                                ""link"": ""https://mcm.sentinelsystem.org/resource/terminology/rx/fdb/fdb-etc/20220804.1"",
                                ""id"": ""20220804.1""
                            }
                        ]
                    }
                }");
        }

        /// <summary>
        /// For checking JSON deserialization when there's a bad name.
        /// Versions object is missing "href" property
        /// </summary>
        /// <returns></returns>
        [HttpGet("MCM/MissingMandatoryInJson/{*url}")]
        public IActionResult GetMcmObjectWithMissingMandatoryInfo()
        {
            return Ok(@"
                {
                  ""meta"": { ""last_updated"": ""08/08/2022""},
                  ""results"": 
                    {
                        ""category_name"": ""rx"",
                        ""class"": ""fdb-etc"",
                        ""id"": ""1.rx"",
                        ""latest"": 
                        {
                            ""id"": ""20220804.1""
                        },
                        ""source"": ""fdb"",
                        ""versions"": 
                        [
                            {
                                ""id"": ""20220428.1""
                            },
                            {
                                ""id"": ""20220804.1""
                            }
                        ]
                    }
                }");
        }

        /// <summary>
        /// Returns a Json object with an additional "description" property in each of the "versions" objects.
        /// Typically, extra properties should just be ignored by the deserializer if they aren't in the target object.
        /// </summary>
        /// <returns></returns>
        [HttpGet("MCM/ExtraPropertyInJson/{*url}")]
        public IActionResult GetMcmObjectWithExtraProperty()
        {
            // versions object has 
            return Ok(@"
                [{
                  ""meta"": { 
                        ""last_updated"": ""08/08/2022""},
                  ""results"": 
                    {
                        ""category"": ""rx"",
                        ""class"": ""fdb-etc"",
                        ""id"": ""1.rx"",
                        ""latest"": 
                        {
                            ""href"": ""https://mcm.sentinelsystem.org/resource/terminology/rx/fdb/fdb-etc/20220804.1"",
                            ""id"": ""20220804.1""
                        },
                        ""source"": ""fdb"",
                        ""versions"": 
                        [
                            {
                                ""href"": ""https://mcm.sentinelsystem.org/resource/terminology/rx/fdb/fdb-etc/20220428.1"",
                                ""id"": ""20220428.1"",
                                ""description"":""This is a version that was released at the end of April, 2022.""
                            },
                            {
                                ""href"": ""https://mcm.sentinelsystem.org/resource/terminology/rx/fdb/fdb-etc/20220804.1"",
                                ""id"": ""20220804.1"",
                                ""description"":""This is a version that was released at the beginning of August, 2022.""
                            }
                        ]
                    }
                }]");
        }

        /// <summary>
        /// returns a Json string that is missing an opening bracket at the start of the "versions" section
        /// </summary>
        /// <returns></returns>
        [HttpGet("MCM/MalformedJson/{*url}")]
        public IActionResult GetMalformedJsonObject()
        {
            return Ok(@"
                {
                  ""meta"": { ""last_updated"": ""08/08/2022""},
                  ""results"": 
                    {
                        ""category"": ""rx"",
                        ""class"": ""fdb-etc"",
                        ""id"": ""1.rx"",
                        ""latest"": 
                        {
                            ""href"": ""https://mcm.sentinelsystem.org/resource/terminology/rx/fdb/fdb-etc/20220804.1"",
                            ""id"": ""20220804.1""
                        },
                        ""source"": ""fdb"",
                        ""versions"": 
                        
                            {
                                ""href"": ""https://mcm.sentinelsystem.org/resource/terminology/rx/fdb/fdb-etc/20220428.1"",
                                ""id"": ""20220428.1""
                            },
                            {
                                ""href"": ""https://mcm.sentinelsystem.org/resource/terminology/rx/fdb/fdb-etc/20220804.1"",
                                ""id"": ""20220804.1""
                            }
                        ]
                    }
                }");
        }

        /// <summary>
        /// For checking JSON deserialization when there's a bad name.
        /// Activities object has "incorrect_activity_name" property instead of "activity_name"
        /// </summary>
        /// <returns></returns>
        [HttpGet("TOM/BadNameInJson/{*url}")]
        public IActionResult GetTomObjectWithBadName()
        {
            return Ok(@"
                    [
                       {
                         ""activities"": [
                           {
                             ""activity_id"": 1,
                             ""activity_name"": ""Drugs on the Market for >2 years"",
                             ""subactivities"": [
                               {
                                 ""subactivity_id"": 1,
                                 ""subactivity_name"": ""Angiodema""
                               },
                               {
                                 ""subactivity_id"": 2,
                                 ""subactivity_name"": ""Asenapine-Aripiprazole""
                               }
                             ]
                           },
                           {
                             ""activity_id"": 2,
                             ""activity_name"": ""Drug Use Studies - Comparison to Nationally Projected Databases"",
                             ""subactivities"": [
                               {
                                 ""subactivity_id"": 4,
                                 ""subactivity_name"": ""Drug Use""
                               }
                             ]
                           }
                         ],
                         ""task_order"": ""03""
                       },
                       {
                         ""activities"": 
                            [
                               {
                                 ""activity_id"": 5,
                                 ""activity_name"": ""HOI Validation/Adjudication"",
                                 ""subactivities"": 
                                    [
                                       {
                                         ""subactivity_id"": 8,
                                         ""incorrect_subactivity_name"": ""Incorrect subactivity""
                                       }
                                    ]
                               }
                             ],
                            ""task_order"": ""04""
                        }
                    ]");
        }

        /// <summary>
        /// For checking JSON deserialization when it's missing a mandatory field.
        /// This JSON object is missing the "activity_name" property.
        /// </summary>
        /// <returns></returns>
        [HttpGet("TOM/MissingMandatoryInJson/{*url}")]
        public IActionResult GetTomObjectWithMissingMandatoryInfo()
        {
            return Ok(@"
                    [
                       {
                         ""activities"": [
                           {
                             ""activity_id"": 1,
                             ""activity_name"": ""Drugs on the Market for >2 years"",
                             ""subactivities"": [
                               {
                                 ""subactivity_id"": 1,
                                 ""subactivity_name"": ""Angiodema""
                               },
                               {
                                 ""subactivity_id"": 2,
                                ""subactivity_name"": ""Asenapine-Aripiprazole"",
                                ""description"":""This is a version that was released at the beginning of August, 2022.""
                               }
                             ]
                           },
                           {
                             ""activity_id"": 2,
                             ""activity_name"": ""Drug Use Studies - Comparison to Nationally Projected Databases"",
                             ""subactivities"": [
                               {
                                 ""subactivity_id"": 4,
                                ""subactivity_name"": ""Drug Use"",
                                ""description"":""This is a version that was released at the beginning of August, 2022.""
                               }
                             ]
                           }
                         ],
                         ""task_order"": ""03""
                       },
                       {
                         ""activities"": 
                            [
                               {
                                 ""activity_id"": 5,
                                 ""activity_name"": ""HOI Validation/Adjudication"",
                                 ""subactivities"": 
                                    [
                                       {
                                         ""subactivity_id"": 8,
                                         ""subactivity_name"": """"
                                       }
                                    ]
                               }
                             ],
                            ""task_order"": ""04""
                        }
                    ]");
        }

        /// <summary>
        /// Returns a Json object with an additional "description" property in each of the "versions" objects
        /// </summary>
        /// <returns></returns>
        [HttpGet("TOM/ExtraPropertyInJson/{*url}")]
        public IActionResult GetTomObjectWithExtraProperty()
        {
            // versions object has 

            return Ok(@"
            [{
	
	            ""activities"": [
                {
			            ""activity_id"": 1,
			            ""activity_name"": ""Drugs on the Market for >2 years"",
			            ""subactivities"": [
                    {
				            ""activity_name"": """",
				            ""subactivity_name"": """",
				            ""task_order"": """",
				            ""requester"": """",
				            ""requester_id"": """",
				            ""subactivity_id"": 1,
				            ""subactivity_name"": ""Angiodema""
				            },
                        {
				            ""activity_name"": """",
				            ""subactivity_name"": """",
				            ""task_order"": """",
				            ""requester"": """",
				            ""requester_id"": """",
				            ""subactivity_id"": 2,
				            ""subactivity_name"": ""Asenapine-Aripiprazole""
				            }
			            ]
			
                        },
                        ""source"": ""fdb"",
                        ""versions"": 
                        [
                            {
			            ""activity_id"": 2,
			            ""activity_name"": ""Drug Use Studies - Comparison to Nationally Projected Databases"",
			            ""subactivities"": [
				            {
				            ""activity_name"": """",
				            ""subactivity_name"": """",
				            ""task_order"": """",
				            ""requester"": """",
				            ""requester_id"": """",
				            ""subactivity_id"": 4,
				            ""subactivity_name"": ""Drug Use""
				            }
			            ]
                            },
                            {
			            ""activity_id"": 5,
			            ""activity_name"": ""HOI Validation/Adjudication"",
			            ""subactivities"": [
				            {
				            ""activity_name"": """",
				            ""subactivity_name"": """",
				            ""task_order"": """",
				            ""requester"": """",
				            ""requester_id"": """",
				            ""subactivity_id"": 8,
				            ""subactivity_name"": """"
			            }
			            ]
                            }
                        ]
               }]");
        }

        /// <summary>
        /// returns a Json string that is missing an opening bracket at the start of the "versions" section
        /// </summary>
        /// <returns></returns>
        [HttpGet("TOM/MalformedJson/{*url}")]
        public IActionResult GetMisformedTomJsonObject()
        {
            return Ok(@"
            
               {
                 ""activities"": [
                {
                     ""activity_id"": 1,
                     ""activity_name"": ""Drugs on the Market for >2 years"",
                     ""subactivities"": [
                    {
                         ""activity_name"": """",
                         ""subactivity_name"": """",
                         ""task_order"": """",
                         ""requester"": """",
                         ""requester_id"": """",
                         ""subactivity_id"": 1,
                         ""subactivity_name"": ""Angiodema"",
                         ""description"":""This is a version that was released at the end of April, 2022.""
                       },
                        {
                         ""activity_name"": """",
                         ""subactivity_name"": """",
                         ""task_order"": """",
                         ""requester"": """",
                         ""requester_id"": """",
                         ""subactivity_id"": 2,
                         ""subactivity_name"": ""Asenapine-Aripiprazole"",
                         ""description"":""This is a version that was released at the end of April, 2022.""
                       }
                     ]
                        },
                   {
                     ""activity_id"": 2,
                     ""activity_name"": ""Drug Use Studies - Comparison to Nationally Projected Databases"",
                     ""subactivities"": [
                            {
                         ""activity_name"": """",
                         ""subactivity_name"": """",
                         ""task_order"": """",
                         ""requester"": """",
                         ""requester_id"": """",
                         ""subactivity_id"": 4,
                         ""subactivity_name"": ""Drug Use""
                       }
                     ]
                   }
                 ],
                 ""task_order"": ""03""
                            },
                            {
                 ""activities"": [
                   {
                     ""activity_id"": 5,
                     ""activity_name"": ""HOI Validation/Adjudication"",
                     ""subactivities"": [
                       {
                         ""activity_name"": """",
                         ""subactivity_name"": """",
                         ""task_order"": """",
                         ""requester"": """",
                         ""requester_id"": """",
                         ""subactivity_id"": 8,
                         ""subactivity_name"": """"
                            }
                        ]
                   }
                 ],
                 ""task_order"": ""04""
               }
            ");
        }

        [HttpGet("NullJson/{*url}")]
        public IActionResult GetNullJsonObject()
        {
            return Ok();
        }

        [HttpGet("EmptyJson/{*url}")]
        public IActionResult GetEmptyJsonObject()
        {
            return Ok(String.Empty);
        }


        ///// <summary>
        ///// Returns valid versions list
        ///// </summary>
        ///// <returns></returns>
        //public IActionResult GetValidVersionsList()
        //{
        //    throw new NotImplementedException("Build versions list...");
        //}

        //public IActionResult GetValidCategoriesList()
        //{
        //    throw new NotImplementedException("Build categories list...");
        //}

        //public IActionResult GetValidCodeValues()
        //{
        //    throw new NotImplementedException("Build values list...");
        //}



    }
}
