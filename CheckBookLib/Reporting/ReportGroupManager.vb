Option Strict On
Option Explicit On

Public MustInherit Class ReportGroupManager
    Private mobjCompany As Company
    Private mobjDict As Dictionary(Of String, LineItemGroup) = New Dictionary(Of String, LineItemGroup)()
    Private mcurGrandTotal As Decimal = 0D

    Public Sub New(ByVal objCompany_ As Company)
        mobjCompany = objCompany_
    End Sub

    Protected ReadOnly Property objCompany() As Company
        Get
            Return mobjCompany
        End Get
    End Property

    Public Function objGetGroup(ByVal strGroupKey As String) As LineItemGroup
        Dim objGroup As LineItemGroup = Nothing

        If Not mobjDict.TryGetValue(strGroupKey, objGroup) Then
            objGroup = New LineItemGroup(Me, strGroupKey)
            mobjDict.Add(strGroupKey, objGroup)
        End If
        Return objGroup
    End Function

    Public ReadOnly Property colGroups() As IEnumerable(Of LineItemGroup)
        Get
            Return mobjDict.Values
        End Get
    End Property

    Public Sub Add(ByVal curAmount As Decimal)
        mcurGrandTotal += curAmount
    End Sub

    Public ReadOnly Property curGrandTotal() As Decimal
        Get
            Return mcurGrandTotal
        End Get
    End Property

    Public MustOverride Function strGetGroupTitle(ByVal strGroupKey As String) As String

    Public MustOverride Function objMakeLineItem(ByVal objParent As LineItemGroup, ByVal strItemKey As String) As ReportLineItem
End Class
