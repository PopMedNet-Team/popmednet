/*****************************************************************************************************/
/* SPAN SAS Code to accompany SPAN Portal Query Builder     				 	         May 29, 2013             
* David Tabano
* Institute for Health Research
* Kaiser Permanente
* (303)614-3148
* david.c.tabano@kp.org
*
* Program reads parameter file from Query Builder XML parameter file and performs queries
* against specified SPAN datamarts to generate patient counts by selected query criteria.
/* Edit Section Testing- Tests current Query Builder Program (latest created in Current Query Folder)*/

**** FOR LOCAL KPCO TESTING *******;
*%let current_query_folder=\\rmlhofile001\users\M426654\My SPAN Files ; 
*%include "&current_query_folder\EditSection.SAS";
options mlogic mprint symbolgen;

/*****************************************************************************************************/
/*****************************************************************************************************/
/************************************* Preliminary Macros ********************************************/
*-----------------------------------------------------;
*       Set Abstract Site Code                        ;
* (Used to create two letter site macro )             ;
*-----------------------------------------------------;
   
%MACRO MakeSiteCode();

%Global SiteCode ;
    %IF &_SiteCode. = 01 %THEN %DO;%* GHC ;%let SiteCode = GH; %END;
    	%ELSE %DO;
    %IF &_SiteCode. = 02 %THEN %DO; %* KPNW ;%let SiteCode = NW;%END;
    	%ELSE %DO;
    %IF &_SiteCode. = 03 %THEN %DO;%* KPNC ;%let SiteCode = NC;%END;
     	%ELSE %DO;
    %IF &_SiteCode. = 05 %THEN %DO;%* KPHI ;%let SiteCode = HI;%END;
    	%ELSE %DO;
    %IF &_SiteCode. = 06 %THEN %DO;%* KPCO ;%let SiteCode = CO;%END;
     	%ELSE %DO;
    %IF &_SiteCode. = 11 %THEN %DO;%* KPGA ;%let SiteCode = GA;%END;
     %ELSE %DO;
    %IF &_SiteCode. = 08 %THEN %DO;%* HRVD ;%let SiteCode = HV;%END;
     	%ELSE %DO;
    %IF &_SiteCode. = 07 %THEN %DO;%* HPRF ;%let SiteCode = HP;%END;
       	%ELSE %DO;
    %IF &_SiteCode. = 14 %THEN %DO;%* GHSX ;%let SiteCode = GS;%END;
       	%ELSE %DO;
    %IF &_SiteCode. = 51 %THEN %DO;%* DHHA ;%let SiteCode = DH;%END;
       	%ELSE %DO;
    %IF &_SiteCode. = 18 %THEN %DO;%* EIRH ;%let SiteCode = ES;%END;
       	%ELSE %DO;     
       %* UNEXPECTED ;%let SiteCode = XX;%END;
    %END;
    %END;
    %END;
    %END;
    %END;
    %END;
    %END;
    %END;
    %END;
    %END;
    %IF &SiteCode. = "XX" %THEN %DO;
        %PUT WARNING: -------------------------------------------------------------------------;
        %PUT WARNING: ---------;
        %PUT WARNING: ---------   UNEXPECTED Site Code detected.;
        %PUT WARNING: ---------   (Site Code is not from an expected data site);
        %PUT WARNING: ---------   Abstract Site Code Created for your site: &SiteCode.;
        %PUT WARNING: ---------;
        %PUT WARNING: -------------------------------------------------------------------------;
    %END;
    %ELSE %DO;
        %PUT NOTE:  * -------------------------------------------------------------------------;
        %PUT NOTE:  *;
        %PUT NOTE:  *             Abstract Site Code Created for your site: &SiteCode.;
        %PUT NOTE:  *;
        %PUT NOTE:  * -------------------------------------------------------------------------;
    %END;

%MEND MakeSiteCode;

%MakeSiteCode;

/**************** Some Sites run SAS on Unix. This macro accounts for paths in the ********************/
/**************** program that need to be corrected for Unix platforms. *******************************/

%macro slash;

%global slash;
%if %upcase(&SiteCode) = HI %then %do;%let slash=/;%end;
%else %do;
%let slash= \;%end;

%mend;

%slash;

/****** Maps to ParameterFile.XML and SAS ParameterMap.MAP from Datamart-generated query folder ******/
libname XML xml "&current_query_folder&slash.ParameterFile.xml"
xmlmap="&current_query_folder&slash.ParameterMap.map" access=readonly;


/************************************** Other Macros **************************************************/

%macro age(date,birth);
floor ((intck("month",&birth,&date) - (day(&date) < day(&birth))) / 12) 
%mend age;



/******************************************* Formats **************************************************/

proc format;

value agegrp1_
  		 .= '00: NULL         '
low -< 1  = '01: 0 Infants under 1'
 1 ='02: 1 yrs old'
2 ='03: 2 yrs old'
3 ='04: 3 yrs old'
4 ='05: 4 yrs old'
5 ='06: 5 yrs old'
6 ='07: 6 yrs old'
7 ='08: 7 yrs old'
8 ='09: 8 yrs old'
9 ='10: 9 yrs old'
10 ='11: 10 yrs old'
11 ='12: 11 yrs old'
12 ='13: 12 yrs old'
13 ='14: 13 yrs old'
14 ='15: 14 yrs old'
15 ='16: 15 yrs old'
16 ='17: 16 yrs old'
17 ='18: 17 yrs old'
18 ='19: 18 yrs old'
19 ='20: 19 yrs old'
20 ='21: 20 yrs old'
21 ='22: 21 yrs old'
22 ='23: 22 yrs old'
23 ='24: 23 yrs old'
24 ='25: 24 yrs old'
25 ='26: 25 yrs old'
26 ='27: 26 yrs old'
27 ='28: 27 yrs old'
28 ='29: 28 yrs old'
29 ='30: 29 yrs old'
30 ='31: 30 yrs old'
31 ='32: 31 yrs old'
32 ='33: 32 yrs old'
33 ='34: 33 yrs old'
34 ='35: 34 yrs old'
35 ='36: 35 yrs old'
36 ='37: 36 yrs old'
37 ='38: 37 yrs old'
38 ='39: 38 yrs old'
39 ='40: 39 yrs old'
40 ='41: 40 yrs old'
41 ='42: 41 yrs old'
42 ='43: 42 yrs old'
43 ='44: 43 yrs old'
44 ='45: 44 yrs old'
45 ='46: 45 yrs old'
46 ='47: 46 yrs old'
47 ='48: 47 yrs old'
48 ='49: 48 yrs old'
49 ='50: 49 yrs old'
50 ='51: 50 yrs old'
51 ='52: 51 yrs old'
52 ='53: 52 yrs old'
53 ='54: 53 yrs old'
54 ='55: 54 yrs old'
55 ='56: 55 yrs old'
56 ='57: 56 yrs old'
57 ='58: 57 yrs old'
58 ='59: 58 yrs old'
59 ='60: 59 yrs old'
60 ='61: 60 yrs old'
61 ='62: 61 yrs old'
62 ='63: 62 yrs old'
63 ='64: 63 yrs old'
64 ='65: 64 yrs old'
65 ='66: 65 yrs old'
66 ='67: 66 yrs old'
67 ='68: 67 yrs old'
68 ='69: 68 yrs old'
69 ='70: 69 yrs old'
70 ='71: 70 yrs old'
71 ='72: 71 yrs old'
72 ='73: 72 yrs old'
73 ='74: 73 yrs old'
74 ='75: 74 yrs old'
75 ='76: 75 yrs old'
76 ='77: 76 yrs old'
77 ='78: 77 yrs old'
78 ='79: 78 yrs old'
79 ='80: 79 yrs old'
80 ='81: 80 yrs old'
81 ='82: 81 yrs old'
82 ='83: 82 yrs old'
83 ='84: 83 yrs old'
84 ='85: 84 yrs old'
85 ='86: 85 yrs old'
86 ='87: 86 yrs old'
87 ='88: 87 yrs old'
88 - high = '89: 88+ yrs old'
;


 value agegrp5_
             .= '00: NULL         '
    low -< 1  = '01: 0 Infants under 1'
    1 -< 5    = '02: 1-4 yrs old'
    5 -< 10   = '03: 5-9 yrs old'
    10 -< 15  = '04: 10-14 yrs old'
    15 -< 20  = '05: 15-19 yrs old'
    20 -< 25  = '06: 20-24 yrs old'
    25 -< 30  = '07: 25-29 yrs old'
    30 -< 35  = '08: 30-34 yrs old'
    35 -< 40  = '09: 35-39 yrs old'
    40 -< 45  = '10: 40-44 yrs old'
    45 -< 50  = '11: 45-49 yrs old'
    50 -< 55  = '12: 50-54 yrs old'
    55 -< 60  = '13: 55-59 yrs old'
    60 -< 65  = '14: 60-64 yrs old'
    65 -< 70  = '15: 65-69 yrs old'
    70 -< 75  = '16: 70-74 yrs old'
    75 -< 80  = '17: 75-79 yrs old'
    80 -< 85  = '18: 80-84 yrs old'
    85 - high = '19: 85+ yrs old'
;
value agegrp10_
              .= '00: NULL         '
    low -< 1  = '01: 0 Infants under 1'
    1 -< 10    = '02: 1-9 yrs old'
    10 -< 20   = '03: 10-19 yrs old'
    20 -< 30  = '04: 20-29 yrs old'
    30 -< 40  = '05: 30-39 yrs old'
    40 -< 50  = '06: 40-49 yrs old'
    50 -< 60  = '07: 50-59 yrs old'
    60 -< 70  = '08: 60-69 yrs old'
    70 -< 80  = '09: 70-79 yrs old'
    80 - high  = '10: 80+ yrs old'
;

value agegrp20_
              .= '00: NULL         '
    low -< 1  = '01: 0 Infants under 1'
    1 -< 20    = '02: 1-19 yrs old'
    20 -< 40   = '03: 20-39 yrs old'
    40 -< 60  = '04: 40-59 yrs old'
    60 -< 80  = '05: 60-79 yrs old'
    80 - high  = '10: 80+ yrs old'
;

/* BMI */

value bmigrp_
              .= '00: NULL BMI    '
    low -< 20  = '01: BMI under 20'
    20 -< 25   = '02: BMI 20-25'
    25 -< 30   = '03: BMI 26-30'
    30 -< 35   = '03: BMI 31-35'
    35 -< 40   = '03: BMI 36-40'
    40 -< 45   = '03: BMI 41-45'
    45 -< 50   = '04: BMI 46-50'
    50 - high  = '05: BMI 50+'
  ;

 /* Hide zero counts */

value count
	. = 'HIDDEN'
	low -< 6 = 'HIDDEN';
	;

   quit;

/******************************************************************************************************/
/*************************** Copy parameters from SPAN port XML file **********************************/
/******************************************************************************************************/

PROC DATASETS;
COPY IN=XML OUT=WORK;
RUN;
quit;

libname xml clear;
/******************************************************************************************************/
/************************ Query Description and Observation Period ************************************/
/******************************************************************************************************/

data _null_;
  set drn_query_builder;
    call symput ('query_type',query_type);
  call symput ('query_name',trim(compress(query_name)));
  call symput ('query_desc',query_desc); /* expand XML for more characters */
  call symput ('email',trim(compress(submitter_email)));
  call symput ('begdate',PUT(period_start, DATE9.));
  call symput ('enddate',PUT(period_end, DATE9.));
    call symput ('enroll_cont',enroll_cont);
  call symput ('enroll_prior',trim(compress(enroll_prior)));
  call symput ('enroll_post',trim(compress(enroll_post)));
run;

/******************************************************************************************************/
/**************************************** Index Code **************************************************/
/******************************************************************************************************/
data _null_;
  set index_variable;
    call symput ('index_code',index_code);
run;

/******************************************************************************************************/
/******************************* Module and Datamart distinction **************************************/
/******************************************************************************************************/
%macro module;

%global adhd_3 ob_3

__demog    
__census         
__diagnoses             
__procedures             
__pharmacy            
__everndc        
__encounters    
__vitals     
__lab            
__labnotes      
__enrollment         
__cont_enrollment    
__death          
__causeofdeath   
__phq9           
__vanderbilt     
__social_history 
;

%let email2=%sysfunc(translate(&email,_,.));

%if &query_type=ADHD Module 1 %then %do;

  %let __demog         			=&_span_a_demog    		 ;
  %let __census              	=&_span_a_census         ;
  %let __diagnoses              =&_span_a_diagnoses      ;
  %let __procedures             =&_span_a_procedures     ;
  %let __pharmacy               =&_span_a_pharmacy       ;
  %let __everndc             	=&_span_a_everndc        ;
  %let __encounters         	=&_span_a_encounters     ;
  %let __vitals          		=&_span_a_vitals     	 ;
  %let __lab                 	=&_span_a_lab            ;
  %let __labnotes           	=&_span_a_labnotes       ;
  %let __enrollment             =&_span_a_enrollment     ;
  %let __cont_enrollment        =&_span_a_cont_enrollment;
  %let __death               	=&_span_a_death          ;
  %let __causeofdeath        	=&_span_a_causeofdeath   ;
  %let __phq9                	=&_span_a_phq9           ;
  %let __vanderbilt          	=&_span_a_vanderbilt     ;
  %let __social_history      	=&_span_a_social_history ;


%end;

%else %if &query_type=Obesity Module 1 %then %do;

  %let __demog         			=&_span_o_demog    		 ;
  %let __census              	=&_span_o_census         ;
  %let __diagnoses              =&_span_o_diagnoses      ;
  %let __procedures             =&_span_o_procedures     ;
  %let __pharmacy               =&_span_o_pharmacy       ;
  %let __everndc             	=&_span_o_everndc        ;
  %let __encounters         	=&_span_o_encounters     ;
  %let __vitals          		=&_span_o_vitals     	 ;
  %let __lab                 	=&_span_o_lab            ;
  %let __labnotes           	=&_span_o_labnotes       ;
  %let __enrollment             =&_span_o_enrollment     ;
  %let __cont_enrollment        =&_span_o_cont_enrollment;
  %let __death               	=&_span_o_death          ;
  %let __causeofdeath        	=&_span_o_causeofdeath   ;
  %let __phq9                	=&_span_o_phq9           ;
  %let __vanderbilt          	=&_span_o_vanderbilt     ;
  %let __social_history      	=&_span_o_social_history ;

%end;

%else %if &query_type=ADHD Module 3 %then %do;

/*
%let adhd_3 = &module3&slash.adhd&slash.%qscan(&email2,1,%str(@));

libname __adhd_3 "&adhd_3";

  %let __demog		         =__adhd_3.demog         ;
  %let __census              =__adhd_3.census        ;
  %let __diagnoses           =__adhd_3.diagnoses     ;
  %let __procedures          =__adhd_3.procedures    ;
  %let __pharmacy            =__adhd_3.pharmacy      ;
  %let __everndc             =__adhd_3.everndc       ;
  %let __encounters          =__adhd_3.encounters    ;
  %let __vitals          	 =__adhd_3.vitals    	 ;
  %let __lab                 =__adhd_3.lab           ;
  %let __labnotes            =__adhd_3.labnotes      ;
  %let __enrollment          =__adhd_3.enrollment    ;
  %let __cont_enrollment     =__adhd_3.cont_enrollment;
  %let __death               =__adhd_3.death         ;
  %let __causeofdeath        =__adhd_3.causeofdeath  ;
  %let __phq9                =__adhd_3.phq9          ;
  %let __vanderbilt          =__adhd_3.vanderbilt    ;
  %let __social_history      =__adhd_3.social_history;

%end;
*/

  %let __demog         			=&_span_a_demog    		 ;
  %let __census              	=&_span_a_census         ;
  %let __diagnoses              =&_span_a_diagnoses      ;
  %let __procedures             =&_span_a_procedures     ;
  %let __pharmacy               =&_span_a_pharmacy       ;
  %let __everndc             	=&_span_a_everndc        ;
  %let __encounters         	=&_span_a_encounters     ;
  %let __vitals          		=&_span_a_vitals     	 ;
  %let __lab                 	=&_span_a_lab            ;
  %let __labnotes           	=&_span_a_labnotes       ;
  %let __enrollment             =&_span_a_enrollment     ;
  %let __cont_enrollment        =&_span_a_cont_enrollment;
  %let __death               	=&_span_a_death          ;
  %let __causeofdeath        	=&_span_a_causeofdeath   ;
  %let __phq9                	=&_span_a_phq9           ;
  %let __vanderbilt          	=&_span_a_vanderbilt     ;
  %let __social_history      	=&_span_a_social_history ;


%end;

%else %if &query_type=Obesity Module 3 %then %do;
/*
%let ob_3 = &module3&slash.obesity&slash.%qscan(&email2,1,%str(@));

libname __ob_3 "&ob_3";

  %let __demog		         =__ob_3.demog         ;
  %let __census              =__ob_3.census        ;
  %let __diagnoses           =__ob_3.diagnoses     ;
  %let __procedures          =__ob_3.procedures    ;
  %let __pharmacy            =__ob_3.pharmacy      ;
  %let __everndc             =__ob_3.everndc       ;
  %let __encounters          =__ob_3.encounters    ;
  %let __vitals          	 =__ob_3.vitals    	   ;
  %let __lab                 =__ob_3.lab           ;
  %let __labnotes            =__ob_3.labnotes      ;
  %let __enrollment          =__ob_3.enrollment    ;
  %let __cont_enrollment     =__ob_3.cont_enrollment;
  %let __death               =__ob_3.death         ;
  %let __causeofdeath        =__ob_3.causeofdeath  ;
  %let __phq9                =__ob_3.phq9          ;
  %let __vanderbilt          =__ob_3.vanderbilt    ;
  %let __social_history      =__ob_3.social_history;

%end;

*/
  %let __demog         			=&_span_o_demog    		 ;
  %let __census              	=&_span_o_census         ;
  %let __diagnoses              =&_span_o_diagnoses      ;
  %let __procedures             =&_span_o_procedures     ;
  %let __pharmacy               =&_span_o_pharmacy       ;
  %let __everndc             	=&_span_o_everndc        ;
  %let __encounters         	=&_span_o_encounters     ;
  %let __vitals          		=&_span_o_vitals     	 ;
  %let __lab                 	=&_span_o_lab            ;
  %let __labnotes           	=&_span_o_labnotes       ;
  %let __enrollment             =&_span_o_enrollment     ;
  %let __cont_enrollment        =&_span_o_cont_enrollment;
  %let __death               	=&_span_o_death          ;
  %let __causeofdeath        	=&_span_o_causeofdeath   ;
  %let __phq9                	=&_span_o_phq9           ;
  %let __vanderbilt          	=&_span_o_vanderbilt     ;
  %let __social_history      	=&_span_o_social_history ;

