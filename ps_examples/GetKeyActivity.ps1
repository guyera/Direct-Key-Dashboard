param([datetime]$StartDate, [datetime]$EndDate, [string]$DKEnv='dkint')

# Load API help functions
$myPath = Split-Path -Parent $MyInvocation.MyCommand.Path;
. "$myPath\CoreAPIFunctions.ps1"

if (!$StartDate){$StartDate = get-date -f MM-yyyy}

if (!$EndDate){$EndDate=$StartDate}
$StartMonth = get-date $StartDate -f MM
$StartYear = get-date $StartDate -f yyyy
$EndMonth = get-date $EndDate -f MM
$EndYear = get-date $EndDate -f yyyy

# Put Month Names into an array
$a = new-object system.globalization.datetimeformatinfo
$MonthNames = $a.AbbreviatedMonthNames

[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12 -bor [System.Net.SecurityProtocolType]::Tls11

#####Variables
$api = $apilookup[$DKEnv]
$authALL = $authalllookup[$DKenv]
$CertNumber = $certlookup[$DKEnv]
$uri = "https://$api/KeyDeviceActivity/Export"
#$uri = "https://$api/KeyDeviceActivity"

[datetime]$month = $StartDate
do
{
    [datetime]$ReportStart = get-date $Month
    [datetime]$ReportEnd = get-date $ReportStart.AddMonths(1) 
    $RepStart = get-date $ReportStart -f MM/1/yyyy
    $RepEnd = get-date $ReportEnd -f MM/1/yyyy

    $Body = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
    $Body.Add("tranDateStart",$RepStart)
    $Body.Add("tranDateEnd",$RepEnd)
    $body.Add("takes",100000)

    $ReportMonth = $MonthNames[$Month.Month-1]
    $repmonthformatted = get-date $Month -f MM
    $cwd = Resolve-Path '.\'
    $ReportName = $cwd.Path + '\Data\KeyDeviceActivity' +$month.Year + $repmonthformatted
    'Generating ' + $ReportMonth

    $data = Invoke-RestMethod -Method GET -Uri $uri -ContentType 'application/json' `
        -CertificateThumbprint $CertNumber `
        -Headers @{Authorization=("Basic $authALL")} `
        -Body $body

    #$data.Data | export-excel $ReportName -AutoSize
    $data > $($ReportName + '.csv')
    $data = Import-Csv $($ReportName+'.csv')
    $data | Export-csv $($ReportName+'.csv') -NoTypeInformation

    rm $($ReportName+'.xlsx')  -ErrorAction Ignore

    $xl = new-object -comobject excel.application
    $xl.visible = $true
    $Workbook = $xl.workbooks.open($($ReportName+'.csv'))
    $Worksheets = $Workbooks.worksheets
    $Workbook.SaveAs($($ReportName+'.xlsx'),51)
    $Workbook.Saved = $True
    $xl.Quit()
#    $data | export-excel $ReportName -AutoSize

    rm $($ReportName+'.csv')  -ErrorAction Ignore
    $Month = $month.AddMonths(1)
} while ($month -le $EndDate)

