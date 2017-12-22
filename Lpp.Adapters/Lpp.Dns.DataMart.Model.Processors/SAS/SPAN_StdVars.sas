***************************************************
StdVars.sas for the SPAN project

This file acts in the same way as the VDW
StdVars except that it points to SPAN data
mart tables instead of the VDW.

SAS programs can reference SPAN data mart tables
using the standard macro parameter names defined
in this file.  The specific pathnames remain local.

***************************************************;

options nonotes nosource;  *suppress printing of stdvars information to log;
options errors=0;  *suppress printing of ERROR information to log;

***********************************
Site Identifiers
***********************************;

  %let _SiteCode = 06; *51; *08; *18;   *O7;   
  %let _SiteAbbr = KPCO; *DHHA;  *HRVD;   *EIRH;    *HPRF;
  %let _SiteName = Kaiser Permanente Colorado;

***********************************
Directory Pathnames

Example:

  %let adhd_datamart=\\root\SPAN\DataMarts\ADHD
  %let obesity_datamart=\\root\SPAN\DataMarts\Obesity

***********************************;

%let adhd_datamart=\\rmregfile004\research\Analytic Projects\2010_SPAN_EX\Data\Distributed Code Data\ADHD\datamart;
*%let obesity_datamart=\\rmregfile004\research\Analytic Projects\2010_SPAN_EX\Data\Distributed Code Data\Obesity\datamart;

*For Testing;
%let obesity_datamart=C:\datamart;

***********************************
Data Mart Pathnames and Libraries

You shouldn't need to edit below
this line.
***********************************;

    libname __adhd "&adhd_datamart";
    libname __ob   "&obesity_datamart";

    %let _span_a_demog                  = __adhd.datamart1_demog ;
    %let _span_a_census                 = __adhd.datamart1_census ;
    %let _span_a_diagnoses              = __adhd.datamart1_diagnoses ;
    %let _span_a_procedures             = __adhd.datamart1_procedures ;
    %let _span_a_pharmacy               = __adhd.datamart1_pharmacy ;
    %let _span_a_everndc                = __adhd.datamart1_everndc ;     * EVERNDC is the same for both data marts;
    %let _span_a_encounters             = __adhd.datamart1_encounters ;
    %let _span_a_vitals                 = __adhd.datamart1_vitals ;
    %let _span_a_lab                    = __adhd.datamart1_labs ;
    %let _span_a_labnotes               = __adhd.datamart1_labnotes ;
    %let _span_a_enrollment             = __adhd.datamart1_enrollment ;
    %let _span_a_cont_enrollment        = __adhd.datamart1_cont_enrollment ;
    %let _span_a_death                  = __adhd.datamart1_death ;
    %let _span_a_causeofdeath           = __adhd.datamart1_causeofdeath ;
    %let _span_a_phq9                   = __adhd.datamart1_phq9 ;
    %let _span_a_vanderbilt             = __adhd.datamart1_vanderbilt ;
    %let _span_a_social_history         = __adhd.datamart1_social_history ;
    %let _span_a_provider         	= __adhd.datamart1_provider;

    %let _span_o_demog                  = __ob.datamart2_demog ;
    %let _span_o_census                 = __ob.datamart2_census ;
    %let _span_o_diagnoses              = __ob.datamart2_diagnoses ;
    %let _span_o_procedures             = __ob.datamart2_procedures ;
    %let _span_o_pharmacy               = __ob.datamart2_pharmacy ;
    %let _span_o_everndc                = __adhd.datamart1_everndc ;     * EVERNDC is the same for both data marts;
    %let _span_o_encounters             = __ob.datamart2_encounters ;
    %let _span_o_vitals                 = __ob.datamart2_vitals ;
    %let _span_o_lab                    = __ob.datamart2_labs ;
    %let _span_o_labnotes               = __ob.datamart2_labnotes ;
    %let _span_o_enrollment             = __ob.datamart2_enrollment ;
    %let _span_o_cont_enrollment        = __ob.datamart2_cont_enrollment ;
    %let _span_o_death                  = __ob.datamart2_death ;
    %let _span_o_causeofdeath           = __ob.datamart2_causeofdeath ;
    %let _span_o_phq9                   = __ob.datamart2_phq9 ;
    %let _span_o_vanderbilt             = __ob.datamart2_vanderbilt ;
    %let _span_o_social_history         = __ob.datamart2_social_history ;
    %let _span_o_provider         	= __adhd.datamart1_provider;


* REFERENCE TO THE STANDARD MACROS FILE ;
  filename vdw_macs  FTP     "standard_macros.sas"
                     HOST  = "vdw.hmoresearchnetwork.org"
                     CD    = "/vdwcode"
                     PASS  = "%2hilario36"
                     USER  = "VDWReader" ;

  %include vdw_macs;

options notes source;  *notes and source turned back on, stdvars.sas completed;

