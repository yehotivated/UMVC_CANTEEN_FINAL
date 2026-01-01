# PowerShell script to remove Zone.Identifier (Mark of the Web) from all .resx files
# This fixes the "Internet or Restricted zone" error when building the project

Write-Host "Removing Zone.Identifier from all .resx files..." -ForegroundColor Yellow

$filesCleaned = 0
Get-ChildItem -Path $PSScriptRoot -Filter "*.resx" -Recurse | ForEach-Object {
    $file = $_.FullName
    $zoneId = "$file`:Zone.Identifier"
    
    try {
        if (Get-Item $file -Stream Zone.Identifier -ErrorAction SilentlyContinue) {
            Remove-Item $zoneId -Force -ErrorAction Stop
            Write-Host "  Removed Zone.Identifier from: $($_.Name)" -ForegroundColor Green
            $filesCleaned++
        }
    }
    catch {
        # File might not have Zone.Identifier, or it was already removed
    }
}

if ($filesCleaned -eq 0) {
    Write-Host "`nAll files are already clean!" -ForegroundColor Green
} else {
    Write-Host "`nCleaned $filesCleaned file(s). You can now rebuild your project." -ForegroundColor Green
}

Write-Host "`nPress any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

