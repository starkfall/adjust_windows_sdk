# Adjust SDK .NET Standard 2.1

This directory contains the Adjust SDK converted from the original Portable Class Library (PCL) to .NET Standard 2.1.

## Conversion Details

### What was converted:
- **Original**: PCL targeting `.NET Framework 4.5` with `Profile259` (portable-net45+win8+wp8+wpa81)
- **New**: .NET Standard 2.1

### Changes made:
1. **Project file**: Converted from old MSBuild format to modern SDK-style format
2. **Target framework**: Changed from `net45+win8+wp8+wpa81` to `netstandard2.1`
3. **Namespace**: Updated from `AdjustSdk.Pcl` to `AdjustSdk.NetStandard`
4. **Dependencies**: 
   - Removed Microsoft.Bcl packages (no longer needed in .NET Standard 2.1)
   - Updated Newtonsoft.Json from 7.0.1 to 13.0.3
   - System.Net.Http is now built into .NET Standard 2.1

### Benefits of .NET Standard 2.1:
- Better API coverage (many APIs that required BCL packages are now built-in)
- Modern tooling support
- Wider platform compatibility
- Better performance
- Access to latest security updates

### Building:
```bash
dotnet build AdjustNetStandard.csproj
```

### Output:
The project builds to `bin/Debug/netstandard2.1/AdjustNetStandard.dll`

## Original Source
This was converted from the `WindowsPcl` directory in the parent project.
