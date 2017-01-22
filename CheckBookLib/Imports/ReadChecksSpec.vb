Option Explicit On
Option Strict On

Public Class ReadChecksSpec

    Public Sub New(ByVal numberColumn_ As Integer,
                   ByVal dateColumn_ As Integer,
                   ByVal descrColumn_ As Integer,
                   ByVal amountColumn_ As Integer,
                   ByVal memoColumn_ As Integer)

        NumberColumn = numberColumn_
        DateColumn = dateColumn_
        DescrColumn = descrColumn_
        AmountColumn = amountColumn_
        MemoColumn = memoColumn_
    End Sub

    'Zero based column numbers in tab delimited input line.
    'Minus one if column is not present.
    Public ReadOnly NumberColumn As Integer
    Public ReadOnly DateColumn As Integer
    Public ReadOnly DescrColumn As Integer
    Public ReadOnly AmountColumn As Integer
    Public ReadOnly MemoColumn As Integer

    Public Function strConvertTrxNum(ByVal strInput As String) As String
        If strInput.StartsWith("Electr") Then
            Return "Pmt"
        End If
        If strInput.StartsWith("Check-") Then
            Return strInput.Substring(6)
        End If
        Return strInput
    End Function
End Class
