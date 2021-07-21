Option Strict On
Option Explicit On

Public Class CategorySearchHandler
    Implements ISearchHandler

    Private mobjHostUI As IHostUI
    Private objComparer As SearchComparer
    Private strParameter As String

    Public Sub New(
        ByVal objHostUI_ As IHostUI,
        ByVal strName_ As String)

        mobjHostUI = objHostUI_
        strName = strName_
    End Sub

    Public ReadOnly Property strName As String _
        Implements ISearchHandler.strName

    Public Overrides Function ToString() As String
        Return Me.strName
    End Function

    Public Sub HandlerSelected(ByVal objHostSearchUI As IHostSearchUI) _
        Implements ISearchHandler.HandlerSelected
        objHostSearchUI.UseComboBoxCriteria(objGetCategories(mobjHostUI.objCompany))
    End Sub

    Public Function blnPrepareSearch(ByVal objHostSearchUI As IHostSearchUI) As Boolean _
        Implements ISearchHandler.blnPrepareSearch

        If objHostSearchUI.objGetComboBoxSearchFor() Is Nothing Then
            mobjHostUI.ErrorMessageBox("Please select a category to search for.")
            Return False
        End If

        objComparer = DirectCast(objHostSearchUI.objGetSearchType(), SearchComparer)
        Dim lngItemData As Integer = DirectCast(objHostSearchUI.objGetComboBoxSearchFor(), CBListBoxItem).intValue
        strParameter = mobjHostUI.objCompany.objCategories.strKey(lngItemData)
        strParameter = mobjHostUI.objCompany.objCategories.strKeyToValue1(strParameter)
        Return True
    End Function

    Public Sub ProcessTrx(
        ByVal objTrx As BaseTrx,
        ByVal dlgAddTrxResult As AddSearchMatchTrxDelegate,
        ByVal dlgAddSplitResult As AddSearchMatchSplitDelegate) _
        Implements ISearchHandler.ProcessTrx

        If TypeOf (objTrx) Is BankTrx Then
            For Each objSplit In DirectCast(objTrx, BankTrx).colSplits
                If objComparer.blnCompare(mobjHostUI.objCompany.objCategories.strKeyToValue1(objSplit.strCategoryKey), strParameter) Then
                    dlgAddSplitResult(DirectCast(objTrx, BankTrx), objSplit)
                End If
            Next
        End If
    End Sub

    Private Iterator Function objGetCategories(ByVal objCompany As Company) As IEnumerable(Of Object)
        Dim objList As IStringTranslator = objCompany.objCategories
        Dim intIndex As Integer
        For intIndex = 1 To objList.intElements
            Yield UITools.CreateListBoxItem(objList.strValue1(intIndex), intIndex)
        Next
    End Function

End Class
