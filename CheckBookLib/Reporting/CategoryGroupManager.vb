Option Strict On
Option Explicit On

Public Class CategoryGroupManager
    Inherits ReportGroupManager

    Public Sub New(objCompany_ As Company)
        MyBase.New(objCompany_)
    End Sub

    Public Overrides Function objMakeLineItem(ByVal objParent As LineItemGroup, strItemKey As String) As ReportLineItem
        Dim intIndex As Integer = objCompany.objCategories.intLookupKey(strItemKey)
        If intIndex = 0 Then
            Throw New Exception("Could not find category")
        End If
        Return New ReportLineItem(objParent, strItemKey, objCompany.objCategories.strValue2(intIndex).TrimStart(" "c))
    End Function

    Public Overrides Function strGetGroupTitle(strGroupKey As String) As String
        Select Case strGroupKey
            Case CategoryTranslator.strTypeSales : Return "Sales"
            Case CategoryTranslator.strTypeReturns : Return "Returns"
            Case CategoryTranslator.strTypeCOGS : Return "Cost of Goods Sold"
            Case CategoryTranslator.strTypeOperatingExpenses : Return "Operating Expenses"
            Case CategoryTranslator.strTypeOfficeExpense : Return "Office Expense"
            Case CategoryTranslator.strTypePayroll : Return "Payroll"
            Case CategoryTranslator.strTypeRentInc : Return "Rental Income"
            Case CategoryTranslator.strTypeRentExp : Return "Rental Expense"
            Case CategoryTranslator.strTypeOtherIncome : Return "Other Income"
            Case CategoryTranslator.strTypeOtherExpense : Return "Other Expense"
            Case CategoryTranslator.strTypeTaxes : Return "Taxes"
            Case CategoryTranslator.strTypeDepreciation : Return "Depreciation"
            Case Else : Return strGroupKey
        End Select
        Throw New NotImplementedException()
    End Function
End Class
