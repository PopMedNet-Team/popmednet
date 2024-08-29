using Lpp.Objects.Dynamic;
using Lpp.QueryComposer;
using Lpp.Dns.DTO.QueryComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Lpp.Dns.DataMart.Model.QueryComposer.Adapters.PCORI.Terms
{
    public class CombinedDiagnosisCodes : TermImplementation
    {
        public CombinedDiagnosisCodes(PCORIQueryBuilder.DataContext db)
            : base(ModelTermsFactory.CombinedDiagnosisCodesID, db)
        {
        }

        public override Objects.Dynamic.IPropertyDefinition[] InnerSelectPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "DiagnosisCodeType",
                        Type = typeof(string).FullName
                    },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "DiagnosisCode",
                        Type = typeof(string).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> InnerSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {

            //to provide for a resulting column to be returned, going to manufacture a column result that will be the combination of the criteria values as a single value.
            //This will still allow for grouping to work as normal, and not affect the results from a count per distinct patient bias.

            List<string> codeTypes = new List<string>();
            List<string> codeValues = new List<string>();

            var codeCriteria = Criteria.SelectMany(c => c.Terms.Where(t => t.Type == ModelTermsFactory.CombinedDiagnosisCodesID).Concat(c.Criteria.SelectMany(cc => cc.Terms.Where(t => t.Type == ModelTermsFactory.CombinedDiagnosisCodesID)))).ToArray();

            if (codeCriteria.Any())
            {

                foreach (var term in codeCriteria)
                {
                    DTO.Enums.DiagnosisCodeTypes codeType;
                    if (!Enum.TryParse<DTO.Enums.DiagnosisCodeTypes>(term.GetStringValue("CodeType"), out codeType))
                    {
                        codeType = DTO.Enums.DiagnosisCodeTypes.Any;
                    }

                    var codes = (term.GetStringValue("CodeValues") ?? "").Split(new[] { ';' }).Where(x => !string.IsNullOrEmpty(x.Trim())).Select(s => s.Trim()).Distinct().ToArray();
                    if (codes.Length == 0)
                        continue;

                    codeTypes.Add(Utilities.ToString(codeType, true));
                    codeValues.AddRange(codes);
                }

            }

            if (codeTypes.Count == 0)
            {
                codeTypes.Add("Any");
            }

            if (codeValues.Count == 0)
            {
                codeValues.Add("Any");
            }

            codeTypes.Sort();
            codeValues.Sort();

            string codeTypeString = string.Join(", ", codeTypes.Distinct().ToArray());
            string codeValuesString = string.Join(", ", codeValues.Distinct().ToArray());

            int index = codeTypeString.LastIndexOf(',');
            if ( index > 0)
            {
                codeTypeString = codeTypeString.Substring(0, index) + codeTypeString.Substring(index).Replace(",", ", or");
            }

            index = codeValuesString.LastIndexOf(',');
            if (index > 0)
            {
                codeValuesString = codeValuesString.Substring(0, index) + codeValuesString.Substring(index).Replace(",", ", or");
            }

            return new[] {
                Expression.Bind(selectType.GetProperty("DiagnosisCodeType"), Expression.Constant(codeTypeString)),
                Expression.Bind(selectType.GetProperty("DiagnosisCode"), Expression.Constant(codeValuesString))
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] GroupKeyPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "DiagnosisCodeType",
                        Type = typeof(string).FullName
                    },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "DiagnosisCode",
                        Type = typeof(string).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> GroupKeySelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            return new[]{
                Expression.Bind(selectType.GetProperty("DiagnosisCodeType"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("DiagnosisCodeType"))),
                Expression.Bind(selectType.GetProperty("DiagnosisCode"), Expression.Property(sourceTypeParameter, sourceTypeParameter.Type.GetProperty("DiagnosisCode")))
            };
        }

        public override Objects.Dynamic.IPropertyDefinition[] FinalSelectPropertyDefinitions()
        {
            return new[]{
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "DiagnosisCodeType",
                        Type = typeof(string).FullName
                    },
                new DTO.QueryComposer.QueryComposerResponsePropertyDefinitionDTO
                    {
                        Name = "DiagnosisCode",
                        Type = typeof(string).FullName
                    }
            };
        }

        public override IEnumerable<System.Linq.Expressions.MemberBinding> FinalSelectBindings(Type selectType, System.Linq.Expressions.ParameterExpression sourceTypeParameter)
        {
            return new[] { 
                Expression.Bind(selectType.GetProperty("DiagnosisCodeType"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "DiagnosisCodeType")),
                Expression.Bind(selectType.GetProperty("DiagnosisCode"), Expressions.ChildPropertyExpression(sourceTypeParameter, "Key", "DiagnosisCode")) 
            };
        }

        /// <summary>
        /// Converts a DIagnosisCodeTypes enum to the appropriate PCORI defined value.
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public static string FromDiagnosisCodeType(DTO.Enums.DiagnosisCodeTypes codeType)
        {
            switch (codeType)
            {
                case DTO.Enums.DiagnosisCodeTypes.Any:
                    return "";
                case DTO.Enums.DiagnosisCodeTypes.Unknown:
                    return "UN";
                case DTO.Enums.DiagnosisCodeTypes.NoInformation:
                    return "NI";
                case DTO.Enums.DiagnosisCodeTypes.Other:
                    return "OT";
                case DTO.Enums.DiagnosisCodeTypes.ICD10:
                    return "10";
                case DTO.Enums.DiagnosisCodeTypes.ICD11:
                    return "11";
                case DTO.Enums.DiagnosisCodeTypes.ICD9:
                    return "09";
                case DTO.Enums.DiagnosisCodeTypes.SNOWMED_CT:
                    return "SM";
            }

            throw new NotSupportedException("The diagnosis code type '" + codeType + "' is not supported.");
        }
    }
}
