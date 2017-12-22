using System;
using System.Collections.Generic;

namespace Lpp.Dns.General.HealthCare
{
    public enum LookupList
    {
	    GenericName = 1,
	    DrugClass = 2,
	    DrugCode = 3,
	    ICD9Diagnosis = 4,
	    ICD9Procedures = 5,
	    HCPCSProcedures = 6,
	    ICD9Diagnosis4Digit = 7,
	    ICD9Diagnosis5Digit = 8,
	    ICD9Procedures4Digit = 9,
	    SPANDiagnosis = 10,
	    SPANProcedure = 11,
	    SPANDrug = 12,
        ZipCode = 13
    }
}
