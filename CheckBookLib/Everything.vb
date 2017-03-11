Option Strict On
Option Explicit On

Public Class Everything

    Public Event SomethingModified()

    Private mcolAccounts As List(Of Account)

    Public Sub Init()
        mcolAccounts = New List(Of Account)
    End Sub

    Public ReadOnly Property colAccounts() As List(Of Account)
        Get
            colAccounts = mcolAccounts
        End Get
    End Property

    Public Function blnAccountKeyUsed(ByVal intKey As Integer) As Boolean
        For Each act As Account In mcolAccounts
            If act.intKey = intKey Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function intGetUnusedAccountKey() As Integer
        Dim intMaxKey As Integer = 0
        For Each act As Account In mcolAccounts
            If act.intKey > intMaxKey Then
                intMaxKey = act.intKey
            End If
        Next
        Return intMaxKey + 1
    End Function

    Public Sub FireSomethingModified()
        RaiseEvent SomethingModified()
    End Sub

    Public Sub Teardown()
        Dim objAccount As Account
        For Each objAccount In mcolAccounts
            objAccount.Teardown()
        Next objAccount
    End Sub
End Class