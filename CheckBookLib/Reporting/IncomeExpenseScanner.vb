Option Strict On
Option Explicit On

Public NotInheritable Class IncomeExpenseScanner
    Public Shared Function Run(ByVal objCompany As Company, ByVal datStartDate As DateTime,
                                  ByVal datEndDate As DateTime, ByVal blnIncludeRetainedEarnings As Boolean) As CategoryGroupManager
        Dim objManager As CategoryGroupManager = New CategoryGroupManager(objCompany)
        Dim objCategories As CategoryTranslator = objCompany.Categories
        For Each objAccount In objCompany.Accounts
            If objAccount.AcctType <> Account.AccountType.Personal Then
                If blnIncludeRetainedEarnings Or objAccount.AcctSubType <> Account.SubType.Equity_RetainedEarnings Then
                    For Each objReg As Register In objAccount.Registers
                        For Each objNormalTrx In objReg.GetDateRange(Of BankTrx)(datStartDate, datEndDate)
                            If Not objNormalTrx.IsFake Then
                                For Each objSplit As TrxSplit In objNormalTrx.Splits
                                    If Not objSplit.HasReplicaTrx Then
                                        Dim intCatIndex As Integer = objCategories.FindIndexOfKey(objSplit.CategoryKey)
                                        Dim objTransElem As StringTransElement = objCategories.GetElement(intCatIndex)
                                        Dim strGroupKey As String = Nothing
                                        If Not objTransElem.ExtraValues.TryGetValue(CategoryTranslator.TypeKey, strGroupKey) Then
                                            strGroupKey = CategoryTranslator.TypeOperatingExpenses
                                        End If
                                        Dim objGroup As LineItemGroup = objManager.GetGroup(strGroupKey)
                                        Dim objLine As ReportLineItem = objGroup.GetItem(objManager, objSplit.CategoryKey)
                                        objLine.Add(objSplit.Amount)
                                    End If
                                Next
                            End If
                        Next
                    Next
                End If
            End If
        Next
        Return objManager
    End Function
End Class
