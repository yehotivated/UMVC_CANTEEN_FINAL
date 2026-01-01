# PowerShell script to kill UMVC_INVENTORY.exe process before rebuilding
# This fixes the "file is locked" error when building

Write-Host "Checking for running UMVC_INVENTORY processes..." -ForegroundColor Yellow

$processes = Get-Process -Name "UMVC_INVENTORY" -ErrorAction SilentlyContinue

if ($processes) {
    Write-Host "Found $($processes.Count) running process(es). Terminating..." -ForegroundColor Yellow
    
    foreach ($process in $processes) {
        try {
            Stop-Process -Id $process.Id -Force -ErrorAction Stop
            Write-Host "  Terminated process ID: $($process.Id)" -ForegroundColor Green
        }
        catch {
            Write-Host "  Failed to terminate process ID: $($process.Id) - $_" -ForegroundColor Red
        }
    }
    
    # Wait a moment for processes to fully terminate
    Start-Sleep -Seconds 1
    
    Write-Host "`nAll processes terminated. You can now rebuild your project." -ForegroundColor Green
}
else {
    Write-Host "No running UMVC_INVENTORY processes found. Safe to build." -ForegroundColor Green
}

Write-Host "`nPress any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

