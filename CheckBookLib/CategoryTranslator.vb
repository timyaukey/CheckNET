Option Strict On
Option Explicit On

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
End Class
