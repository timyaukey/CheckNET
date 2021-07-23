Option Strict On
Option Explicit On

Public NotInheritable Class BalanceSheetScanner
    Public Shared Function Run(ByVal objCompany As Company, ByVal datEndDate As DateTime) As AccountGroupManager
        Dim objManager As AccountGroupManager = New AccountGroupManager(objCompany)
        For Each objAccount In objCompany.Accounts
            If objAccount.AcctType <> Account.AccountType.Personal Then
                Dim objGroup As LineItemGroup = objManager.GetGroup(objAccount.AcctSubType.ToString())
                Dim objLine As ReportLineItem = objGroup.GetItem(objManager, objAccount.AccountKey.ToString())
                For Each objReg As Register In objAccount.Registers
                    For Each objTrx As BaseTrx In objReg.GetAllTrx(Of BaseTrx)()
                        If objTrx.TrxDate > datEndDate Then
                            Exit For
                        End If
                        If Not objTrx.IsFake Then
                            objLine.Add(objTrx.Amount)
                        End If
                    Next
                Next
            End If
        Next
        Return objManager
    End Function
End Class
