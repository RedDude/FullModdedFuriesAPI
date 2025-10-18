@echo off

REM make sure we're not running within a zip folder
echo "%~dp0" | findstr /C:"%TEMP%" 1>nul
if %ERRORLEVEL% EQU 0 (
    echo "Oops! It looks like you're running the installer from inside a zip file. Make sure you unzip the download first."
    pause
    exit
)

WHERE dotnet /q
if %ERRORLEVEL% NEQ 0 (
    echo "Oops! You must have .NET 4.5 (desktop x64) installed to use FMODF: https://dotnet.microsoft.com/download/dotnet/5.0/runtime"
    pause
    exit
)

REM make sure an antivirus hasn't deleted the install DLL
if not exist "internal\install.exe" (
    echo "Oops! FMODF can't find its 'internal\install.exe' file. Your antivirus might have deleted the file."
    pause
    exit
)

REM start installer
dotnet "internal\install.exe"
