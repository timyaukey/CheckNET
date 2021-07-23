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
        mstrGroupTitle = mobjParent.GetGroupTitle(mstrGroupKey)
        blnPrinted = False
    End Sub

    Public ReadOnly Property GroupKey() As String
        Get
            Return mstrGroupKey
        End Get
    End Property

    Public ReadOnly Property GroupTitle() As String
        Get
            Return mstrGroupTitle
        End Get
    End Property

    Public ReadOnly Property Items() As IEnumerable(Of ReportLineItem)
        Get
            Return mcolItems
        End Get
    End Property

    Public Function GetItem(ByVal objManager As ReportGroupManager, ByVal strItemKey As String) As ReportLineItem
        Dim objItem As ReportLineItem = Nothing
        If Not mobjDict.TryGetValue(strItemKey, objItem) Then
            objItem = objManager.MakeLineItem(Me, strItemKey)
            mobjDict.Add(strItemKey, objItem)
            mcolItems.Add(objItem)
            mcolItems.Sort(AddressOf SortComparer)
        End If
        Return objItem
    End Function

    Private Function SortComparer(ByVal objItem1 As ReportLineItem, ByVal objItem2 As ReportLineItem) As Integer
        Return objItem1.ItemTitle.CompareTo(objItem2.ItemTitle)
    End Function

    Public Sub Add(ByVal curAmount As Decimal)
        mcurGroupTotal += curAmount
        mobjParent.Add(curAmount)
    End Sub

    Public ReadOnly Property GroupTotal() As Decimal
        Get
            Return mcurGroupTotal
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return mstrGroupTitle + " " + Utilities.strFormatCurrency(mcurGroupTotal)
    End Function
End Class
