Option Strict On
Option Explicit On

Public MustInherit Class ReportGroupManager
    Private mobjCompany As Company
    Private mobjDict As Dictionary(Of String, LineItemGroup) = New Dictionary(Of String, LineItemGroup)()
    Private mcurGrandTotal As Decimal = 0D

    Public Sub New(ByVal objCompany_ As Company)
        mobjCompany = objCompany_
    End Sub

    Protected ReadOnly Property Company() As Company
        Get
            Return mobjCompany
        End Get
    End Property

    Public Function GetGroup(ByVal strGroupKey As String) As LineItemGroup
        Dim objGroup As LineItemGroup = Nothing

        If Not mobjDict.TryGetValue(strGroupKey, objGroup) Then
            objGroup = New LineItemGroup(Me, strGroupKey)
            mobjDict.Add(strGroupKey, objGroup)
        End If
        Return objGroup
    End Function

    Public ReadOnly Property Groups() As IEnumerable(Of LineItemGroup)
        Get
            Return mobjDict.Values
        End Get
    End Property

    Public Sub Add(ByVal curAmount As Decimal)
        mcurGrandTotal += curAmount
    End Sub

    Public ReadOnly Property GrandTotal() As Decimal
        Get
            Return mcurGrandTotal
        End Get
    End Property

    Public MustOverride Function GetGroupTitle(ByVal strGroupKey As String) As String

    Public MustOverride Function MakeLineItem(ByVal objParent As LineItemGroup, ByVal strItemKey As String) As ReportLineItem
End Class
