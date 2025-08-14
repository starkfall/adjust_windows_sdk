# Build script for AdjustWinUI3 NuGet package
# This script builds the solution and creates a NuGet package

Write-Host "Building AdjustWinUI3 solution..." -ForegroundColor Green

# Build the solution in Release configuration
Write-Host "Building solution in Release configuration..." -ForegroundColor Yellow
dotnet build AdjustWinUI3.sln --configuration Release --verbosity normal

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "Build completed successfully!" -ForegroundColor Green

# Check if NuGet.exe exists, if not download it
$nugetPath = "nuget.exe"
if (-not (Test-Path $nugetPath)) {
    Write-Host "NuGet.exe not found. Downloading..." -ForegroundColor Yellow
    Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile $nugetPath
    Write-Host "NuGet.exe downloaded successfully!" -ForegroundColor Green
}

# Create NuGet package
Write-Host "Creating NuGet package..." -ForegroundColor Yellow
& $nugetPath pack WindowsStoreWinUI3.nuspec -OutputDirectory . -Verbosity normal

if ($LASTEXITCODE -ne 0) {
    Write-Host "NuGet package creation failed with exit code $LASTEXITCODE" -ForegroundColor Red
    exit $LASTEXITCODE
}

Write-Host "NuGet package created successfully!" -ForegroundColor Green
Write-Host "Package location: $(Get-Location)" -ForegroundColor Cyan

# List created packages
Write-Host "`nCreated packages:" -ForegroundColor Cyan
Get-ChildItem -Filter "*.nupkg" | ForEach-Object { Write-Host "  $($_.Name)" -ForegroundColor White }
