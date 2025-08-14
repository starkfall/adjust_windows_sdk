@echo off
echo Building AdjustWinUI3 solution...

REM Build the solution in Release configuration
echo Building solution in Release configuration...
dotnet build AdjustWinUI3.sln --configuration Release --verbosity normal

if %ERRORLEVEL% neq 0 (
    echo Build failed with exit code %ERRORLEVEL%
    pause
    exit /b %ERRORLEVEL%
)

echo Build completed successfully!

REM Check if NuGet.exe exists, if not download it
if not exist "nuget.exe" (
    echo NuGet.exe not found. Downloading...
    powershell -Command "Invoke-WebRequest -Uri 'https://dist.nuget.org/win-x86-commandline/latest/nuget.exe' -OutFile 'nuget.exe'"
    if %ERRORLEVEL% neq 0 (
        echo Failed to download NuGet.exe
        pause
        exit /b 1
    )
    echo NuGet.exe downloaded successfully!
)

REM Create NuGet package
echo Creating NuGet package...
nuget pack WindowsStoreWinUI3.nuspec -OutputDirectory . -Verbosity normal

if %ERRORLEVEL% neq 0 (
    echo NuGet package creation failed with exit code %ERRORLEVEL%
    pause
    exit /b %ERRORLEVEL%
)

echo NuGet package created successfully!
echo Package location: %CD%

REM List created packages
echo.
echo Created packages:
for %%f in (*.nupkg) do echo   %%f

pause
