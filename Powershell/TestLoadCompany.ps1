import-module .\bin\debug\Willowsoft.Checkbook.Powershell.dll
$global:cmp = open-checkbookcompany -path "c:\programdata\Willow Creek Checkbook\Tim Test Company"
$global:acct = get-checkbookaccount -company $global:cmp -accountname "Checking Account"
$global:reg = get-checkbookregister -account $global:acct -registername "Checking Account"
