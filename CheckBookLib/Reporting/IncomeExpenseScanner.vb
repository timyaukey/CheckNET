Option Strict On
Option Explicit On
Imports CheckBookLib

Public NotInheritable Class IncomeExpenseScanner
    Public Shared Function objRun(ByVal objCompany As Company, ByVal datStartDate As DateTime,
                                  ByVal datEndDate As DateTime, ByVal blnIncludeRetainedEarnings As Boolean) As CategoryGroupManager
        Dim objManager As CategoryGroupManager = New CategoryGroupManager(objCompany)
        Dim objCategories As CategoryTranslator = objCompany.objCategories
        For Each objAccount In objCompany.colAccounts
            If objAccount.lngType <> Account.AccountType.Personal Then
                If blnIncludeRetainedEarnings Or objAccount.lngSubType <> Account.SubType.Equity_RetainedEarnings Then
                    For Each objReg As Register In objAccount.colRegisters
                        For Each objTrx As Trx In objReg.colDateRange(datStartDate, datEndDate)
                            If Not objTrx.blnFake Then
                                Dim objNormalTrx As NormalTrx = TryCast(objTrx, NormalTrx)
                                If Not objNormalTrx Is Nothing Then
                                    For Each objSplit As TrxSplit In objNormalTrx.colSplits
                                        If Not objSplit.blnHasReplicaTrx Then
                                            Dim intCatIndex As Integer = objCategories.intLookupKey(objSplit.strCategoryKey)
                                            Dim objTransElem As StringTransElement = objCategories.objElement(intCatIndex)
                                            Dim strGroupKey As String = Nothing
                                            If Not objTransElem.colValues.TryGetValue(CategoryTranslator.strTypeKey, strGroupKey) Then
                                                strGroupKey = CategoryTranslator.strTypeOperatingExpenses
                                            End If
                                            Dim objGroup As LineItemGroup = objManager.objGetGroup(strGroupKey)
                                            Dim objLine As ReportLineItem = objGroup.objGetItem(objManager, objSplit.strCategoryKey)
                                            objLine.Add(-objSplit.curAmount)
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    Next
                End If
            End If
        Next
        Return objManager
    End Function
End Class
