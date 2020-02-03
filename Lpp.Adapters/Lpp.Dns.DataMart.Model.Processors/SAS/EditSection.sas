******************************************;
*  EditSection BEGIN                      ;
******************************************;

/* SPAN ADHD STUDY1_Essentia and DenverHealth_WP01V01 */

* Pick a seed to randomly sample potential ADHD cases from your datamart.;
*%let seed=345762;     *<== Example;
%let seed=6794657;

* Create a folder for files that will remain at your site.  Point to that folder with &localfiles.;
*%let localfiles=\\root\SPAN Science\ADHD\ADHD Study 1_Validity of ICD9 Codes\LOCAL;    *<== Example;
%let localfiles=\\rmregfile004\research\Analytic Projects\2010_SPAN_EX\Data\Distributed Code Data\SPAN Science\ADHD\ADHD Study 1_Validity of ICD9 Codes\LOCAL;





/* SPAN Obesity Substudy 1 and 3_WP01V02.sas */
*Select the appropriate procedure date variable for your site;
%let procdate = procdate;     *if your site populates the procdate variable;
*%let procdate = adate;     *if your site does not populate the procdate variable;


/* SPAN Obesity Enrollment QA WP01V02 */
/* Provide local location where program can output files not to be returned to the DCC */
/* Notice final \ */
%let outlocal = \\Rmregfile016\span\WORKPLANS\Enrollment QA\LOCAL;

/* PATHNAME DELIMITER */
%let SLASH=\;

/* SPAN_STDVARS FOR SPAN DATA MARTS */
%INCLUDE '\\rmlhofile001\users\M426654\My SPAN Files\SPAN_StdVars.sas';

/* FIND LOCATION OF UPLOAD FOLDER */
%INCLUDE '\\rmlhofile001\users\M426654\My SPAN Files\current_query_folder.sas';

******************************************;
*  EditSection END                        ;
******************************************;

