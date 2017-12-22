using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RequestCriteria.Models;

namespace Lpp.Dns.General.Metadata
{
    public class SearchTermCodeTranslator
    {
        public const string NullProject = "00000000-0000-0000-0000-000000000000";

        static public string ClinicalSettingDataTypeToSearchTermCode(ClinicalSettingTypes Type)
        {
            string code;
            switch (Type)
            {
                case ClinicalSettingTypes.Any:
                    code = "AN";
                    break;
                case ClinicalSettingTypes.Emergency:
                    code = "ED";
                    break;
                case ClinicalSettingTypes.InPatient:
                    code = "IP";
                    break;
                case ClinicalSettingTypes.OutPatient:
                    code = "AV";
                    break;
                case ClinicalSettingTypes.NotSpecified:
                    code = "";
                    break;
                default:
                    code = "";
                    break;
            }
            return code;
        }

        static public int SexDataTypeToSearchTermCode(SexTypes Type)
        {
            int code;
            switch (Type)
            {
                case SexTypes.Male:
                    code = 2;
                    break;
                case SexTypes.Female:
                    code = 1;
                    break;
                case SexTypes.Both:
                    code = 3;
                    break;
                case SexTypes.Aggregated:
                    code = 4;
                    break;
                default:
                    code = 0;
                    break;
            }
            return code;
        }

        static public int AgeStratifierDataTypeToSearchTermCode(AgeStratifierTypes Type)
        {
            int code;
            switch (Type)
            {
                case AgeStratifierTypes.None:
                    code = 0;
                    break;
                case AgeStratifierTypes.Two:
                    code = 4;
                    break;
                case AgeStratifierTypes.Four:
                    code = 3;
                    break;
                case AgeStratifierTypes.Seven:
                    code = 2;
                    break;
                case AgeStratifierTypes.Ten:
                    code = 1;
                    break;
                default:
                    code = 0;
                    break;
            }
            return code;
        }

        static public RequestSearchTermType CriteraTermTypeToSearchTermType(CodesTermTypes Type)
        {
            RequestSearchTermType searchTermType;
            switch (Type)
            {
                case CodesTermTypes.Diagnosis_ICD9Term:
                    searchTermType = RequestSearchTermType.ICD9DiagnosisCode;
                    break;
                case CodesTermTypes.Procedure_ICD9Term:
                    searchTermType = RequestSearchTermType.ICD9ProcedureCode;
                    break;
                case CodesTermTypes.DrugClassTerm:
                    searchTermType = RequestSearchTermType.DrugClassCode;
                    break;
                case CodesTermTypes.GenericDrugTerm:
                    searchTermType = RequestSearchTermType.GenericDrugCode;
                    break;
                case CodesTermTypes.HCPCSTerm:
                    searchTermType = RequestSearchTermType.HCPCSCode;
                    break;
                default:
                    searchTermType = RequestSearchTermType.Text;
                    break;
            }
            return searchTermType;
        }

    }
}