%end;
%mend;

%module;

/******************************************************************************************************/
/************************************* Module 1 Processing ********************************************/
/******************************************************************************************************/
/* First determine Site which determines which Hash join is performed.
   		- Denver Health & Hospital Authority, Essentia Health have no enrollment, so the hash 
   		  join omits enrollment.
   		- KPNC uses ICD-9 BMI group codes for BMI ???
/* Next, pull in parameters (Index Variable, Index Codes, Enrollment, Observation Period, etc.)

   						Index Variable and Corresponding Datamart tables
   								Variable		Table
   								-BMI			-Vitals
   								-Dx				-Diagnoses
   								-Px				-Procedures
   								-Rx				-Pharmacy, EverNDC
	   							-Age			-Demographics

   		- If Index Variable is a Drug (Rx), a seperate join from other index variables 
   		  must be performed since the code lookup is based on LIKE and wildcard statements
   		  off generic name and NDC match from EverNDC and Pharmacy tables.
/* Join Index Variable-generated table to Demographics, Enrollment to build Step1 table
/******************************************************************************************************/

%macro step1;

%global index index_dx index_px index_rx index_bmi_var index_bmi_grp index_age_op index_age age_index_date
		index_num;

%if &query_type=ADHD Module 1 %then %do;
%let type=ADHD; %end;
%if &query_type=ADHD Module 3 %then %do;
%let type=ADHD; %end;
%if &query_type=Obesity Module 1 %then %do;
%let type=Obesity; %end;
%if &query_type=Obesity Module 3 %then %do;
%let type=Obesity; %end;

/* Waterfall Table- Identify full DataMart Cohort at Site */
proc sql;
create table &sitecode._count as
select "&Sitecode. &type. DataMart Population"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from &__demog;
quit;

/* Community Sites have no enrollment, so next section performs Index Variable processing without Enrollment */
%if %upcase(&sitecode) = DH %then %do;

%if &index_code = rx %then %do;

data _null_;
set index_rx_var;
call symputx ('index_rx_bool', bool_operator, 'G');
run;

proc sql;
create table codelist0 as
select distinct upcase(compress(trim(index_rx_codes))) as index_rx_codes
from index_rx_code;

%let index_num=&sqlobs;

quit;


data codelist;
set codelist0;
likecode=put(cats("generic like '%",index_rx_codes,"%'"),$100.);
run;

proc sql noprint;
select likecode into : clause separated by ' &index_rx_bool ' from codelist;
quit;

data step1a;
keep studyid generic index_date ndc;
declare hash a();
rc = a.DefineKey ('ndc');
rc = a.DefineData ('ndc','studyid','index_date');
rc = a.DefineDone ();
do until (eof1);
	set &__pharmacy end= eof1;
	index_date = rxdate; format index_date mmddyy10.;
	rc = a.add();
	end;
do until (eof2);
set &__everndc end = eof2;
	where &clause;
call missing (studyid,index_date);
rc = a.find();
if studyid ne ' '
and index_date ne . 
    then output ;
	end;
	stop;
	run;


data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
    set step1a end = eof3;
    index_code = generic;
    call missing(gender, race1, birth_date);
    rc = a.find ();
    if ("&begdate"d <= index_date <= "&enddate"d) 
    then output ;
	end;
	stop;
	run;
%end;


%else %if &index_code = px %then %do;
	proc sql;
    select distinct cats("%str(%')",index_px_codes,"%str(%')") into:index_px
    separated by ' '
    from index_px_code;

%let index_num=&sqlobs;

    quit;

data _null_;
set index_px_var;
call symputx ('index_px_bool', bool_operator, 'G');
run;
%let index_px_bool="&index_px_bool";
proc sql noprint;
select distinct index_px_codes into :px1-:px1000000
from index_px_code;
quit;
%let runs=&sqlobs;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
  set &__procedures end = eof3;
    index_code = &index_code;
    index_date = procdate; format index_date mmddyy10.;
    call missing(gender, race1, birth_date);
    rc = a.find ();
    if &index_code in (&index_px)
    and ("&begdate"d <= procdate <= "&enddate"d)
    then output ;
	end;
	stop;
	run;

%if &index_px_bool="And" %then %do;
proc sql noprint;
create table testa as
select studyid, count(distinct px) as count
from step1
group by studyid;

create table step1b as
select * from step1
where studyid in (select studyid from testa where count=&runs)
order by studyid;

quit;

proc datasets;
   delete step1 testa;
   change step1b = step1;
run;
quit;
%end;
%end;

%else %if &index_code = dx %then %do;
	proc sql noprint;
    select distinct cats("%str(%')",index_dx_codes,"%str(%')") into:index_dx
    separated by ' '
    from index_dx_code;
	
%let index_num=&sqlobs;

    quit;

data _null_;
set index_dx_var;
call symputx ('index_dx_bool', bool_operator, 'G');
run;
%let index_dx_bool="&index_dx_bool";
proc sql noprint;
select distinct index_dx_codes into :dx1-:dx1000000
from index_dx_code;
quit;
%let runs=&sqlobs;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
  set &__diagnoses end = eof3;
    index_code = &index_code;
    index_date = adate; format index_date mmddyy10.;
    call missing(gender, race1, birth_date);
    rc = a.find ();
    if &index_code in (&index_dx)
    and ("&begdate"d <= adate <= "&enddate"d)
    then output ;
	end;
	stop;
	run;

%if &index_dx_bool="And" %then %do;
proc sql noprint;
create table testa as
select studyid, count(distinct dx) as count
from step1
group by studyid;

create table step1b as
select * from step1
where studyid in (select studyid from testa where count=&runs)
order by studyid;

quit;

proc datasets;
   delete step1 testa;
   change step1b = step1;
run;
quit;
%end;
%end;

%else %if &index_code=bmi %then %do;


%let index_num=9;

    data _null_;
    set index_bmi_var;
      call symput ('index_bmi_var',compress(trim(bmi_var)));
    call symput ('index_bmi_grp',compress(trim(group)));
    run;
    %if &index_bmi_grp = 2 %then %do;
    data _null_;
    set index_bmi_var;
    call symput ('bmi_low',substr(bmi_var,1,index(bmi_var,'-')-1));
    call symput ('bmi_high',substr(bmi_var,index(bmi_var,'-')+1));
    run;
    %end;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
    set &__vitals end = eof3;
    index_code = &index_code;
    index_date = measure_date; format index_date mmddyy10.; 
    call missing(gender, race1, birth_date);
    rc = a.find ();
    call missing (enr_start, enr_end);
    rc = b.find ();
      %if &index_bmi_grp = 1 %then %do;
    if &index_code >= &index_bmi_var
    and ("&begdate"d <= measure_date <= "&enddate"d)
    then output ;
	end;
	stop;
	run;
    %end;
    %else %if &index_bmi_grp = 2 %then %do;
    if &bmi_low
    <= &index_code <=
    &bmi_high
    and ("&begdate"d <= measure_date <= "&enddate"d)
    then output ;
	end;
	stop;
	run;
    %end;
	%end;
	

%else %if &index_code=age %then %do;

%let index_num=88;

    data _null_;
    set index_age_var;
      call symput ('index_age_op',compress(trim(age_operator)));
    call symput ('index_age',compress(trim(age)));
    call symput ('age_index_date',PUT(as_of_index_event, date9.));
    run;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
    set &__demog end = eof2;
	index_code = %age("&age_index_date"d,birth_date);
	index_date = "&age_index_date"d; 
	format index_date mmddyy10.;
	if index_code &index_age_op &index_age
    then output ;
	run;

  %end;

  	data step1_enroll;
	set step1;
	run;

  %end;
/* Community Sites have no enrollment, so next section performs Index Variable processing without Enrollment */
%else %if %upcase(&sitecode) = ES %then %do;

%if &index_code = rx %then %do;

data _null_;
set index_rx_var;
call symputx ('index_rx_bool', bool_operator, 'G');
run;

proc sql;
create table codelist0 as
select distinct upcase(index_rx_codes) as index_rx_codes
from index_rx_code;

%let index_num=&sqlobs;

quit;

data codelist;
set codelist0;
likecode=put(cats("generic like '%",index_rx_codes,"%'"),$100.);
run;

proc sql noprint;
select likecode into : clause separated by ' &index_rx_bool ' from codelist;
quit;

data step1a;
keep studyid generic index_date ndc;
declare hash a();
rc = a.DefineKey ('ndc');
rc = a.DefineData ('ndc','studyid','index_date');
rc = a.DefineDone ();
do until (eof1);
	set &__pharmacy end= eof1;
	index_date = rxdate; format index_date mmddyy10.;
	rc = a.add();
	end;
do until (eof2);
set &__everndc end = eof2;
	where &clause;
call missing (studyid,index_date);
rc = a.find();
if studyid ne ' '
and index_date ne . 
    then output ;
	end;
	stop;
	run;


data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
    set step1a end = eof3;
    index_code = generic;
    call missing(gender, race1, birth_date);
    rc = a.find ();
    if ("&begdate"d <= index_date <= "&enddate"d) 
    then output ;
	end;
	stop;
	run;
%end;


%else %if &index_code = px %then %do;
	proc sql;
    select distinct cats("%str(%')",index_px_codes,"%str(%')") into:index_px
    separated by ' '
    from index_px_code;

%let index_num=&sqlobs;

    quit;

data _null_;
set index_px_var;
call symputx ('index_px_bool', bool_operator, 'G');
run;
%let index_px_bool="&index_px_bool";
proc sql noprint;
select distinct index_px_codes into :px1-:px1000000
from index_px_code;
quit;
%let runs=&sqlobs;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
  set &__procedures end = eof3;
    index_code = &index_code;
    index_date = procdate; format index_date mmddyy10.;
    call missing(gender, race1, birth_date);
    rc = a.find ();
    if &index_code in (&index_px)
    and ("&begdate"d <= procdate <= "&enddate"d)
    then output ;
	end;
	stop;
	run;

%if &index_px_bool="And" %then %do;
proc sql noprint;
create table testa as
select studyid, count(distinct px) as count
from step1
group by studyid;

create table step1b as
select * from step1
where studyid in (select studyid from testa where count=&runs)
order by studyid;

quit;

proc datasets;
   delete step1 testa;
   change step1b = step1;
run;
quit;
%end;
%end;

%else %if &index_code = dx %then %do;
	proc sql noprint;
    select distinct cats("%str(%')",index_dx_codes,"%str(%')") into:index_dx
    separated by ' '
    from index_dx_code;

%let index_num=&sqlobs;

    quit;

data _null_;
set index_dx_var;
call symputx ('index_dx_bool', bool_operator, 'G');
run;
%let index_dx_bool="&index_dx_bool";
proc sql noprint;
select distinct index_dx_codes into :dx1-:dx1000000
from index_dx_code;
quit;
%let runs=&sqlobs;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
  set &__diagnoses end = eof3;
    index_code = &index_code;
    index_date = adate; format index_date mmddyy10.;
    call missing(gender, race1, birth_date);
    rc = a.find ();
    if &index_code in (&index_dx)
    and ("&begdate"d <= adate <= "&enddate"d)
    then output ;
	end;
	stop;
	run;

%if &index_dx_bool="And" %then %do;
proc sql noprint;
create table testa as
select studyid, count(distinct dx) as count
from step1
group by studyid;

create table step1b as
select * from step1
where studyid in (select studyid from testa where count=&runs)
order by studyid;

quit;

proc datasets;
   delete step1 testa;
   change step1b = step1;
run;
quit;
%end;
%end;

%else %if &index_code=bmi %then %do;

%let index_num=9;

    data _null_;
    set index_bmi_var;
      call symput ('index_bmi_var',compress(trim(bmi_var)));
    call symput ('index_bmi_grp',compress(trim(group)));
    run;
    %if &index_bmi_grp = 2 %then %do;
    data _null_;
    set index_bmi_var;
    call symput ('bmi_low',substr(bmi_var,1,index(bmi_var,'-')-1));
    call symput ('bmi_high',substr(bmi_var,index(bmi_var,'-')+1));
    run;
    %end;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
    set &__vitals end = eof3;
    index_code = &index_code;
    index_date = measure_date; format index_date mmddyy10.; 
    call missing(gender, race1, birth_date);
    rc = a.find ();
    call missing (enr_start, enr_end);
    rc = b.find ();
      %if &index_bmi_grp = 1 %then %do;
    if &index_code >= &index_bmi_var
    and ("&begdate"d <= measure_date <= "&enddate"d)
    then output ;
	end;
	stop;
	run;
    %end;
    %else %if &index_bmi_grp = 2 %then %do;
    if &bmi_low
    <= &index_code <=
    &bmi_high
    and ("&begdate"d <= measure_date <= "&enddate"d)
    then output ;
	end;
	stop;
	run;
    %end;
	%end;
	

%else %if &index_code=age %then %do;

%let index_num=89;

    data _null_;
    set index_age_var;
      call symput ('index_age_op',compress(trim(age_operator)));
    call symput ('index_age',compress(trim(age)));
    call symput ('age_index_date',PUT(as_of_index_event, date9.));
    run;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
    set &__demog end = eof2;
	index_code = %age("&age_index_date"d,birth_date);
	index_date = "&age_index_date"d; format index_date mmddyy10.;
	if index_code &index_age_op &index_age
    then output ;
	run;

  %end;

data step1_enroll;
set step1;
run;

  %end;


%else %do;
/**************** HMO Sites with Enrollment *****************************/
%if %upcase(&enroll_cont) = Y %then %let enrollment = &__cont_enrollment;
%else %let enrollment = &__enrollment;


%if &index_code = rx %then %do;

data _null_;
set index_rx_var;
call symputx ('index_rx_bool', bool_operator, 'G');
run;

proc sql;
create table codelist0 as
select distinct upcase(index_rx_codes) as index_rx_codes
from index_rx_code;

%let index_num=&sqlobs;

quit;

data codelist;
set codelist0;
likecode=put(cats("generic like '%",index_rx_codes,"%'"),$100.);
run;

proc sql noprint;
select likecode into : clause separated by ' &index_rx_bool ' from codelist;
quit;

data step1a;
keep studyid generic index_date ndc;
declare hash a();
rc = a.DefineKey ('ndc');
rc = a.DefineData ('ndc','studyid','index_date');
rc = a.DefineDone ();
do until (eof1);
	set &__pharmacy end= eof1;
	index_date = rxdate; format index_date mmddyy10.;
	rc = a.add();
	end;
do until (eof2);
set &__everndc end = eof2;
	where &clause;
call missing (studyid,index_date);
rc = a.find();
if studyid ne ' '
and index_date ne . 
then output;
end;
run;


data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
    set step1a end = eof3;
    index_code = generic;
    call missing(gender, race1, birth_date);
    rc = a.find ();
	if ("&begdate"d <= index_date <= "&enddate"d) 
    then output ;
	end;
	stop;
	run;

%if %sysfunc(exist(step1)) ne 0 %then %do;
	%let dsn=%upcase(step1);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;

proc sql;

create table step1_enroll as
select a.*, b.enr_start, b.enr_end
from step1 a
left outer join &enrollment b
on b.studyid = a.studyid
where (b.enr_start-(&enroll_prior*30) <= a.index_date <= (b.enr_end+(&enroll_post*30)));

quit;

		%end;
		%else %if &nobs = 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end; %end;
%else %if %sysfunc(exist(step1))= 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end;

%end;

%else %if &index_code = px %then %do;
	proc sql noprint;
    select distinct cats("%str(%')",index_px_codes,"%str(%')") into:index_px
    separated by ' '
    from index_px_code;

%let index_num=&sqlobs;

    quit;

data _null_;
set index_px_var;
call symputx ('index_px_bool', bool_operator, 'G');
run;
%let index_px_bool="&index_px_bool";

proc sql noprint;
select distinct index_px_codes into :px1-:px1000000
from index_px_code;
quit;
%let runs=&sqlobs;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
  set &__procedures end = eof3;
    index_code = &index_code;
    index_date = adate; format index_date mmddyy10.;
    call missing(gender, race1, birth_date);
    rc = a.find ();
    if &index_code in (&index_px)
    and ("&begdate"d <= adate <= "&enddate"d)
    then output ;
	end;
	stop;
	run;

%if &index_px_bool="And" %then %do;
proc sql noprint;
create table testa as
select studyid, count(distinct index_code) as count
from step1
group by studyid;

create table step1b as
select * from step1
where studyid in (select studyid from testa where count=&runs)
order by studyid;

quit;

proc datasets;
   delete step1 testa;
   change step1b = step1;
run;
quit;
%end;

%if %sysfunc(exist(step1)) ne 0 %then %do;
	%let dsn=%upcase(step1);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;

proc sql;

create table step1_enroll as
select a.*, b.enr_start, b.enr_end
from step1 a
left outer join &enrollment b
on b.studyid = a.studyid
where (b.enr_start-(&enroll_prior*30) <= a.index_date <= (b.enr_end+(&enroll_post*30)));

quit;

		%end;
		%else %if &nobs = 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end; %end;
%else %if %sysfunc(exist(step1))= 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end;

%end;

