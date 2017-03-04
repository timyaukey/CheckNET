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