﻿import-module .\bin\debug\Willowsoft.Checkbook.Powershell.dll
$cmp = open-checkbookcompany -path "c:\programdata\Willow Creek Checkbook\Tim Test Company"
try
{
$acct = get-checkbookaccount -company $cmp -accountname "Checking Account"
$reg = get-checkbookregister -account $acct -registername "Checking Account"

get-checkbooktrx -register $reg -startdate 2/1/2019 -enddate 2/28/2019|remove-checkbooktrx

$split = new-checkbooksplit -company $cmp -category E:Groceries -amount -100.00
add-checkbooknormaltrx -register $reg -date 2/10/2019 -number Card -description "Amazon Warehouse" -status Unreconciled -fake -OneSplit $split

$split = new-checkbooksplit -company $cmp -category A:Savings -amount 500.00
add-checkbooknormaltrx -register $reg -date 2/10/2019 -number Dep -description "Transfer from savings" -status Reconciled -OneSplit $split

$split = new-checkbooksplit -company $cmp -category E:Groceries -budget Groceries -amount -20.05
add-checkbooknormaltrx -register $reg -date 2/11/2019 -number Card -description "Thriftway" -status Unreconciled -memo "short trip" -OneSplit $split

get-checkbooktrx -register $reg -startdate 2/1/2019 -enddate 2/28/2019|get-checkbooksimpletrx|format-table
}
finally
{
close-checkbookcompany -company $cmp
}
