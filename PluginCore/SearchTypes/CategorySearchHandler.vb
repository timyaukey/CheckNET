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
        Name = strName_
    End Sub

    Public ReadOnly Property Name As String _
        Implements ISearchHandler.Name

    Public Overrides Function ToString() As String
        Return Me.Name
    End Function

    Public Sub HandlerSelected(ByVal objHostSearchUI As IHostSearchUI) _
        Implements ISearchHandler.HandlerSelected
        objHostSearchUI.UseComboBoxCriteria(GetCategories(mobjHostUI.Company))
    End Sub

    Public Function PrepareSearch(ByVal objHostSearchUI As IHostSearchUI) As Boolean _
        Implements ISearchHandler.PrepareSearch

        If objHostSearchUI.GetComboBoxSearchFor() Is Nothing Then
            mobjHostUI.ErrorMessageBox("Please select a category to search for.")
            Return False
        End If

        objComparer = DirectCast(objHostSearchUI.GetSearchType(), SearchComparer)
        Dim lngItemData As Integer = DirectCast(objHostSearchUI.GetComboBoxSearchFor(), CBListBoxItem).LBValue
        strParameter = mobjHostUI.Company.Categories.GetKey(lngItemData)
        strParameter = mobjHostUI.Company.Categories.KeyToValue1(strParameter)
        Return True
    End Function

    Public Sub ProcessTrx(
        ByVal objTrx As BaseTrx,
        ByVal dlgAddTrxResult As AddSearchMatchTrxDelegate,
        ByVal dlgAddSplitResult As AddSearchMatchSplitDelegate) _
        Implements ISearchHandler.ProcessTrx

        If TypeOf (objTrx) Is BankTrx Then
            For Each objSplit In DirectCast(objTrx, BankTrx).Splits
                If objComparer.Compare(mobjHostUI.Company.Categories.KeyToValue1(objSplit.CategoryKey), strParameter) Then
                    dlgAddSplitResult(DirectCast(objTrx, BankTrx), objSplit)
                End If
            Next
        End If
    End Sub

    Private Iterator Function GetCategories(ByVal objCompany As Company) As IEnumerable(Of Object)
        Dim objList As IStringTranslator = objCompany.Categories
        Dim intIndex As Integer
        For intIndex = 1 To objList.ElementCount
            Yield UITools.CreateListBoxItem(objList.GetValue1(intIndex), intIndex)
        Next
    End Function

End Class
