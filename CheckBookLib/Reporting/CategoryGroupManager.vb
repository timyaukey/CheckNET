Option Strict On
Option Explicit On

Public Class CategoryGroupManager
    Inherits ReportGroupManager

    Public Sub New(objCompany_ As Company)
        MyBase.New(objCompany_)
    End Sub

    Public Overrides Function MakeLineItem(ByVal objParent As LineItemGroup, strItemKey As String) As ReportLineItem
        Dim intIndex As Integer = Company.Categories.FindIndexOfKey(strItemKey)
        If intIndex = 0 Then
            Throw New Exception("Could not find category")
        End If
        Return New ReportLineItem(objParent, strItemKey, Company.Categories.GetValue2(intIndex).TrimStart(" "c))
    End Function

    Public Overrides Function GetGroupTitle(strGroupKey As String) As String
        Select Case strGroupKey
            Case CategoryTranslator.TypeSales : Return "Sales"
            Case CategoryTranslator.TypeReturns : Return "Returns"
            Case CategoryTranslator.TypeCOGS : Return "Cost of Goods Sold"
            Case CategoryTranslator.TypeOperatingExpenses : Return "Operating Expenses"
            Case CategoryTranslator.TypeOfficeExpense : Return "Office Expense"
            Case CategoryTranslator.TypePayroll : Return "Payroll"
            Case CategoryTranslator.TypeRentInc : Return "Rental Income"
            Case CategoryTranslator.TypeRentExp : Return "Rental Expense"
            Case CategoryTranslator.TypeOtherIncome : Return "Other Income"
            Case CategoryTranslator.TypeOtherExpense : Return "Other Expense"
            Case CategoryTranslator.TypeTaxes : Return "Taxes"
            Case CategoryTranslator.TypeDepreciation : Return "Depreciation"
            Case Else : Return strGroupKey
        End Select
        Throw New NotImplementedException()
    End Function
End Class
