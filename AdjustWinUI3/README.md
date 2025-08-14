# AdjustWinUI3 Solution

This solution is designed to build NuGet packages from the WindowsStoreWinUI3 project, which provides the Adjust SDK for Windows Store applications using the WinUI3 framework.

## Solution Structure

- **WindowsStoreWinUI3** - Main WinUI3 project containing the Adjust SDK implementation
- **NetStandard2.1** - Core Adjust SDK library referenced by the WinUI3 project

## Prerequisites

- Visual Studio 2019 or later
- .NET 8.0 SDK
- Windows 10 SDK (10.0.18362.0 or later)
- Windows App SDK

## Building the Solution

### Using Visual Studio

1. Open `AdjustWinUI3.sln` in Visual Studio
2. Set the build configuration to `Release`
3. Build the solution (Build → Build Solution)

### Using Command Line

```bash
dotnet build AdjustWinUI3.sln --configuration Release
```

## Creating NuGet Packages

### Automatic Build Script

Run the PowerShell build script to automatically build the solution and create the NuGet package:

```powershell
.\build-nuget.ps1
```

### Manual NuGet Package Creation

1. Build the solution in Release configuration
2. Use NuGet.exe to pack the .nuspec file:

```bash
nuget pack WindowsStoreWinUI3.nuspec
```

## Package Information

- **Package ID**: AdjustSdk.WindowsStoreWinUI3
- **Target Framework**: net8.0-windows10.0.18362.0
- **Dependencies**: 
  - Microsoft.WindowsAppSDK (1.4.231115000)
  - Microsoft.Windows.SDK.BuildTools (10.0.22621.2428)

## Output

The build process will generate:
- Compiled assemblies in the `bin/Release` directories
- NuGet package (.nupkg) file in the solution directory

## Notes

- The solution is configured to support multiple platforms (x86, x64, ARM64)
- The WinUI3 project uses the latest C# language features
- MSIX packaging is enabled for Windows Store deployment
