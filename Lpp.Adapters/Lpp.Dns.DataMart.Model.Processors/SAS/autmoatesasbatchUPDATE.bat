@ECHO OFF

:: Initialize parameter values with appropriate pathnames
SET SASEXE=C:\Program Files\SAS92\SASFoundation\9.2\sas.exe 
SET WZZIP=C:\Program Files\WinZip\WZZIP.EXE
SET EditSection=\\rmlhofile001\users\M426654\My SPAN Files\EditSection.sas
SET Current_Query_Libname=\\rmlhofile001\users\M426654\My SPAN Files\current_query_folder.sas

:: Set to Y if you want to include the log file in the zip file UPLOAD_queryid.zip
SET ZIPLOG=Y


::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
::NO EDITS BELOW HERE
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


:: Remove files from previous runs if they exist
IF EXIST "%~2\UPLOAD*.ZIP" DEL "%~2\UPLOAD*.ZIP"
IF EXIST "%~2\Result.txt" DEL "%~2\Result.txt"
IF EXIST "%~2\*.LOG" DEL "%~2\*.LOG"


IF EXIST "%~2\QueryBuilder.SAS" ECHO !!!!! The statement resolves to: IF EXIST "%~2\QueryBuilder.SAS" SET QB=Y > "\\rmlhofile001\users\M426654\My SPAN Files\CheckMyParameters.txt"


:: Create the UPLOAD__ library under the current query folder
ECHO %%let current_query_folder=%~2 ; > "%Current_Query_Libname%"
ECHO libname UPLOAD__ "&current_query_folder" ; >> "%Current_Query_Libname%"

:: Submit program to SAS
START/w "" "%SASEXE%" -config "\\rmlhofile001\users\M426654\My SPAN Files\sasv9.cfg" -sysin %1 -AUTOEXEC "%EditSection%" -LOG %2 -PRINT %2 -nosplash -logparm "rollover=session"

:: If QuerBuilder zip but do not encrypt
IF EXIST "%~2\QueryBuilder.SAS" (
  IF %ZIPLOG%==Y (
    DIR %2/B/A-D > "%~2\ReturnedFiles.txt"
    START/w "" "%WZZIP%" -x*.sas7bdat -x*.xml -x*.map -x*.txt -x*.sas -x*.log -x*.lst -m "%~2\UPLOAD_%~nx2.ZIP" "%~2\*.*"
  ) ELSE (
    DIR %2/B/A-D | findstr /vi ".log" > "%~2\ReturnedFiles.txt"
    START/w "" "%WZZIP%" -x*.sas7bdat -x*.xml -x*.map -x*.txt -x*.sas -x*.log -x*.lst -m "%~2\UPLOAD_%~nx2.ZIP" "%~2\*.*"
  ) 
) ELSE (
  IF %ZIPLOG%==Y (
    DIR %2/B/A-D > "%~2\ReturnedFiles.txt"
    START/w "" "%WZZIP%" -m -s -ycAES256 "%~2\UPLOAD_%~nx2.ZIP" "%~2\*.*"
  ) ELSE (
    DIR %2/B/A-D | findstr /vi ".log" > "%~2\ReturnedFiles.txt"
    START/w "" "%WZZIP%" -x*.log -m -s -ycAES256 "%~2\UPLOAD_%~nx2.ZIP" "%~2\*.*"
  )
)
:: When WinZip finishes, write Result.txt which indicates to the Data Mart Client Tool that the job is ready to upload.
ECHO QUERY COMPLETED > "%~2\Result.txt"
@ECHO ON

