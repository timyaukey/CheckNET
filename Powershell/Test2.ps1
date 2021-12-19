import-module .\bin\debug\Willowsoft.Checkbook.Powershell.dll
& .\Initialize.ps1
$cmp = open-checkbookcompany -path $companypath
try
{
$acct = get-checkbookaccount -company $cmp -accountname "Checking Account"
$reg = get-checkbookregister -account $acct -registername "Checking Account"

get-checkbooktrx -register $reg -startdate 2/1/2019 -enddate 2/28/2019|remove-checkbooktrx

$split = new-checkbooksplit -company $cmp -category E:Groceries -amount -100.00 
add-checkbooknormaltrx -register $reg -date 2/10/2019 -number Card -description "Thriftway" -status Unreconciled -fake -OneSplit $split

$split = new-checkbooksplit -company $cmp -category E:Groceries -amount -500.00
add-checkbooknormaltrx -register $reg -date 2/11/2019 -number Card -description "Safeway" -status Reconciled -OneSplit $split

$matches = find-checkbooknormaltrx -register $reg -description "Safeway" -date 2/11/2019
if($matches.length -gt 0) {
	$match = $matches[0]
    $split = new-checkbooksplit -company $cmp -category E:Groceries -amount -400.00
	update-checkbooknormaltrx -normaltrx $match -number 2042 -onesplit $split
}

$matches = find-checkbooknormaltrx -register $reg -description "Thriftway" -date 2/11/2019 -daysbefore 2 -daysafter 2
update-checkbooknormaltrx -normaltrx $matches[0] -description "Thrifty Plumbing"

get-checkbooktrx -register $reg -startdate 2/1/2019 -enddate 2/28/2019|get-checkbooksimpletrx|format-table

save-checkbookcompany -company $cmp
}
finally
{
close-checkbookcompany -company $cmp
}
