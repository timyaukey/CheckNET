Option Strict On
Option Explicit On

Imports CheckBookLib
Imports PluginCore

Public Class CategorySearchHandler
    Implements ISearchHandler

    Private mobjHostUI As IHostUI
    Private dlgComparer As SearchComparerDelegate
    Private strParameter As String

    Public Shared cboSearchType As System.Windows.Forms.ComboBox
    Public Shared cboSearchCats As System.Windows.Forms.ComboBox

    Public Sub New(
        ByVal objHostUI_ As IHostUI,
        ByVal strName_ As String)

        mobjHostUI = objHostUI_
        strName = strName_
    End Sub

    Public ReadOnly Property strName As String _
        Implements ISearchHandler.strName

    Public Function blnPrepareSearch() As Boolean _
        Implements ISearchHandler.blnPrepareSearch

        If cboSearchCats.SelectedIndex = -1 Then
            mobjHostUI.ErrorMessageBox("Please select a category to search for.")
            Return False
        End If

        dlgComparer = SearchComparers.objGetComparer(cboSearchType)
        Dim lngItemData As Integer = UITools.GetItemData(cboSearchCats, cboSearchCats.SelectedIndex)
        strParameter = mobjHostUI.objCompany.objCategories.strKey(lngItemData)
        strParameter = mobjHostUI.objCompany.objCategories.strKeyToValue1(strParameter)
        Return True
    End Function

    Public Sub ProcessTrx(
        ByVal objTrx As Trx,
        ByVal dlgAddTrxResult As Trx.AddSearchMaxTrxDelegate,
        ByVal dlgAddSplitResult As Trx.AddSearchMaxSplitDelegate) _
        Implements ISearchHandler.ProcessTrx

        If TypeOf (objTrx) Is NormalTrx Then
            For Each objSplit In DirectCast(objTrx, NormalTrx).colSplits
                If dlgComparer(mobjHostUI.objCompany.objCategories.strKeyToValue1(objSplit.strCategoryKey), strParameter) Then
                    dlgAddSplitResult(DirectCast(objTrx, NormalTrx), objSplit)
                End If
            Next
        End If
    End Sub
End Class
