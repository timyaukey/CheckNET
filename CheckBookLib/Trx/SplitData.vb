Option Strict On
Option Explicit On

Public Class SplitData

    'All typed as string
    Public Memo As String
    Public CategoryKey As String
    Public PONumber As String
    Public InvoiceNum As String
    Public InvoiceDate As String
    Public DueDate As String
    Public Terms As String
    Public BudgetKey As String
    Public Amount As String
    Public Selected As Boolean

    Public ReadOnly Property blnUsed() As Boolean
        Get
            blnUsed = (CategoryKey <> "") Or (PONumber <> "") Or (InvoiceNum <> "") Or (InvoiceDate <> "") Or (DueDate <> "") Or (Terms <> "") Or (Memo <> "") Or (BudgetKey <> "") Or (Amount <> "")
        End Get
    End Property
End Class