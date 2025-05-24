# Get SQL LocalDB pipe name directly from SqlLocalDB.exe
$localDbInfo = SqlLocalDB.exe info MSSQLLocalDB

# Use the localdb instance name directly
$sqlInstance = "(localdb)\MSSQLLocalDB"

# Read database name from appsettings.json
$appSettings = Get-Content -Raw -Path "..\appsettings.json" | ConvertFrom-Json
$database = $appSettings.InitialCatalog

Write-Host "Using SQL Instance: $sqlInstance"
Write-Host "Using Database: $database"

$sqlFiles = Get-ChildItem -Path ".\Queries" -Filter "*.sql" -Recurse

Write-Host "Found $($sqlFiles.Count) SQL files to execute"

foreach ($file in $sqlFiles) {
    Write-Host "Executing $($file.FullName)"
    $command = "sqlcmd -S `"$sqlInstance`" -d $database -i `"$($file.FullName)`" -E"

    Write-Host "Running command: $command"

    try {
        Invoke-Expression $command
        Write-Host "Successfully executed $($file.Name)" -ForegroundColor Green
    } catch {
        Write-Host "Error executing $($file.Name): $_" -ForegroundColor Red
    }
}

Write-Host "All procedures executed successfully!"