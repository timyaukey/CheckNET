import-module .\bin\debug\Willowsoft.Checkbook.Powershell.dll
$cmp = open-checkbookcompany -path "c:\programdata\Willow Creek Checkbook\Tim Test Company"
$cmp.colAccounts
$acct = get-checkbookaccount -company $cmp -accountname "Checking Account"
$reg = get-checkbookregister -account $acct -registername "Checking Account"
$reg
$trx = new-checkbooknormaltrx -register $reg -date 2/10/2019 -number Card -description "Amazon Warehouse" -status Reconciled
$trx
close-checkbookcompany -company $cmp
