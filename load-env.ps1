[CmdletBinding()]
param (
    [Parameter()] [string] $EnvFile = ".env",
    [Parameter()] [switch] $Override
)

$root = $PSScriptRoot
$dllPath = "$root\.tmp\SharpDotEnv.0.1.0\lib\net7.0\SharpDotEnv.dll"

if (-not (Test-Path $dllPath)) {
    Write-Host "Package not found. Installing..."
    New-Item -ItemType Directory -Path "$root\.tmp\SharpDotEnv.0.1.0" -Force | Out-Null
    Install-Package -Name SharpDotEnv -ProviderName NuGet -Scope CurrentUser -RequiredVersion 0.1.0 -Destination .\.tmp -Force | Out-Null
}

Write-Host "Root: $root"

$job = Start-Job -ScriptBlock {
    Add-Type -Path $args[0]

    $options = New-Object -TypeName SharpDotEnv.DotEnvConfigOptions -Property @{
        Path = $args[1]
    }

    [SharpDotEnv.DotEnv]::Parse($options)
} -ArgumentList $dllPath, $EnvFile
Wait-Job $job | Out-Null

$dotenv = Receive-Job $job

foreach ($kvp in $dotenv.GetEnumerator()) {
    $key = $kvp.Key
    $val = $kvp.Value

    if (-not (Test-Path "env:$key") -or $Override) {
        Write-Host "Setting: $key"
        Set-Item -Path "env:$key" -Value "$val" | Out-Null
    }
}
