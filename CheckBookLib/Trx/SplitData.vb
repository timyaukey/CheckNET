Option Strict On
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
            blnUsed = (strCategoryKey <> "") Or (strPONumber <> "") Or (strInvoiceNum <> "") Or (strInvoiceDate <> "") Or (strDueDate <> "") Or (strTerms <> "") Or (strMemo <> "") Or (strBudgetKey <> "") Or (strAmount <> "") Or (strImageFiles <> "")
        End Get
    End Property
End Class