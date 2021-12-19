function global:format-trx
{
}

#$global:companypath="c:\programdata\Willow Creek Checkbook\Tim Test Company"
$temppath = [System.IO.Path]::GetTempPath()
$tempname = "WillowCreekCheckbookTestData"
$global:companypath = Join-Path $temppath $tempname
$exists=Test-Path -Path $companypath
if($exists)
{
Remove-Item -Path $companypath -Recurse
}
$dummy=New-Item -Path $temppath -Name $tempname -ItemType "directory"
Copy-Item -Path ".\TestData\*" -Destination $companypath -Recurse