%else %if &index_code = dx %then %do;
	proc sql noprint;
    select distinct cats("%str(%')",index_dx_codes,"%str(%')") into:index_dx
    separated by ' '
    from index_dx_code;

%let index_num=&sqlobs;

    quit;

data _null_;
set index_dx_var;
call symputx ('index_dx_bool', bool_operator, 'G');
run;
%let index_dx_bool="&index_dx_bool";
proc sql noprint;
select distinct index_dx_codes into :dx1-:dx1000000
from index_dx_code;
quit;
%let runs=&sqlobs;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
  set &__diagnoses end = eof3;
    index_code = &index_code;
    index_date = adate; format index_date mmddyy10.;
    call missing(gender, race1, birth_date);
    rc = a.find ();
    if &index_code in (&index_dx)
    and ("&begdate"d <= adate <= "&enddate"d)
    then output ;
	end;
	stop;
	run;

%if &index_dx_bool="And" %then %do;
proc sql noprint;
create table testa as
select studyid, count(distinct index_code) as count
from step1
group by studyid;

create table step1b as
select * from step1
where studyid in (select studyid from testa where count=&runs)
order by studyid;

quit;

proc datasets;
   delete step1 testa;
   change step1b = step1;
run;
quit;
%end;


%if %sysfunc(exist(step1)) ne 0 %then %do;
	%let dsn=%upcase(step1);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;

proc sql;

create table step1_enroll as
select a.*, b.enr_start, b.enr_end
from step1 a
left outer join &enrollment b
on b.studyid = a.studyid
where (b.enr_start-(&enroll_prior*30) <= a.index_date <= (b.enr_end+(&enroll_post*30)));

quit;

		%end;
		%else %if &nobs = 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end; %end;
%else %if %sysfunc(exist(step1))= 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end;

%end;

%else %if &index_code=bmi %then %do;

%let index_num=9;

    data _null_;
    set index_bmi_var;
      call symput ('index_bmi_var',compress(trim(bmi_var)));
    call symput ('index_bmi_grp',compress(trim(group)));
    run;
    %if &index_bmi_grp = 2 %then %do;
    data _null_;
    set index_bmi_var;
    call symput ('bmi_low',substr(bmi_var,1,index(bmi_var,'-')-1));
    call symput ('bmi_high',substr(bmi_var,index(bmi_var,'-')+1));
    run;
    %end;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
  declare Hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid', 'gender', 'race1','hispanic', 'birth_date');
  rc = a.DefineDone ();
  do until (eof1);
    set &__demog end = eof1;
    rc = a.add ();
  end;
  do until (eof3) ;
    set &__vitals end = eof3;
    index_code = &index_code;
    index_date = measure_date; format index_date mmddyy10.; 
    call missing(gender, race1, birth_date);
    rc = a.find ();
      %if &index_bmi_grp = 1 %then %do;
    if &index_code >= &index_bmi_var
    and ("&begdate"d <= measure_date <= "&enddate"d)
    then output ;
	end;
	stop;
	run;
    %end;
    %else %if &index_bmi_grp = 2 %then %do;
    if &bmi_low
    <= &index_code <=
    &bmi_high
    and ("&begdate"d <= measure_date <= "&enddate"d)
    then output ;
	end;
	stop;
	run;
%end;

%if %sysfunc(exist(step1)) ne 0 %then %do;
	%let dsn=%upcase(step1);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;

proc sql;

create table step1_enroll as
select a.*, b.enr_start, b.enr_end
from step1 a
left outer join &enrollment b
on b.studyid = a.studyid
where (b.enr_start-(&enroll_prior*30) <= a.index_date <= (b.enr_end+(&enroll_post*30)));

quit;

		%end;
		%else %if &nobs = 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end; %end;
		%else %if %sysfunc(exist(step1))= 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end;

%end;

%else %if &index_code=age %then %do;

%let index_num=89;

    data _null_;
    set index_age_var;
      call symput ('index_age_op',compress(trim(age_operator)));
    call symput ('index_age',compress(trim(age)));
    call symput ('age_index_date',PUT(as_of_index_event, date9.));
    run;

data step1;
keep studyid index_date index_code birth_date gender race1 hispanic; 
    set &__demog;
	index_code = %age("&age_index_date"d,birth_date);
	index_date = "&age_index_date"d; format index_date mmddyy10.;
    if index_code &index_age_op &index_age
    then output ;
	run;
%end;

%if %sysfunc(exist(step1)) ne 0 %then %do;
	%let dsn=%upcase(step1);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;

proc sql;

create table step1_enroll as
select a.*, b.enr_start, b.enr_end
from step1 a
left outer join &enrollment b
on b.studyid = a.studyid
where (b.enr_start-(&enroll_prior*30) <= a.index_date <= (b.enr_end+(&enroll_post*30)));

quit;

		%end;
		%else %if &nobs = 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end; 

		data step1_enroll;
		set step1;
		run;
		%end;

%else %if %sysfunc(exist(step1))= 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end;

%end;

%if %sysfunc(exist(step1)) ne 0 %then %do;
	%let dsn=%upcase(step1);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;

%let index = 1;

proc sql;
create table step1a as
select a.studyid, a.index_date, a.index_code, a.birth_date, a.gender, a.race1, a.hispanic, b.bmi, 
	   b.measure_date, abs(a.index_date - b.measure_date) as min
from step1_enroll a
left outer join &__vitals b
on b.studyid=a.studyid
where ("&begdate"d <= b.measure_date <= "&enddate"d)
order by a.studyid, a.index_date, min;

data step2 (keep=studyid index_date index_code birth_date gender race1 hispanic bmigrp measure_date);
set step1a;
bmigrp = put(bmi, bmigrp_.);
by studyid index_date;
if first.studyid;
run;

proc sql;
create table &sitecode._Index_count as
select "&Sitecode. Cohort Index Var &index_code."  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step1;
quit;

proc sql;
create table &sitecode._Enroll_count as
select "&Sitecode. Cohort with Enrollment"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step2;
quit;

		%end;
		%else %if &nobs = 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end; %end;
%else %if %sysfunc(exist(step1))= 0 %then %do;
		%let index = 0;
		%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.; %end;

%mend;

%step1;

/******************************************************************************************************/
/************************************* Inclusion Criteria *********************************************/
/******************************************************************************************************/


%macro incl_dx;

