=========================================
SPAN Supporting Files Documentation
=========================================

This document explains the purpose of the following SPAN Supporting Files:

1) AUTOMATESASBATCH.BAT
2) EDITSECTION.SAS
3) STDVARS.SAS
4) CURRENT_QUERY_FOLDER.SAS


=========================================
IMPORTANT INSTALLATION INFORMATION
=========================================
The DataMart Client Application Setup Program installs the SPAN Supporting Files.
The SPAN Supporting Files must be edited to include valid local pathnames.
Right-click the file and select Edit to open any of the supporting files in NotePad.

1) The following statements in AUTOMATESASBATCH.BAT must be edited:

   a) SET SASEXE=<pathname>\SAS.EXE
      Replace <pathname> with the valid path to SAS.EXE.  This tells the DataMart 
      Client Application where to find SAS.

   b) SET WZZIP=<pathname>\WZZIP.EXE
      Replace <pathname> with the valid path to WZZIP.EXE.  This tells the 
      DataMart Client Application where to find WinZip Command Line.

   c) SET EditSection=<pathname>\EDITSECTION.SAS
      Replace <pathname> with the valid path to EDITSECTION.SAS.  EDITSECTION.SAS 
      is one of the SPAN Supporting Files.

   d) SET Current_Query_Libname=<pathname>\CURRENT_QUERY_FOLDER.SAS
      Replace <pathname> with the valid path to CURRENT_QUERY_FOLDER.SAS.  
      CURRENT_QUERY_FOLDER.SAS is one of the SPAN Supporting Files.

   e) SET ZIPLOG=N
      Leave as is if you DO NOT want to include log files in the encrypted zip
      file.  Your log file will remain unencrypted locally.  Change to ZIPLOG=Y
      if you DO want to include log files in the encrypted zip file.

2) The following statements in EDITSECTION.SAS must be edited:

   a) %INCLUDE '<pathname>\STDVARS.SAS';
      Replace <pathname> with the valid path to STDVARS.SAS.  STDVARS.SAS is one 
      of the SPAN Supporting Files.

   b) %INCLUDE '<pathname>\CURRENT_QUERY_FOLDER.SAS';
      Replace <pathname> with the valid path to CURRENT_QUERY_FOLDER.SAS.  
      CURRENT_QUERY_FOLDER.SAS is one of the SPAN Supporting Files.

3) The following statements in STDVARS.SAS must be edited:

   a) %let _SiteCode =  <your site code (same as in VDW StdVars)>;
      Replace <your site code> with your site's code as given in your VDW 
      StdVars.sas.

   b) %let _SiteAbbr =  <your site abbreviation (same as in VDW StdVars)>;
      Replace <your site abbreviation> with your site's abbreviation as given in 
      your VDW StdVars.sas.

   c) %let _SiteName =  <your site name (same as in VDW StdVars)>;
      Replace <your site name> with your site's name as given in your VDW 
      StdVars.sas.

   d) %let adhd_datamart=<pathname to your SPAN ADHD datamart>;
      Replace <pathname to your SPAN ADHD datamart> with the valid path to the 
      SPAN ADHD datamart tables.

   e) %let obesity_datamart=<pathname to your SPAN Obesity datamart>;
      Replace <pathname to your SPAN Obesity datamart> with the valid path to the 
      SPAN Obesity datamart tables.

   f) %let module3=<pathname to your sandbox area for SPAN Module 3 data sets>;
      Replace <pathname to your sandbox area for SPAN Module 3 data sets> with 
      the valid path to a local memory storage area where an analyst can place 
      SAS data sets.

4) CURRENT_QUERY_FOLDER.SAS does not need to be edited.  It is updated 
   automatically by AUTOMATESASBATCH.BAT.


=========================================
DESCRIPTION OF FILES AND PROCESS FLOW
=========================================
The SPAN Supporting Files allow the DataMart Client Application to sequentially 
launch SAS and WinZip.  

The following sketch shows program references among SPAN Supporting Files:

DATAMARTCLIENT.EXE
|
---AUTOMATESASBATCH.BAT
   |
   ---CURRENT_QUERY_FOLDER.SAS
   |
   ---EDITSECTION.SAS
      |
      ---STDVARS.SAS
      |
      ---CURRENT_QUERY_FOLDER.SAS

1) DATAMARTCLIENT.EXE starts a SAS Query by launching AUTOMATESASBATCH.BAT.  It
   passes two parameters to AUTOMATESASBATCH.BAT:
   a) %1 is the full path and file name of the SAS program to be executed.  The
      SAS program is placed into a folder created by the DataMart Client
      Application at runtime.  
      Example:  %1 = "C:\TEMP\1_51_418\MySASprogram.sas"
   b) %2 is the full path of the output folder.  
      Example:  %2 = C:\TEMP\1_51_418

2) AUTOMATESASBATCH.BAT coordinates the execution of a SAS Query in this order:

   a) Delete old versions of UPLOAD*.ZIP, RESULT.TXT, and *.LOG if they exist.
      See step 2.e and 2.f.

   b) Create SAS code that will create a SAS library called UPLOAD__ once it is 
      read into SAS.  UPLOAD__ points to the current query folder.  SAS code 
      submitted through the SPAN system can retrieve data from data partners 
      by writing data sets to UPLOAD__.

   c) Launch SAS as a batch session.  Execute the SAS commands in EDITSECTION.SAS
      followed by the SAS commands in the SAS program sent to the DataMart Client
      Application.  Output log and list files to the current query folder.

   d) Create a text file called ReturnedFiles.txt which lists all files in the 
      current query folder except log files if ZIPLOG=N.  Log files are also 
      listed if ZIPLOG=Y.

   e) Create an encrypted zip file and move all files in the current query folder
      to the zip file except log files if ZIPLOG=N.  Log files are also moved to
      the zip file if ZIPLOG=Y.  The zip file follows the naming convention 
      UPLOAD_queryid, where queryid is the name of the current folder, eg, 1_51_418.

   f) Create a file called Result.txt with the phrase QUERY COMPLETED to send a 
      signal to the DataMart Client Application that the query is ready to upload.

3) CURRENT_QUERY_FOLDER.SAS is a scratchpad to transfer the pathname of the 
   current query folder from the DataMart Client Application to SAS.

4) EDITSECTION.SAS is auto-executed before any other SAS code in the batch session.
   At a minimum, it contains the pathnames to the STDVARS.SAS and 
   CURRENT_QUERY_FOLDER.SAS files.  It can also be used to specify other local 
   pathnames, if these are required for a given workplan.

5) STDVARS.SAS is a site-modified program that specifies a set of standard macro
   variables for things that vary by site.  The macro parameters typically
   include site identifiers and pathnames to local SAS data sets.  It is basically
   the same as the VDW STDVARS.SAS except that it points to SPAN datamart tables
   instead of VDW tables.
