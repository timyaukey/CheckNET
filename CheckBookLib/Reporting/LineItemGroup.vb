Option Strict On
Option Explicit On

Public Class LineItemGroup
    Private mobjParent As ReportGroupManager
    Private mstrGroupKey As String
    Private mstrGroupTitle As String
    Private mobjDict As Dictionary(Of String, ReportLineItem) = New Dictionary(Of String, ReportLineItem)()
    Private mcolItems As List(Of ReportLineItem) = New List(Of ReportLineItem)()
    Private mcurGroupTotal As Decimal = 0D
    Public blnPrinted As Boolean

    Public Sub New(ByVal objParent_ As ReportGroupManager, ByVal strGroupKey_ As String)
        mobjParent = objParent_
        mstrGroupKey = strGroupKey_
        mstrGroupTitle = mobjParent.strGetGroupTitle(mstrGroupKey)
        blnPrinted = False
    End Sub

    Public ReadOnly Property strGroupKey() As String
        Get
            Return mstrGroupKey
        End Get
    End Property

    Public ReadOnly Property strGroupTitle() As String
        Get
            Return mstrGroupTitle
        End Get
    End Property

    Public ReadOnly Property colItems() As IEnumerable(Of ReportLineItem)
        Get
            Return mcolItems
        End Get
    End Property

    Public Function objGetItem(ByVal objManager As ReportGroupManager, ByVal strItemKey As String) As ReportLineItem
        Dim objItem As ReportLineItem = Nothing
        If Not mobjDict.TryGetValue(strItemKey, objItem) Then
            objItem = objManager.objMakeLineItem(Me, strItemKey)
            mobjDict.Add(strItemKey, objItem)
            mcolItems.Add(objItem)
            mcolItems.Sort(AddressOf intSorter)
        End If
        Return objItem
    End Function

    Private Function intSorter(ByVal objItem1 As ReportLineItem, ByVal objItem2 As ReportLineItem) As Integer
        Return objItem1.strItemTitle.CompareTo(objItem2.strItemTitle)
    End Function

    Public Sub Add(ByVal curAmount As Decimal)
        mcurGroupTotal += curAmount
        mobjParent.Add(curAmount)
    End Sub

    Public ReadOnly Property curGroupTotal() As Decimal
        Get
            Return mcurGroupTotal
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return mstrGroupTitle + " " + gstrFormatCurrency(mcurGroupTotal)
    End Function
End Class