%global incl_dx incl_dx_num;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%let dsn=%upcase(incl_dx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No dx inclusion criteria.;
%return;
%end;

proc sql noprint;
select distinct cats("%str(%')",incl_dx_codes,"%str(%')") into:incl_dx
separated by ' '
from incl_dx_code;
quit;
proc sql noprint;
select distinct incl_dx_codes into :incldx1-:incldx1000000
from incl_dx_code;

%let incl_dx_num=&sqlobs;


quit;

data _null_;
set incl_dx_var;
call symputx ('incl_dx_bool', bool_operator, 'G');
run;
%let incl_dx_bool="&incl_dx_bool";

data step3_incldxa;
keep studyid birth_date index_date index_code gender race1 hispanic dx dxdate;
  declare hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid','dx', 'dxdate');
  rc = a.DefineDone ();
  do until (eof1);
    set &__diagnoses end = eof1;
	dxdate = adate; format dxdate mmddyy10.;
    where "&begdate"d <= adate <= "&enddate"d
	and dx in (&incl_dx);
    rc = a.add ();
  end;
  do until (eof4) ;
    set Step2 end = eof4;
    call missing (dx,dxdate);
    rc = a.find ();
    if index_date ne .
	and adate ne .
    then output ;
  end;
stop; run;


%if &incl_dx_bool="And" %then %do;

proc sql;
create table testa as
select studyid, count(distinct dx) as count
from step3_incldxa
group by studyid;

create table step3_incldx2 as
select *
from step3_incldxa
where studyid in (select studyid from testa where count=&incl_dx_num)
order by studyid;

quit;

proc datasets;
   delete step3_incldx testa;
   change step3_incldx2 = step3_incldxa;
run;
quit;
%end;

/* Retain only studyids that match */
proc sql;

create table step3_incldx as
select * from step3_incldxa
where dxdate ne .;

quit;

proc sql;
create table &sitecode._incldx_count as
select "&Sitecode. Incl Dx codes"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3_incldx;
quit;

%mend incl_dx;

%macro incl_px;

%global incl_px incl_px_num;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%let dsn=%upcase(incl_px_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No px inclusion criteria.;
%return;
%end;

proc sql noprint;
select distinct cats("%str(%')",incl_px_codes,"%str(%')") into:incl_px
separated by ' '
from incl_px_code;
quit;
proc sql noprint;
select distinct incl_px_codes into :inclpx1-:inclpx1000000
from incl_px_code;

%let incl_px_num=&sqlobs;


quit;

data _null_;
set incl_px_var;
call symputx ('incl_px_bool', bool_operator, 'G');
run;
%let incl_px_bool="&incl_px_bool";


data step3_inclpx;
keep studyid birth_date index_date index_code gender race1 hispanic px pxdate;
  declare hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid','index_date', 'index_code','birth_date', 'gender','race1','hispanic');
  rc = a.DefineDone ();
  do until (eof1);
    set Step2 end = eof1;
    rc = a.add ();
  end;
  do until (eof4) ;
    set &__procedures end = eof4;
	pxdate = adate; format pxdate mmddyy10.;
    where "&begdate"d <= adate <= "&enddate"d
    and px in (&incl_px);
    call missing (index_date,index_code,birth_date,gender,race1,hispanic);
    rc = a.find ();
    if index_date ne .
    then output ;
  end;
stop; run;

%if &incl_px_bool="And" %then %do;

proc sql;
create table testa as
select studyid, count(distinct px) as count
from step3_inclpx
group by studyid;

create table step3_inclpx2 as
select studyid, birth_date, index_date, index_code, gender, race1, hispanic, px, pxdate
from step3_inclpx
where studyid in (select studyid from testa where count=&incl_px_num)
order by studyid;

quit;

proc datasets;
   delete step3_inclpx testa;
   change step3_inclpx2 = step3_inclpx;
run;
quit;
%end;

proc sql;
create table &sitecode._inclpx_count as
select "&Sitecode. Incl Px codes"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3_inclpx;
quit;

%mend incl_px;

%macro incl_rx;

%global incl_rx incl_rx_num;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%let dsn=%upcase(incl_rx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No rx inclusion criteria.;
%return;
%end;


proc sql;
create table codelist_incl as
select distinct upcase(incl_rx_codes) as incl_rx_codes
from incl_rx_code;

%let incl_rx_num=&sqlobs;

quit;

proc sql noprint;
select distinct cats("%str(%')",incl_rx_codes,"%str(%')") into:incl_rx
separated by ' '
from codelist_incl;
quit;

data _null_;
set incl_rx_var;
call symputx ('incl_rx_bool', bool_operator, 'G');
run;

data codelist;
set incl_rx_code;
likecode=put(cats("generic like '%",incl_rx_codes,"%'"),$100.);
run;

proc sql noprint;
select likecode into : clause separated by ' &incl_rx_bool ' from codelist;
quit;

data step3a_inclrx;
keep studyid generic rxdate ndc;
declare hash a();
rc = a.DefineKey ('ndc');
rc = a.DefineData ('ndc','studyid','index_date');
rc = a.DefineDone ();
do until (eof1);
	set &__pharmacy end= eof1;
	rc = a.add();
	end;
do until (eof2);
set &__everndc end = eof2;
	where &clause;
call missing (studyid,index_date);
rc = a.find();
if studyid ne ' '
and index_date ne . 
then output;
end;
run;

data step3_inclrx;
keep studyid birth_date index_date index_code gender race1 hispanic ndc generic rxdate;
  declare hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid','index_code','index_date','birth_date', 'gender','race1','hispanic');
  rc = a.DefineDone ();
  do until (eof1);
    set step2 end = eof1;
    rc = a.add ();
  end;
do until (eof3) ;
    set step3a_inclrx end = eof3;
    call missing (index_date,index_code,birth_date,gender,race1,hispanic);
    rc = a.find ();
    if index_date ne .
    and generic ne ' '
    then output ;
  end;
stop; run;

proc sql;
create table &sitecode._inclrx_count as
select "&Sitecode. Incl Rx codes"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3_inclrx;
quit;

%mend incl_rx;

%incl_dx;
%incl_px;
%incl_rx;

%macro incl_table;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%global incldx inclpx inclrx ;

%if %sysfunc(exist(step3_incldx)) ne 0 %then %do;
	%let dsn=%upcase(step3_incldx);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;
		%let incldx=,b.dx, b.dxdate; 
		%let dxtable=inner join step3_incldx b on a.studyid = b.studyid; %end;
		%else %if &nobs = 0 %then %do;
		%let incldx =  ; 
		%let dxtable=  ; 
		%put NOTE: No inclusion dx observations selected.; %end; %end;
%else %if %sysfunc(exist(step3_incldx))= 0 %then %do;
%let incldx =  ; 
%let dxtable=  ; 
%put NOTE: No dx inclusion criteria.; %end;

%if %sysfunc(exist(step3_inclpx)) ne 0 %then %do;
	%let dsn=%upcase(step3_inclpx);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;
		%let inclpx=,c.px, c.pxdate; 
		%let pxtable=inner join step3_inclpx c on a.studyid = c.studyid; %end;
		%else %if &nobs = 0 %then %do;
		%let inclpx =  ; 
		%let pxtable=  ; 
		%put NOTE: No inclusion px observations selected.; %end; %end;
%else %if %sysfunc(exist(step3_inclpx))= 0 %then %do;
%let inclpx =  ; 
%let pxtable=  ; 
%put NOTE: No px inclusion criteria.; %end;

%if %sysfunc(exist(step3_inclrx)) ne 0 %then %do;
	%let dsn=%upcase(step3_inclrx);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;
		%let inclrx=,d.rx, d.rxdate; 
		%let rxtable=inner join step3_inclrx d on a.studyid = d.studyid; %end;
		%else %if &nobs = 0 %then %do;
		%let inclrx =  ; 
		%let rxtable=  ; 
		%put NOTE: No inclusion rx observations selected.; %end; %end;
%else %if %sysfunc(exist(step3_inclrx))= 0 %then %do;
%let inclrx =  ; 
%let rxtable=  ; 
%put NOTE: No rx inclusion criteria.; %end;

proc sql;

create table step3_incl as
select a.* &incldx &inclpx &inclrx
from step2 a
&dxtable
&pxtable
&rxtable
;

quit;

proc sql;
create table &Sitecode._incl_count as
select "&Sitecode. all Incl"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3_incl;
quit;

%mend incl_table;

%incl_table;

/******************************************************************************************************/
/************************************* Exclusion Criteria *********************************************/
/******************************************************************************************************/

%macro excl_dx;

%global excl_dx;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%if %sysfunc(exist(step3_incl)) ne 0 %then %do; 
%let dsn=%upcase(step3_incl);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;
%let inclmerge=step3_incl; %end;
%else %if &nobs = 0 %then %do;
%let inclmerge=Step2; %end; %end;


%let dsn=%upcase(excl_dx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No dx exclusion criteria.;
%return;
%end;

proc sql noprint;
select distinct cats("%str(%')",excl_dx_codes,"%str(%')") into:excl_dx
separated by ' '
from excl_dx_code;
quit;
proc sql noprint;
select distinct excl_dx_codes into :excldx1-:excldx1000000
from excl_dx_code;

%let excl_dx_num=&sqlobs;


quit;

data _null_;
set excl_dx_var;
call symputx ('excl_dx_bool', bool_operator, 'G');
run;
%let excl_dx_bool="&excl_dx_bool";

/* 11/16/2012 Rename dataset to end in a*/
data step3_excldxa;
keep studyid birth_date index_date index_code gender race1 hispanic dx dxdate;
  declare hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid','dx', 'dxdate');
  rc = a.DefineDone ();
  do until (eof1);
    set &__diagnoses end = eof1;
	dxdate = adate; format dxdate mmddyy10.;
    where "&begdate"d <= adate <= "&enddate"d
	and dx in (&excl_dx);
    rc = a.add ();
  end;
  do until (eof4) ;
    set &inclmerge end = eof4;
    call missing (dx,dxdate);
    rc = a.find ();
    if index_date ne .
	and adate ne .
    then output ;
  end;
stop; run;


%if &excl_dx_bool="And" %then %do;

proc sql;
create table testa as
select studyid, count(distinct dx) as count
from step3_excldxa
group by studyid;

create table step3_excldx2 as
select *
from step3_excldxa
where studyid in (select studyid from testa where count=&excl_dx_num)
order by studyid;

quit;

proc datasets;
   delete step3_excldx testa;
   change step3_excldx2 = step3_excldxa;
run;
quit;
%end;

%if %sysfunc(exist(step3_excldxa)) ne 0 %then %do; 
%let dsn=%upcase(step3_excldxa);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;

/* Retain only studyids that match */
proc sql;
create table step3_excldx as
select * from step3_excldxa
where dxdate ne .
order by studyid;
quit;

proc sql;
create table &sitecode._excldx_count as
select "&Sitecode. Excl Dx codes"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3_excldx;
quit;

%end;
%end;


%mend excl_dx;

%macro excl_px;

%global excl_px;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%if %sysfunc(exist(step3_incl)) ne 0 %then %do; 
%let dsn=%upcase(step3_incl);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;
%let inclmerge=step3_incl; %end;
%else %if &nobs = 0 %then %do;
%let inclmerge=Step2; %end; %end;


%let dsn=%upcase(excl_px_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No px exclusion criteria.;
%return;
%end;

proc sql noprint;
select distinct cats("%str(%')",excl_px_codes,"%str(%')") into:excl_px
separated by ' '
from excl_px_code;
quit;
proc sql noprint;
select distinct excl_px_codes into :exclpx1-:exclpx1000000
from excl_px_code;

%let excl_px_num=&sqlobs;


quit;

data _null_;
set excl_px_var;
call symputx ('excl_px_bool', bool_operator, 'G');
run;
%let excl_px_bool="&excl_px_bool";


data step3_exclpx;
keep studyid birth_date index_date index_code gender race1 hispanic px pxdate;
  declare hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid','index_date', 'index_code','birth_date', 'gender','race1','hispanic');
  rc = a.DefineDone ();
  do until (eof1);
    set &inclmerge end = eof1;
    rc = a.add ();
  end;
  do until (eof4) ;
    set &__procedures end = eof4;
	pxdate = adate; format pxdate mmddyy10.;
    where "&begdate"d <= adate <= "&enddate"d
    and px in (&excl_px);
    call missing (index_date,index_code,birth_date,gender,race1,hispanic);
    rc = a.find ();
    if index_date ne .
    then output ;
  end;
stop; run;

%if &excl_px_bool="And" %then %do;

proc sql;
create table testa as
select studyid, count(distinct px) as count
from step3_exclpx
group by studyid;

create table step3_exclpx2 as
select studyid, birth_date, index_date, index_code, gender, race1, hispanic, px, adate as pxdate
from step3_exclpx
where studyid in (select studyid from testa where count=&excl_px_num)
order by studyid;

quit;

proc datasets;
   delete step3_exclpx testa;
   change step3_exclpx2 = step3_exclpx;
run;
quit;
%end;

%if %sysfunc(exist(step3_exclpx)) ne 0 %then %do; 
%let dsn=%upcase(step3_exclpx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;

proc sort data=step3_exclpx;
by studyid;
run;

proc sql;
create table &sitecode._exclpx_count as
select "&Sitecode. Excl Px codes"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3_exclpx;
quit;

%end;
%end;

%mend excl_px;

%macro excl_rx;

%global excl_rx;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%if %sysfunc(exist(step3_incl)) ne 0 %then %do; 
%let dsn=%upcase(step3_incl);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;
%let inclmerge=step3_incl; %end;
%else %if &nobs = 0 %then %do;
%let inclmerge=Step2; %end; %end;

%let dsn=%upcase(excl_rx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No rx exclusion criteria.;
%return;
%end;


proc sql;
create table codelist_excl as
select distinct upcase(excl_rx_codes) as excl_rx_codes
from excl_rx_code;
quit;

proc sql noprint;
select distinct cats("%str(%')",excl_rx_codes,"%str(%')") into:excl_rx
separated by ' '
from codelist_excl;
quit;

data _null_;
set excl_rx_var;
call symputx ('excl_rx_bool', bool_operator, 'G');
run;

data codelist;
set excl_rx_code;
likecode=put(cats("generic like '%",excl_rx_codes,"%'"),$100.);
run;

proc sql noprint;
select likecode into : clause separated by ' &excl_rx_bool ' from codelist;
quit;

data step3a_exclrx;
keep studyid generic rxdate ndc;
declare hash a();
rc = a.DefineKey ('ndc');
rc = a.DefineData ('ndc','studyid','index_date');
rc = a.DefineDone ();
do until (eof1);
	set &__pharmacy end= eof1;
	rc = a.add();
	end;
do until (eof2);
set &__everndc end = eof2;
	where &clause;
call missing (studyid,index_date);
rc = a.find();
if studyid ne ' '
and index_date ne . 
then output;
end;
run;

data step3_exclrx;
keep studyid birth_date index_date index_code gender race1 hispanic ndc generic rxdate;
  declare hash a ();
  rc = a.DefineKey ('studyid');
  rc = a.DefineData ('studyid','index_code','index_date','birth_date', 'gender','race1','hispanic');
  rc = a.DefineDone ();
  do until (eof1);
    set &inclmerge end = eof1;
    rc = a.add ();
  end;
do until (eof3) ;
    set step3a_exclrx end = eof3;
    call missing (index_date,index_code,birth_date,gender,race1,hispanic);
    rc = a.find ();
    if index_date ne .
    and generic ne ' '
    then output ;
  end;
stop; run;

%if %sysfunc(exist(step3_exclrx)) ne 0 %then %do; 
%let dsn=%upcase(step3_exclrx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;

proc sort data=step3_exclrx;
by studyid;
run;

proc sql;
create table &sitecode._exclrx_count as
select "&Sitecode. Excl Rx codes"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3_exclrx;
quit;

%end;
%end;


%mend excl_rx;


%macro excl_age;

%global age_op age;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%if %sysfunc(exist(step3_incl)) ne 0 %then %do; 
%let dsn=%upcase(step3_incl);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;
%let inclmerge=step3_incl; %end;
%else %if &nobs = 0 %then %do;
%let inclmerge=Step2; %end; %end;

%let dsn=%upcase(excl_age_var);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No age exclusion criteria.;
%return;
%end;

data _null_;
  set excl_age_var;
    call symput ('age_op',COMPRESS(TRIM(age_operator)));
  call symput ('age',COMPRESS(TRIM(age)));
run;

proc sql;
create table step3_exclage as
select studyid, birth_date, index_date, index_code, %age(index_date,birth_date) as age, gender, race1, hispanic
from &inclmerge
having age &age_op &age
order by studyid;
quit;

proc sql;
create table &sitecode._exclage_count as
select "&Sitecode. Excl Age"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3_exclage;
quit;

%mend excl_age;

%excl_dx;
%excl_px;
%excl_rx;
%excl_age;


%macro excl_table;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%if %sysfunc(exist(step3_excldx)) ne 0 %then %do;
	%let dsn=%upcase(step3_excldx);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;
		%let excldx=step3_excldx; %end;
		%else %if &nobs = 0 %then %do;
		%let excldx =  ; 
		%put NOTE: No exclusion dx observations selected.; %end; %end;
%else %if %sysfunc(exist(step3_excldx))= 0 %then %do;
%let excldx =  ; 
%let dxtable=  ; 
%put NOTE: No dx exclusion criteria.; %end;

%if %sysfunc(exist(step3_exclpx)) ne 0 %then %do;
	%let dsn=%upcase(step3_exclpx);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;
		%let exclpx=step3_exclpx; %end;
		%else %if &nobs = 0 %then %do;
		%let exclpx =  ; 
		%put NOTE: No exclusion px observations selected.; %end; %end;
%else %if %sysfunc(exist(step3_exclpx))= 0 %then %do;
%let exclpx =  ; 
%let pxtable=  ; 
%put NOTE: No px exclusion criteria.; %end;

%if %sysfunc(exist(step3_exclrx)) ne 0 %then %do;
	%let dsn=%upcase(step3_exclrx);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;
		%let exclrx=step3_exclrx; %end;
		%else %if &nobs = 0 %then %do;
		%let exclrx =  ; 
		%put NOTE: No exclusion rx observations selected.; %end; %end;
%else %if %sysfunc(exist(step3_exclrx))= 0 %then %do;
%let exclrx =  ; 
%let rxtable=  ; 
%put NOTE: No rx exclusion criteria.; %end;

%if %sysfunc(exist(step3_exclage)) ne 0 %then %do;
	%let dsn=%upcase(step3_exclage);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs ne 0 %then %do;
		%let exclage=step3_exclage; %end;
		%else %if &nobs = 0 %then %do;
		%let exclage =  ; 
		%put NOTE: No exclusion age criteria selected.; %end; %end;
%else %if %sysfunc(exist(step3_exclage))= 0 %then %do;
%let exclage =  ; 
%let agetable=  ; 
%put NOTE: No age exclusion criteria.; %end;


proc sql;
  create table exl_null (studyid char(12));
  quit;

data step3_excl0;
merge exl_null &excldx &exclpx &exclrx &exclage;
by studyid;
run;

proc sql;

create table step3_excl as
select distinct studyid
from step3_excl0;

quit;

proc sql;
create table &Sitecode._excl_count as
select "&Sitecode. all Excl"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3_excl;
quit;

%mend excl_table;

%excl_table;

/******************************************************************************************************/
/******************************** Join Inclusion, Exlcusion Criteria **********************************/
/******************************************************************************************************/

%macro step3;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%if %sysfunc(exist(step3_incl)) ne 0 %then %do; 
%let dsn=%upcase(step3_incl);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;
proc sql;
create table step3 as
select *
from step3_incl
%if %sysfunc(exist(step3_excl)) ne 0 %then %do; 
%let dsn=%upcase(step3_excl);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;
where studyid not in (select studyid from step3_excl);
%end;
quit;
%end;
%end;
%end;

%else %if &nobs = 0 %then %do;
proc sql;
create table step3 as
select *
from step2
%if %sysfunc(exist(step3_excl)) ne 0 %then %do; 
%let dsn=%upcase(step3_excl);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs ne 0 %then %do;
where studyid not in (select studyid from step3_excl);
%end;
quit;
%end;
%end;

proc sql;
create table &sitecode._cohort_count as
select "&Sitecode. Final Cohort"  as Sample_Size format=$char50.
	 , count(distinct studyid) as patient_count
from step3;
quit;

%mend;

%step3;

/* Construct Waterfall table for output */



%macro waterfall;

%global sitelist;

data waterfall Upload__.waterfall;
set &sitecode._count
%if %sysfunc(exist(&sitecode._Index_count)) ne 0 %then %do; &sitecode._Index_count %end;
%if %sysfunc(exist(&sitecode._Enroll_count)) ne 0 %then %do; &sitecode._Enroll_count %end;
%if %sysfunc(exist(&sitecode._incldx_count)) ne 0 %then %do; &sitecode._incldx_count %end;
%if %sysfunc(exist(&sitecode._inclpx_count)) ne 0 %then %do; &sitecode._inclpx_count %end;
%if %sysfunc(exist(&sitecode._inclrx_count)) ne 0 %then %do; &sitecode._inclrx_count %end;
%if %sysfunc(exist(&sitecode._incl_count)) ne 0 %then %do; &sitecode._incl_count %end;
%if %sysfunc(exist(&sitecode._excldx_count)) ne 0 %then %do; &sitecode._excldx_count %end;
%if %sysfunc(exist(&sitecode._exclpx_count)) ne 0 %then %do; &sitecode._exclpx_count %end;
%if %sysfunc(exist(&sitecode._exclrx_count)) ne 0 %then %do; &sitecode._exclrx_count %end;
%if %sysfunc(exist(&sitecode._exclage_count)) ne 0 %then %do; &sitecode._exclage_count %end;
%if %sysfunc(exist(&sitecode._excl_count)) ne 0 %then %do; &sitecode._excl_count %end;
%if %sysfunc(exist(&sitecode._cohort_count)) ne 0 %then %do; &sitecode._cohort_count %end;
;
run;

proc sql;
create table aggregate_waterfall as
select substr(trim(sample_size),3,30) as sample_size
from waterfall;

%let waterfall=&sqlobs;
quit;

/* Total Aggregrate table reporting */
proc sql noprint;
select distinct datamart into :datamart1-:datamart100
from datamart;

%let sites=&sqlobs;
quit;

%do i=1 %to &sites;

%if %upcase(&&datamart&i)=SPAN KPCO %then %do; %let excel&i=CO; %end;
%else %if %upcase(&&datamart&i)=SPAN HPHC %then %do; %let excel&i=HV; %end;
%else %if %upcase(&&datamart&i)=SPAN GHC %then %do; %let excel&i=GH; %end;
%else %if %upcase(&&datamart&i)=SPAN KPNW %then %do; %let excel&i=NW; %end;
%else %if %upcase(&&datamart&i)=SPAN KPNC %then %do; %let excel&i=NC; %end;
%else %if %upcase(&&datamart&i)=SPAN KPHI %then %do; %let excel&i=HI; %end;
%else %if %upcase(&&datamart&i)=SPAN KPSE %then %do; %let excel&i=GA; %end;
%else %if %upcase(&&datamart&i)=SPAN HPRF %then %do; %let excel&i=HP; %end;
%else %if %upcase(&&datamart&i)=SPAN GHS %then %do; %let excel&i=GS; %end;
%else %if %upcase(&&datamart&i)=SPAN DHHA %then %do; %let excel&i=DH; %end;
%else %if %upcase(&&datamart&i)=SPAN EIRH %then %do; %let excel&i=ES; %end;

%else %if %upcase(&&datamart&i)=KPSPAN %then %do; %let excel&i=CO; %end;
%else %if %upcase(&&datamart&i)=KPCO1 %then %do; %let excel&i=GH; %end;
%else %if %upcase(&&datamart&i)=KPCO2 %then %do; %let excel&i=NC; %end;
%else %if %upcase(&&datamart&i)=KPCO3 %then %do; %let excel&i=HI; %end;
%else %if %upcase(&&datamart&i)=KPCO4 %then %do; %let excel&i=GA; %end;
%else %if %upcase(&&datamart&i)=KPCO5 %then %do; %let excel&i=HP; %end;
%else %if %upcase(&&datamart&i)=KPCO6 %then %do; %let excel&i=GS; %end;
%else %if %upcase(&&datamart&i)=KPCO7 %then %do; %let excel&i=DH; %end;
%else %if %upcase(&&datamart&i)=KPCO8 %then %do; %let excel&i=ES; %end;
%else %if %upcase(&&datamart&i)=KPCO9 %then %do; %let excel&i=XX; %end;
%else %if %upcase(&&datamart&i)=KPCO10 %then %do; %let excel&i=XY; %end;
%else %if %upcase(&&datamart&i)=TEST DATAMART %then %do; %let excel&i=HV; %end;
%end;

%let sitelist= ;
%do i=1 %to &sites;
%let sitelist=&sitelist &&datamart&i;
%end;

%do i=1 %to &sites;
data &&&excel&i.._wf;
datamart="&&excel&i";
do k=2 to %eval(&waterfall+1);
		call symput('k',k);
		excel&i=cats("'&&&excel&i.._waterfall'!$C$",k, "=",'"HIDDEN"',",0,'&&&excel&i.._waterfall'!$C$",k,"+0");
		excel0="IF(";
		output;
		end;
		run;
	%end;

%if &sites gt 1 %then %do;
%global sitelist;
%let sitelist = ;
%let excelfunct= ; 
%do i=1 %to &sites;
%let sitelist =&sitelist &&&excel&i.._wf;
%let excelfunct=&excelfunct.trim(EXCEL0)||''||trim(EXCEL&i)||")"||'+'||;
%end;

data site_merge_wf;
merge
&sitelist;
by k;
run;

data site_cats_wf (keep= Patient_Count);
length Patient_Count $ 5000;
set site_merge_wf;
%do i=2 %to &sites;
Patient_Count=cats("'=",&excelfunct."0");
/*Patient_Count="'="||trim(EXCEL0)||''||trim(EXCEL1)||"+"||trim(EXCEL0)||''||trim(EXCEL&i)||")"||")";*/
%end;
run;

%end;

%else %do;
data site_merge_wf;
set &excel1._wf;
run;

data site_cats_wf (keep= Patient_Count);
set site_merge_wf;
Patient_Count="'="||trim(EXCEL0)||''||trim(EXCEL1)||")";
run;

%end;

data aggregate_waterfall;
merge aggregate_waterfall site_cats_wf;
run;

%mend;

%waterfall;

/******************************************************************************************************/
/*************************************** Generate Reports *********************************************/
/******************************************************************************************************/

* Process formats and report structure for aggregation;
data ageformat age;
input age;
datalines;
0
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
61
62
63
64
65
66
67
68
69
70
71
72
73
74
75
76
77
78
79
80
81
82
83
84
85
86
87
88
;
run;

data ageformat;
set ageformat;
*agegrpoption_1=age;
agegrpoption_1=put(age, agegrp1_.);
agegrpoption_5=put(age, agegrp5_.);
agegrpoption_10=put(age, agegrp10_.);
agegrpoption_20=put(age, agegrp20_.);
run;

proc sql;
create table ageformat1 as
select distinct agegrpoption_1 as age1 from ageformat;

create table ageformat5 as
select distinct agegrpoption_5 as age5 from ageformat;

create table ageformat10 as
select distinct agegrpoption_10 as age10 from ageformat;

create table ageformat20 as
select distinct agegrpoption_20 as age20 from ageformat;

quit;

data bmi;
input bmi;
datalines;
.
0
1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
27
28
29
30
31
32
33
34
35
36
37
38
39
40
41
42
43
44
45
46
47
48
49
50
51
52
53
54
55
56
57
58
59
60
61
62
63
64
65
66
67
68
69
70
71
72
73
74
75
76
77
78
79
80
81
82
83
84
85
86
87
88
89
90
91
92
93
94
95
96
97
98
99
100
;
run;

data bmiformat0;
set bmi;
bmi2 = put(bmi, bmigrp_.);
run;

proc sql;
create table bmiformat as
select distinct bmi2 as bmi from bmiformat0;
quit;


data genderformat;
input gender $char1.;
datalines;
M
F
U
O
;
run;
quit;

data raceformat;
input race $char2.;
datalines;
UN
MU
AS
BA
HI
HP
IN
WH
;
run;
*Full Obeservation period range for all SPAN datamarts;
data dateformat;
input year;
datalines;
2004
2005
2006
2007
2008
2009
2010
;
run;
* Limit t Observation Period Range of Query;
data dateformat;
set dateformat;
if year > year("&enddate"d) then delete;
if year < year("&begdate"d) then delete;
run;



%macro reports;

%global par_row_1 par_column_1 par_group_1
		par_row_2 par_column_2 par_group_2
		par_row_3 par_column_3 par_group_3
		par_row_4 par_column_4 par_group_4
		par_row_5 par_column_5 par_group_5
		reports
		lookupincldx lookupinclpx lookupinclrx
		transrow_1 transcolumn_1 transgroup_1
		transrow_2 transcolumn_2 transgroup_2
		transrow_3 transcolumn_3 transgroup_3
		transrow_4 transcolumn_4 transgroup_4
		transrow_5 transcolumn_5 transgroup_5
;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%if %sysfunc(exist(report)) ne 0 %then %do; 

%let dsn=%upcase(report);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No reports requested.;
%return;
%end;
%else %if %sysfunc(exist(report))= 0 %then %do; 
%put NOTE: No reports requested.;
%return;
%end;
%end;

/*REPORT */

/* Report Macro */

proc sql noprint;
select distinct report into :report1-:report1000000
from report;

%let reports=&sqlobs;
quit;

%do i=1 %to &reports;
data report&i (keep=report row column group option);
set report;
if report="&&report&i";
run;

data _null_;
  set report&i;
    call symputx ("row_&i",row, 'G');
  call symputx ("column_&i",column, 'G');
  call symputx ("group_&i",group, 'G');
  call symputx ("option_&i",option, 'G');
run;

/* Rows */

%let par_row_&i = &&row_&i;

%if %upcase(&&row_&i)=DX %then %do;

%if %sysfunc(exist(step3_incldx)) ne 0 %then %do; 

%let dsn=%upcase(step3_incldx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs lt 1 %then %do;
%let lookupincldx= dx;
%let row_&i = "NoDxSelected" as dx, ;
%let par_row_&i = NoDxSelected; 
%end;
%else %do;
%let par_row_&i = &&row_&i; 
%end;
%end;
%else %if %sysfunc(exist(step3_incldx)) = 0 %then %do;
%let row_&i = "NoDxSelected" as dx, ;
%let par_row_&i = NoDxSelected; 
%end;
%end;

%if %upcase(&&row_&i)=PX %then %do;

%if %sysfunc(exist(step3_inclpx)) ne 0 %then %do; 

%let dsn=%upcase(step3_inclpx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs lt 1 %then %do;
%let lookupinclpx= px;
%let row_&i = "NoPxSelected" as px, ;
%let par_row_&i = NoPxSelected; 
%end;
%else %do;
%let par_row_&i = &&row_&i; 
%end;
%end;
%else %if %sysfunc(exist(step3_inclpx)) = 0 %then %do;
%let row_&i = "NoPxSelected" as px, ;
%let par_row_&i = NoPxSelected; 
%end;
%end;


%if %upcase(&&row_&i)=RX %then %do;

%if %sysfunc(exist(step3_inclrx)) ne 0 %then %do; 

%let dsn=%upcase(step3_inclrx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs lt 1 %then %do;
%let lookupinclrx= rx;
%let row_&i = "NoRxSelected" as rx, ;
%let par_row_&i = NoRxSelected; 
%end;
%else %do;
%let par_row_&i = &&row_&i; 
%end;
%end;
%else %if %sysfunc(exist(step3_inclrx)) = 0 %then %do;
%let row_&i = "NoRxSelected" as rx, ;
%let par_row_&i = NoRxSelected; 
%end;
%end;


/* Columns */
%let par_column_&i = &&column_&i; 
%if %upcase(&&column_&i)=DX %then %do;

%if %sysfunc(exist(step3_incldx)) ne 0 %then %do; 

%let dsn=%upcase(step3_incldx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs lt 1 %then %do;
%let lookupincldx= dx;
%let column_&i = "NoDxSelected" as dx, ;
%let par_column_&i = NoDxSelected; 
%end;
%else %do;
%let par_column_&i = &&column_&i; 
%end;
%end;
%else %if %sysfunc(exist(step3_incldx)) = 0 %then %do;
%let column_&i = "NoDxSelected" as dx, ;
%let par_column_&i = NoDxSelected; 
%end;
%end;


%if %upcase(&&column_&i)=PX %then %do;

%if %sysfunc(exist(step3_inclpx)) ne 0 %then %do; 

%let dsn=%upcase(step3_inclpx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs lt 1 %then %do;
%let lookupinclpx= px;
%let column_&i = "NoPxSelected" as px, ;
%let par_column_&i = NoPxSelected; 
%end;
%else %do;
%let par_column_&i = &&column_&i; 
%end;
%end;
%else %if %sysfunc(exist(step3_inclpx)) = 0 %then %do;
%let column_&i = "NoPxSelected" as px, ;
%let par_column_&i = NoPxSelected;  
%end;
%end;


%if %upcase(&&column_&i)=RX %then %do;

%if %sysfunc(exist(step3_inclrx)) ne 0 %then %do; 

%let dsn=%upcase(step3_inclrx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs lt 1 %then %do;
%let lookupinclrx= rx;
%let column_&i = "NoRxSelected" as rx, ;
%let par_column_&i = NoRxSelected;  
%end;
%else %do;
%let par_column_&i = &&column_&i; 
%end;
%end;
%else %if %sysfunc(exist(step3_inclrx)) = 0 %then %do;
%let column_&i = "NoRxSelected" as rx, ;
%let par_column_&i = NoRxSelected;  
%end;
%end;


/* Groups */
%let par_group_&i = &&group_&i; 
%if %upcase(&&group_&i)=Dx %then %do;

%if %sysfunc(exist(step3_incldx)) ne 0 %then %do; 

%let dsn=%upcase(step3_incldx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs lt 1 %then %do;
%let lookupincldx= dx;
%let group_&i = "NoDxSelected" as dx, ;
%let par_group_&i = NoDxSelected; 
%end;
%else %do;
%let par_group_&i = &&group_&i; 
%end;
%end;
%else %if %sysfunc(exist(step3_incldx)) = 0 %then %do;
%let group_&i = "NoDxSelected" as dx, ;
%let par_group_&i = NoDxSelected; 
%end;
%end;


%if %upcase(&&group_&i)=PX %then %do;

%if %sysfunc(exist(step3_inclpx)) ne 0 %then %do; 

%let dsn=%upcase(step3_inclpx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs lt 1 %then %do;
%let lookupinclpx= px;
%let group_&i = "NoPxSelected" as px, ;
%let par_group_&i = NoPxSelected; 
%end;
%else %do;
%let par_group_&i = &&group_&i; 
%end;
%end;
%else %if %sysfunc(exist(step3_inclpx)) = 0 %then %do;
%let group_&i = "NoPxSelected" as px, ;
%let par_group_&i = NoPxSelected; 
%end;
%end;


%if %upcase(&&group_&i)=RX %then %do;

%if %sysfunc(exist(step3_inclrx)) ne 0 %then %do; 

%let dsn=%upcase(step3_inclrx);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs lt 1 %then %do;
%let lookupinclrx= rx;
%let group_&i = "NoRxSelected" as rx, ;
%let par_group_&i = NoRxSelected; 
%end;
%else %do;
%let par_group_&i = &&group_&i; 
%end;
%end;
%else %if %sysfunc(exist(step3_inclrx)) = 0 %then %do;
%let group_&i = "NoRxSelected" as rx, ;
%let par_group_&i = NoRxSelected; 
%end;
%end;
******************************************************************;
******************************************************************;
/* Lookup tables based on reports */
******************************************************************;
******************************************************************;

%if %sysfunc(exist(incl_dx_code)) ne 0 %then %do;
	%let dsn=%upcase(incl_dx_code);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs gt 0 %then %do;
		%let dx= incl_dx_codes as dx,;
		%let dxtab= incl_dx_code,;
		%end;
		%else %if &nobs = 0 %then %do;
		%let dx= ;
		%let dxtab= ;
		%end;
		%end;
	
%if %sysfunc(exist(incl_px_code)) ne 0 %then %do;
	%let dsn=%upcase(incl_px_code);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs gt 0 %then %do;
		%let px=incl_px_codes as px,;
		%let pxtab=incl_px_code,;
		%end;
		%else %if &nobs = 0 %then %do;
		%let px= ;
		%let pxtab= ;
		%end;
		%end;

%if %sysfunc(exist(incl_rx_code)) ne 0 %then %do;
	%let dsn=%upcase(incl_rx_code);
	%let dsid=%sysfunc(open(&dsn));
	%let nobs= %sysfunc(attrn(&dsid,nlobs));
	%let dsidc=%sysfunc(close(&dsid));
		%if &nobs gt 0 %then %do;
		%let rx=incl_rx_codes as rx,;
		%let rxtab=incl_rx_code,;
		%end;
		%else %if &nobs = 0 %then %do;
		%let rx= ;
		%let rxtab= ;
		%end;
		%end;


proc sql;
create table  lookup&i as
select "lookup" as Lookup_Start,
%if (%upcase(&&column_&i)=YEAR or %upcase(&&row_&i)=YEAR or %upcase(&&group_&i)=YEAR) %then %do; put(Year,4.) as Year, %end;
%if (%upcase(&&column_&i)=GENDER or %upcase(&&row_&i)=GENDER or %upcase(&&group_&i)=GENDER) %then %do; Gender, %end;
%if (%upcase(&&column_&i)=INDEXVARIABLE or %upcase(&&row_&i)=INDEXVARIABLE or %upcase(&&group_&i)=INDEXVARIABLE) %then %do;
		  %if %upcase(&index_code)= DX %then %do;	input(index_Dx_codes,$char6.) as IndexVariable, %end;
	%else %if %upcase(&index_code) = PX %then %do; input(index_Px_codes,$char6.) as IndexVariable, %end;
	%else %if %upcase(&index_code) = RX %then %do; translate(compress(trim(index_Rx_codes),'!@#$%^&*()\/?:-[]{}|.'),'_',' ') as IndexVariable, %end;
	%else %if %upcase(&index_code) = BMI %then %do; bmi as IndexVariable, %end;
	%else %if %upcase(&index_code) = AGE %then %do; 
		%if &&option_&i le 1 %then %do; cats("AGEGRP",Age1) as IndexVariable, %end;
		%else %if &&option_&i = 5 %then %do; cats("AGEGRP",Age5) as IndexVariable, %end;
		%else %if &&option_&i = 10 %then %do; cats("AGEGRP",Age10) as IndexVariable, %end;
		%else %if &&option_&i = 20 %then %do; cats("AGEGRP",Age20) as IndexVariable, %end;
	%end;
%end;
%if (%upcase(&&column_&i)=AGE or %upcase(&&row_&i)=AGE or %upcase(&&group_&i)=AGE) %then %do;
age&&option_&i. as age, %end;
%if (%upcase(&&column_&i)=RACE or %upcase(&&row_&i)=Race or %upcase(&&group_&i)=RACE) %then %do; race, %end;
%if (%upcase(&&column_&i)=BMI or %upcase(&&row_&i)=Bmi or %upcase(&&group_&i)=BMI) %then %do; bmi, %end;
%if (%upcase(&&column_&i)=DX or %upcase(&&row_&i)=DX or %upcase(&&group_&i)=DX) %then %do; &Dx %end;
%if (%upcase(&&column_&i)=PX or %upcase(&&row_&i)=PX or %upcase(&&group_&i)=PX) %then %do; &Px %end;
%if (%upcase(&&column_&i)=RX or %upcase(&&row_&i)=RX or %upcase(&&group_&i)=RX) %then %do; &Rx %end;
"lookup" as Lookup_end
from  
%if (%upcase(&&column_&i)=YEAR or %upcase(&&row_&i)=YEAR or %upcase(&&group_&i)=YEAR) %then %do; dateformat, %end;
%if (%upcase(&&column_&i)=GENDER or %upcase(&&row_&i)=GENDER or %upcase(&&group_&i)=GENDER) %then %do; genderformat, %end;
%if (%upcase(&&column_&i)=INDEXVARIABLE or %upcase(&&row_&i)=INDEXVARIABLE or %upcase(&&group_&i)=INDEXVARIABLE) %then %do;
	%if %upcase(&index_code)= DX %then %do;	index_Dx_code, %end;
	%else %if %upcase(&index_code)= PX %then %do; index_Px_code, %end;
	%else %if %upcase(&index_code)= RX %then %do; index_Rx_code, %end;
	%else %if %upcase(&index_code)= BMI %then %do; bmiformat, %end;
	%else %if %upcase(&index_code)= AGE %then %do; 
		%if &&option_&i le 1 %then %do; Ageformat1, %end;
		%else %if &&option_&i = 5 %then %do; Ageformat5, %end;
		%else %if &&option_&i = 10 %then %do; Ageformat10, %end;
		%else %if &&option_&i = 20 %then %do; Ageformat20, %end;
	%end;
%end;
%if (%upcase(&&column_&i)=BMI or %upcase(&&row_&i)=BMI or %upcase(&&group_&i)=BMI) %then %do; bmiformat, %end;
%if (%upcase(&&column_&i)=AGE or %upcase(&&row_&i)=AGE or %upcase(&&group_&i)=AGE) %then %do; ageformat&&option_&i., %end;
%if (%upcase(&&column_&i)=RACE or %upcase(&&row_&i)=Race or %upcase(&&group_&i)=RACE) %then %do; raceformat, %end;
%if (%upcase(&&column_&i)=DX or %upcase(&&row_&i)=DX or %upcase(&&group_&i)=DX) %then %do; &Dxtab %end;
%if (%upcase(&&column_&i)=PX or %upcase(&&row_&i)=PX or %upcase(&&group_&i)=PX) %then %do; &Pxtab %end;
%if (%upcase(&&column_&i)=RX or %upcase(&&row_&i)=RX or %upcase(&&group_&i)=RX) %then %do; &Rxtab %end;
index_variable;
quit;

data lookup&i (drop=lookup_start lookup_end);
set lookup&i;
run;


/* Final Aggregation */

%if &incldx = ,b.dx, b.dxdate %then %do;
%let incldx = dx, dxdate, ; %end;

%if &inclpx = ,c.px, c.pxdate   %then %do;
%let inclpx = px, pxdate, ; %end;

%if &inclrx = ,d.rx, d.rxdate   %then %do;
%let inclrx = rx, rxdate, ; %end;

proc sql;
create table summary&i as
select put(year(index_date),4.) as year,
	%if &index_code =bmi %then %do;
	put(index_code,bmigrp_.) as IndexVariable,
	%end;
	%else %if &index_code = age %then %do;
    round(index_code) as IndexVariable,
	%end; 
	%else %do;
    index_code as IndexVariable,
	%end; 
     gender, 
    (case when Hispanic = 'Y' then 'HI' else Race1 end) as Race, 
	%if &&option_&i le 1 %then %do;
	put((%age(index_date,birth_date)),3.) as age,
	%end;
	%else %if &&option_&i gt 1 %then %do;
	put(%age(index_date,birth_date),agegrp&&option_&i.._.) as age,
	%end;
     bmigrp as bmi, 
     &incldx
     &inclpx
     &inclrx
     studyid,
     count(distinct studyid) as pat_count
from step3
group by year,
       IndexVariable, 
       gender,  
       age, 
	   bmi, 
       &incldx
       &inclpx
       &inclrx
     studyid,
     race
order by year,
       IndexVariable, 
       gender,  
       age,
	   bmi, 
       &incldx
       &inclpx
       &inclrx
     studyid,
     race;
quit;


/* Mask patients with ages greater than 88 years old */
/* Remove Other, Unkown gender from lookup- only at KPNC */
data summary&i.2;
set summary&i;
if age ge 88 then age = 88;
%if %UPCASE(&index_code) = AGE %then %do;
if IndexVariable ge 88 then IndexVariable=88;
IndexVariable2=cats("AGEGRP",put(IndexVariable,agegrp1_.));
format IndexVariable2 $100.;
%end;
run;

%if %UPCASE(&index_code) = AGE %then %do;
data summary&i.2(drop=IndexVariable);
set summary&i.2;
rename IndexVariable2=IndexVariable;
run;
%end;

%put NOTE: WARNINGS FOR MISSING VALUES ARE EXPECTED AND NORMAL HERE.;
PROC APPEND
     BASE=summary&i.2
     DATA=lookup&i. force;
RUN;

data summary&i.3;
set summary&i.2;
%if &&column_&i=  %then %do;
column=&&group_&i;
%end;
%else %do;
column=&&par_column_&i;
%end;
%if &&&column_&i="NoDxSelected" as dx, %then %do;
	IV=cats(column,&&par_column_&i); %end;
%else %if &&&column_&i="NoPxSelected" as px, %then %do;
	IV=cats(column,&&par_column_&i); %end;
%else %if &&&column_&i="NoRxSelected" as rx, %then %do;
	IV=cats(column,&&par_column_&i); %end;
%else %do;
IV=cats(column,column);
%end;
%if %UPCASE(&index_code) = AGE %then %do;
index_code_3=compress(translate(translate(translate(translate(trim(IndexVariable),'_',':'),'_',' '),'_to_','-'),"_plus_",'+'));
year=year("&age_index_date"d);
%end;
/*
if Gender='O' then delete;
if Gender='U' then delete;
*/
run;


proc sql;
create table IV&i as
select distinct IV as IVa, compress(trim(IV),'./\()!@#$%^&*-') as IV
from summary&i.3;
quit;


proc sql noprint;
select distinct IV into : IV_&i.s separated by ' '
from IV&i;
quit;

%let IVs = &sqlobs;

%if %UPCASE(&index_code) = AGE %then %do;
data summary&i.4; ;
set summary&i.3 (drop=IndexVariable IV);
rename index_code_3=IndexVariable;
run;
%end;
%else %do;
data summary&i.4; ;
set summary&i.3 (drop=column IV);
run;
%end;

/* Begin report processing */

%if &&row_&i ne  %then %do;
	%if &&row_&i="NoDxSelected" as dx, %then %do; %let sqlrow_&i= &&&row_&i; %end;
	%else %if &&row_&i="NoPxSelected" as px, %then %do; %let sqlrow_&i= &&&row_&i; %end;
	%else %if &&row_&i="NoRxSelected" as rx, %then %do; %let sqlrow_&i= &&&row_&i; %end;
	%else %do; %let sqlrow_&i= &&row_&i,; %end;
%end;
%else %if &&row_&i =  %then %do; %let sqlrow_&i= ; %end;

%if &&column_&i ne  %then %do;
	%if &&column_&i="NoDxSelected" as dx, %then %do; %let sqlcolumn_&i= &&&column_&i; %end;
	%else %if &&column_&i="NoPxSelected" as px, %then %do; %let sqlcolumn_&i= &&&column_&i; %end;
	%else %if &&column_&i="NoRxSelected" as rx, %then %do; %let sqlcolumn_&i= &&&column_&i; %end;
	%else %do; %let sqlcolumn_&i= &&column_&i,; %end;
%end;
%else %if &&column_&i =  %then %do; %let sqlcolumn_&i= ; %end;

%if &&group_&i ne  %then %do;
	%if &&group_&i="NoDxSelected" as dx, %then %do; %let sqlgroup_&i= &&&group_&i; %end;
	%else %if &&group_&i="NoPxSelected" as px, %then %do; %let sqlgroup_&i= &&&group_&i; %end;
	%else %if &&group_&i="NoRxSelected" as rx, %then %do; %let sqlgroup_&i= &&&group_&i; %end;
	%else %do; %let sqlgroup_&i= &&group_&i,; %end;
%end;
%else %if &&group_&i =  %then %do; %let sqlgroup_&i= ; %end;


proc sql noprint;

create table report_&i as
select "Module 1" as query_type,
     &&&sqlrow_&i
     &&&sqlcolumn_&i
     &&&sqlgroup_&i
     "Module 1" as query_type_,
     count(distinct studyid) as patient_count
from summary&i.4
group by query_type,
	%if &&sqlgroup_&i="NoDxSelected" as dx, %then %do; dx, %end;
	%else %if &&sqlgroup_&i="NoPxSelected" as px, %then %do; px, %end;
	%else %if &&sqlgroup_&i="NoRxSelected" as rx, %then %do; rx, %end;
	%else %do; &&&sqlgroup_&i %end;
	%if &&sqlrow_&i="NoDxSelected" as dx, %then %do; dx, %end;
	%else %if &&sqlrow_&i="NoPxSelected" as px, %then %do; px, %end;
	%else %if &&sqlrow_&i="NoRxSelected" as rx, %then %do; rx, %end;
	%else %do; &&&sqlrow_&i %end;
	%if &&sqlcolumn_&i="NoDxSelected" as dx, %then %do; dx, %end;
	%else %if &&sqlcolumn_&i="NoPxSelected" as px, %then %do; px, %end;
	%else %if &&sqlcolumn_&i="NoRxSelected" as rx, %then %do; rx, %end;
	%else %do; &&&sqlcolumn_&i %end;
     query_type_
order by query_type,
	%if &&sqlgroup_&i="NoDxSelected" as dx, %then %do; dx, %end;
	%else %if &&sqlgroup_&i="NoPxSelected" as px, %then %do; px, %end;
	%else %if &&sqlgroup_&i="NoRxSelected" as rx, %then %do; rx, %end;
	%else %do; &&&sqlgroup_&i %end;
	%if &&sqlrow_&i="NoDxSelected" as dx, %then %do; dx, %end;
	%else %if &&sqlrow_&i="NoPxSelected" as px, %then %do; px, %end;
	%else %if &&sqlrow_&i="NoRxSelected" as rx, %then %do; rx, %end;
	%else %do; &&&sqlrow_&i %end;
	%if &&sqlcolumn_&i="NoDxSelected" as dx, %then %do; dx, %end;
	%else %if &&sqlcolumn_&i="NoPxSelected" as px, %then %do; px, %end;
	%else %if &&sqlcolumn_&i="NoRxSelected" as rx, %then %do; rx, %end;
	%else %do; &&&sqlcolumn_&i %end;
     query_type_;

data report_&i (drop=patient_count);
set report_&i;
count = put(patient_count, count.);
if count=' ' then count='HIDDEN';
run;

%if &&row_&i ne  %then %do;
	%if &&&row_&i="NoDxSelected" as dx, %then %do;
	%let transrow_&i= dx; %end;
	%else %if &&&row_&i="NoPxSelected" as px, %then %do;
	%let transrow_&i= px; %end;
	%else %if &&&row_&i="NoRxSelected" as rx, %then %do;
	%let transrow_&i= rx; %end;
%else %do;
%let transrow_&i= &&row_&i ; %end; %end;
%if &&row_&i =  %then %do;
%let transrow_&i= ; %end;

%if &&column_&i ne  %then %do;
	%if &&&column_&i="NoDxSelected" as dx, %then %do;
	%let transcolumn_&i= dx; %end;
	%else %if &&&column_&i="NoPxSelected" as px, %then %do;
	%let transcolumn_&i= px; %end;
	%else %if &&&column_&i="NoRxSelected" as rx, %then %do;
	%let transcolumn_&i= rx; %end;
%else %do;
%let transcolumn_&i= &&column_&i ; %end; %end;
%if &&column_&i =  %then %do;
%let transcolumn_&i= ; %end;

%if &&group_&i ne  %then %do;
	%if &&&group_&i="NoDxSelected" as dx, %then %do;
	%let transgroup_&i= dx; %end;
	%else %if &&&group_&i="NoPxSelected" as px, %then %do;
	%let transgroup_&i= px; %end;
	%else %if &&&group_&i="NoRxSelected" as rx, %then %do;
	%let transgroup_&i= rx; %end;
%else %do;
%let transgroup_&i= &&group_&i ; %end; %end;
%if &&group_&i =  %then %do;
%let transgroup_&i= ; %end;

	%if %UPCASE(&&&column_&i) = YEAR %then %do;
	%let prefix= PREFIX= ;
	%let hdr=YR; %end;
	%else %if %UPCASE(&&&column_&i) = DX %then %do;
	%let prefix= PREFIX= ;
	%let  hdr=DX; %end;
	%else %if %UPCASE(&&&column_&i) = PX %then %do;
	%let prefix= PREFIX= ;
	%let  hdr=PX; %end;
	%else %if %UPCASE(&&&column_&i) = RX %then %do;
	%let prefix= PREFIX= ;
	%let  hdr=RX; %end;
	%else %if %UPCASE(&&&column_&i) = INDEXVARIABLE %then %do;
		%if %UPCASE(&index_code) = AGE %then %do;
		%let prefix= PREFIX= ;
		%let  hdr=AGE; %end;
	%else %do;
	%let prefix= PREFIX= ;
	%let  hdr=&index_code; %end;
	%end;
	%else %if %UPCASE(&&&column_&i) = AGE %then %do;
	%let prefix= PREFIX= ;
	%let  hdr=AGE_; %end;
	%else %do;
	%let prefix= ;
	%let  hdr=  ; %end;

/* Seperate processing for Rx in columns */

%if %upcase(&&&transcolumn_&i)=INDEXVARIABLE %then %do;
		%if &index_code=rx %then %do;
		proc sql;
		create table columnlist&i as
		select distinct &&&transcolumn_&i
		from lookup&i;
		quit;
	
		proc transpose data=columnlist&i out=columns_&i._trans;
		var &&&transcolumn_&i;
		run;

		data columns_&i._trans (drop=_NAME_);
		retain &&&transrow_&i &&&transgroup_&i;
		set columns_&i._trans;
			%if &&&transrow_&i ne %then %do;
			&&&transrow_&i ='                           ';
			%end;
			%if &&&transgroup_&i ne %then %do;
			&&&transgroup_&i ='                         ';
			%end;
		run;

		proc transpose data=report_&i out=report_b_&i (DROP=_NAME_ query_type);
		by query_type &&&transgroup_&i &&&transrow_&i ;
		var count;
		run;

		data report_a_&i;
		set columns_&i._trans report_b_&i;
		run;
		%end;
		%else %do;
		proc transpose data=report_&i out=report_a_&i (DROP=_NAME_ ) &PREFIX.&HDR;
		by query_type &&&transgroup_&i &&&transrow_&i ;
		var count;
		%if &&&transcolumn_&i ne  %then %do;
		id &&&transcolumn_&i ;
		%end;
		run;

		data report_a_&i (DROP=Query_Type);
		set report_a_&i;
		array chars _character_;
		do over chars;
		if chars='' then chars='HIDDEN';
		end;
		run;
		%end;
%end;

%else %if %upcase(&&&transcolumn_&i)=RX %then %do;
		proc sql;
		create table columnlist&i as
		select distinct &Rx
		from lookup&i;
		quit;
	
		proc transpose data=columnlist&i out=columns_&i._trans;
		var &&&transcolumn_&i;
		run;

		data columns_&i._trans (drop=_NAME_);
		retain &&&transrow_&i &&&transgroup_&i;
		set columns_&i._trans;
			%if &&&transrow_&i ne %then %do;
			&&&transrow_&i =' ';
			%end;
			%if &&&transgroup_&i ne %then %do;
			&&&transgroup_&i =' ';
			%end;
		run;

		proc transpose data=report_&i out=report_b_&i (DROP=_NAME_ query_type);
		by query_type &&&transgroup_&i &&&transrow_&i ;
		var count;
		run;

		data report_a_&i;
		set columns_&i._trans report_a_&i;
		run;
%end;

%else %do;
proc transpose data=report_&i out=report_a_&i (DROP=_NAME_ ) &PREFIX.&HDR;
by query_type &&&transgroup_&i &&&transrow_&i ;
var count;
%if &&&transcolumn_&i ne  %then %do;
id &&&transcolumn_&i ;
%end;
run;

data report_a_&i (DROP=Query_Type);
set report_a_&i;
array chars _character_;
do over chars;
if chars='' then chars='HIDDEN';
end;
run;
%end;
%end;
%mend;

%reports;


/******************************************************************************************************/
/*********************************** Aggregate Table Formatting and Creation***************************/
/******************************************************************************************************/

/* Excel Output Formatting */
%let ncycle=2;

data excel_colnames;
  keep start label fmtname;
  array alphabet{26} $ ('A' 'B' 'C' 'D' 'E' 'F' 'G' 'H' 'I' 'J' 'K' 'L' 'M' 'N' 'O' 'P' 'Q' 'R' 'S' 'T' 'U' 'V' 'W' 'X' 'Y' 'Z');
  do cycle=0 to &ncycle;
    do letter=1 to 26;
      start=26*cycle+letter;
      if cycle=0 then label=alphabet[letter];
      else label=cats(alphabet[cycle],alphabet[letter]);
      fmtname='XLFMT';
      output;
    end;
  end;
run;

proc format cntlin=excel_colnames;
quit;

data test;
  do col=1 to 55;
    trythis=put(col,xlfmt.);
    output;
  end;
run;

data excel_colnames2;
  keep start label fmtname;
  array alphabet{103} $ ('B' 'C' 'D' 'E' 'F' 'G' 'H' 'I' 'J' 'K' 'L' 'M' 'N' 'O' 'P' 'Q' 'R' 'S' 'T' 'U' 'V' 'W' 'X' 'Y' 'Z'
						'AA' 'AB' 'AC' 'AD' 'AE' 'AF' 'AG' 'AH' 'AI' 'AJ' 'AK' 'AL' 'AM' 'AN' 'AO' 'AP' 'AQ' 'AR' 'AS' 'AT'
					    'AU' 'AV' 'AW' 'AX' 'AY' 'AZ' 'BA' 'BB' 'BC' 'BD' 'BE' 'BF' 'BG' 'BH' 'BI' 'BJ' 'BK' 'BL' 'BM' 'BN'
						'BO' 'BP' 'BQ' 'BR' 'BS' 'BT' 'BU' 'BV' 'BW' 'BX' 'BY' 'BZ' 'CA' 'CB' 'CC' 'CD' 'CE' 'CF' 'CG' 'CH'
						'CI' 'CJ' 'CK' 'CL' 'CM' 'CN' 'CO' 'CP' 'CQ' 'CR' 'CS' 'CT' 'CU' 'CV' 'CW' 'CX' 'CY' 'CZ');
  do cycle=0 to &ncycle;
    do letter=1 to 103;
      start=103*cycle+letter;
      if cycle=0 then label=alphabet[letter];
      else label=cats(alphabet[cycle],alphabet[letter]);
      fmtname='XLFMTB';
      output;
    end;
  end;
run;

proc format cntlin=excel_colnames2;
quit;
data test2;
  do col=1 to 103;
    trythis=put(col,xlfmtB.);
    output;
  end;
run;

data excel_colnames3;
  keep start label fmtname;
  array alphabet{102} $ ('C' 'D' 'E' 'F' 'G' 'H' 'I' 'J' 'K' 'L' 'M' 'N' 'O' 'P' 'Q' 'R' 'S' 'T' 'U' 'V' 'W' 'X' 'Y' 'Z'
						'AA' 'AB' 'AC' 'AD' 'AE' 'AF' 'AG' 'AH' 'AI' 'AJ' 'AK' 'AL' 'AM' 'AN' 'AO' 'AP' 'AQ' 'AR' 'AS' 'AT'
					    'AU' 'AV' 'AW' 'AX' 'AY' 'AZ' 'BA' 'BB' 'BC' 'BD' 'BE' 'BF' 'BG' 'BH' 'BI' 'BJ' 'BK' 'BL' 'BM' 'BN'
						'BO' 'BP' 'BQ' 'BR' 'BS' 'BT' 'BU' 'BV' 'BW' 'BX' 'BY' 'BZ' 'CA' 'CB' 'CC' 'CD' 'CE' 'CF' 'CG' 'CH'
						'CI' 'CJ' 'CK' 'CL' 'CM' 'CN' 'CO' 'CP' 'CQ' 'CR' 'CS' 'CT' 'CU' 'CV' 'CW' 'CX' 'CY' 'CZ');
  do cycle=0 to &ncycle;
    do letter=1 to 102;
      start=102*cycle+letter;
      if cycle=0 then label=alphabet[letter];
      else label=cats(alphabet[cycle],alphabet[letter]);
      fmtname='XLFMTC';
      output;
    end;
  end;
run;

proc format cntlin=excel_colnames3;
quit;
data test3;
  do col=1 to 102;
    trythis=put(col,xlfmtC.);
    output;
  end;
run;


data excel_colnames4;
  keep start label fmtname;
  array alphabet{102} $ ('D' 'E' 'F' 'G' 'H' 'I' 'J' 'K' 'L' 'M' 'N' 'O' 'P' 'Q' 'R' 'S' 'T' 'U' 'V' 'W' 'X' 'Y' 'Z'
						'AA' 'AB' 'AC' 'AD' 'AE' 'AF' 'AG' 'AH' 'AI' 'AJ' 'AK' 'AL' 'AM' 'AN' 'AO' 'AP' 'AQ' 'AR' 'AS' 'AT'
					    'AU' 'AV' 'AW' 'AX' 'AY' 'AZ' 'BA' 'BB' 'BC' 'BD' 'BE' 'BF' 'BG' 'BH' 'BI' 'BJ' 'BK' 'BL' 'BM' 'BN'
						'BO' 'BP' 'BQ' 'BR' 'BS' 'BT' 'BU' 'BV' 'BW' 'BX' 'BY' 'BZ' 'CA' 'CB' 'CC' 'CD' 'CE' 'CF' 'CG' 'CH'
						'CI' 'CJ' 'CK' 'CL' 'CM' 'CN' 'CO' 'CP' 'CQ' 'CR' 'CS' 'CT' 'CU' 'CV' 'CW' 'CX' 'CY' 'CZ' 'DA');
  do cycle=0 to &ncycle;
    do letter=1 to 102;
      start=102*cycle+letter;
      if cycle=0 then label=alphabet[letter];
      else label=cats(alphabet[cycle],alphabet[letter]);
      fmtname='XLFMTD';
      output;
    end;
  end;
run;

proc format cntlin=excel_colnames4;
quit;
data test4;
  do col=1 to 102;
    trythis=put(col,xlfmtD.);
    output;
  end;
run;

/* Column Output Formats */


*GENDER;
%let ncycle=1;
%macro index_var_format;

%if &index_code = dx %then %do;
*INDEX DX;
data INDEX_colnames;
  keep start label fmtname;
  array index{100} $ (&index_dx);
    do letter=1 to &index_num;
      start=0+letter;
      label=index[letter];
      fmtname='INDEX';
      output;
    end;
run;
%end;

%else %if &index_code = px %then %do;
*INDEX PX;
data INDEX_colnames;
  keep start label fmtname;
  array index{100} $ (&index_px);
    do letter=1 to &index_num;
      start=0+letter;
      label=index[letter];
      fmtname='INDEX';
      output;
    end;
run;
%end;

%else %if &index_code = rx %then %do;
*INDEX RX;

proc sql noprint;
select distinct translate(compress(cats("%str(%')",index_rx_codes,"%str(%')"), '!@#$%^&*()\/?:-[]{}|.'),'_',' ') into:index_rx
separated by ' '
from codelist0;
quit;

data INDEX_colnames;
  keep start label fmtname;
  array index{256} $ 256 (&index_rx);
    do letter=1 to &index_num;
      start=0+letter;
      label=index[letter];
      fmtname='INDEX';
      output;
    end;
run;

%end;

%else %if &index_code=bmi %then %do;
*INDEX BMI;
data INDEX_colnames;
  keep start label fmtname;
  array index{100} $ (&BMIFMT);
    do letter=1 to &index_num;
      start=0+letter;
      label=Index[letter];
      fmtname='INDEX';
      output;
    end;
run;

%end;

%else %if &index_code=age %then %do;
*INDEX AGE;
proc sql noprint;
select distinct compress(trim(translate(translate(translate(translate(cats("%str(%'AGEGRP)",AGE1,"%str(%')"),'_',':'),'_',' '),'_to_','-'),'_','+'))) into:AGELIST1
separated by ' '
from AGEFORMAT1;
quit;

data INDEX_colnames;
  keep start label fmtname;
  array INDEX{100} $100 (&AGELIST1);
    do letter=1 to &index_num;
      start=0+letter;
      label=Index[letter];
      fmtname="INDEX";
      output;
    end;
run;

%end;

proc format cntlin=INDEX_colnames;
quit;

%mend;

%index_var_format;

proc sql noprint;
select distinct cats("%str(%')",gender,"%str(%')") into:GENDER
separated by ' '
from GENDERFORMAT;
quit;

proc sql noprint;
select distinct gender into :gender1-:gender1000000
from genderformat;

%let gender_num=&sqlobs;
quit;

%put &gender;

data GENDER_colnames;
  keep start label fmtname;
  array gender{10} $ (&GENDER);
    do letter=1 to &gender_num;
      start=0+letter;
      label=GENDER[letter];
      fmtname='GENDER';
      output;
    end;
run;

proc format cntlin=GENDER_colnames;
quit;

*RACE;
%let ncycle=1;
proc sql noprint;
select distinct cats("%str(%')",RACE,"%str(%')") into:RACE
separated by ' '
from RACEFORMAT;
quit;

proc sql noprint;
select distinct race into :race1-:race1000000
from raceformat;

%let race_num=&sqlobs;
quit;

%put &race;

data RACE_colnames;
  keep start label fmtname;
  array race{10} $ (&RACE);
    do letter=1 to &race_num;
      start=0+letter;
      label=RACE[letter];
      fmtname='RACE';
      output;
    end;
run;

proc format cntlin=RACE_colnames;
quit;

*AGES;
%macro ages(T);

%LET AGE=AGE&T;

proc sql noprint;
select distinct compress(trim(translate(translate(translate(translate(cats("%str(%'AGEGRP)",&AGE,"%str(%')"),'_',':'),'_',' '),'_to_','-'),'_','+'))) into:AGELIST&T
separated by ' '
from AGEFORMAT&T;
quit;

proc sql noprint;
select distinct &AGE into :&AGE._1-:&AGE._1000000
from AGEFORMAT&T;

%let &AGE._num=&sqlobs;
quit;

data &AGE._colnames;
  keep start label fmtname;
  array &AGE{100} $100 (&&AGELIST&T);
    do letter=1 to &&AGE&T._num;
      start=0+letter;
      label=&AGE.[letter];
      fmtname="AGE&T._";
      output;
    end;
run;

proc format cntlin=&AGE._colnames;
quit;

%mend;

%ages(1);
%ages(5);
%ages(10);
%ages(20);

*BMI;
proc sql noprint;
select distinct compress(trim(translate(translate(translate(translate(cats("%str(%'BMIGRP)",BMI,"%str(%')"),'_',':'),'_',' '),'_to_','-'),'_','+'))) into:BMIFMT
separated by ' '
from BMIFORMAT;
quit;

proc sql noprint;
select distinct BMI into :BMI_1-:BMI_1000000
from BMIFORMAT;

%let BMI_num=&sqlobs;
quit;

data BMI_colnames;
  keep start label fmtname;
  array BMI{100} $50 (&BMIFMT);
    do letter=1 to &BMI_num;
      start=0+letter;
      label=BMI[letter];
      fmtname='BMI';
      output;
    end;
run;

proc format cntlin=BMI_colnames;
quit;

*YEAR;
proc sql noprint;
select distinct compress(trim(translate(translate(translate(translate(cats("%str(%'YR)",YEAR,"%str(%')"),'_',':'),'_',' '),'_to_','-'),'_','+'))) into:YRFMT
separated by ' '
from DATEFORMAT;
quit;

proc sql noprint;
select distinct YEAR into :YEAR_1-:YEAR_1000000
from DATEFORMAT;

%let YEAR_num=&sqlobs;
quit;

data YEAR_colnames;
  keep start label fmtname;
  array YEAR{100} $ (&YRFMT);
    do letter=1 to &YEAR_num;
      start=0+letter;
      label=YEAR[letter];
      fmtname='YEAR_';
      output;
    end;
run;

proc format cntlin=YEAR_colnames;
quit;

*need macro around incl criteria;
%macro incldx_fmt;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%let dsn=%upcase(incl_dx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No dx inclusion criteria.;
%return;
%end;

*INCL DX;
data INCLDX_colnames;
  keep start label fmtname;
  array incldx{100} $ (&incl_dx);
    do letter=1 to &incl_dx_num;
      start=0+letter;
      label=incldx[letter];
      fmtname='INCLDX';
      output;
    end;
run;

proc format cntlin=INCLDX_colnames;
quit;

%mend;

%macro inclpx_fmt;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%let dsn=%upcase(incl_px_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No px inclusion criteria.;
%return;
%end;

*INCL PX;
data INCLPX_colnames;
  keep start label fmtname;
  array inclPx{100} $ (&incl_Px);
    do letter=1 to &incl_Px_num;
      start=0+letter;
      label=inclPx[letter];
      fmtname='INCLPX';
      output;
    end;
run;

proc format cntlin=INCLPX_colnames;
quit;

%mend;


%macro inclrx_fmt;

%if &index = 0 %then %do;
%put NOTE: No observations selected from Index Variable/Period/Enrollment Criteria.;
%return;
%end;

%let dsn=%upcase(incl_rx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));

%if &nobs= 0 %then %do;
%put NOTE: No rx inclusion criteria.;
%return;
%end;

*INCL RX;
proc sql noprint;

create table incl_rx_compress as 
select distinct compress(trim(incl_rx_codes)) as incl_rx_codes
from incl_rx_code;

/*
select distinct compress(trim(translate(translate(translate(translate(translate(translate(translate(translate(translate(translate(translate(cats("%str(%')",incl_rx_codes,"%str(%')"),'_','&'),'_','('),'_',')'),'_','/'),'_','%'),'_','.'),'','*'),'_',':'),'_',' '),'_to_','-'),'_','%'))) into:incl_rx_compress
*/
select distinct translate(compress(cats("%str(%')",incl_rx_codes,"%str(%')"),'!@#$%^&*()\/?:-[]{}|.'),'_',' ') into:incl_rx_compress
separated by ' '
from incl_rx_compress;
quit;

data INCLRX_colnames;
  keep start label fmtname;
  array inclRx{256} $256 (&incl_rx_compress);
    do letter=1 to &incl_Rx_num;
      start=0+letter;
      label=inclRx[letter];
      fmtname='INCLRX';
      output;
    end;
run;

proc format cntlin=INCLRX_colnames;
quit;

%mend;

%incldx_fmt;
%inclpx_fmt;
%inclrx_fmt;


%macro aggregate;

/* Total Aggregrate table reporting */
proc sql noprint;
select distinct datamart into :datamart1-:datamart100
from datamart;

%let sites=&sqlobs;
quit;

proc sql noprint;
select distinct report into :report1-:report1000000
from report;

%let reports=&sqlobs;
quit;

*** Process row counts ****;
%do j=1 %to &reports;
proc sql noprint;
select count(*) into :report_rows_&j.
from report_a_&j.;

%let report_rows_&j.=%eval(&&&report_rows_&j.+1);

*** Process Column Counts ****;
%if &&&par_column_&j. =  %then %do;
%let report_columns_&j. =0;
%end;

%else %if &&&par_column_&j. ne  %then %do;
	%if (%upcase(&&&par_column_&j.)=NORXSELECTED or %upcase(&&&par_group_&j.)=NORXSELECTED)%then %do;
	proc sql noprint;
	select count(*) into :report_columns_&j.
	from report_a_&j.;
	%end;
	%else %if (%upcase(&&&par_column_&j.)=NOPXSELECTED or %upcase(&&&par_group_&j.)=NOPXSELECTED) %then %do;
	proc sql noprint;
	select count(*) into :report_columns_&j.
	from report_a_&j.;
	%end;
	%else %if (%upcase(&&&par_column_&j.)=NODXSELECTED or %upcase(&&&par_group_&j.)=NODXSELECTED) %then %do;
	proc sql noprint;
	select count(*) into :report_columns_&j.
	from report_a_&j.;
	%end;
	%else %do;
proc sql noprint;
select count (distinct &&&par_column_&j.) into: report_columns_&j.
from lookup&j.;
quit;
	%end;
%end;
%end;

*%put &report_columns_1 &report_columns_2 &report_columns_3 &report_columns_4 &report_columns_5;

%do i=1 %to &sites;

%if %upcase(&&datamart&i)=SPAN KPCO %then %do; %let excel&i=CO; %end;
%else %if %upcase(&&datamart&i)=SPAN HPHC %then %do; %let excel&i=HV; %end;
%else %if %upcase(&&datamart&i)=SPAN GHC %then %do; %let excel&i=GH; %end;
%else %if %upcase(&&datamart&i)=SPAN KPNW %then %do; %let excel&i=NW; %end;
%else %if %upcase(&&datamart&i)=SPAN KPNC %then %do; %let excel&i=NC; %end;
%else %if %upcase(&&datamart&i)=SPAN KPHI %then %do; %let excel&i=HI; %end;
%else %if %upcase(&&datamart&i)=SPAN KPSE %then %do; %let excel&i=GA; %end;
%else %if %upcase(&&datamart&i)=SPAN HPRF %then %do; %let excel&i=HP; %end;
%else %if %upcase(&&datamart&i)=SPAN GHS %then %do; %let excel&i=GS; %end;
%else %if %upcase(&&datamart&i)=SPAN DHHA %then %do; %let excel&i=DH; %end;
%else %if %upcase(&&datamart&i)=SPAN EIRH %then %do; %let excel&i=ES; %end;

%else %if %upcase(&&datamart&i)=KPSPAN %then %do; %let excel&i=CO; %end;
%else %if %upcase(&&datamart&i)=KPCO1 %then %do; %let excel&i=GH; %end;
%else %if %upcase(&&datamart&i)=KPCO2 %then %do; %let excel&i=NC; %end;
%else %if %upcase(&&datamart&i)=KPCO3 %then %do; %let excel&i=HI; %end;
%else %if %upcase(&&datamart&i)=KPCO4 %then %do; %let excel&i=GA; %end;
%else %if %upcase(&&datamart&i)=KPCO5 %then %do; %let excel&i=HP; %end;
%else %if %upcase(&&datamart&i)=KPCO6 %then %do; %let excel&i=GS; %end;
%else %if %upcase(&&datamart&i)=KPCO7 %then %do; %let excel&i=DH; %end;
%else %if %upcase(&&datamart&i)=KPCO8 %then %do; %let excel&i=ES; %end;
%else %if %upcase(&&datamart&i)=KPCO9 %then %do; %let excel&i=XX; %end;
%else %if %upcase(&&datamart&i)=KPCO10 %then %do; %let excel&i=XY; %end;
%else %if %upcase(&&datamart&i)=TEST DATAMART %then %do; %let excel&i=HV; %end;
quit;

data &&&excel&i.._waterfall(keep=sample_size);
set waterfall;
run;

%end;

%do i=1 %to &sites;
%do a=1 %to &reports;

data &&&excel&i.._report_&a;
run;

%end;
%end;

%do a=1 %to &reports;
%do i=1 %to &sites;
data site_&&&excel&i.._&a;
datamart="&&excel&i";
%if &&&par_row_&a ne  %then %do;
	%if &&&par_column_&a =  %then %do;
		%if &&&par_group_&a =  %then %do;
		j=1;
		do k=1 to &&&report_rows_&a;
		call symput('k',k);
		excel&i=cats("'&&&excel&i.._report_&a'!$B$",k, "=",'"HIDDEN"',",0,'&&&excel&i.._report_&a'!$B$",k,"+0");
		excel0="IF(";
		output;
		end;
		run;
		%end;
		%else %if &&&par_group_&a ne  %then %do;
		j=1;
		do k=2 to &&&report_rows_&a;
		call symput('k',k);
		excel&i=cats("'&&&excel&i.._report_&a'!$D$",k, "=",'"HIDDEN"',",0,'&&&excel&i.._report_&a'!$D$",k,"+0");
		excel0="IF(";
		output;
		end;
		run;
		%end;
	%end;
	%else %if &&&par_column_&a ne  %then %do;
		%if &&&par_group_&a =  %then %do;
		do j=1 to &&&report_columns_&a;
		do k=2 to &&&report_rows_&a;
		call symput('j',j);
		call symput('k',k);
		excel&i=cats("'&&&excel&i.._report_&a'!$",put(j,XLFMTC.),"$",k, "=",'"HIDDEN"',",0,'&&&excel&i.._report_&a'!$",put(j,XLFMTC.),"$",k,"+0");
		excel0="IF(";
		output;
		end;
		end;
		run;
		%end;
		%else %if &&&par_group_&a ne  %then %do;
		do j=1 to &&&report_columns_&a;
		do k=2 to &&&report_rows_&a;
		call symput('j',j);
		call symput('k',k);
		excel&i=cats("'&&&excel&i.._report_&a'!$",put(j,xlfmtD.),"$",k, "=",'"HIDDEN"',",0,'&&&excel&i.._report_&a'!$",put(j,xlfmtD.),"$",k,"+0");
		excel0="IF(";
		output;
		end;
		end;
		run;
		%end;
	%end;
%end;
%else %if &&&par_row_&a =  %then %do;
	%if &&&par_column_&a =  %then %do;
		j=1;
		do k=1 to &&&report_rows_&a;
		call symput('k',k);
		excel&i=cats("'&&&excel&i.._report_&a'!$B$",k, "=",'"HIDDEN"',",0,'&&&excel&i.._report_&a'!$B$",k,"+0");
		excel0="IF(";
		output;
		end;
		run;
	%end;
	%else %if &&&par_column_&a ne  %then %do;
		do j=1 to &&&report_columns_&a;
		do k=2 to &&&report_rows_&a;
		call symput('j',j);
		call symput('k',k);
		excel&i=cats("'&&&excel&i.._report_&a'!$",put(j,xlfmt.),"$",k, "=",'"HIDDEN"',",0,'&&&excel&i.._report_&a'!$",put(j,xlfmt.),"$",k,"+0");
		excel0="IF(";
		output;
		end;
		end;
		run;
	%end;
%end;

%end;
%end;


%if &sites gt 1 %then %do;
%do a=1 %to &reports;
%let sitelist_&a = ;
%end;
%let excelfunct= ; 

%do a=1 %to &reports;
%do i=1 %to &sites;
%let sitelist_&a=&&&sitelist_&a site_&&&excel&i.._&a.;
%end;
%end;
%do i=1 %to &sites;
%let excelfunct=&excelfunct.trim(EXCEL0)||''||trim(EXCEL&i)||")"||'+'||;
%end;

%do a=1 %to &reports;
data site_merge_&a;
merge
&&&sitelist_&a;
by j k;
run;

data site_cats_&a (keep= datamart j k excel3);
length excel3 $ 5000;
set site_merge_&a;
EXCEL3=cats("'=",&excelfunct."0");
run;
%end;
%end;

%else %do;

%do a=1 %to &reports;
data site_merge_&a;
set site_&excel1._&a;
run;


data site_cats_&a (keep= datamart j k excel3);
set site_merge_&a;
EXCEL3=trim("'="||trim(EXCEL0)||trim(EXCEL1)||")");
run;
%end;
%end;


%do a=1 %to &reports;

%let null_column=0;
data site_cats_&a;
%if %upcase(&&&par_column_&a) ne  %then %do;
length column $256;
%end;
set site_cats_&a;
***columns;
%if %upcase(&&&par_column_&a)=INDEXVARIABLE %then %do;
column=TRANSLATE(CATS("&INDEX_CODE.",TRIM(TRANSLATE(COMPRESS(put(j,INDEX.),'!@#$%^&*():{}[]|\/-'),'_','.'))),'_',' ');
%end;
%else %if %upcase(&&&par_column_&a)=AGE %then %do;
column=put(j,AGE&&option_&a.._.);
%end;
%else %if %upcase(&&&par_column_&a)=BMI %then %do;
column=put(j,BMI.);
%end;
%else %if %upcase(&&&par_column_&a)=YEAR %then %do;
column=put(j,YEAR_.);
%end;
%else %if %upcase(&&&par_column_&a)=GENDER %then %do;
column=put(j,GENDER.);
%end;
%else %if %upcase(&&&par_column_&a)=RACE %then %do;
column=put(j,RACE.);
%end;
%else %if %upcase(&&&par_column_&a)=DX %then %do;
column=TRANSLATE(CATS("dx",TRIM(TRANSLATE(COMPRESS(put(j,INCLDX.),'!@#$%^&*():{}[]|\/-'),'_','.'))),'_',' ');
%end;
%else %if %upcase(&&&par_column_&a)=NODXSELECTED %then %do;
j=1;
column="&&&par_column_&a";
%let null_column=1;
%end;
%else %if %upcase(&&&par_column_&a)=PX %then %do;
column=TRANSLATE(CATS("dx",TRIM(TRANSLATE(COMPRESS(put(j,INCLPX.),'!@#$%^&*():{}[]|\/-'),'_','.'))),'_',' ');
%end;
%else %if %upcase(&&&par_column_&a)=NOPXSELECTED %then %do;
j=1;
column="&&&par_column_&a";
%let null_column=1;
%end;
%else %if %upcase(&&&par_column_&a)=RX %then %do;
column=put(j,INCLRX.);
%end;
%else %if %upcase(&&&par_column_&a)=NORXSELECTED %then %do;
j=1;
column="&&&par_column_&a";
%let null_column=1;
%end;
%else %if %upcase(&&&par_column_&a)=  %then %do;
column="Patient_Count";
%end;
***rows;
%if %upcase(&&&par_row_&a)=INDEXVARIABLE %then %do;
row=put(k,INDEX.);
%end;
%else %if %upcase(&&&par_row_&a)=AGE %then %do;
row=put(k,AGE&&option_&a.._.);
%end;
%else %if %upcase(&&&par_row_&a)=BMI %then %do;
row=put(k,BMI.);
%end;
%else %if %upcase(&&&par_row_&a)=YEAR %then %do;
row=put(k,YEAR_.);
%end;
%else %if %upcase(&&&par_row_&a)=GENDER %then %do;
row=put(k,GENDER.);
%end;
%else %if %upcase(&&&par_row_&a)=RACE %then %do;
row=put(k,RACE.);
%end;
%else %if %upcase(&&&par_row_&a)=DX %then %do;
row=put(k,INCLDX.);
%end;
%else %if %upcase(&&&par_row_&a)=NODXSELECTED %then %do;
row=cats("&&&par_row_&a",k);
%end;
%else %if %upcase(&&&par_row_&a)=PX %then %do;
row=put(k,INCLPX.);
%end;
%else %if %upcase(&&&par_row_&a)=NOPXSELECTED %then %do;
row=cats("&&&par_row_&a",k);
%end;
%else %if %upcase(&&&par_row_&a)=RX %then %do;
row=put(k,INCLRX.);
%end;
%else %if %upcase(&&&par_row_&a)=NORXSELECTED %then %do;
row=cats("&&&par_row_&a",k);
%end;
%else %if %upcase(&&&par_row_&a)=   %then %do;
	%if %upcase(&&&par_group_&a)=INDEXVARIABLE %then %do;
	row=put(k,INDEX.);
	%end;
	%else %if %upcase(&&&par_group_&a)=AGE %then %do;
	row=put(k,AGE&&option_&a.._.);
	%end;
	%else %if %upcase(&&&par_group_&a)=BMI %then %do;
	row=put(k,BMI.);
	%end;
	%else %if %upcase(&&&par_group_&a)=YEAR %then %do;
	row=put(k,YEAR_.);
	%end;
	%else %if %upcase(&&&par_group_&a)=GENDER %then %do;
	row=put(k,GENDER.);
	%end;
	%else %if %upcase(&&&par_group_&a)=RACE %then %do;
	row=put(k,RACE.);
	%end;
	%else %if %upcase(&&&par_group_&a)=DX %then %do;
	row=put(k,INCLDX.);
	%end;
	%else %if %upcase(&&&par_group_&a)=NODXSELECTED %then %do;
	row=cats("&&&par_group_&a",k);
	%end;
	%else %if %upcase(&&&par_group_&a)=PX %then %do;
	row=put(k,INCLPX.);
	%end;
	%else %if %upcase(&&&par_group_&a)=NOPXSELECTED %then %do;
	row=cats("&&&par_group_&a",k);
	%end;
	%else %if %upcase(&&&par_group_&a)=RX %then %do;
	row=put(k,INCLRX.);
	%end;
	%else %if %upcase(&&&par_group_&a)=NORXSELECTED %then %do;
	row=cats("&&&par_group_&a",k);
	%end;
	%else %if %upcase(&&&par_group_&a)=   %then %do;
	row=' ';
	%end;
%end;
run;

proc sort data=site_cats_&a;
by k;
run;

proc transpose data=site_cats_&a out=trans_test&a;
by k;
var excel3;
%if &null_column ne 1 %then %do;
	%if %upcase(&&&par_column_&a)=RX %then %do;
	id j;
	%end;
	%else %if %upcase(&&&par_column_&a)=INDEXVARIABLE %then %do;
		%if &index_code=rx %then %do;
		id j;
		%end;
		%else %do;
		id column;
		%end;
	%end;
	%else %do;
	id column;
	%end;
%end;
run;

%if &null_column =1 %then %do;
data trans_test&a(keep=&&&par_column_&a);
set trans_test&a;
rename col1=&&&par_column_&a;
run;
%end;


data report_row&a (keep=&&&transrow_&a &&&transgroup_&a);
set report_a_&a;
run;

data aggregate_&a (drop=k _NAME_);
set report_row&a;
set trans_test&a;
		array chars _character_;
		do over chars;
		if chars='' then chars='0';
		end;
		array nums _numeric_;
		do over nums;
		if nums='' then nums='0';
		end;
run;

%do i=1 %to &sites;

data &&&excel&i.._report_&a;
set trans_test&a (obs=0);
run;

data &&&excel&i.._report_&a (drop=k _NAME_);
set report_row&a &&&excel&i.._report_&a;
		array chars _character_;
		do over chars;
		if chars='' then chars='0';
		end;
		array nums _numeric_;
		do over nums;
		if nums='' then nums='0';
		end;
run;

%end;

%end;

%mend;

%aggregate;


%macro receipt;

proc sql noprint;
select distinct report into :report1-:report5
from report;

%let reports=&sqlobs;
quit;

data receipt; set drn_query_builder; run;

/* Begin Reciept Processing */

%do i=1 %to 5;
%if %sysfunc(exist(report&i))<= 0 %then %do;
%let par_report&i =  ;
%let par_row&i =  ;
%let par_column&i =  ;
%let par_group&i =  ;
%let par_option&i =  ;
%end;
%else %do;
%let par_report&i = Report&i;
%let par_row&i = &&&par_row_&i;
%let par_column&i = &&&par_column_&i;
%let par_group&i = &&&par_group_&i;
%let par_option&i = &&&option_&i;
%end;
%end;

%if &index_code = dx %then %do; %let par_index = DX; 
%let par_index_cd= &index_dx; 
%LET par_index_bool = &index_dx_bool; 
%end;
%if &index_code = px %then %do; %let par_index = PX; 
%let par_index_cd= &index_px; 
%LET par_index_bool = &index_px_bool; 
%end;
%if &index_code = rx %then %do; 
%let par_index_cd= &index_rx; 
%let par_index = RX; 
%LET par_index_bool = &index_rx_bool; 
%end;

%if &index_code = BMI %then %do; 
%let par_index = BMI; 
%let par_index_cd= BMI; 
%LET par_index_bool = &index_bmi_var group &index_bmi_grp; 
%end;
%if &index_code = age %then %do; 
%let par_index = AGE;
%let par_index_cd= AGE; 
%LET par_index_bool = &index_age_op&index_age as of &age_index_date; 
%end;
quit;

proc sql noprint;

create table query_receipt as 
select distinct "&query_name" as query_name
	 , "&query_type" as query_type
	 , "&query_desc" as query_desc
	 , "&email" as email
	 , "&begdate" as observation_start
	 , "&enddate" as observation_end
	 , "&enroll_cont" as cont_enrollment
	 , "&enroll_prior" as enroll_prior
	 , "&enroll_post" as enroll_post
	 , "&par_index" as Index_Variable
	 , "&par_index_cd" as index_dx_codes
	 , "&par_index_bool" as index_bool
%if %sysfunc(exist(incl_dx_code)) ne 0 %then %do; 
%let dsn=%upcase(incl_dx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs gt 1 %then %do;
, "&incl_dx" as dx_inclusion_codes
, "&incl_dx_bool" as dx_inclusion_bool
%end;
%else %do; 
, ' ' as dx_inclusion_codes
, ' ' as dx_inclusion_bool
%end;
%end;
%if %sysfunc(exist(incl_px_code)) ne 0 %then %do; 
%let dsn=%upcase(incl_px_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs gt 1 %then %do;
, "&incl_px" as px_inclusion_codes
, "&incl_px_bool" as px_inclusion_bool
%end;
%else %do; 
, ' ' as px_inclusion_codes
, ' ' as px_inclusion_bool
%end;
%end;
%if %sysfunc(exist(incl_rx_code)) ne 0 %then %do; 
%let dsn=%upcase(incl_rx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs gt 1 %then %do;
, "&incl_rx" as rx_inclusion_codes
, "&incl_rx_bool" as rx_inclusion_bool
%end;
%else %do; 
, ' ' as rx_inclusion_codes
, ' ' as rx_inclusion_bool
%end;
%end;
%if %sysfunc(exist(excl_dx_code)) ne 0 %then %do; 
%let dsn=%upcase(excl_dx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs gt 1 %then %do;
, "&excl_dx" as dx_exclusion_codes
, "&excl_dx_bool" as dx_exclusion_bool
%end;
%else %do; 
, ' ' as dx_exclusion_codes
, ' ' as dx_exclusion_bool
%end;
%end;
%if %sysfunc(exist(excl_px_code)) ne 0 %then %do; 
%let dsn=%upcase(excl_px_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs gt 1 %then %do;
, "&excl_px" as px_exclusion_codes
, "&excl_px_bool" as px_exclusion_bool
%end;
%else %do; 
, ' ' as px_exclusion_codes
, ' ' as px_exclusion_bool
%end;
%end;
%if %sysfunc(exist(excl_rx_code)) ne 0 %then %do; 
%let dsn=%upcase(excl_rx_code);
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs gt 1 %then %do;
, "&excl_rx" as rx_exclusion_codes
, "&excl_rx_bool" as rx_exclusion_bool
%end;
%else %do; 
, ' ' as rx_exclusion_codes
, ' ' as rx_exclusion_bool
%end;
%end;
%if %sysfunc(exist(excl_age_var)) ne 0 %then %do; 
%let dsn=%upcase(exist(excl_age_var));
%let dsid=%sysfunc(open(&dsn));
%let nobs= %sysfunc(attrn(&dsid,nlobs));
%let dsidc=%sysfunc(close(&dsid));
%if &nobs gt 1 %then %do;
, "&age" as age_exclusion
, "&age_op"  as age_excl_operator
%end;
%else %do; 
, ' ' as age_exclusion
, ' ' as age_excl_operator
%end;
%end;
, compress("&sitelist","_wf"," ") as datamarts
,"&par_report1" as report1
,"&par_row1" as row1
,"&par_column1" as column1
,"&par_group1" as group1
,"&par_option1" as option1
,"&par_report2" as report2
,"&par_row2" as row2
,"&par_column2" as column2
,"&par_group2" as group2
,"&par_option2" as option2
,"&par_report3" as report3
,"&par_row3" as row3
,"&par_column3" as column3
,"&par_group3" as group3
,"&par_option3" as option3
,"&par_report4" as report4
,"&par_row4" as row4
,"&par_column4" as column4
,"&par_group4" as group4
,"&par_option4" as option4
,"&par_report5" as report5
,"&par_row5" as row5
,"&par_column5" as column5
,"&par_group5" as group5
,"&par_option5" as option5
from receipt;
quit;

/*REPORT */

/* Report Macro */

data report_receipt; run;

%do i=1 %to &reports;
data report_receipt;
set report_receipt report&i;

report&i = cats("report","&i");
row&i = "&&par_row_&i";
column&i = "&&par_column_&i";
group&i = "&&par_group_&i";
option&i = "&&option_&i";
run;
%end;

data query_receipt2 (keep= parameter values);
set query_receipt;
length parameter $50 values $2000;
parameter = "Query Name";
values = query_name; output;
parameter = "Query Type";
values = query_type; output;
parameter = "Query Description";
values = query_desc; output;
parameter = "Email";
values = email; output;
parameter = "Observation Period Start Date";
values = observation_start; output;
parameter = "Observation Period End Date";
values = observation_end; output;
parameter = "Continuous Enrollment Flag";
values = upcase(cont_enrollment); output;
parameter = "Months Prior Enrollment Range";
values = enroll_prior; output;
parameter = "Months Post Enrollment Range";
values = enroll_post; output;
parameter = "Index Variable";
values = upcase(index_Variable); output;
parameter = "Index Variable Values";
values = index_dx_codes; output;
parameter = "Index Bool Operator";
values = index_bool; output;
parameter = "ICD Inclusion Dx Codes";
values = dx_inclusion_codes; output;
parameter = "ICD Inclusion Dx Bool Operator";
values = dx_inclusion_bool; output;
parameter = "ICD/CPT Inclusion Px Codes";
values = px_inclusion_codes; output;
parameter = "ICD/CPT Inclusion Px Bool Operator";
values = px_inclusion_bool; output;
parameter = "Generic Name Inclusion Rx Codes";
values = rx_inclusion_codes; output;
parameter = "Generic Name Inclusion Rx Bool Operators";
values = rx_inclusion_bool; output;
parameter = "ICD Exclusion Dx Codes";
values = dx_exclusion_codes; output;
parameter = "ICD Exclusion Dx Bool Operator";
values = dx_exclusion_bool; output;
parameter = "ICD/CPT Exclusion Px Codes";
values = px_exclusion_codes; output;
parameter = "ICD/CPT Exclusion Px Bool Operator";
values = px_exclusion_bool; output;
parameter = "Generic Name Exclusion Rx Codes";
values = rx_exclusion_codes; output;
parameter = "Generic Name Exclusion Rx Bool Operator";
values = rx_exclusion_bool; output;
parameter = "Age Exclusion Value";
values = age_exclusion; output;
parameter = "Age Exclusion Operator";
values = age_excl_operator; output;
parameter= "SPAN DataMarts Requested";
values = datamarts; output;
parameter = "Report 1";
values = report1; output;
parameter = "Row 1";
values = row1; output;
parameter = "Column 1";
values = column1; output;
parameter = "Group 1";
values = group1; output;
parameter = "Report 2";
values = report2; output;
parameter = "Row 2";
values = row2; output;
parameter = "Column 2";
values = column2; output;
parameter = "Group 2";
values = group2; output;
parameter = "Report 3";
values = report3; output;
parameter = "Row 3";
values = row3; output;
parameter = "Column 3";
values = column3; output;
parameter = "Group 3";
values = group3; output;
parameter = "Report 4";
values = report4; output;
parameter = "Row 4";
values = row4; output;
parameter = "Column 4";
values = column4; output;
parameter = "Group 4";
values = group4; output;
parameter = "Report 5";
values = report5; output;
parameter = "Row 5";
values = row5; output;
parameter = "Column 5";
values = column5; output;
parameter = "Group 5";
values = group5; output;
run;

/* Output to Excel */

/* Total Aggregrate table reporting */
proc sql noprint;
select distinct datamart into :datamart1-:datamart100
from datamart;

%let sites=&sqlobs;
quit;

%do i=1 %to &sites;

%if %upcase(&&datamart&i)=SPAN KPCO %then %do; %let excel&i=CO; %end;
%else %if %upcase(&&datamart&i)=SPAN HPHC %then %do; %let excel&i=HV; %end;
%else %if %upcase(&&datamart&i)=SPAN GHC %then %do; %let excel&i=GH; %end;
%else %if %upcase(&&datamart&i)=SPAN KPNW %then %do; %let excel&i=NW; %end;
%else %if %upcase(&&datamart&i)=SPAN KPNC %then %do; %let excel&i=NC; %end;
%else %if %upcase(&&datamart&i)=SPAN KPHI %then %do; %let excel&i=HI; %end;
%else %if %upcase(&&datamart&i)=SPAN KPSE %then %do; %let excel&i=GA; %end;
%else %if %upcase(&&datamart&i)=SPAN HPRF %then %do; %let excel&i=HP; %end;
%else %if %upcase(&&datamart&i)=SPAN GHS %then %do; %let excel&i=GS; %end;
%else %if %upcase(&&datamart&i)=SPAN DHHA %then %do; %let excel&i=DH; %end;
%else %if %upcase(&&datamart&i)=SPAN EIRH %then %do; %let excel&i=ES; %end;

%else %if %upcase(&&datamart&i)=KPSPAN %then %do; %let excel&i=CO; %end;
%else %if %upcase(&&datamart&i)=KPCO1 %then %do; %let excel&i=XX; %end;
%else %if %upcase(&&datamart&i)=KPCO2 %then %do; %let excel&i=XX; %end;
%else %if %upcase(&&datamart&i)=TEST DATAMART %then %do; %let excel&i=HV; %end; %end;


%if %upcase(&sitecode) = NW %then %do;

libname myxls excel "&&current_query_folder.&slash.&SiteCode._&QUERY_NAME Results.XLS" ;
data myxls.waterfall;
set waterfall;
run;

%do i=1 %to &reports;
data myxls.&SiteCode._report_&i;
set report_a_&i;
run;
%end;

data myxls.&SiteCode._query_receipt;
set query_receipt2;
run;
LIBNAME MYXLS CLEAR;

%do i=1 %to &reports;
data upload__.&SiteCode._report_&i;
set report_a_&i;
run;
%end;

data upload__.&SiteCode._query_receipt;
set query_receipt2;
run;

libname myxls excel "&&current_query_folder.&slash.Aggregate_Tables_&QUERY_NAME Results.XLS" ;
data myxls.aggregate_waterfall;
set aggregate_waterfall;
run;
%do i=1 %to &sites;
%do a=1 %to &reports;

data myxls.Total_report_&a;
set Aggregate_&a;
run;

data myxls.&&&excel&i.._report_&a;
set &&&excel&i.._report_&a;
data myxls.&&&excel&i.._waterfall;
set &&&excel&i.._waterfall;
run;

%end;
%end;
data myxls.Total_query_receipt;
set query_receipt2;
run;
LIBNAME MYXLS CLEAR;

%end;


%else %if %upcase(&sitecode) ne NW %then %do;

ods tagsets.ExcelXP file="&&current_query_folder.&slash.&SiteCode._&QUERY_NAME Results.XLS" 
					style=statdoc options (sheet_name = "&SiteCode._waterfall");
proc print data=waterfall;  run;
%do i=1 %to &reports;
ods tagsets.ExcelXP style=statdoc options (sheet_name = "&SiteCode._report_&i");
proc print data=report_a_&i; run; 
%end;
ods tagsets.ExcelXP style=statdoc options (sheet_name = "&SiteCode._query_receipt");
proc print data=query_receipt2; run; 
ods tagsets.ExcelXP close;

data upload__.&SiteCode._waterfall;
set waterfall;
run;
%do i=1 %to &reports;
data upload__.&SiteCode._report_&i;
set report_a_&i;
run;
%end;
data upload__.&SiteCode._query_receipt;
set query_receipt2;
run;

ods tagsets.ExcelXP file="&&current_query_folder.&slash.Aggregate_Tables_&QUERY_NAME Results.XLS" 
					style=statdoc options (sheet_name = "Total_waterfall" width_fudge='0.8');
proc print data=aggregate_waterfall;  run;
%do a=1 %to &reports;
ods tagsets.ExcelXP style=statdoc options (sheet_name = "Total_report_&a" width_fudge='0.8');
proc print data=Aggregate_&a; run;
%end;
%do i=1 %to &sites;
%do a=1 %to &reports;
ods tagsets.ExcelXP style=statdoc options (sheet_name = "&&&excel&i.._report_&a");
proc print data=&&&excel&i.._report_&a; run;
%end;
%end;
%do i=1 %to &sites;
ods tagsets.ExcelXP style=statdoc options (sheet_name = "&&&excel&i.._waterfall");
proc print data=&&&excel&i.._waterfall; run;
%end;
ods tagsets.ExcelXP style=statdoc options (sheet_name = "Total_query_receipt");
proc print data=query_receipt2; run;
ods tagsets.ExcelXP close;


%end;

%mend;

%receipt;

proc printto; run;

/******************************************************************************************************/
/****************************************** END PROGRAM ***********************************************/
/******************************************************************************************************/

