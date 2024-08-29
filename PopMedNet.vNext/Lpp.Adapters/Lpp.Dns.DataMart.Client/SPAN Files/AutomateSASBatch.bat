@ECHO OFF

:: Initialize parameter values with appropriate pathnames
SET SASEXE=<pathname>\sas.exe
SET WZZIP=<pathname>\WZZIP.EXE
SET EditSection=<pathname>\EditSection.sas
SET Current_Query_Libname=<pathname>\current_query_folder.sas

:: Set to Y if you want to include the log file in the zip file UPLOAD_queryid.zip
SET ZIPLOG=N


::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
::NO EDITS BELOW HERE
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::


:: Remove files from previous runs if they exist
IF EXIST "%~2\UPLOAD*.ZIP" DEL "%~2\UPLOAD*.ZIP"
IF EXIST "%~2\Result.txt" DEL "%~2\Result.txt"
IF EXIST "%~2\*.LOG" DEL "%~2\*.LOG"

:: Create the UPLOAD__ library under the current query folder
ECHO %%let current_query_folder=%~2 ; > "%Current_Query_Libname%"
ECHO libname UPLOAD__ "&current_query_folder" ; >> "%Current_Query_Libname%"

:: Submit program to SAS
START/w "" "%SASEXE%" -sysin %1 -AUTOEXEC "%EditSection%" -LOG %2 -PRINT %2 -nosplash -logparm "rollover=session"

:: When SAS finishes, list the files to be zipped in the output folder
IF %ZIPLOG%==Y (
DIR %2/B/A-D > "%~2\ReturnedFiles.txt"
) ELSE (
DIR %2/B/A-D | findstr /vi ".log" > "%~2\ReturnedFiles.txt"
)

:: After Result.txt is created, zip all files (m=move, s=secure password) except possibly the log file
IF %ZIPLOG%==Y (
START/w "" "%WZZIP%" -m -s -ycAES256 "%~2\UPLOAD_%~nx2.ZIP" "%~2\*.*"
) ELSE (
START/w "" "%WZZIP%" -x*.log -m -s -ycAES256 "%~2\UPLOAD_%~nx2.ZIP" "%~2\*.*"
)

:: When WinZip finishes, write Result.txt which indicates to the Data Mart Client Tool that the job is ready to upload.
ECHO QUERY COMPLETED > "%~2\Result.txt"


@ECHO ON

