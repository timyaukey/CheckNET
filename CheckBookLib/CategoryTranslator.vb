Option Strict On
Option Explicit On
Imports CheckBookLib

Public Class CategoryTranslator
    Inherits SimpleStringTranslator

    Public Shared ReadOnly strTypeKey As String = "Type"
    Public Shared ReadOnly strTypeSales As String = "SALES"
    Public Shared ReadOnly strTypeReturns As String = "RETS"
    Public Shared ReadOnly strTypeCOGS As String = "COGS"
    Public Shared ReadOnly strTypeOperatingExpenses As String = "OPEXP"
    Public Shared ReadOnly strTypeOfficeExpense As String = "OFFICE"
    Public Shared ReadOnly strTypePayroll As String = "PAYROLL"
    Public Shared ReadOnly strTypeRentInc As String = "RENTINC"
    Public Shared ReadOnly strTypeRentExp As String = "RENTEXP"
    Public Shared ReadOnly strTypeOtherIncome As String = "OTINC"
    Public Shared ReadOnly strTypeOtherExpense As String = "OTEXP"
    Public Shared ReadOnly strTypeTaxes As String = "TAXES"

    Public Shared Function blnIsPersonal(ByVal strValue1 As String) As Boolean
        If strValue1 = "" Then
            Return False
        End If
        Return Char.ToUpper(strValue1(0)) = "C"c
    End Function

    Public Overrides Function strFormatElement(objElement As StringTransElement) As String
        Dim strResult As String = objElement.strValue1
        Dim strType As String = Nothing
        If objElement.colValues.TryGetValue(strTypeKey, strType) Then
            strResult = strResult + " (" + strTranslateType(strType) + ")"
        End If
        Return strResult
    End Function

    Public Shared Function strTranslateType(ByVal strType As String) As String
        If strType = CategoryTranslator.strTypeSales Then Return "Sales"
        If strType = CategoryTranslator.strTypeReturns Then Return "Returns"
        If strType = CategoryTranslator.strTypeCOGS Then Return "Cost of Goods Sold"
        If strType = CategoryTranslator.strTypeOperatingExpenses Then Return "Operating Expenses"
        If strType = CategoryTranslator.strTypeOfficeExpense Then Return "Office Expense"
        If strType = CategoryTranslator.strTypePayroll Then Return "Payroll"
        If strType = CategoryTranslator.strTypeRentInc Then Return "Rental Income"
        If strType = CategoryTranslator.strTypeRentExp Then Return "Rental Expense"
        If strType = CategoryTranslator.strTypeOtherIncome Then Return "Other Income"
        If strType = CategoryTranslator.strTypeOtherExpense Then Return "Other Expenses"
        If strType = CategoryTranslator.strTypeTaxes Then Return "Taxes"
        Return strType
    End Function

    Public Function strTranslateKey(ByVal strKey As String) As String
        Dim strName As String
        Dim strRoot As String
        strName = Me.strKeyToValue1(strKey)
        If strName = "" Then
            strRoot = "TmpCat#" & strKey
            strName = "E:" & strRoot
            Me.Add(New StringTransElement(Me, strKey, strName, " " & strRoot))
            MsgBox("Error: Could not find code " & strKey & " in category " & "list. Have assigned it temporary category name " & strName & ", which " & "you will probably want to edit to make this category " & "permanent.", MsgBoxStyle.Information)
        End If
        Return strName
    End Function
End Class
