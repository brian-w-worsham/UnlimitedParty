<#
.SYNOPSIS
    Build and deploy UnlimitedParty mod to Bannerlord Modules folder.
.DESCRIPTION
    Builds the project and copies all module files to the game's Modules directory.
.PARAMETER GameFolder
    Path to your Bannerlord installation. Defaults to Steam's default location.
.PARAMETER Configuration
    Build configuration. Default: Release.
#>
param(
    [string]$GameFolder = "C:\Games\steamapps\common\Mount & Blade II Bannerlord",
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

$ModuleName = "UnlimitedParty"
$TargetModuleDir = Join-Path $GameFolder "Modules\$ModuleName"

Write-Host "=== Building $ModuleName ===" -ForegroundColor Cyan

# Build the project
dotnet build "src\UnlimitedParty\UnlimitedParty.csproj" `
    -c $Configuration `
    -p:GameFolder="$GameFolder"

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed!"
    exit 1
}

Write-Host "=== Deploying to $TargetModuleDir ===" -ForegroundColor Cyan

# Create target directories
$binDir = Join-Path $TargetModuleDir "bin\Win64_Shipping_Client"

New-Item -ItemType Directory -Path $binDir -Force | Out-Null

# Copy SubModule.xml
Copy-Item "Module\SubModule.xml" -Destination $TargetModuleDir -Force

# Copy built DLL
$builtDll = "src\UnlimitedParty\bin\$Configuration\net472\UnlimitedParty.dll"
if (Test-Path $builtDll) {
    Copy-Item $builtDll -Destination $binDir -Force
}

# Copy Harmony DLL if needed
$harmonyDll = "src\UnlimitedParty\bin\$Configuration\net472\0Harmony.dll"
if (Test-Path $harmonyDll) {
    Copy-Item $harmonyDll -Destination $binDir -Force
}

Write-Host "=== Done! ===" -ForegroundColor Green
Write-Host "Module deployed to: $TargetModuleDir"
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "  1. Launch Bannerlord"
Write-Host "  2. Go to Mods and enable 'Unlimited Party'"
Write-Host "  3. Start or load any campaign"
Write-Host "  4. Recruit unlimited companions and form unlimited clan parties"
