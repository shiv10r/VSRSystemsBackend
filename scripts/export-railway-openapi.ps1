param(
    [Parameter(Mandatory = $true)]
    [string] $OutputPath
)

$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path -Parent $PSScriptRoot
$resolvedOutput = [System.IO.Path]::GetFullPath((Join-Path $repositoryRoot $OutputPath))
$env:RAILWAY_OPENAPI_OUTPUT = $resolvedOutput

try {
    dotnet test (Join-Path $repositoryRoot 'tests\VSRSystemsBackend.IntegrationTests\VSRSystemsBackend.IntegrationTests.csproj') `
        --filter 'FullyQualifiedName~RailwayOpenApiExportTests'
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    Write-Host "Railway OpenAPI exported to $resolvedOutput"
}
finally {
    Remove-Item Env:RAILWAY_OPENAPI_OUTPUT -ErrorAction SilentlyContinue
}
