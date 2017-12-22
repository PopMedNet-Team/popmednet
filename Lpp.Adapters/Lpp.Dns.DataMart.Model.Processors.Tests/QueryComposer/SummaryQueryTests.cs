using Lpp.Dns.DTO.QueryComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Model.Processors.Tests.QueryComposer
{
    [TestClass]
    public class SummaryQueryTests
    {
        static readonly Guid IncidenceModelID = new Guid("4F364773-20A0-4036-800B-841421CB3209");
        static readonly Guid PrevalenceModelID = new Guid("4C99FA21-CDEA-4B09-B95B-EEBDDA05ADEA");
        static readonly Guid MFU_ModelID = new Guid("805DF412-4ACC-4BA0-B0AD-7C4C24FD9F67");

        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [ClassInitialize]
        public static void StartUp(TestContext context)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
        }

        [TestMethod]
        public void Inci_ICD9DiagnosisQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Criteria = new []{
                            new QueryComposerCriteriaDTO{
                                Name = "i_codeterms",
                                ID = Guid.NewGuid(),
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = DTO.Enums.QueryComposerCriteriaTypes.Paragraph,
                                Terms = new []{
                                    new QueryComposerTermDTO
                                    {
                                        Operator = DTO.Enums.QueryComposerOperators.Or,
                                        Type = Lpp.QueryComposer.ModelTermsFactory.ICD9DiagnosisCodes3digitID,
                                        Values = new Dictionary<string,object>{
                                            {"Values", new Dictionary<string,object>
                                                { 
                                                    {"Codes", null}, 
                                                    {"CodeValues", new []{ new Dns.DTO.CodeSelectorValueDTO{ Code="250", Name="DIABETES MELLITUS"}} }  
                                                } 
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.SettingID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Setting", "1"}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.YearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        Type = Lpp.QueryComposer.ModelTermsFactory.ICD9DiagnosisCodes3digitID
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Ten.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(IncidenceModelID, request);
        }

        [TestMethod]
        public void Inci_PharaDispensingGenericDrugNameQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Criteria = new []{
                            new QueryComposerCriteriaDTO{
                                Name = "i_codeterms",
                                ID = Guid.NewGuid(),
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = DTO.Enums.QueryComposerCriteriaTypes.Paragraph,
                                Terms = new []{
                                    new QueryComposerTermDTO
                                    {
                                        Operator = DTO.Enums.QueryComposerOperators.Or,
                                        Type = Lpp.QueryComposer.ModelTermsFactory.DrugNameID,
                                        Values = new Dictionary<string,object>{
                                            {"Values", new Dictionary<string,object>
                                                { 
                                                    {"Codes", null}, 
                                                    {"CodeValues", new []{ new Dns.DTO.CodeSelectorValueDTO{ Code="DOBUTAMINE HCL", Name="DOBUTAMINE HCL"}} }  
                                                } 
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.QuarterYearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        },
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        Type = Lpp.QueryComposer.ModelTermsFactory.DrugNameID
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Ten.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(IncidenceModelID, request);
        }

        [TestMethod]
        public void Inci_PharaDispensingDrugClassQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Criteria = new []{
                            new QueryComposerCriteriaDTO{
                                Name = "i_codeterms",
                                ID = Guid.NewGuid(),
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = DTO.Enums.QueryComposerCriteriaTypes.Paragraph,
                                Terms = new []{
                                    new QueryComposerTermDTO
                                    {
                                        Operator = DTO.Enums.QueryComposerOperators.Or,
                                        Type = Lpp.QueryComposer.ModelTermsFactory.DrugClassID,
                                        Values = new Dictionary<string,object>{
                                            {"Values", new Dictionary<string,object>
                                                { 
                                                    {"Codes", null}, 
                                                    {"CodeValues", new []{ new Dns.DTO.CodeSelectorValueDTO{ Code="Ophthalmic - Anti-infective-Anti-inflammatory Combinations", Name="Ophthalmic - Anti-infective-Anti-inflammatory Combinations"}} }  
                                                } 
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.QuarterYearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", "2008"},
                                            {"EndYear", "2008"}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        Type = Lpp.QueryComposer.ModelTermsFactory.DrugClassID
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Ten.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(IncidenceModelID, request);
        }

        [TestMethod]
        public void Prev_EnrollmentQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.CoverageID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Coverage", "1"}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.YearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        Type = Lpp.QueryComposer.ModelTermsFactory.CoverageID
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Ten.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(PrevalenceModelID, request);
        }

        [TestMethod]
        public void Prev_HCPCSProceduresQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Criteria = new []{
                            new QueryComposerCriteriaDTO{
                                Name = "i_codeterms",
                                ID = Guid.NewGuid(),
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = DTO.Enums.QueryComposerCriteriaTypes.Paragraph,
                                Terms = new []{
                                    new QueryComposerTermDTO
                                    {
                                        Operator = DTO.Enums.QueryComposerOperators.Or,
                                        Type = Lpp.QueryComposer.ModelTermsFactory.HCPCSProcedureCodesID,
                                        Values = new Dictionary<string,object>{
                                            {"Values", new Dictionary<string,object>
                                                { 
                                                    {"Codes", null}, 
                                                    {"CodeValues", new []{ 
                                                        new Dns.DTO.CodeSelectorValueDTO{ Code="87252", Name="87252"},
                                                        new Dns.DTO.CodeSelectorValueDTO{ Code="86790", Name="86790"},
                                                        new Dns.DTO.CodeSelectorValueDTO{ Code="87260", Name="87260"}
                                                        }
                                                    }  
                                                } 
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.SettingID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Setting", "4"}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.YearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        Type = Lpp.QueryComposer.ModelTermsFactory.HCPCSProcedureCodesID
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Four.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleOnly.ToString("D")
                    }
                }
            };

            ExecuteRequest(PrevalenceModelID, request);
        }

        [TestMethod]
        public void Prev_ICD9ProceduresQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Criteria = new []{
                            new QueryComposerCriteriaDTO{
                                Name = "i_codeterms",
                                ID = Guid.NewGuid(),
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = DTO.Enums.QueryComposerCriteriaTypes.Paragraph,
                                Terms = new []{
                                    new QueryComposerTermDTO
                                    {
                                        Operator = DTO.Enums.QueryComposerOperators.Or,
                                        Type = Lpp.QueryComposer.ModelTermsFactory.ICD9ProcedureCodes3digitID,
                                        Values = new Dictionary<string,object>{
                                            {"Values", new Dictionary<string,object>
                                                { 
                                                    {"Codes", null}, 
                                                    {"CodeValues", new []{ 
                                                        new Dns.DTO.CodeSelectorValueDTO{ Code="012", Name="012"},
                                                        new Dns.DTO.CodeSelectorValueDTO{ Code="011", Name="011"},
                                                        new Dns.DTO.CodeSelectorValueDTO{ Code="010", Name="010"}
                                                        }
                                                    }  
                                                } 
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.SettingID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Setting", DTO.Enums.Settings.AN.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.YearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        Type = Lpp.QueryComposer.ModelTermsFactory.ICD9ProcedureCodes3digitID
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Four.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleOnly.ToString("D")
                    }
                }
            };

            ExecuteRequest(PrevalenceModelID, request);
        }

        [TestMethod]
        public void Prev_ICD9DiagnosisQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Criteria = new []{
                            new QueryComposerCriteriaDTO{
                                Name = "i_codeterms",
                                ID = Guid.NewGuid(),
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = DTO.Enums.QueryComposerCriteriaTypes.Paragraph,
                                Terms = new []{
                                    new QueryComposerTermDTO
                                    {
                                        Operator = DTO.Enums.QueryComposerOperators.Or,
                                        Type = Lpp.QueryComposer.ModelTermsFactory.ICD9ProcedureCodes3digitID,
                                        Values = new Dictionary<string,object>{
                                            {"Values", new Dictionary<string,object>
                                                { 
                                                    {"Codes", null}, 
                                                    {"CodeValues", new []{ 
                                                        new Dns.DTO.CodeSelectorValueDTO{ Code="99813", Name="99813"},
                                                        new Dns.DTO.CodeSelectorValueDTO{ Code="27542", Name="27542"},
                                                        new Dns.DTO.CodeSelectorValueDTO{ Code="19881", Name="19881"}
                                                        }
                                                    }  
                                                } 
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.SettingID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Setting", DTO.Enums.Settings.IP.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.YearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        Type = Lpp.QueryComposer.ModelTermsFactory.ICD9ProcedureCodes3digitID
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Four.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(PrevalenceModelID, request);
        }

        [TestMethod]
        public void MFU_HCPCSProcedureCodesQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.SettingID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Setting", DTO.Enums.Settings.AN.ToString("D")}
                                        }
                                    }
                                }

                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.CodeMetricID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Metric", DTO.Enums.CodeMetric.Events.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.CriteriaID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Criteria", DTO.Enums.OutputCriteria.Top10.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.YearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        },
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        FieldName = "HCPCS Procedure Code",
                        Type = Lpp.QueryComposer.ModelTermsFactory.HCPCSProcedureCodesID,
                        Aggregate = null,
                        StratifyBy = null
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Ten.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(MFU_ModelID, request);
        }

        [TestMethod]
        public void MFU_ICD9ProcedureCodesQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.SettingID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Setting", DTO.Enums.Settings.AN.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.CodeMetricID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Metric", DTO.Enums.CodeMetric.Events.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.CriteriaID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Criteria", DTO.Enums.OutputCriteria.Top20.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.YearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        },
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        FieldName = "ICD-9 Procedure Code",
                        Type = Lpp.QueryComposer.ModelTermsFactory.ICD9ProcedureCodes3digitID,
                        Aggregate = null,
                        StratifyBy = null
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Four.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(MFU_ModelID, request);
        }

        [TestMethod]
        public void MFU_ICD9DiagnosisCodesQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.SettingID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Setting", DTO.Enums.Settings.AN.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.CodeMetricID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Metric", DTO.Enums.CodeMetric.Events.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.CriteriaID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Criteria", DTO.Enums.OutputCriteria.Top20.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.YearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        },
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        FieldName = "ICD-9 Diagnosis Code",
                        Type = Lpp.QueryComposer.ModelTermsFactory.ICD9DiagnosisCodes5digitID,
                        Aggregate = null,
                        StratifyBy = null
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Four.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(MFU_ModelID, request);
        }

        [TestMethod]
        public void MFU_PharmaDispensingDrugNameQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.DispensingMetricID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Metric", DTO.Enums.DispensingMetric.Dispensing_DrugOnly.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.CriteriaID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Criteria", DTO.Enums.OutputCriteria.Top20.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.QuarterYearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        },
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Pharmacy Dispensing by Generic Name",
                        Type = Lpp.QueryComposer.ModelTermsFactory.DrugNameID,
                        Aggregate = null,
                        StratifyBy = null
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Four.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(MFU_ModelID, request);
        }

        [TestMethod]
        public void MFU_PharmaDispensingDrugClassQuery()
        {
            QueryComposerRequestDTO request = CreateRequest();
            request.Where = new QueryComposerWhereDTO
            {
                Criteria = new QueryComposerCriteriaDTO[] 
                {                        
                    new QueryComposerCriteriaDTO
                    {
                        Name = "Group 1",
                        Operator = DTO.Enums.QueryComposerOperators.And,
                        Exclusion = false,
                        Terms = new QueryComposerTermDTO[]
                        {
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.DispensingMetricID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Metric", DTO.Enums.DispensingMetric.Dispensing_DrugOnly.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.CriteriaID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"Criteria", DTO.Enums.OutputCriteria.Top20.ToString("D")}
                                        }
                                    }
                                }
                            },
                            new QueryComposerTermDTO
                            {
                                Operator = DTO.Enums.QueryComposerOperators.And,
                                Type = Lpp.QueryComposer.ModelTermsFactory.QuarterYearID,
                                Values = new Dictionary<string,object>{
                                    { "Values", new Dictionary<string,object>{
                                            {"StartYear", 2008},
                                            {"EndYear", 2008}
                                        }
                                    }
                                }
                            }
                        },
                    }
                }
            };
            request.Select = new QueryComposerSelectDTO
            {
                Fields = new[] 
                {
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Pharmacy Dispensing by Drug Class",
                        Type = Lpp.QueryComposer.ModelTermsFactory.DrugClassID,
                        Aggregate = null,
                        StratifyBy = null
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Age Range",
                        Type = Lpp.QueryComposer.ModelTermsFactory.AgeRangeID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.AgeStratifications.Four.ToString("D")
                    },
                    new QueryComposerFieldDTO
                    {
                        FieldName = "Sex",
                        Type = Lpp.QueryComposer.ModelTermsFactory.SexID,
                        Aggregate = null,
                        StratifyBy = DTO.Enums.SexStratifications.MaleAndFemale.ToString("D")
                    }
                }
            };

            ExecuteRequest(MFU_ModelID, request);
        }

        QueryComposerRequestDTO CreateRequest()
        {
            return new QueryComposerRequestDTO
            {
                ID = Guid.NewGuid(),
                Timestamp = BitConverter.GetBytes(DateTime.UtcNow.Ticks),
                Header = new QueryComposerHeaderDTO
                {
                    Name = "Example Request",
                    Description = "This is a unit test request."
                }
            };
        }

        void ExecuteRequest(Guid modelID, DTO.QueryComposer.QueryComposerRequestDTO request)
        {
            ExecuteRequest(modelID, System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(request)));
        }

        void ExecuteRequest(Guid modelID, byte[] requestDocumentContent)
        {
            System.IO.File.WriteAllBytes("request.json", requestDocumentContent);

            var processor = CreateProcessor(modelID);            

            IDictionary<string, string> properties;
            Document[] documents;

            string requestID = Guid.NewGuid().ToString("D");
            string requestDocumentID = Guid.NewGuid().ToString("D");

            processor.Request(requestID,
                new NetworkConnectionMetadata
                {
                    OrganizationName = "Test Org",
                    UserEmail = "test@test.com",
                    UserFullName = "Administrator",
                    UserLogin = "administrator"
                },
                new RequestMetadata
                {
                    DataMartId = "dm1",
                    DataMartName = "Test DataMart",
                    DataMartOrganizationId = "1",
                    DataMartOrganizationName = "Test Org",
                    IsMetadataRequest = false,
                    RequestTypeId = ""
                },
                new Document[] {
                    new Document(requestDocumentID, "text/json", "request.json"){ IsViewable = false, Size = requestDocumentContent.Length }
                },
                out  properties,
                out documents);

            using (var ms = new System.IO.MemoryStream(System.IO.File.ReadAllBytes("request.json")))
            {
                processor.RequestDocument(requestID, requestDocumentID, ms);
            }

            processor.Start(requestDocumentID);

            Document[] responseDocuments = processor.Response(requestID);

            string json = null;
            System.IO.Stream responseStream = null;
            try
            {
                processor.ResponseDocument(requestID, responseDocuments.First().DocumentID, out responseStream, int.MaxValue);

                using (System.IO.StreamReader reader = new System.IO.StreamReader(responseStream))
                {
                    json = reader.ReadToEnd();
                }
                System.IO.File.WriteAllText("response.json", json, System.Text.Encoding.UTF8);
            }
            finally
            {
                if (responseStream != null)
                    responseStream.Dispose();
            }

            Console.WriteLine(json);

            var serializationSettings = new Newtonsoft.Json.JsonSerializerSettings();
            serializationSettings.Converters.Add(new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionConverter());

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DTO.QueryComposer.QueryComposerResponseDTO>(json, serializationSettings);
            Assert.IsNotNull(obj);
            Assert.IsTrue(obj.Errors == null || !obj.Errors.Any());
        }

        Lpp.Dns.DataMart.Model.IModelProcessor CreateProcessor(Guid modelID)
        {
            var processor = new Lpp.Dns.DataMart.Model.QueryComposerModelProcessor();
            processor.Settings = new Dictionary<string, object> {
                {"ModelID", modelID},
                {"Server", "localhost"},
                {"UserID", ""},
                {"Password", ""},
                {"Database", "SummaryQuery"},
                {"DataProvider", "SQLServer"},
                {"ConnectionTimeout", "60"},
                {"CommandTimeout", "120"}
            };
            return processor;
        }
    }
}
