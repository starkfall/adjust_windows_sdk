# WindowsStoreWinUI3

This is the WinUI3 version of the Adjust SDK for Windows Store applications.

## Target Framework

- **Target Framework**: `net8.0-windows10.0.18362.0`
- **Minimum Windows Version**: 10.0.18362.0 (Windows 10 version 1903)

## Dependencies

- **AdjustNetStandard**: Core Adjust SDK functionality
- **Microsoft.WindowsAppSDK**: WinUI3 runtime and controls
- **Microsoft.Windows.SDK.BuildTools**: Windows SDK build tools

## Supported Platforms

- x86
- x64  
- ARM64

## Key Changes from WindowsStore

1. **Updated Target Framework**: Now targets .NET 8.0 with WinUI3 support
2. **Dependency Changes**: 
   - Uses `AdjustNetStandard` instead of `WindowsPCL` and `WindowsUap`
   - Updated to use WinUI3 APIs (`Microsoft.UI.Xaml` instead of `Windows.UI.Xaml`)
3. **Dispatcher Changes**: Uses `DispatcherQueue` instead of `CoreDispatcher`
4. **Namespace**: Updated to `AdjustSdk.WindowsStoreWinUI3`

## Usage

Include this project in your WinUI3 application to use the Adjust SDK with modern Windows development technologies.

## Building

This project requires:
- Visual Studio 2022 17.8 or later
- Windows 10 SDK 10.0.18362.0 or later
- .NET 8.0 SDK
