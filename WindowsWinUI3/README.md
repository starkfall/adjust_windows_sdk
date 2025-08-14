# WindowsWinUI3

This project is a WinUI3 conversion of the original WindowsUap project. It has been updated to use modern .NET 8 and WinUI3 technologies.

## Key Changes

- **Target Framework**: Updated from `net4.6` to `net8.0-windows10.0.18362.0`
- **UI Framework**: Migrated from UWP to WinUI3
- **Dependencies**: Now uses `AdjustNetStandard` project instead of `WindowsPCL`
- **Namespace**: Changed from `AdjustSdk.Uap` to `AdjustSdk.WinUI3`
- **Dispatcher**: Updated from `CoreDispatcher` to `DispatcherQueue`

## Requirements

- Windows 10 version 18362.0 or later
- .NET 8 SDK
- Windows App SDK 1.4 or later
- Visual Studio 2022 17.8 or later with WinUI3 workload

## Project Structure

- `WindowsWinUI3.csproj` - Main project file with WinUI3 configuration
- `UtilWinUI3.cs` - Utility class with WinUI3-specific implementations
- `SystemInfoEstimate.cs` - System information estimation utilities
- `Properties/AssemblyInfo.cs` - Assembly metadata

## Dependencies

- `AdjustNetStandard` - Core Adjust SDK functionality
- `Microsoft.WindowsAppSDK` - Windows App SDK runtime
- `Microsoft.Windows.SDK.BuildTools` - Windows SDK build tools

## Migration Notes

The main changes from the original WindowsUap project:

1. **Dispatcher API**: Changed from `CoreDispatcher.RunAsync()` to `DispatcherQueue.TryEnqueue()`
2. **Namespace Updates**: All references updated to use `AdjustSdk.NetStandard` instead of `AdjustSdk.Pcl`
3. **Modern SDK**: Uses the latest Windows App SDK and .NET 8
4. **WinUI3 Support**: Full WinUI3 compatibility with modern Windows development patterns

## Building

To build this project:

1. Ensure you have the required SDKs installed
2. Open the solution in Visual Studio 2022
3. Restore NuGet packages
4. Build the project

The project will generate a WinUI3-compatible assembly that can be used in modern Windows applications.
