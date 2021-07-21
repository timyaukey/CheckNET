Option Strict On
Option Explicit On

Public NotInheritable Class BalanceSheetScanner
    Public Shared Function objRun(ByVal objCompany As Company, ByVal datEndDate As DateTime) As AccountGroupManager
        Dim objManager As AccountGroupManager = New AccountGroupManager(objCompany)
        For Each objAccount In objCompany.Accounts
            If objAccount.lngType <> Account.AccountType.Personal Then
                Dim objGroup As LineItemGroup = objManager.objGetGroup(objAccount.lngSubType.ToString())
                Dim objLine As ReportLineItem = objGroup.objGetItem(objManager, objAccount.intKey.ToString())
                For Each objReg As Register In objAccount.colRegisters
                    For Each objTrx As BaseTrx In objReg.colAllTrx(Of BaseTrx)()
                        If objTrx.datDate > datEndDate Then
                            Exit For
                        End If
                        If Not objTrx.blnFake Then
                            objLine.Add(objTrx.curAmount)
                        End If
                    Next
                Next
            End If
        Next
        Return objManager
    End Function
End Class
