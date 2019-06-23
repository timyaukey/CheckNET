Option Strict On
Option Explicit On

Imports CheckBookLib
Imports PluginCore

Public Class TrxSearchHandler
    Implements ISearchHandler

    Private mobjHostUI As IHostUI
    Private dlgGetTrxData As GetTrxSearchDataDelegate
    Private dlgComparer As SearchComparerDelegate
    Private strParameter As String

    Public Shared txtSearchFor As System.Windows.Forms.TextBox
    Public Shared cboSearchType As System.Windows.Forms.ComboBox

    Public Sub New(
        ByVal objHostUI_ As IHostUI,
        ByVal strName_ As String,
        ByVal dlgGetTrxData_ As GetTrxSearchDataDelegate)

        mobjHostUI = objHostUI_
        strName = strName_
        dlgGetTrxData = dlgGetTrxData_
    End Sub

    Public ReadOnly Property strName As String _
        Implements ISearchHandler.strName

    Public Function blnPrepareSearch() As Boolean _
        Implements ISearchHandler.blnPrepareSearch
        dlgComparer = SearchComparers.objGetComparer(cboSearchType)
        strParameter = txtSearchFor.Text
        Return True
    End Function

    Public Sub ProcessTrx(
        ByVal objTrx As Trx,
        ByVal dlgAddTrxResult As Trx.AddSearchMaxTrxDelegate,
        ByVal dlgAddSplitResult As Trx.AddSearchMaxSplitDelegate) _
        Implements ISearchHandler.ProcessTrx

        If dlgComparer(dlgGetTrxData(objTrx), strParameter) Then
            dlgAddTrxResult(objTrx)
        End If
    End Sub
End Class
