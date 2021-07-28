Option Strict On
Option Explicit On

Public Class CategoryTranslator
    Inherits SimpleStringTranslator

    ''' <summary>
    ''' To add a new category type, define a constant here and add code to the following places:
    ''' 1) CategoryTranslator.TranslateType().
    ''' 2) CategoryEditorForm.blnShowDialog().
    ''' 3) TrialBalanceForm.btnIncomeExpenseStatement_Click().
    ''' 4) CategoryGroupManager.strGetGroupTitle().
    ''' </summary>
    Public Shared ReadOnly TypeKey As String = "Type"
    Public Shared ReadOnly TypeSales As String = "SALES"
    Public Shared ReadOnly TypeReturns As String = "RETS"
    Public Shared ReadOnly TypeCOGS As String = "COGS"
    Public Shared ReadOnly TypeOperatingExpenses As String = "OPEXP"
    Public Shared ReadOnly TypeOfficeExpense As String = "OFFICE"
    Public Shared ReadOnly TypePayroll As String = "PAYROLL"
    Public Shared ReadOnly TypeRentInc As String = "RENTINC"
    Public Shared ReadOnly TypeRentExp As String = "RENTEXP"
    Public Shared ReadOnly TypeOtherIncome As String = "OTINC"
    Public Shared ReadOnly TypeOtherExpense As String = "OTEXP"
    Public Shared ReadOnly TypeTaxes As String = "TAXES"
    Public Shared ReadOnly TypeDepreciation As String = "DEPREC"

    Public Shared Function IsPersonal(ByVal strValue1 As String) As Boolean
        If strValue1 = "" Then
            Return False
        End If
        Return Char.ToUpper(strValue1(0)) = "C"c
    End Function

    Public Overrides Function FormatElement(objElement As StringTransElement) As String
        Dim strResult As String = objElement.Value1
        Dim strType As String = Nothing
        If objElement.ExtraValues.TryGetValue(TypeKey, strType) Then
            strResult = strResult + " (" + TranslateType(strType) + ")"
        End If
        Return strResult
    End Function

    Public Shared Function TranslateType(ByVal strType As String) As String
        If strType = CategoryTranslator.TypeSales Then Return "Sales"
        If strType = CategoryTranslator.TypeReturns Then Return "Returns"
        If strType = CategoryTranslator.TypeCOGS Then Return "Cost of Goods Sold"
        If strType = CategoryTranslator.TypeOperatingExpenses Then Return "Operating Expenses"
        If strType = CategoryTranslator.TypeOfficeExpense Then Return "Office Expense"
        If strType = CategoryTranslator.TypePayroll Then Return "Payroll"
        If strType = CategoryTranslator.TypeRentInc Then Return "Rental Income"
        If strType = CategoryTranslator.TypeRentExp Then Return "Rental Expense"
        If strType = CategoryTranslator.TypeOtherIncome Then Return "Other Income"
        If strType = CategoryTranslator.TypeOtherExpense Then Return "Other Expenses"
        If strType = CategoryTranslator.TypeTaxes Then Return "Taxes"
        If strType = CategoryTranslator.TypeDepreciation Then Return "Depreciation"
        Return strType
    End Function

    Public Function TranslateKey(ByVal strKey As String) As String
        Dim strName As String
        Dim strRoot As String
        strName = Me.KeyToValue1(strKey)
        If strName = "" Then
            strRoot = "TmpCat#" & strKey
            strName = "E:" & strRoot
            Me.Add(New StringTransElement(Me, strKey, strName, " " & strRoot))
        End If
        Return strName
    End Function
End Class
