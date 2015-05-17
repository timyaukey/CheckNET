Option Strict Off
Option Explicit On
Public Class SplitData

    'All typed as string
    Public strMemo As String
    Public strCategoryKey As String
    Public strPONumber As String
    Public strInvoiceNum As String
    Public strInvoiceDate As String
    Public strDueDate As String
    Public strTerms As String
    Public strBudgetKey As String
    Public strAmount As String
    Public strImageFiles As String
    Public blnChoose As Boolean

    Public ReadOnly Property blnUsed() As Object
        Get
            'UPGRADE_WARNING: Couldn't resolve default property of object blnUsed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            blnUsed = (strCategoryKey <> "") Or (strPONumber <> "") Or (strInvoiceNum <> "") Or (strInvoiceDate <> "") Or (strDueDate <> "") Or (strTerms <> "") Or (strMemo <> "") Or (strBudgetKey <> "") Or (strAmount <> "") Or (strImageFiles <> "")
        End Get
    End Property
End Class