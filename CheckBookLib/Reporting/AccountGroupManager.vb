Option Strict On
Option Explicit On

Public Class AccountGroupManager
    Inherits ReportGroupManager

    Public Sub New(objCompany_ As Company)
        MyBase.New(objCompany_)
    End Sub

    Public Overrides Function MakeLineItem(ByVal objParent As LineItemGroup, strItemKey As String) As ReportLineItem
        For Each objAccount As Account In Company.Accounts
            If objAccount.AccountKey.ToString() = strItemKey Then
                Return New ReportLineItem(objParent, strItemKey, objAccount.Title)
            End If
        Next
        Throw New Exception("Could not find matching account")
    End Function

    Public Overrides Function GetGroupTitle(strGroupKey As String) As String
        For Each objDef As Account.SubTypeDef In Account.SubTypeDefs
            If objDef.AcctSubType.ToString() = strGroupKey Then
                Return objDef.Name
            End If
        Next
        Throw New Exception("Unrecognized account group")
    End Function
End Class
