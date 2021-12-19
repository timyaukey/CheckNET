import-module .\bin\debug\Willowsoft.Checkbook.Powershell.dll
# This is not a formal test script - its purpose is to open a company
# and set some global variables to allow manual experimentation.
# Be sure to close the company when you are done, or the real test scripts
# will fail because the company is open.
& .\Initialize.ps1
$global:cmp = open-checkbookcompany -path $companypath
$global:acct = get-checkbookaccount -company $global:cmp -accountname "Checking Account"
$global:reg = get-checkbookregister -account $global:acct -registername "Checking Account"
