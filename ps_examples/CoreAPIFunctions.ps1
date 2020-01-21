#. $PSScriptRoot\CreateOwner.ps1
#. $PSScriptRoot\CreateDeviceNames.ps1
#. $PSScriptRoot\CreateAPIUser.ps1

$APILookup = @{'dkint'='dkintapi.keytest.net/api/ver6';'prod' = 'api.directkey.net/api/ver6'}
# updated dkint for creds - username and API password - base64 encode "<username>:<API password>" and put into 'dkint' for authAllLookup
$authAllLookup = @{'dkint'='base64 encode "<username>:<API password>';'prod' = 'cndlc3Q6MTIzSG9ydG9uUnV0c2NobWFuIQ=='}

# At PS cmdline after installing certificate: # cd cert:\CurrentUser\my to get thumbprint value for 'dkint' in certLookup
$certLookup = @{'dkint'='thumbprint value from cert:\CurrentUser\my';'prod' = 'EB84ADC8ECF64022551B706DA3E64199164017CC'}

function SetupAPI
{
    Set-Variable -Name api,authALL,CertNumber -Option AllScope
    [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12 -bor [System.Net.SecurityProtocolType]::Tls11
    $Script:api = $apilookup[$DKEnv]
    $Script:authALL = $authalllookup[$DKenv]
    $Script:CertNumber = $certlookup[$DKEnv]
}

function GetOwnersInOrg($OrgID)
{
if(!$DKEnv){$DKEnv='Prod'}

SetupAPI

#$api = $apilookup[$DKEnv]
#$authALL = $authalllookup[$DKenv]
#$CertNumber = $certlookup[$DKEnv]
$uri = "https://$api/Owner/Org/$OrgID"
$Body = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
try
{
    $Owners = Invoke-RestMethod -Method GET -Uri $uri -ContentType 'application/octet-json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")}
    $Owners.data
}
catch
{
}
}

function GetAllOwners([string]$Ownername)
{
if(!$DKEnv){$DKEnv='Prod'}

setupAPI

$uri = "https://$api/Owner"
$Body = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
if ($Ownername -ne ''){$body.Add("name",$Ownername)}
$Owners = Invoke-RestMethod -Method GET -Uri $uri -ContentType 'application/octet-json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")} -Body $body 
$Owners.data
}



function GetOrgs
{
if(!$DKEnv){$DKEnv='Prod'}

setupAPI

$uri = "https://$api/Organization"
$Body = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$Orgs = Invoke-RestMethod -Method GET -Uri $uri -ContentType 'application/octet-json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")}
$Orgs.data
}

function GetOwnerID($OwnerName,[string]$DKEnv='Prod')
{

if(!$DKEnv){$DKEnv='Prod'}

setupAPI

$Owners=GetAllOwners
$owner = $Owners | Where-Object name -eq $OwnerName
$owner.id
}


function CreateDeviceNames
{
[CmdletBinding()]
param([string]$OwnerName,[string]$LockOwnerID,[string]$KeyingForm,[switch]$ReturnList)

if ($OwnerName -eq '') 
{
    $OwnerName = read-host -Prompt 'Owner Name'
}
if ($LockOwnerID -eq '') 
{
    $LockOwnerID = read-host -Prompt 'Owner ID'
}
if ($KeyingForm -eq '') 
{
    $KeyingForm = read-host -Prompt 'Locking Plan'
}

$KeyingForm = Get-ChildItem($KeyingForm)

$startRow = 43

$excel = new-object -com excel.application
$wb = $excel.workbooks.open($KeyingForm)

$sh = $wb.Sheets.Item(1)
$endRow = 49
$APIRoomList = @()
for ($row=$startRow;$row -le $endRow;$row++)
{
    $startroom = $sh.Cells.Item($row,2).value2
    $endroom = $sh.cells.item($row,3).value2
    #$SkipRooms = $sh.cells.item($row,4).text -split ","
    $SkipRooms = expand-range $sh.cells.item($row,4).text
    $SkipRooms = $SkipRooms.replace(' ','')
    $SkipRooms = $SkipRooms -split ","
    if ($startroom -eq ''){break}
    for ([int16]$room = $startroom;$room -le $endRoom;$room++)
    {
        $APIRoom = New-Object -TypeName PSObject
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name OwnerID -Value $LockOwnerID
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name DeviceName -Value $room
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name Description -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name AccessCategory_ID -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name BeaconTransmitPowerPercentage -Value 100
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name BeaconPeriodMs -Value 1000
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name GPSLatitude -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name GPSLongitude -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name EventDataEnabled -Value 'Disabled'
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconEnabled -Value false
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconUUID -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconMajorVer -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconMinorVer -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconPower -Value 0

        if ($SkipRooms -notcontains $room)
        {
            $APIRoomList += $APIRoom
        }
    }
}

###################################################
$sh2 = $wb.Sheets.Item('Extra Space')
$row = 3
while (($sh2.Cells.Item($row,2).value2) -and `
    ($sh2.Cells.Item($row,1).value2 -ne 'Masters for Additional Guest Rooms on this page'))
{
    $startroom = $sh2.Cells.Item($row,2).value2
    $endroom = $sh2.cells.item($row,3).value2
    $SkipRooms = expand-range $sh2.cells.item($row,4).text
    $SkipRooms = $SkipRooms.replace(' ','')
    $SkipRooms = $SkipRooms -split ","
    for ([int16]$room = $startroom;$room -le $endRoom;$room++)
    {
        $APIRoom = New-Object -TypeName PSObject
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name OwnerID -Value $LockOwnerID
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name DeviceName -Value $room
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name Description -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name AccessCategory_ID -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name BeaconTransmitPowerPercentage -Value 100
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name BeaconPeriodMs -Value 1000
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name GPSLatitude -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name GPSLongitude -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name EventDataEnabled -Value 'Disabled'
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconEnabled -Value false
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconUUID -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconMajorVer -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconMinorVer -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconPower -Value 0
        if ($SkipRooms -notcontains $room)
        {
            $APIRoomList += $APIRoom
        }
    }
    $row++
}
###################################################


$startRow = 53
$endRow = 59
for ($row=$startRow;$row -le $endRow;$row++)
{
    $startroom = $sh.Cells.Item($row,2).value2
    $endroom = $sh.cells.item($row,3).value2
    $SkipRooms = expand-range $sh.cells.item($row,4).text
    $SkipRooms = $SkipRooms.replace(' ','')
    $SkipRooms = $SkipRooms -split ","
    if ($startroom -eq ''){break}
    for ([int16]$room = $startroom;$room -le $endRoom;$room++)
    {
        $APIRoom = New-Object -TypeName PSObject
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name OwnerID -Value $LockOwnerID
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name DeviceName -Value $room
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name Description -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name AccessCategory_ID -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name BeaconTransmitPowerPercentage -Value 100
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                   -Name BeaconPeriodMs -Value 1000
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name GPSLatitude -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name GPSLongitude -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name EventDataEnabled -Value 'Disabled'
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconEnabled -Value false
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconUUID -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconMajorVer -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconMinorVer -Value ''
        Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                    -Name iBeaconPower -Value 0
        if ($SkipRooms -notcontains $room)
        {
            $APIRoomList += $APIRoom
        }
    }
}

$startRow = 100
$endRow = 114
$pubroom = ''
for ($row=$startRow;$row -le $endRow;$row++)
{
    $pubroom = $sh.Cells.Item($row,1).Text
    if ($pubroom -eq ''){break}
    $APIRoom = New-Object -TypeName PSObject
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name OwnerID -Value $LockOwnerID
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name DeviceName -Value $pubroom
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name Description -Value $sh.Cells.Item($row,2).Text
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name AccessCategory_ID -Value '9'
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name BeaconTransmitPowerPercentage -Value 100
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name BeaconPeriodMs -Value 1000
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name GPSLatitude -Value ''
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name GPSLongitude -Value ''
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name EventDataEnabled -Value 'Disabled'
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name iBeaconEnabled -Value false
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name iBeaconUUID -Value ''
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name iBeaconMajorVer -Value ''
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name iBeaconMinorVer -Value ''
    Add-Member -InputObject $APIRoom -MemberType NoteProperty `
                -Name iBeaconPower -Value 0
    $APIRoomList += $APIRoom
}
$APIRoom = New-Object -TypeName PSObject
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name OwnerID -Value $LockOwnerID
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name DeviceName -Value 'DEMO LOCK ON BLOCK'
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name Description -Value ''
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name AccessCategory_ID -Value '9'
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name BeaconTransmitPowerPercentage -Value 100
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name BeaconPeriodMs -Value 1000
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name GPSLatitude -Value ''
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name GPSLongitude -Value ''
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name EventDataEnabled -Value 'Disabled'
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name iBeaconEnabled -Value false
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name iBeaconUUID -Value ''
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name iBeaconMajorVer -Value ''
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name iBeaconMinorVer -Value ''
Add-Member -InputObject $APIRoom -MemberType NoteProperty `
            -Name iBeaconPower -Value 0
$APIRoomList += $APIRoom
    
$excel.Workbooks.Close()

$APIRoomList | Export-Csv $OwnerName'.csv' -NoTypeInformation

if ($ReturnList){$APIRoomList}
}

function expand-range($array) {
    function expansion($arr) { 
        if($arr) {
            $arr = $arr.Split(',')
             $arr | foreach{
                $a = $_
                $b, $c, $d, $e = $a.Split('-')
                switch($a) {
                    $b {return $a}
                    "-$c" {return $a}
                    "$b-$c" {return "$(([Int]$b)..([Int]$c))"}
                    "-$c-$d" {return "$(([Int]$("-$c"))..([Int]$d))"}
                    "-$c--$e" {return "$(([Int]$("-$c"))..([Int]$("-$e")))"}
                }
             }
        } else {""}
    }
    $OFS = ","
    "$(expansion $array)"
    $OFS = " "
}

function CreateOwner {

[CmdLetBinding()]
param([string]$DKEnv='Prod',[string]$Owner, [string]$KeyingForm, [switch]$Marriott, [switch]$Gateway, [switch]$OnPortal, [string]$GenEMail, [string]$PMEmail, [string]$ServU)

if ($Owner -eq '')
{
    $Owner = read-host -Prompt 'Owner'
    $xlFiles = get-item *.xlsx
    foreach ($xlFile in $xlFiles)
    {
        $ans = Read-host -Prompt "$($xlFile) (Y/N)"
        if ($ans.toupper() -eq 'Y')
        {
            $KeyingForm = $xlFile
            break
        }
    }
    $OnPortalYN = Read-Host -Prompt 'OnPortal (Y/N)'
    if ($OnPortalYN.ToUpper() -eq 'Y'){$OnPortal=$true}
    $MarriottYN = Read-Host -Prompt 'Is this a Marriott (Y/N)'
    if ($MarriottYN.ToUpper() -eq 'Y'){$Marriott=$true}
    if ($Marriott)
    {
        $GenEMail=Read-Host -Prompt 'Generic Email Prefix'
        $PMEmail = Read-Host -Prompt 'Project Manager Email'
        $GatewayYN = Read-Host -Prompt 'Is there a Gateway (Y/N)'
        if ($GatewayYN.ToUpper() -eq 'Y'){$Gateway=$true}
        $ServU = Read-Host -Prompt 'ServU Password'
    }
}

if ($DKEnv -eq 'DKInt')
{
    if ($Marriott){$OrgID=1} else {$OrgID=9}
}
else
{
    if ($Marriott){$OrgID=2} else {$OrgID=1}
}

SetupAPI

$KeyingForm = Get-ChildItem($KeyingForm)
$excel = new-object -com excel.application
$wb = $excel.workbooks.open($KeyingForm)
$sh = $wb.Sheets.Item(1)

$ZipHash = @{}
Import-Csv "$ProfilePath\zipcode.csv" |`
ForEach-Object `
{
    $ZipHash.Add($_.zip,$_.offset)
}

$Address = $sh.Cells.Item(29,2).text+" `n"
$Address += $sh.Cells.Item(30,2).text
$email = $sh.Cells.Item(33,2).text
$phone = $sh.Cells.Item(32,2).text
$excel.Workbooks.Close()

if ($address.trim() -match "[0-9][0-9][0-9][0-9][0-9]$")
{
    $TZ = $ZipHash[$matches.Values]
    $TimeZoneOff = [int]$TZ.Item(0) * 60
}
else
{
    $TimeZoneOff = read-host -Prompt 'No Zip code found, Time Zone Offset'
}

$NewOwner = New-Object -TypeName PSObject

Add-Member -InputObject $NewOwner -MemberType NoteProperty `
    -Name 'Name' -Value $Owner
Add-Member -InputObject $NewOwner -MemberType NoteProperty `
    -Name 'TimezoneOffset' -Value $TimeZoneOff
Add-Member -InputObject $NewOwner -MemberType NoteProperty `
    -Name 'Address' -Value $Address
Add-Member -InputObject $NewOwner -MemberType NoteProperty `
    -Name 'DaysToRenew' -Value 2
Add-Member -InputObject $NewOwner -MemberType NoteProperty `
    -Name 'Enabled' -Value TRUE
if (($Marriott) -and ($GenEMail -ne ""))
{
    Add-Member -InputObject $NewOwner -MemberType NoteProperty `
        -Name 'Email' -Value "$($GenEmail).$($Owner).gm@marriott.com"
}
else
{
    Add-Member -InputObject $NewOwner -MemberType NoteProperty `
        -Name 'Email' -Value $email
}
Add-Member -InputObject $NewOwner -MemberType NoteProperty `
    -Name 'Phone' -Value $phone


$body = $NewOwner | ConvertTo-Json
$uri = "https://$api/Owner"

"Creating $Owner"
$APIOwner = Invoke-RestMethod -Method POST -Uri $uri -ContentType 'application/json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")} -Body $body 
$OwnerID = $APIOwner.ID
$uri = "https://$api/Owner/$OwnerID/Org/$OrgID"
$OrgAdd = Invoke-RestMethod -Method POST -Uri $uri -ContentType 'application/json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")}


$RoomList = CreateDeviceNames -OwnerName $Owner -LockOwner $OwnerID -KeyingForm $KeyingForm -ReturnList

$uri = "https://$api/DeviceName"
$body = $RoomList | ConvertTo-Json

$DeviceNames = Invoke-RestMethod -Method POST -Uri $uri -ContentType 'application/json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")} -Body $body 
$NumDeviceNames = $DeviceNames.Count
"Created $NumDeviceNames Device Names"

$uri = "https://$api/Key?ownerID=$OwnerID&numKeys=2&AllowPinReleaseShackle=TRUE"

"Key Name`tPIN`t`tAuthorization Code" > dkt.txt
$DKTKeys = Invoke-RestMethod -Method POST -Uri $uri -ContentType 'application/json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")}
$DKTKeys = $DKTKeys.Data
foreach ($DKTKey in $DKTKeys)
{
    [string]$newPIN = Get-Random
    $newPIN = $newPIN.Substring(0,4)
    $DKTKey.Pin = $newPIN
    $idx=$DKTKeys.IndexOf($DKTKey) + 1
    $keyname = "$Owner-DKT$idx"
    $DKTKey.Name = $keyname
    $serno = $DKTKey.SerialNumber
    $uri = "https://$api/Key/$serno"
    $body = $DKTKey | ConvertTo-Json
    $foo = Invoke-RestMethod -Method PUT -Uri $uri -ContentType 'application/json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")} -Body $body
    $uri = "https://$api/AuthenticationCode/$serno"
    $AuthCode = Invoke-RestMethod -Method GET -Uri $uri -ContentType 'application/json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")}
    $AuthCode = $Authcode.Code
    "$keyname`t$newPIN`t$AuthCode">>dkt.txt
}
"Created Toolkit keys"

if ($Gateway)
{
    CreateAPIUser -DKEnv $DKEnv -user "$Owner-GW" -email $email -OwnerID $OwnerID
    "Created API for Gateway $Owner-GW"
}

if ($OnPortal)
{
    CreateAPIUser -DKEnv $DKEnv -user "$($APIOwner.SystemCode)" -email $email -OwnerID $OwnerID
    "Created API for OnPortal $($APIOwner.SystemCode)"
}

if ($Marriott)
{
    $From = "mobilecredentials@fs.utc.com"
    $To = "$Email", "$($NewOwner.Email)"
    $cc = "Salm.Spina@marriott.com","MobileServices@marriott.com","Casey.Heitz@fs.utc.com","Donna.Dettling@marriott.com","$($PMEmail)"
    $bcc = "mobilecredentials@fs.utc.com"
    $Subject = "DirectKey Credentials - $Owner"
    $EmailBody = "Hello,

    In order to complete the DirectKey setup, our Installer/Trainer will need to access some files from our ServU site.   We have created a login for your hotel that will contain the necessary files.
  
    Web Site:  https://suprafs.suprakim.com
    Login Name: $Owner

    An additional email will follow with the password(s) for the site.
    
    Onity Installation Team
    
    Carrier
    4001 Fairview Industrial Dr SE
    Salem, Oregon 97302"

    $SMTPServer = "mail.utc.com"
    Send-MailMessage -From $From -to $To -cc $cc -bcc $bcc -Subject $Subject `
    -Body $EmailBody `
    -SmtpServer $SMTPServer 

    $Subject = "DirectKey Installation - $Owner"
    $EmailBody = 
    "The password for the ServU web page is $ServU
    "
    if ($Gateway)
    {
    $EmailBody += "The password for the self-extracting executable is $($APIOwner.SystemCode)
    "
    }
    $EmailBody += "  
    
    Onity Installation Team
    
    Carrier
    4001 Fairview Industrial Dr SE
    Salem, Oregon 97302"

    $SMTPServer = "mail.utc.com"
    Send-MailMessage -From $From -to $To -cc $cc -bcc $bcc -Subject $Subject `
    -Body $EmailBody `
    -SmtpServer $SMTPServer 
}

}

function CreateAPIUser {
[CmdLetBinding()]

param([string]$DKEnv='Prod',[string]$user,$permissions,[string]$email,[switch]$Tech,$OwnerID)


#. ($Profilepath + '\CoreAPIFunctions.ps1')
SetupAPI
if ($tech)
{
    $features = Import-Csv 'x:\powershell\Default API Permissions.csv'
    $Level = "ALL"
}
else
{
    if ($permissions -eq '')
    {
        $features = Import-Csv $permissions
    }
    else
    {
        $uri = "https://$api/APIFeature"
        $features = Invoke-RestMethod -Method GET -Uri $uri `
            -ContentType 'application/json' `
            -CertificateThumbprint $CertNumber `
            -Headers @{Authorization=("Basic $authALL")}
        $features = $features.data
        $Level = "ONE"
    }
}

$NewAPIUser = New-Object -TypeName PSObject
"Creating $user"
$pw = New-RandomPassword -Length 15 -Uppercase -Lowercase -Numbers -Symbols

Add-Member -InputObject $NewAPIUser -MemberType NoteProperty `
    -Name 'UserName' -Value $user
Add-Member -InputObject $NewAPIUser -MemberType NoteProperty `
    -Name 'Active' -Value 1
Add-Member -InputObject $NewAPIUser -MemberType NoteProperty `
    -Name 'APIPassword' -Value $pw
Add-Member -InputObject $NewAPIUser -MemberType NoteProperty `
    -Name 'EMail' -Value $email
Add-Member -InputObject $NewAPIUser -MemberType NoteProperty `
    -Name 'Level' -Value $Level
Add-Member -InputObject $NewAPIUser -MemberType NoteProperty `
    -Name 'Password' -Value $pw

$body = $NewAPIUser | ConvertTo-Json
$body = "[$body]"

$uri = "https://$api/APIUser"

$APIUser = Invoke-RestMethod -Method POST -Uri $uri -ContentType 'application/json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")} -Body $body 

$body = $features | ConvertTo-Json
$id = $APIUser.ApiUser_ID
$uri = "https://$api/APIUser/$id/ApiFeature"
$APIFeatures = Invoke-RestMethod -Method POST -Uri $uri -ContentType 'application/json' -CertificateThumbprint $CertNumber -Headers @{Authorization=("Basic $authALL")} -Body $body

if (!$Tech)
{
    $uri = "https://$api/APIUser/$id/Owner"
    $body = New-Object System.Collections.Generic.List[System.Object]
    $body.Add($OwnerID)
    $body = "[$body]"
    Invoke-RestMethod -Method POST -Uri $uri `
            -ContentType 'application/json' `
            -CertificateThumbprint $CertNumber `
            -Headers @{Authorization=("Basic $authALL")} -Body $Body
}

    
$CertPassword = New-RandomPassword -Length 15 -Uppercase -Lowercase -Numbers -Symbols
$Cert = New-Object -TypeName PSObject
$APIpw = From-Base64 $myAPIpw
#$APIpw = $myAPIpw
Add-Member -InputObject $Cert -MemberType NoteProperty `
    -Name 'LoggedOnUserWebPassword' -Value $APIpw
Add-Member -InputObject $Cert -MemberType NoteProperty `
    -Name 'CertificatePassword' -Value $CertPassword
Add-Member -InputObject $Cert -MemberType NoteProperty `
    -Name 'UserName' -Value $user
$body = $Cert | ConvertTo-Json
$uri = "https://$api/APIUser/Certificate"
$CertFile = $user + '@' + $api.Split('/')[0] + '.pfx'
$tmpCert = New-TemporaryFile
$body
$c = Invoke-RestMethod -Method POST -Uri $uri -ContentType 'application/json' `
    -CertificateThumbprint $CertNumber `
    -Headers @{Authorization=("Basic $authALL")} `
    -Body $body
$c | ConvertFrom-Base64 -OutputPath $CertFile

if ($Tech)
{
    $attachments = @()

    $From = "tier2@fs.utc.com"
    $To = $apiuser.email
    #$bcc = "tier2@fs.utc.com"
    $bcc = "ron.west@onity.com"
    $Attachments += $CertFile
    $Subject = "SaaS Web Account"
    $EmailBody = "A Web Account has been created for you to be able to access
    the SaaS information in our production environment.  

    Web Site:  https://api.directkey.net
    User Name: $user
    Password: $pw

    There is also a certificate that will need to be installed It should
    be attached to this email.  The password for the certificate is:
    $CertPassword"

    $SMTPServer = "mail.utc.com"
    Send-MailMessage -From $From -to $To -Subject $Subject -Bcc $bcc `
    -Body $EmailBody `
    -SmtpServer $SMTPServer `
    -Attachments $Attachments
}
else
{
    $outstring = "API User = $user`r`n"
    $outstring += "API PW = $pw`r`n"
    $outstring += "Cert PW = $CertPassword"
    $outstring > "Configuration.txt"
}
}

function To-Base64
{
    [CmdletBinding(DefaultParameterSetName='String')]
    [OutputType([String])]
    Param
    (
        # String to convert to base64
        [Parameter(Mandatory=$true,
                   ValueFromPipeline=$true,
                   ValueFromRemainingArguments=$false,
                   Position=0,
                   ParameterSetName='String')]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [string]
        $String,
 
        # Param2 help description
        [Parameter(ParameterSetName='ByteArray')]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [byte[]]
        $ByteArray
    )
 
    if($String) {
        return [System.Convert]::ToBase64String(([System.Text.Encoding]::UTF8.GetBytes($String)));
    } else {
        return [System.Convert]::ToBase64String($ByteArray);
    }
}
 
 
 
function From-Base64 {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory=$True,
                   Position=0,
                   ValueFromPipeline=$true)]
        [ValidateNotNull()]
        [ValidateNotNullOrEmpty()]
        [string]
        $Base64String
    )
 
    return [System.Text.Encoding]::UTF8.GetString(([System.Convert]::FromBase64String($Base64String)));
}

function InstallsNotCompleted {
    [CmdletBinding()]
param([string]$DKEnv='Prod')

#. ($Profilepath + '\CoreAPIFunctions.ps1')
#. X:\PowerShell\CoreAPI.PSM1

SetupAPI

$all = $true
$OrgsTable = GetOrgs
$OrgsTable = $orgstable[1]
$OwnersTable = @()
$ProgressTable = @()
foreach ($org in $OrgsTable){
    $OwnersTable = GetOwnersInOrg $org.id



    foreach ($Owner in $OwnersTable)
    {
        $uri = "https://$api/Report/Property_Installation_Progress"
        if ($Owner.name -notin "Onity Test Owner", "Uptown Hotel" )
        {
            $Body = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
            $body.Add("OwnerID",$Owner.ID)

            $tmpfile = New-TemporaryFile
            
            try 
            {
                Invoke-RestMethod -Method GET -Uri $uri `
                                  -ContentType 'application/octet-json' `
                                  -CertificateThumbprint $CertNumber `
                                  -Headers @{Authorization=("Basic $authALL")} `
                                  -Body $body -OutFile $tmpfile.FullName
                $progress = Import-Excel $tmpfile.FullName
                rm $tmpfile.FullName -ErrorAction Ignore


                $installed = $progress | where {($_.InstallSatus -eq 'INSTALLED') `
                            -and ($_.DeviceCommittedSystemCode -eq $_.OwnerSystemCode)}
                $notInstalled = $progress | where {$_.InstallSatus -ne 'INSTALLED'}
                $wrongcode = $progress | where {($_.InstallSatus -eq 'INSTALLED') `
                            -and ($_.DeviceCommittedSystemCode -ne $_.OwnerSystemCode)}
                if (((@($notInstalled).Count -gt 0) -or (@($wrongcode).count -gt 0)) -or $all)
                {
                    $ProgressTableRow = New-Object -TypeName PSObject
                    Add-Member -InputObject $ProgressTableRow -MemberType NoteProperty `
                            -Name 'Org' -Value $org.Name
                    Add-Member -InputObject $ProgressTableRow -MemberType NoteProperty `
                            -Name 'Owner' -Value $Owner.Name
                    Add-Member -InputObject $ProgressTableRow -MemberType NoteProperty `
                            -Name 'Installed' -Value @($installed).count
                    Add-Member -InputObject $ProgressTableRow -MemberType NoteProperty `
                            -Name 'Not Installed' -Value @($notInstalled).Count
                    Add-Member -InputObject $ProgressTableRow -MemberType NoteProperty `
                            -Name 'Installed w/wrong code' -Value @($wrongcode).count

                    $uri = "https://$api/KeyDevicePermission"
                    $Body = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
                    $body.Add("DeviceOwnerID",$Owner.ID)
                    try
                    {
                        $kdp = Invoke-RestMethod -Method GET -Uri $uri `
                            -ContentType 'application/octet-json' `
                            -CertificateThumbprint $CertNumber `
                            -Headers @{Authorization=("Basic $authALL")} `
                            -Body $body
                        $perms = $kdp.TotalRecords
                    }
                    catch
                    {
                        $perms = 0
                    }
                    Add-Member -InputObject $ProgressTableRow -MemberType NoteProperty `
                            -Name 'MobileKey Permissions' -Value $perms
                    
                    $ProgressTable += $ProgressTableRow
                }
            }
            Catch
            {
                "No Device names for $($owner.Name)"
            }
        }
    }
}
$ProgressTable
}


function GetAuthCodes {
    [CmdletBinding()]
param([string]$DKEnv='Prod',[string]$OwnerName)

. ($Profilepath + '\CoreAPIFunctions.ps1')
SetupAPI

if ($OwnerName -eq '')
{
    $OwnerName = read-host -Prompt 'Owner'
}

$OwnerID = GetOwnerID $OwnerName -DKEnv $DKEnv

$uri = "https://$api/Key"
$Body = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$body.Add("ownerid",$OwnerID)


$keys = Invoke-RestMethod -Method GET `
    -Uri $uri `
    -ContentType 'application/octet-json' `
    -CertificateThumbprint $CertNumber `
    -Headers @{Authorization=("Basic $authALL")} `
    -body $body

$keys = $keys.Data
$AuthCodes = @()
foreach ($key in $keys)
{
    if ($key.AllowPinReleaseShackle)
    {
        $AuthCode = New-Object -TypeName PSObject
        $sn = $key.SerialNumber
        $uri = "https://$api/AuthenticationCode/$sn"
        $Code = Invoke-RestMethod -Method GET `
            -Uri $uri `
            -ContentType 'application/octet-json' `
            -CertificateThumbprint $CertNumber `
            -Headers @{Authorization=("Basic $authALL")}
        Add-Member -InputObject $AuthCode -MemberType NoteProperty `
            -Name 'Owner' -Value $OwnerName
        Add-Member -InputObject $AuthCode -MemberType NoteProperty `
            -Name 'Key#' -Value $sn
        Add-Member -InputObject $AuthCode -MemberType NoteProperty `
            -Name 'PIN' -Value $key.Pin
        Add-Member -InputObject $AuthCode -MemberType NoteProperty `
            -Name 'Auth' -Value $Code.code
        $AuthCodes += $AuthCode
    }
}
$AuthCodes
}

function SearchOwner {
    [CmdletBinding()]
param([Parameter(Mandatory=$True,Position=1)][string]$search,[switch]$Marriott)

$DKEnv='Prod'
. ($Profilepath + '\CoreAPIFunctions.ps1')
SetupAPI
$orgid=1
if ($marriott){$orgid=2}
GetOwnersInOrg $OrgID | where {$_.address -like "*$search*"}
}

function GetKDP{
    [CmdletBinding()]
    param([string]$DKEnv='Prod',[string]$owner,[string]$OwnerID)
    . ($Profilepath + '\CoreAPIFunctions.ps1')
    SetupAPI

    if ($OwnerID -eq '')
    {
        if ($owner -eq '') 
        {
            $owner = read-host -Prompt 'Owner Name'
        }
        $OwnerID = GetOwnerID $owner -DKEnv $DKEnv
    }
    $uri = "https://$api/KeyDevicePermission"
    $Body = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
    $body.Add("deviceOwnerID",$OwnerID)
    $body.Add("takes",1)
    $body.Add("orderBy","ID")
    $body.Add("orderDirection","Desc")

    $devicepermission = Invoke-RestMethod -Method GET `
        -Uri $uri `
        -ContentType 'application/octet-json' `
        -CertificateThumbprint $CertNumber `
        -Headers @{Authorization=("Basic $authALL")} `
        -body $body
    $devicepermission = $devicepermission.data
    $devicepermission
}