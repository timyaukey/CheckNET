Option Strict On
Option Explicit On
Imports CheckBookLib

Public Class AccountGroupManager
    Inherits ReportGroupManager

    Public Sub New(objCompany_ As Company)
        MyBase.New(objCompany_)
    End Sub

    Public Overrides Function objMakeLineItem(ByVal objParent As LineItemGroup, strItemKey As String) As ReportLineItem
        For Each objAccount As Account In objCompany.colAccounts
            If objAccount.intKey.ToString() = strItemKey Then
                Return New ReportLineItem(objParent, strItemKey, objAccount.strTitle)
            End If
        Next
        Throw New Exception("Could not find matching account")
    End Function

    Public Overrides Function strGetGroupTitle(strGroupKey As String) As String
        For Each objDef As Account.SubTypeDef In Account.arrSubTypeDefs
            If objDef.lngSubType.ToString() = strGroupKey Then
                Return objDef.strName
            End If
        Next
        Throw New Exception("Unrecognized account group")
    End Function
End Class
