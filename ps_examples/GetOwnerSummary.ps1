param([datetime]$StartDate, [datetime]$EndDate, [string]$DKEnv='dkint', [string]$report)

# Load API help functions
$myPath = Split-Path -Parent $MyInvocation.MyCommand.Path;
. "$myPath\CoreAPIFunctions.ps1"

if (!$StartDate)
{
    $StartDate = get-date 
    $StartDate = $StartDate.AddMonths(-3)
    $StartDate = get-date $StartDate -f MM/dd/yyyy
}

if (!$EndDate){$EndDate = get-date -f MM/dd/yyyy}

if ($report -eq '') 
{
    $report = read-host -Prompt 'Filename'
}
SetupAPI

$uri = "https://$api/Report/Owner_Summary_Billing_Counts"

$Body = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$Body.Add("StartDate",$StartDate)
$Body.Add("EndDate",$EndDate)

Invoke-RestMethod -Method GET -Uri $uri -ContentType 'application/octet-stream' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")} -Body $body -outfile $report
