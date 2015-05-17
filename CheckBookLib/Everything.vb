Option Strict Off
Option Explicit On
Public Class Everything

    Public Event SomethingModified()

    Private mcolAccounts As Collection

    Public Sub Init()
        mcolAccounts = New Collection
    End Sub

    Public ReadOnly Property colAccounts() As Collection
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